using System.Collections.Generic;
using TravelWebsite.Models;

namespace TravelWebsite.ViewModels
{
    public class TourIndexViewModel
    {
        // Danh sách tour du lịch
        public List<Tour> Tours { get; set; }
        
        // Thông tin phân trang
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        
        // Thông tin lọc và tìm kiếm
        public string SearchName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public int? DestinationId { get; set; }
        public bool? IsPopular { get; set; }
        public string SortOrder { get; set; }
        
        // Các thuộc tính phân trang bổ sung
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
} 