using TravelWebsite.Models;

namespace TravelWebsite.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Destination> PopularDestinations { get; set; } = new List<Destination>();
        public IEnumerable<Tour> FeaturedTours { get; set; } = new List<Tour>();
    }
} 