using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Data;
using MovieReservationSystem.Models;

namespace MovieReservationSystem.Controllers {
    public class SeatsController : Controller {
        private readonly ApplicationDbContext _context;

        public SeatsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Seats
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Seat.Include(s => s.Showtime)
                .ThenInclude(showtime => showtime!.Movie)
                .Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Showtime)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null) {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create/id
        public IActionResult Create(int? id) {
            if (id == null) {
                return NotFound();
            }

            ViewData["ShowtimeId"] = id;

            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) {
                return Unauthorized();
            }

            ViewData["UserId"] = userId;

            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatNumber,SeatType,ShowtimeId,UserId")] Seat seat) {
            if (ModelState.IsValid) {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seat);
        }

        // GET: Seats/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var seat = await _context.Seat.FindAsync(id);
            if (seat == null) {
                return NotFound();
            }
            ViewData["ShowtimeId"] = new SelectList(_context.Showtime, "Id", "Id", seat.ShowtimeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", seat.UserId);
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeatNumber,SeatType,ShowtimeId,UserId")] Seat seat) {
            if (id != seat.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!SeatExists(seat.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowtimeId"] = new SelectList(_context.Showtime, "Id", "Id", seat.ShowtimeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", seat.UserId);
            return View(seat);
        }

        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Showtime)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null) {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var seat = await _context.Seat.FindAsync(id);
            if (seat != null) {
                _context.Seat.Remove(seat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id) {
            return _context.Seat.Any(e => e.Id == id);
        }
    }
}
