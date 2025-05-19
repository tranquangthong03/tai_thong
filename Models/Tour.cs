using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelWebsite.Models
{
    public class Tour
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int? Duration { get; set; } // Số ngày
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        public bool IsPopular { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign keys
        public int DestinationId { get; set; }
        
        // Navigation properties
        [ForeignKey("DestinationId")]
        public virtual Destination Destination { get; set; }
    }
} 