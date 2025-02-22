using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TargDeMuzica.Data;
using TargDeMuzica.Models;
using static TargDeMuzica.Models.IncomingRequest;

namespace TargDeMuzica.Controllers
{
    public class IncomingRequestsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncomingRequestsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            var pendingRequests = _db.IncomingRequests
                .Include(r => r.ProposedProduct)
                .Include(r => r.User)
                .Where(r => r.Status == RequestStatus.Pending)
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            return View(pendingRequests);
        }

       
        [Authorize(Roles = "Colaborator")]
        [HttpPost]
        public async Task<IActionResult> Submit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var user = await _userManager.GetUserAsync(User);

            var request = new IncomingRequest
            {
                RequestDate = DateTime.Now,
                Status = RequestStatus.Pending,
                ProposedProduct = product,
                User = user
            };

            _db.IncomingRequests.Add(request);
            await _db.SaveChangesAsync();

            TempData["message"] = "Your product has been submitted for review";
            return RedirectToAction("Index", "Products");
        }

       
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Review(int requestId, bool approved, string? comment)
        {
            var request = await _db.IncomingRequests
                .Include(r => r.ProposedProduct)
                .FirstOrDefaultAsync(r => r.RequestID == requestId);

            if (request == null)
                return NotFound();

            request.Status = approved ? RequestStatus.Approved : RequestStatus.Rejected;
            request.AdminComment = comment;


            await _db.SaveChangesAsync();

            TempData["message"] = approved ? "Product approved and published" : "Product request rejected";
            return RedirectToAction("Index");
        }
    }
}

