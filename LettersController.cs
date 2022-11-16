using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicLesson.Models;
using Newtonsoft.Json;
using SelectPdf;

namespace MusicLesson.Controllers
{
    public class LettersController : Controller
    {
        private readonly MusicLessonsDBContext _context;

        public LettersController(MusicLessonsDBContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
              return View(await _context.Letters.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Letters == null)
            {
                return NotFound();
            }

            var letters = await _context.Letters
                .FirstOrDefaultAsync(m => m.LetterID == id);
            letters.student = await _context.Students.FirstOrDefaultAsync(a => a.StudentID == letters.StudentID);
            if (letters == null)
            {
                return NotFound();
            }

            return View(letters);
        }
        
        public async Task<IActionResult> DetailDataAsync([FromForm] List<Lessons> lessons)
        {
            
            foreach(var item in lessons)
            {
                item.Duration = _context.Duration.SingleOrDefault(l => l.DurationID == item.DurationID);
                item.Instrument = _context.Instrument.SingleOrDefault(l => l.InstrumentID == item.InstrumentID);
                if(item.LetterID > 0)
                item.Letter = _context.Letters.SingleOrDefault(l => l.LetterID == item.LetterID);
                item.Student = _context.Students.SingleOrDefault(l => l.StudentID == item.StudentID);
                item.Tutor = _context.Tutors.SingleOrDefault(l => l.TutorID == item.TutorID);
               
            }
            TempData["LessonDetails"] = JsonConvert.SerializeObject(lessons);
            return RedirectToAction("Create");
        }
        // GET: Letters/Create
        public IActionResult Create()
        {
            var TermList = new List<int>();
            TermList.Add(1);
            TermList.Add(2);
            TermList.Add(3);
            TermList.Add(4);
            ViewData["Term"] = TermList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.ToString(),
                    Value = a.ToString(),
                    Selected = false
                };
            });

            var SemesterList = new List<int>();
            TermList.Add(1);
            TermList.Add(2);

            ViewData["SemesterList"] = SemesterList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.ToString(),
                    Value = a.ToString(),
                    Selected = false
                };
            });
            return View();
        }
        [HttpGet]
        public IActionResult LetterDemo([Bind("LetterID,Reference,PaymentStatus,BeginningComment,Signature,BankAccount,BSB,AccountNo,TotalCost,Term,Semester,Year,StudentID")] Letters letter)
        {
            //var LessonList = JsonConvert.DeserializeObject<List<Lessons>>(TempData["LessonDetails"].ToString());
            letter.student = _context.Students.Find(letter.StudentID);
            //var data = JsonConvert.DeserializeObject(TempData["LessonDetails"]);
            return View(letter);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("LetterID,Reference,PaymentStatus,BeginningComment,Signature,BankAccount,BSB,AccountNo,TotalCost,Term,Semester,Year,StudentID")] Letters letters)
        {
            letters.Reference = "default";
            var lessons = await _context.Lessons.Where(a => a.StudentID == letters.StudentID).ToListAsync();
            _context.Add(letters);
            await _context.SaveChangesAsync();
            var student = _context.Students.Find(letters.StudentID);
            letters.Reference = letters.Year + student.LastName + letters.LetterID;
            _context.Update(letters);
            foreach (var item in lessons)
            {
                item.LetterID = letters.LetterID;
                _context.Update(item);
            }
            await _context.SaveChangesAsync();
            letters.TermStartDate = (await _context.Lessons.FirstOrDefaultAsync(a => a.LessonID == lessons[0].LessonID && a.StudentID == lessons[0].StudentID)).TermStartDate;

            return RedirectToAction("LetterDemo", letters);
        }

        // GET: Letters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Letters == null)
            {
                return NotFound();
            }

            var letters = await _context.Letters.FindAsync(id);
            if (letters == null)
            {
                return NotFound();
            }
            return View(letters);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LetterID,Reference,PaymentStatus,BeginningComment,Signature,BankAccount,BSB,AccountNo,TotalCost,Term,Semester,Year,StudentID")] Letters letters)
        {
            if (id != letters.LetterID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(letters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LettersExists(letters.LetterID))
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
            return View(letters);
        }

        // GET: Letters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Letters == null)
            {
                return NotFound();
            }

            var letters = await _context.Letters
                .FirstOrDefaultAsync(m => m.LetterID == id);
            if (letters == null)
            {
                return NotFound();
            }

            return View(letters);
        }

        // POST: Letters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Letters == null)
            {
                return Problem("Entity set 'MusicLessonsDBContext.Letters'  is null.");
            }
            var letters = await _context.Letters.FindAsync(id);
            if (letters != null)
            {
                _context.Letters.Remove(letters);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LettersExists(int id)
        {
          return _context.Letters.Any(e => e.LetterID == id);
        }
    }
}
