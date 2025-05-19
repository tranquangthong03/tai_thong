using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TravelWebsite.Data;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;

namespace TravelWebsite.Controllers
{
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destination
        public async Task<IActionResult> Index()
        {
            var destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Country)
                .ThenBy(d => d.Name)
                .ToListAsync();

            return View(destinations);
        }

        // GET: Destination/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .Include(d => d.Tours.Where(t => t.IsActive))
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // GET: Destination/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Destination/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DestinationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var destination = new Destination
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    ImageUrl = viewModel.ImageUrl,
                    Country = viewModel.Country,
                    City = viewModel.City,
                    Rating = viewModel.Rating,
                    IsPopular = viewModel.IsPopular,
                    IsActive = viewModel.IsActive
                };

                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Destination/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            var viewModel = new DestinationViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                ImageUrl = destination.ImageUrl,
                Country = destination.Country,
                City = destination.City,
                Rating = destination.Rating,
                IsPopular = destination.IsPopular,
                IsActive = destination.IsActive
            };

            return View(viewModel);
        }

        // POST: Destination/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DestinationViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var destination = await _context.Destinations.FindAsync(id);
                    if (destination == null)
                    {
                        return NotFound();
                    }

                    destination.Name = viewModel.Name;
                    destination.Description = viewModel.Description;
                    destination.ImageUrl = viewModel.ImageUrl;
                    destination.Country = viewModel.Country;
                    destination.City = viewModel.City;
                    destination.Rating = viewModel.Rating;
                    destination.IsPopular = viewModel.IsPopular;
                    destination.IsActive = viewModel.IsActive;
                    destination.UpdatedAt = DateTime.UtcNow;

                    _context.Update(destination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Destination/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // POST: Destination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            
            // Soft delete
            destination.IsActive = false;
            destination.UpdatedAt = DateTime.UtcNow;
            _context.Update(destination);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinationExists(int id)
        {
            return _context.Destinations.Any(e => e.Id == id);
        }
    }
} 