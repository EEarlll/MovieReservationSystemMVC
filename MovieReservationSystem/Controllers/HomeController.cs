using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Data;
using MovieReservationSystem.Models;
using System.Diagnostics;

namespace MovieReservationSystem.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() {
            var role = User.Claims
                .FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role)
                ?.Value;

            ViewBag.UserRole = role;
            ViewBag.UserName = User.Identity?.Name;

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
