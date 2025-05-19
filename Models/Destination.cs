using System.ComponentModel.DataAnnotations;

namespace TravelWebsite.Models
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        public decimal Rating { get; set; }
        
        public bool IsPopular { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
} 