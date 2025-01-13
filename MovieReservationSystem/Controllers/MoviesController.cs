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
    public class MoviesController : Controller {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index() {
            var movies = await _context.Movies.OrderBy(m => m.Genre).ToListAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movies = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movies == null) {
                return NotFound();
            }

            return View(movies);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Plot,Genre,Runtime,Poster")] Movies movies) {
            if (ModelState.IsValid) {
                _context.Add(movies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movies);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movies = await _context.Movies.FindAsync(id);
            if (movies == null) {
                return NotFound();
            }
            return View(movies);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Plot,Genre,Runtime,Poster")] Movies movies) {
            if (id != movies.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(movies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!MoviesExists(movies.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movies);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movies = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movies == null) {
                return NotFound();
            }

            return View(movies);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var movies = await _context.Movies.FindAsync(id);
            if (movies != null) {
                _context.Movies.Remove(movies);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoviesExists(int id) {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
