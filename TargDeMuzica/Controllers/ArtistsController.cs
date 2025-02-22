using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TargDeMuzica.Data;
using TargDeMuzica.Models;

namespace TargDeMuzica.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext db;
      

        public ArtistsController(ApplicationDbContext context)
        {
            db = context;
        }
        [Authorize(Roles = "Administrator")]

        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var artists = from artist in db.Artists
                             orderby artist.ArtistName
                             select artist;

            ViewBag.Artists = artists;
            return View();
        }
        [Authorize(Roles = "Administrator")]

        public ActionResult Show(int id)
        {
            Artist artist = db.Artists.Find(id);
            return View(artist);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult New(Artist art)
        {
            try
            {
                db.Artists.Add(art);
                db.SaveChanges();
                TempData["message"] = "Artistul a fost adaugat";
                return RedirectToAction("Index");
            }

            catch (Exception e)
            {
                return View(art);
            }
        }
        [Authorize(Roles = "Administrator")]

        public ActionResult Edit(int id)
        {
            Artist artist = db.Artists.Find(id);
            return View(artist);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, Artist requestArtist)
        {
            Artist artist = db.Artists.Find(id);

            try
            {

                artist.ArtistName = requestArtist.ArtistName;
                db.SaveChanges();
                TempData["message"] = "Artistul a fost adaugat cu succes!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(requestArtist);
            }
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
            db.SaveChanges();
            TempData["message"] = "Artistul a fost sters";
            return RedirectToAction("Index");
        }
    }
}
