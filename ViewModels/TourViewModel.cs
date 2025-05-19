using System.ComponentModel.DataAnnotations;

namespace TravelWebsite.ViewModels
{
    public class TourViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên tour")]
        [StringLength(100, ErrorMessage = "Tên tour không được vượt quá 100 ký tự")]
        [Display(Name = "Tên tour")]
        public string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá tour")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tour phải là số dương")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Price { get; set; }
        
        [Display(Name = "Thời gian (ngày)")]
        public int? Duration { get; set; }
        
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }
        
        [Display(Name = "Phổ biến")]
        public bool IsPopular { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;
        
        [Required(ErrorMessage = "Vui lòng chọn điểm đến")]
        [Display(Name = "Điểm đến")]
        public int DestinationId { get; set; }
        
        [Display(Name = "Tên điểm đến")]
        public string? DestinationName { get; set; }
    }
} 