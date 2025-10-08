using System.ComponentModel.DataAnnotations;

namespace ThucHanh1.Models
{
    public class Student
    {
        public int Id { get; set; } // Mã sinh viên

        [Required(ErrorMessage = "Họ tên bắt buộc phải được nhập")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Tên phải từ 4 đến 100 ký tự")]
        public string? Name { get; set; } // Họ tên

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        //    ErrorMessage = "Email không hợp lệ")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$",
            ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; } // Email

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [Required(ErrorMessage = "Mật khẩu bắt buộc phải được nhập")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt")]
        public string? Password { get; set; } // Mật khẩu

        [Required(ErrorMessage = "Bắt buộc phải chọn ngành học")]
        public Branch? Branch { get; set; } // Ngành học

        [Required(ErrorMessage = "Bắt buộc phải chọn giới tính")]
        public Gender? Gender { get; set; } // Giới tính

        public bool IsRegular { get; set; } // Hệ: true–chính quy, false–phi CQ

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Địa chỉ bắt buộc phải được nhập")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Địa chỉ phải nhập từ 0 - 100 ký tự")]

        public string? Address { get; set; } // Địa chỉ

        [Range(typeof(DateTime), "1/1/1963", "31/12/2005",
            ErrorMessage = "Ngày sinh phải trong khoảng 1963 - 2005")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh bắt buộc phải được nhập")]
        public DateTime DateOfBirth { get; set; } // Ngày sinh
        //[Required(ErrorMessage = "Cần có ảnh đại diện")]
        public string? Avatar { get; set; } // Ảnh đại diện (đường dẫn)

        [Required(ErrorMessage = "Điểm bắt buộc phải được nhập")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải là số thực và miền giá trị từ 0.0 đến 10.0")]
        public double? Score { get; set; } //Điểm
    }
}
