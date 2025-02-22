using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Composition;
using System.Drawing.Printing;
using TargDeMuzica.Data;
using TargDeMuzica.Models;
using static TargDeMuzica.Models.IncomingRequest;

namespace TargDeMuzica.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IActionResult Index(string search, string sortBy = "name", string sortOrder = "asc")
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var pendingProductIds = db.IncomingRequests
                .Where(r => r.Status == IncomingRequest.RequestStatus.Rejected || r.Status == IncomingRequest.RequestStatus.Pending)
                .Select(r => r.ProposedProduct.ProductID);

            
            var productsQuery = db.Products
                                    .Include(p => p.User)  
                                    .Where(p => !pendingProductIds.Contains(p.ProductID));

            
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(search));
            }

            
            productsQuery = sortBy.ToLower() switch
            {
                "price" => sortOrder.ToLower() == "asc"
                    ? productsQuery.OrderBy(p => p.ProductPrice)
                    : productsQuery.OrderByDescending(p => p.ProductPrice),
                "rating" => sortOrder.ToLower() == "asc"
                    ? productsQuery.OrderBy(p => p.ProductScore)
                    : productsQuery.OrderByDescending(p => p.ProductScore),
                _ => sortOrder.ToLower() == "asc"  
                    ? productsQuery.OrderBy(p => p.ProductName)
                    : productsQuery.OrderByDescending(p => p.ProductName)
            };

            var products = productsQuery.ToList();

            
            ViewBag.Products = products;
            ViewBag.SearchString = search;
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.PaginationBaseUrl = $"/Products/Index/?search={search}&sortBy={sortBy}&sortOrder={sortOrder}&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = $"/Products/Index/?sortBy={sortBy}&sortOrder={sortOrder}&page";
            }

            return View();
        }




        public ActionResult Show(int id)
        {
            Product product = db.Products
                .Include(p => p.Reviews)
                .Include(p => p.MusicSuport)
                .Include(p => p.Artist)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.Reviews != null && product.Reviews.Any())
            {
                product.ProductScore = (float)product.Reviews.Average(r => r.StarRating);
                db.SaveChanges();
            }

            return View(product);
        }



        [NonAction]
        public IEnumerable<SelectListItem> GetAllMusicSup()
        {
           
            var selectList = new List<SelectListItem>();
            var sup = from su in db.MusicSuports
                             select su;

            foreach (var su in sup)
            {
                selectList.Add(new SelectListItem
                {
                    Value = su.MusicSuportID.ToString(),
                    Text = su.MusicSuportName
                });
            }

            return selectList;
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllArtists()
        {
            var selectList = new List<SelectListItem>();
            var artist = from art in db.Artists
                      select art;

            foreach (var art in artist)
            {
                selectList.Add(new SelectListItem
                {
                    Value = art.ArtistID.ToString(),
                    Text = art.ArtistName
                });
            }

            return selectList;
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            Product produs = new Product();
            produs.MusicSup = GetAllMusicSup();
            produs.ArtistList = GetAllArtists();
            return View(produs);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> New(Product prod, IFormFile Image)
        {
            try
            {
                if (Image != null && Image.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };
                    var fileExtension = Path.GetExtension(Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ArticleImage", "Fișierul trebuie să fie o imagine(jpg, jpeg, png, gif) sau un video(mp4, mov).");
                        prod.MusicSup = GetAllMusicSup();
                        prod.ArtistList = GetAllArtists();
                        return View(prod);
                    }

                    var imagesPath = Path.Combine(_env.WebRootPath, "images");
                    var storagePath = Path.Combine(imagesPath, Image.FileName);
                    var databaseFileName = "/images/" + Image.FileName;

                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    prod.ProductImageLocation = databaseFileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "An image file is required");
                    prod.MusicSup = GetAllMusicSup();
                    prod.ArtistList = GetAllArtists();
                    return View(prod);
                }

                var currentUser = await _userManager.GetUserAsync(User);
                prod.User = currentUser;

                prod.ProductGenres = prod.ProductGenresTemp.Split(' ').ToList();
                db.Products.Add(prod);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                prod.MusicSup = GetAllMusicSup();
                prod.ArtistList = GetAllArtists();
                return View(prod);
            }
        }
        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult Edit(int id)
        {
            Product prod = db.Products.Find(id);
            prod.MusicSup = GetAllMusicSup();
            prod.ArtistList = GetAllArtists();
            return View(prod);
        }

        [Authorize(Roles = "Colaborator,Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, Product requestProd)
        {
            Product prod = db.Products.Find(id);

            try
            {

                prod.ProductName = requestProd.ProductName;
                prod.ProductDescription = requestProd.ProductDescription;
                prod.MusicSuportID = requestProd.MusicSuportID;
                prod.ProductPrice = requestProd.ProductPrice;
                prod.ProductStock = requestProd.ProductStock;
                prod.ProductImageLocation = requestProd.ProductImageLocation;
                prod.ProductScore = requestProd.ProductScore;
                prod.ProductGenres = requestProd.ProductGenres;
                prod.ArtistID = requestProd.ArtistID;
                prod.Carts = requestProd.Carts;
                prod.Reviews = requestProd.Reviews;
                prod.User = requestProd.User;

                //de trimis in incomingrequests si sa fie de acolo bagat in
                //baza de date ce am facut mai sus

                db.SaveChanges();
                TempData["message"] = "Produsul a fost editat cu succes";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                prod.MusicSup = GetAllMusicSup();
                prod.ArtistList = GetAllArtists();
                return View(requestProd);
            }
        }

        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = db.Products
                    .Include(p => p.Reviews)  
                    .FirstOrDefault(p => p.ProductID == id);

                if (product == null)
                {
                    return NotFound();
                }

                if (product.Reviews != null && product.Reviews.Any())
                {
                    db.Reviews.RemoveRange(product.Reviews);
                }

                db.Products.Remove(product);
                db.SaveChanges();

                TempData["message"] = "Produsul si review-urile asociate au fost sterse";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["message"] = "A aparut o eroare la stergerea produsului: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult Submit()
        {
            Product product = new Product();
            product.MusicSup = GetAllMusicSup();
            product.ArtistList = GetAllArtists();
            return View(product);
        }
        [Authorize(Roles = "Colaborator,Administrator")]
        [HttpPost]
        [Authorize(Roles = "Colaborator,Administrator")]
        [HttpPost]
        public async Task<ActionResult> Submit(Product product, IFormFile Image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    product.MusicSup = GetAllMusicSup();
                    product.ArtistList = GetAllArtists();
                    return View(product);
                }

                if (Image != null && Image.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };
                    var fileExtension = Path.GetExtension(Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ArticleImage", "Fisierul trebuie să fie o imagine(jpg, jpeg, png, gif) sau un video(mp4, mov).");
                        product.MusicSup = GetAllMusicSup();
                        product.ArtistList = GetAllArtists();
                        return View(product);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
                    var imagesPath = Path.Combine(_env.WebRootPath, "images");
                    var storagePath = Path.Combine(imagesPath, uniqueFileName);
                    var databaseFileName = "/images/" + uniqueFileName;

                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    product.ProductImageLocation = databaseFileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "An image file is required");
                    product.MusicSup = GetAllMusicSup();
                    product.ArtistList = GetAllArtists();
                    return View(product);
                }

                if (!string.IsNullOrEmpty(product.ProductGenresTemp))
                {
                    product.ProductGenres = product.ProductGenresTemp.Split(' ').ToList();
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    var userFromDb = await db.Users.FindAsync(currentUser.Id);
                    product.User = userFromDb;
                    product.UserId = currentUser.Id;

                    var request = new IncomingRequest
                    {
                        RequestDate = DateTime.Now,
                        Status = RequestStatus.Pending,
                        ProposedProduct = product,
                        User = userFromDb
                    };

                    db.IncomingRequests.Add(request);
                    await db.SaveChangesAsync();

                    TempData["message"] = "Your product has been submitted for review. An administrator will review it shortly.";
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    TempData["message"] = "Error: Unable to associate user with product.";
                    return RedirectToAction("Index", "Products");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Submit: {e.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {e.StackTrace}");

                product.MusicSup = GetAllMusicSup();
                product.ArtistList = GetAllArtists();
                return View(product);
            }
        }
    }
}



