using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicLesson.Models;

namespace MusicLesson.Controllers
{
    public class DurationsController : Controller
    {
        private readonly MusicLessonsDBContext _context;

        public DurationsController(MusicLessonsDBContext context)
        {
            _context = context;
        }

        // GET: Durations
        public async Task<IActionResult> Index()
        {
              return View(await _context.Duration.ToListAsync());
        }

        // GET: Durations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Duration == null)
            {
                return NotFound();
            }

            var duration = await _context.Duration
                .FirstOrDefaultAsync(m => m.DurationID == id);
            if (duration == null)
            {
                return NotFound();
            }

            return View(duration);
        }

        // GET: Durations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Durations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DurationID,Length,Cost")] Duration duration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(duration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(duration);
        }

        // GET: Durations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Duration == null)
            {
                return NotFound();
            }

            var duration = await _context.Duration.FindAsync(id);
            if (duration == null)
            {
                return NotFound();
            }
            return View(duration);
        }

        // POST: Durations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DurationID,Length,Cost")] Duration duration)
        {
            if (id != duration.DurationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DurationExists(duration.DurationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(duration);
        }

        // GET: Durations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Duration == null)
            {
                return NotFound();
            }

            var duration = await _context.Duration
                .FirstOrDefaultAsync(m => m.DurationID == id);
            if (duration == null)
            {
                return NotFound();
            }

            return View(duration);
        }

        // POST: Durations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Duration == null)
            {
                return Problem("Entity set 'MusicLessonsDBContext.Duration'  is null.");
            }
            var duration = await _context.Duration.FindAsync(id);
            if (duration != null)
            {
                _context.Duration.Remove(duration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DurationExists(int id)
        {
          return _context.Duration.Any(e => e.DurationID == id);
        }
    }
}
