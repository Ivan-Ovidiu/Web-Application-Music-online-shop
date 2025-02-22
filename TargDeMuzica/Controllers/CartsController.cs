using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TargDeMuzica.Data;
using TargDeMuzica.Models;

namespace TargDeMuzica.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CartsController(ApplicationDbContext context)
        {
            db = context;
        }
        [Authorize(Roles = "UserI, UserN, Administrator, Colaborator")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var carts = from cart in db.Carts
                          orderby cart.TotalPrice
                          select cart;

            ViewBag.Carts = carts;
            return View();
        }
        public ActionResult Show(int id)
        {
            Cart cart = db.Carts.Find(id);
            return View(cart);
        }


        public ActionResult Edit(int id)
        {
           Cart cart = db.Carts.Find(id);
            return View(cart);
        }

        [HttpPost]
        public ActionResult Edit(int id, Cart requestCart)
        {
            Cart cart = db.Carts.Find(id);

            try
            {

                cart.TotalPrice = requestCart.TotalPrice;
                db.SaveChanges();
                TempData["message"] = "Cartul a fost editat cu succes!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(requestCart);
            }
        }

    
    }
}
