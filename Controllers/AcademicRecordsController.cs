using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAndEmployeeAppsMVC.Models.DataAccess;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StudentAndEmployeeAppsMVC.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentRecordContext _context;

        public AcademicRecordsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public async Task<IActionResult> Index()
        {
            var academicRecord = await _context.AcademicRecords.Include(a=>a.CourseCodeNavigation).Include(a=>a.Student).ToListAsync();
            return View(academicRecord);

            //var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            //return View(await studentRecordContext.ToListAsync());
        }

        // GET: AcademicRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AcademicRecords == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "codeWithName");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "NameWithId");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AcademicRecord academicRecord)
        {
            var dataInsideErrorMsg = _context.AcademicRecords.Include(a => a.Student).Include(a=>a.CourseCodeNavigation).SingleOrDefault(a => a.StudentId == academicRecord.StudentId);
            if (_context.AcademicRecords.Any(a => a.CourseCode == academicRecord.CourseCode && a.StudentId == academicRecord.StudentId))
            {
                ModelState.AddModelError("StudentId", $"This record is exists with {dataInsideErrorMsg.Student.NameWithId} for course of {dataInsideErrorMsg.CourseCodeNavigation.codeWithName}.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(academicRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "codeWithName", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "NameWithId", academicRecord.StudentId);
            return View(academicRecord);
        }
        // GET: AcademicRecords/EditAll/5
        public async Task<IActionResult> EditAll()
        {
            var academicReocrds = await _context.AcademicRecords.Include(a=>a.Student).Include(a=>a.CourseCodeNavigation).ToListAsync();
            return View(academicReocrds);

            //var studentContext =_context.AcademicRecords.Include(s => s.CourseCodeNavigation).Include(s => s.Student); // need this two eneity for the EditAll page. 
            //return View(await studentContext.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll([Bind("CourseCode,StudentId,Grade")] IList<AcademicRecord> academicRecord) // when using Bind[], remember to set the type same as the model in view page.
        {
            if (_context.AcademicRecords ==null || academicRecord == null) 
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                try
                {
                    _context.AcademicRecords.UpdateRange(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));  // to Index page
            }

            var studentContext = _context.AcademicRecords.Include(s => s.CourseCodeNavigation).Include(s => s.Student);
            return View(await studentContext.ToListAsync());
        }
            // GET: AcademicRecords/Edit/5
            public async Task<IActionResult> Edit(string id, string code)
        {
            if (id == null || code ==null || _context.AcademicRecords == null)
            {
                return NotFound();
            }
            var academicRecord = await _context.AcademicRecords.Include(a=>a.CourseCodeNavigation).Include(a=>a.Student)
                .FirstOrDefaultAsync(a=>a.StudentId==id && a.CourseCode==code); // return the first element that matches code and id
            //var academicRecord = await _context.AcademicRecords.FindAsync(id); not working because not specific code and id
            if (academicRecord == null)
            {
                return NotFound();
            }
            //ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (id != academicRecord.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(academicRecord.StudentId))
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
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AcademicRecords == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("CourseCode", "StudentId")] AcademicRecord record) // view delete page, asp-for
        {

            if (_context.AcademicRecords == null)
            {
                return Problem("Entity set 'StudentRecordContext.AcademicRecords'  is null.");
            }
            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == record.StudentId && m.CourseCode == record.CourseCode); // two entiries need to be deleted.
            if (academicRecord != null)
            {
                _context.AcademicRecords.Remove(academicRecord);
            }
            //var academicRecord = await _context.AcademicRecords.FindAsync(id);
            //if (academicRecord != null)
            //{
            //    _context.AcademicRecords.Remove(academicRecord);
            //}
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool AcademicRecordExists(string id)
        {
          return _context.AcademicRecords.Any(e => e.StudentId == id);
        }
    }
}
