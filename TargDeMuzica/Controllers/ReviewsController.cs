using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TargDeMuzica.Data;
using TargDeMuzica.Models;

namespace TargDeMuzica.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ReviewsController(
            ApplicationDbContext _db,
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            db = _db;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var reviews = from review in db.Reviews
                          orderby review.ReviewContent
                          select review;

            ViewBag.Reviews = reviews;
            return View();
        }
        public ActionResult Show(int id)
        {
            Review review = db.Reviews.Find(id);
            return View(review);
        }

        public ActionResult New()
        {
            return View();
        }
        [Authorize(Roles = "UserI, Colaborator, Administrator")]
        [HttpPost]
        public async Task<ActionResult> New(Review rev)
        {


            if(ModelState.IsValid)
            {
                rev.ReviewDate = DateTime.Now;
                rev.User = await userManager.GetUserAsync(User);
                db.Reviews.Add(rev);
                db.SaveChanges();
                TempData["message"] = "Review-ul a fost adaugat";

                


                return RedirectToAction("Show", "Products", new { id = rev.ProductId });

            }
            else
            {
                return RedirectToAction("Show", "Products", new { id = rev.ProductId });
            }
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            return View(review);
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, Review requestReview)
        {
            Review review = db.Reviews.Find(id);

            try
            {

                review.ReviewContent = requestReview.ReviewContent;
                review.ReviewDate = requestReview.ReviewDate;
                review.StarRating = requestReview.StarRating;

                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adugat cu succes!!";
                return RedirectToAction("Show", "Products", new { id = review.ProductId });
            }
            catch (Exception e)
            {
                return View(requestReview);
            }
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
           Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            TempData["message"] = "Review-ul a fost sters";
            return RedirectToAction("Show", "Products", new { id = review.ProductId });
        }
    }
}
