using Microsoft.AspNetCore.Mvc;
using ThucHanh1.Models;

namespace ThucHanh1.Controllers
{
    [Route("Branch/List")]
    public class BranchController : Controller
    {
        private static List<Branch> listStudents = new List<Branch>();
        public IActionResult Index()
        {
            return View();
        }
    }
}
