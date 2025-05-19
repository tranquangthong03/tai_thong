using System.ComponentModel.DataAnnotations;

namespace TravelWebsite.ViewModels
{
    public class DestinationViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên điểm đến")]
        [StringLength(100, ErrorMessage = "Tên điểm đến không được vượt quá 100 ký tự")]
        [Display(Name = "Tên điểm đến")]
        public string Name { get; set; }
        
        [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập quốc gia")]
        [StringLength(100, ErrorMessage = "Tên quốc gia không được vượt quá 100 ký tự")]
        [Display(Name = "Quốc gia")]
        public string Country { get; set; }
        
        [StringLength(100, ErrorMessage = "Tên thành phố không được vượt quá 100 ký tự")]
        [Display(Name = "Thành phố")]
        public string? City { get; set; }
        
        [Display(Name = "Xếp hạng")]
        public decimal Rating { get; set; }
        
        [Display(Name = "Phổ biến")]
        public bool IsPopular { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;
    }
} 