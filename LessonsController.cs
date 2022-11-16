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
    public class LessonsController : Controller
    {
        private readonly MusicLessonsDBContext _context;

        public LessonsController(MusicLessonsDBContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var musicLessonsDBContext = _context.Lessons.Include(l => l.Duration).Include(l => l.Instrument).Include(l => l.Letter).Include(l => l.Student).Include(l => l.Tutor);
            return View(await musicLessonsDBContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons
                .Include(l => l.Duration)
                .Include(l => l.Instrument)
                .Include(l => l.Letter)
                .Include(l => l.Student)
                .Include(l => l.Tutor)
                .FirstOrDefaultAsync(m => m.LessonID == id);
            if (lessons == null)
            {
                return NotFound();
            }

            return View(lessons);
        }
        public async Task<IActionResult> DetailsByStudentID(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons
                .Include(l => l.Student)
                .Include(l => l.Duration)
                .Include(l => l.Instrument)
                .Include(l => l.Letter)
                .Include(l => l.Student)
                .Include(l => l.Tutor)
                .Where(m => m.StudentID == id).ToListAsync();
            if (lessons == null)
            {
                return NotFound();
            }

            return View(lessons);
        }
        // GET: Lessons/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["DurationID"] = new SelectList(_context.Duration, "DurationID", "Length","Select Duration");
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentName", "Select Instrument");
            ViewData["LetterID"] = new SelectList(_context.Letters, "LetterID", "AccountNo" , "Select Letter");
            var data = new SelectList(_context.Students, "StudentID", "FirstName");
            foreach (var item in data)
            {
                item.Text += " " + (await _context.Students.SingleOrDefaultAsync(a => a.StudentID.ToString() == item.Value)).LastName;
            }
            ViewData["StudentID"] = data;
            ViewData["TutorID"] = new SelectList(_context.Tutors, "TutorID", "TutorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonID,StudentID,InstrumentID,TutorID,DurationID,LessonDateTime,LetterID,Term,Semester,Year,TermStartDate")] Lessons lessons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationID"] = new SelectList(_context.Duration, "DurationID", "Length", lessons.DurationID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentName", lessons.InstrumentID);
            ViewData["LetterID"] = new SelectList(_context.Letters, "LetterID", "AccountNo", lessons.LetterID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "Email", lessons.StudentID);
            ViewData["TutorID"] = new SelectList(_context.Tutors, "TutorID", "TutorName", lessons.TutorID);
            return View(lessons);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons == null)
            {
                return NotFound();
            }
            ViewData["DurationID"] = new SelectList(_context.Duration, "DurationID", "Length", lessons.DurationID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentName", lessons.InstrumentID);
            ViewData["LetterID"] = new SelectList(_context.Letters, "LetterID", "AccountNo", lessons.LetterID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "Email", lessons.StudentID);
            ViewData["TutorID"] = new SelectList(_context.Tutors, "TutorID", "TutorName", lessons.TutorID);
            return View(lessons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonID,StudentID,InstrumentID,TutorID,DurationID,LessonDateTime,LetterID,Term,Semester,Year,TermStartDate")] Lessons lessons)
        {
            if (id != lessons.LessonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonsExists(lessons.LessonID))
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
            ViewData["DurationID"] = new SelectList(_context.Duration, "DurationID", "Length", lessons.DurationID);
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "InstrumentID", "InstrumentName", lessons.InstrumentID);
            ViewData["LetterID"] = new SelectList(_context.Letters, "LetterID", "AccountNo", lessons.LetterID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "Email", lessons.StudentID);
            ViewData["TutorID"] = new SelectList(_context.Tutors, "TutorID", "TutorName", lessons.TutorID);
            return View(lessons);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lessons = await _context.Lessons
                .Include(l => l.Duration)
                .Include(l => l.Instrument)
                .Include(l => l.Letter)
                .Include(l => l.Student)
                .Include(l => l.Tutor)
                .FirstOrDefaultAsync(m => m.LessonID == id);
            if (lessons == null)
            {
                return NotFound();
            }

            return View(lessons);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'MusicLessonsDBContext.Lessons'  is null.");
            }
            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons != null)
            {
                _context.Lessons.Remove(lessons);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonsExists(int id)
        {
          return _context.Lessons.Any(e => e.LessonID == id);
        }
    }
}
