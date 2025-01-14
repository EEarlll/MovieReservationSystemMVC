using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Data;

namespace MovieReservationSystem.Controllers {
    public class RevenueController : Controller {
        private readonly ApplicationDbContext _context;

        public RevenueController(ApplicationDbContext context) {
            _context = context;
        }   
        public IActionResult Index() {
            return View();
        }
    }
}
