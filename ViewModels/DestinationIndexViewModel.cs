using System.Collections.Generic;
using TravelWebsite.Models;

namespace TravelWebsite.ViewModels
{
    public class DestinationIndexViewModel
    {
        // Danh sách điểm đến
        public List<Destination> Destinations { get; set; }
        
        // Thông tin phân trang
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        
        // Thông tin lọc
        public string SearchString { get; set; }
        public string Country { get; set; }
        public decimal? MinRating { get; set; }
        public bool? IsPopular { get; set; }
        public string SortOrder { get; set; }
        
        // Các thuộc tính phân trang bổ sung
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
} 