using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWebsite.Models
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public class Booking
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int TourId { get; set; }
        
        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public DateTime TravelDate { get; set; }
        
        [Required]
        public int NumberOfPeople { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [StringLength(500)]
        public string? SpecialRequests { get; set; }
        
        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        
        [ForeignKey("TourId")]
        public virtual Tour Tour { get; set; }
    }
} 