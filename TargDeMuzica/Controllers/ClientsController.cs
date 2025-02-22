using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TargDeMuzica.Data;
using TargDeMuzica.Models;

namespace TargDeMuzica.Controllers
{
    [Authorize(Roles = "Administrator,Colaborator")]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClientsController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            
            var users = await _userManager.Users.ToListAsync();
            var clientsList = new List<Clients>();
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            foreach (var user in users)
            {
                
                var userRoles = await _userManager.GetRolesAsync(user);
                var currentRole = userRoles.FirstOrDefault() ?? "No Role";

                clientsList.Add(new Clients
                {
                    ClientId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    CurrentRole = currentRole,
                    AllAvailableRoles = allRoles
                });
            }

            return View(clientsList);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string newRole)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Utilizatorul nu a fost găsit.";
                    return RedirectToAction(nameof(Index));
                }

               
                var currentRoles = await _userManager.GetRolesAsync(user);

                
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                var result = await _userManager.AddToRoleAsync(user, newRole);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Rolul utilizatorului a fost actualizat cu succes.";
                }
                else
                {
                    TempData["Error"] = "Eroare la actualizarea rolului: " +
                        string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "A apărut o eroare la actualizarea rolului: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}