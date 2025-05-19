using System.ComponentModel.DataAnnotations;
using TravelWebsite.Models;

namespace TravelWebsite.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn tour")]
        [Display(Name = "Tour")]
        public int TourId { get; set; }
        
        [Display(Name = "Tên tour")]
        public string? TourName { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn ngày khởi hành")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày khởi hành")]
        public DateTime TravelDate { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số người tham gia")]
        [Range(1, 100, ErrorMessage = "Số người tham gia phải từ 1 đến 100")]
        [Display(Name = "Số người")]
        public int NumberOfPeople { get; set; }
        
        [Display(Name = "Tổng tiền (VNĐ)")]
        public decimal TotalPrice { get; set; }
        
        [StringLength(500, ErrorMessage = "Yêu cầu đặc biệt không được vượt quá 500 ký tự")]
        [Display(Name = "Yêu cầu đặc biệt")]
        public string? SpecialRequests { get; set; }
        
        [Display(Name = "Trạng thái")]
        public BookingStatus Status { get; set; }
        
        [Display(Name = "Ngày đặt")]
        public DateTime BookingDate { get; set; }
    }
} 