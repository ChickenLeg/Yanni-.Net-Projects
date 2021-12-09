using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? id)
        {
            var studentMembership = new CommunityViewModel();
            studentMembership.Students = await _context.Students
                  .Include(i => i.CommunityMemberships)
                  .AsNoTracking()
                  .OrderBy(i => i.StudentID)
                  .ToListAsync();

            if (id != null)
            {
                ViewData["StudentID"] = id;
                studentMembership.Communities = (from c in _context.Communities
                                                 join m in _context.CommunityMemberships
                                                 on c.CommunityID equals m.CommunityID
                                                 where m.StudentID == id
                                                 select c);
            }
            return View(studentMembership);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpGet]
        public ActionResult EditMembership(int? id, string? communityId, int? operation)
        {
            var view = new CommunityMembershipViewModel();
            view.Membership = new List<StudentMembershipViewModel>();
            if (id != null && communityId != null)
            {
                if (operation == 1)
                {
                    var studentMembership = new CommunityMembership
                    {
                        StudentID = (int)id,
                        CommunityID = communityId
                    };
                    _context.CommunityMemberships.Remove(studentMembership);
                    _context.SaveChanges();
                }
                else if (operation == 0)
                {
                    var studentMembership = new CommunityMembership
                    {
                        StudentID = (int)id,
                        CommunityID = communityId
                    };
                    _context.CommunityMemberships.Add(studentMembership);
                    _context.SaveChanges();
                }
            }
            var communities = _context.Communities.ToList();
            var joinCommunity = (from c in _context.Communities
                                 join m in _context.CommunityMemberships
                                 on c.CommunityID equals m.CommunityID
                                 where m.StudentID == id
                                 select c).ToList();
            var student1 = (from s in _context.Students
                           where s.StudentID == id
                           select s).ToList().First();
            view.Student = student1;
            foreach (var community in communities)
            {
                view.Membership.Add(new StudentMembershipViewModel
                {
                    CommunityId = community.CommunityID,
                    Title = community.Title,
                    IsMember = false
                });
            }
            foreach (var f in view.Membership)
            {
                foreach (var m in joinCommunity)
                {
                    if (f.CommunityId == m.CommunityID)
                    {
                        f.IsMember = true;
                    }
                }
            }

            return View(view);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
