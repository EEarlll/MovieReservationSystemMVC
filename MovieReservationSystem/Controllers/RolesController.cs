using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Models;

namespace MovieReservationSystem.Controllers {


    public class RolesController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index() {

            var users = _userManager.Users.ToList();
            var model = new List<ViewRole>();

            foreach (var user in users) {
                var userRole = await _userManager.GetRolesAsync(user);

                if (userRole.Contains("User")) {
                    model.Add(new ViewRole {
                        UserId = user.Id,
                        Username = user.UserName,
                        Role = userRole.FirstOrDefault()
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PromoteToAdmin(string id) {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }
            var currentRole = await _userManager.GetRolesAsync(user);
            if (currentRole.Count == 0) {
                await _userManager.AddToRoleAsync(user, "Admin");
                return View();
            }
            else {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRole);
                if (!removeResult.Succeeded) {
                    ModelState.AddModelError("", "Failed to remove current roles.");
                    return View(); // Return to the same view with an error.
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!addResult.Succeeded) {
                ModelState.AddModelError("", "Failed to add new role");
                return View();
            }

            return RedirectToAction("Index");
        }

    }
}
