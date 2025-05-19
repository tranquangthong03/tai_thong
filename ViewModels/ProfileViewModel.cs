using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TravelWebsite.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        
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
        
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        
        [Display(Name = "Ảnh đại diện")]
        public IFormFile ProfilePicture { get; set; }
        
        public byte[] CurrentProfilePicture { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
    }
} 