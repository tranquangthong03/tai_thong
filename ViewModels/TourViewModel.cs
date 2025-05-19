using System.ComponentModel.DataAnnotations;
using TravelWebsite.Models;

namespace TravelWebsite.ViewModels
{
    public class TourViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên tour")]
        [StringLength(100, ErrorMessage = "Tên tour không được vượt quá 100 ký tự")]
        [Display(Name = "Tên Tour")]
        public string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá tour")]
        [Range(0, 9999999999, ErrorMessage = "Giá tour phải lớn hơn 0")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Price { get; set; }
        
        [Display(Name = "Thời gian (ngày)")]
        [Range(1, 100, ErrorMessage = "Thời gian tour phải từ 1-100 ngày")]
        public int? Duration { get; set; }
        
        [Display(Name = "URL hình ảnh")]
        [StringLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn điểm đến")]
        [Display(Name = "Điểm đến")]
        public int DestinationId { get; set; }
        
        [Display(Name = "Tour phổ biến")]
        public bool IsPopular { get; set; }
        
        [Display(Name = "Trạng thái hoạt động")]
        public bool IsActive { get; set; } = true;
        
        // Collection để hiển thị trong dropdown
        public IEnumerable<Destination>? Destinations { get; set; }
    }
} 