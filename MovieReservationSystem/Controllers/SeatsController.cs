using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index() {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) {
                return Unauthorized();
            }

            IQueryable<Seat> applicationDbContext = _context.Seat
                .Include(s => s.Showtime)
                .ThenInclude(showtime => showtime!.Movie)
                .Include(s => s.User)
                .OrderBy(s => s.Showtime.Movie.Title);

            if (User!.IsInRole("User")) {
                applicationDbContext = applicationDbContext.Where(s => s.UserId == userId);
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Seats/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create(int? id) {
            if (id == null) {
                return NotFound();
            }

            ViewData["ShowtimeId"] = id;

            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) {
                return Unauthorized();
            }

            var showtime = await _context.Showtime
                            .FirstOrDefaultAsync(st => st.Id == id);

            if (showtime == null) {
                return NotFound();
            }

            var seats = await _context.Seat
                .Where(s => s.ShowtimeId == id)
                .ToListAsync();

            ViewData["UserId"] = userId;

            ViewBag.SeatCount = showtime.SeatCount;

            return View(seats);
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create([Bind("SeatNumber,ShowtimeId,UserId")] Seat seat) {
            var showtime = await _context.Showtime.FindAsync(seat.ShowtimeId);
            if (showtime == null) {
                return NotFound();
            }
            if (seat.SeatNumber <= showtime.SeatCount / 3)  // Front seats (Value)
            {
                seat.SeatType = "Value";
            }
            else if (seat.SeatNumber <= (2 * showtime.SeatCount) / 3)  // Middle seats (Standard)
            {
                seat.SeatType = "Standard";
            }
            else  // Back seats (Premium)
            {
                seat.SeatType = "Premium";
            }

            if (ModelState.IsValid) {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Seats/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
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
