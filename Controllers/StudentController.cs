using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThucHanh1.Models;

namespace ThucHanh1.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> listStudents = new List<Student>();

        static StudentController()
        {
            // Tạo danh sách sinh viên với 4 dữ liệu mẫu
            listStudents = new List<Student>()
            {
                new Student()
                {
                    Id = 101,
                    Name = "Hải Nam",
                    Branch = Branch.IT,
                    Gender = Gender.Male,
                    IsRegular = true,
                    Address = "A1-2018",
                    Email = "nam@g.com",
                    Score = 5.2
                },
                new Student()
                {
                    Id = 102,
                    Name = "Minh Tú",
                    Branch = Branch.BE,
                    Gender = Gender.Female,
                    IsRegular = true,
                    Address = "A1-2019",
                    Email = "tu@g.com",
                    Score = 9.0
                },
                new Student()
                {
                    Id = 103,
                    Name = "Hoàng Phong",
                    Branch = Branch.CE,
                    Gender = Gender.Male,
                    IsRegular = false,
                    Address = "A1-2020",
                    Email = "phong@g.com",
                    Score = 10.0
                },
                new Student()
                {
                    Id = 104,
                    Name = "Xuân Mai",
                    Branch = Branch.EE,
                    Gender = Gender.Female,
                    IsRegular = false,
                    Address = "A1-2021",
                    Email = "mai@g.com",
                    Score = 2.3
                }
            };
        }
        [Route("Admin/Student/List")]
        public IActionResult Index()
        {
            // Trả về View Index.cshtml cùng Model là danh sách sv listStudents
            return View(listStudents);
        }

        [Route("Admin/Student/Add", Name = "StudentAddGet")]
        [HttpGet]
        public IActionResult Create()
        {

            // Lấy danh sách các giá trị Gender để hiển thị radio button trên form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            // Lấy danh sách các giá trị Branch để hiển thị select-option trên form
            // Để hiển thị select-option trên View cần dùng List<SelectListItem>
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "0" },
                new SelectListItem { Text = "BE", Value = "1" },
                new SelectListItem { Text = "CE", Value = "2" },
                new SelectListItem { Text = "EE", Value = "3" }
            };

            return View();
        }

        [Route("Admin/Student/Add", Name = "StudentAddPost")]
        [HttpPost]
        public IActionResult Create(Student s, IFormFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    // Lấy tên file
                    var fileName = Path.GetFileName(AvatarFile.FileName);

                    // Đường dẫn lưu trong wwwroot/images
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }

                    // Lưu tên file vào Student
                    s.Avatar = fileName;
                }

                // Tạo Id mới tăng dần
                s.Id = listStudents.Last<Student>().Id + 1;

                // Thêm sinh viên mới vào danh sách
                listStudents.Add(s);

                // Quay lại Index để hiển thị danh sách
                return View("Index", listStudents);
            }

            // Nếu không hợp lệ thì nạp lại ViewBag để render lại form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "0" },
                new SelectListItem { Text = "BE", Value = "1" },
                new SelectListItem { Text = "CE", Value = "2" },
                new SelectListItem { Text = "EE", Value = "3" }
            };

            return View(s); // Trả lại view cùng dữ liệu người nhập
        }

    }
}
