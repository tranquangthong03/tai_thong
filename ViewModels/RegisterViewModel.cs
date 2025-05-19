using System.ComponentModel.DataAnnotations;

namespace TravelWebsite.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        [StringLength(100, ErrorMessage = "Họ không được vượt quá 100 ký tự")]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
    }
} 