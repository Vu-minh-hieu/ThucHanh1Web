using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ThucHanh1.Data;
using ThucHanh1.Models;
using ThucHanh1.Models.Entities;

namespace ThucHanh1.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        // Khai báo biến toàn cục pageSize
        private int pageSize = 3;
        public IActionResult Index(int? mid)
        {
            var learners = (IQueryable<Learner>)db.Learners
                    .Include(m => m.Major);

            if (mid != null)
            {
                learners = (IQueryable<Learner>)db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major);
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling((float)learners.Count() / pageSize);

            // Trả số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Lấy dữ liệu trang đầu
            var result = learners.Take(pageSize).ToList();

            return View(result);
        }
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            // Lấy toàn bộ learners trong dbset chuyển về IQueryable<Learner> để query
            var learners = (IQueryable<Learner>)db.Learners;

            // Lấy chỉ số trang, nếu chỉ số trang null thì gán ngầm định bằng 1
            int page = (pageIndex == null || pageIndex <= 0) ? 1 : pageIndex.Value;

            // Nếu có mã chuyên ngành thì lọc learner theo mid
            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
                // Gửi mid về view để ghi lại trên nav-phân trang
                ViewBag.mid = mid;
            }

            // Nếu có keyword thì tìm kiếm theo tên
            if (!string.IsNullOrEmpty(keyword))
            {
                learners = learners.Where(l => l.FirstMidName.ToLower()
                    .Contains(keyword.ToLower()));
                // Gửi keyword về view để ghi lại trên nav-phân trang
                ViewBag.keyword = keyword;
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling((float)learners.Count() / pageSize);
            // Gửi số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Lấy dữ liệu trong trang hiện tại
            var result = learners.Skip(pageSize * (page - 1)).Take(pageSize)
                .Include(l => l.Major).ToList();

            return PartialView("LearnerTable", result);
        }

        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners.Where(l => l.MajorID == mid)
                    .Include(m => m.Major).ToList();
            return PartialView("LearnerTable", learners);
        }

        // Thêm 2 action Create
        public IActionResult Create()
        {
            // Dùng 1 trong 2 cách để tạo SelectList gửi về View qua ViewBag
            // hiển thị danh sách chuyên ngành (Majors)

            var majors = new List<SelectListItem>();  // Cách 1
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem
                {
                    Text = item.MajorName,
                    Value = item.MajorID.ToString()
                });
            }
            ViewBag.MajorID = majors;

            // Cách 2
            //ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Lại dùng 1 trong 2 cách tạo SelectList gửi về View để hiển thị danh sách Majors
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        //thêm 2 action edit
        public IActionResult Edit(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
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
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }

        //thêm 2 action delete
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners
                .Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);

            if (learner == null)
            {
                return NotFound();
            }

            if (learner.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }

            return View(learner);
        }

        // POST: Learner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null.");
            }

            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
