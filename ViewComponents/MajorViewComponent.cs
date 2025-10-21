using Microsoft.AspNetCore.Mvc;
using ThucHanh1.Data;
using ThucHanh1.Models;
using ThucHanh1.Models.Entities;

namespace ThucHanh1.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        SchoolContext db;
        List<Major> majors;
        public MajorViewComponent(SchoolContext _context)
        {
            db = _context;
            majors = db.Majors.ToList();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
