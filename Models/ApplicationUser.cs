using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TravelWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        
        public string? ProfilePicture { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 