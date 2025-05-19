using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWebsite.Data;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;

namespace TravelWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var homeViewModel = new HomeViewModel
        {
            PopularDestinations = await _context.Destinations
                .Where(d => d.IsPopular && d.IsActive)
                .Take(6)
                .ToListAsync(),

            FeaturedTours = await _context.Tours
                .Include(t => t.Destination)
                .Where(t => t.IsPopular && t.IsActive)
                .Take(6)
                .ToListAsync()
        };

        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
