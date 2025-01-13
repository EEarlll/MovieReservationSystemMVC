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
    public class ShowtimesController : Controller {
        private readonly ApplicationDbContext _context;

        public ShowtimesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Showtime.Include(s => s.Movie);


            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Showtimes/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var showtime = await _context.Showtime
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtime == null) {
                return NotFound();
            }

            return RedirectToAction("Details", "Movies", new { id = showtime.MovieId });
        }

        // GET: Showtimes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,MovieId,SeatCount")] Showtime showtime) {
            if (showtime.Movie == null) {
                showtime.Movie = await _context.Movies.FindAsync(showtime.MovieId);
                if (showtime.Movie == null) {
                    ModelState.AddModelError("MovieId", "Invalid Movie ID");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showtime.MovieId);
                    return View(showtime);
                }
            }

            showtime.EndTime = showtime.StartTime.AddMinutes(showtime.Movie.Runtime);

            if (ModelState.IsValid) {
                _context.Add(showtime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showtime.MovieId);
            return View(showtime);
        }

        // GET: Showtimes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var showtime = await _context.Showtime.FindAsync(id);
            if (showtime == null) {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", showtime.MovieId);
            return View(showtime);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,MovieId,SeatCount")] Showtime showtime) {
            if (id != showtime.Id) {
                return NotFound();
            }

            if (showtime.Movie == null) {
                showtime.Movie = await _context.Movies.FindAsync(showtime.MovieId);
                if (showtime.Movie == null) {
                    ModelState.AddModelError("MovieId", "Invalid Movie ID");
                    ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showtime.MovieId);
                    return View(showtime);
                }
            }

            showtime.EndTime = showtime.StartTime.AddMinutes(showtime.Movie!.Runtime);

            if (ModelState.IsValid) {
                try {
                    _context.Update(showtime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ShowtimeExists(showtime.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", showtime.MovieId);
            return View(showtime);
        }

        // GET: Showtimes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var showtime = await _context.Showtime
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showtime == null) {
                return NotFound();
            }

            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var showtime = await _context.Showtime.FindAsync(id);
            if (showtime != null) {
                _context.Showtime.Remove(showtime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Reserve(int? id) {
            var showtime = await _context.Showtime.FindAsync(id);
            if (showtime == null || id == null) {
                return NotFound();
            }
            return RedirectToAction("Create", "Seats", new { id });
        }

        private bool ShowtimeExists(int id) {
            return _context.Showtime.Any(e => e.Id == id);
        }
    }
}
