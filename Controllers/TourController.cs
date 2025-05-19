using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TravelWebsite.Data;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;

namespace TravelWebsite.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tour
        public async Task<IActionResult> Index()
        {
            var tours = await _context.Tours
                .Include(t => t.Destination)
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.IsPopular)
                .ThenBy(t => t.Name)
                .ToListAsync();

            return View(tours);
        }

        // GET: Tour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Tour/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Country)
                .ThenBy(d => d.Name)
                .ToListAsync();

            var viewModel = new TourViewModel
            {
                Destinations = destinations,
                IsActive = true
            };

            return View(viewModel);
        }

        // POST: Tour/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TourViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tour = new Tour
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Duration = viewModel.Duration,
                    ImageUrl = viewModel.ImageUrl,
                    DestinationId = viewModel.DestinationId,
                    IsPopular = viewModel.IsPopular,
                    IsActive = viewModel.IsActive
                };

                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            viewModel.Destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Country)
                .ThenBy(d => d.Name)
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Tour/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            var destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Country)
                .ThenBy(d => d.Name)
                .ToListAsync();

            var viewModel = new TourViewModel
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                Price = tour.Price,
                Duration = tour.Duration,
                ImageUrl = tour.ImageUrl,
                DestinationId = tour.DestinationId,
                IsPopular = tour.IsPopular,
                IsActive = tour.IsActive,
                Destinations = destinations
            };

            return View(viewModel);
        }

        // POST: Tour/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, TourViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tour = await _context.Tours.FindAsync(id);
                    if (tour == null)
                    {
                        return NotFound();
                    }

                    tour.Name = viewModel.Name;
                    tour.Description = viewModel.Description;
                    tour.Price = viewModel.Price;
                    tour.Duration = viewModel.Duration;
                    tour.ImageUrl = viewModel.ImageUrl;
                    tour.DestinationId = viewModel.DestinationId;
                    tour.IsPopular = viewModel.IsPopular;
                    tour.IsActive = viewModel.IsActive;
                    tour.UpdatedAt = DateTime.UtcNow;

                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(viewModel.Id))
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

            // If we got this far, something failed, redisplay form
            viewModel.Destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Country)
                .ThenBy(d => d.Name)
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Tour/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Tour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            
            // Soft delete
            tour.IsActive = false;
            tour.UpdatedAt = DateTime.UtcNow;
            
            _context.Update(tour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
        }
    }
} 