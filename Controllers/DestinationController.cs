using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TravelWebsite.Data;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;
using System.Linq;

namespace TravelWebsite.Controllers
{
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 6; // Số lượng điểm đến mỗi trang

        public DestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destination
        public async Task<IActionResult> Index(int page = 1, string searchString = "", string country = "", decimal? minRating = null, bool? isPopular = null, string sortOrder = "")
        {
            ViewData["CurrentPage"] = page;
            ViewData["SearchString"] = searchString;
            ViewData["Country"] = country;
            ViewData["MinRating"] = minRating;
            ViewData["IsPopular"] = isPopular;
            ViewData["SortOrder"] = sortOrder;

            // Khởi tạo query
            var query = _context.Destinations
                .Where(d => d.IsActive);

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => 
                    d.Name.Contains(searchString) || 
                    d.Country.Contains(searchString) ||
                    (d.City != null && d.City.Contains(searchString)) ||
                    (d.Description != null && d.Description.Contains(searchString))
                );
            }

            // Lọc theo quốc gia
            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(d => d.Country.Contains(country));
            }

            // Lọc theo đánh giá
            if (minRating.HasValue)
            {
                query = query.Where(d => d.Rating >= minRating.Value);
            }

            // Lọc theo phổ biến
            if (isPopular.HasValue && isPopular.Value)
            {
                query = query.Where(d => d.IsPopular);
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(d => d.Name);
                    break;
                case "rating_asc":
                    query = query.OrderBy(d => d.Rating);
                    break;
                case "rating_desc":
                    query = query.OrderByDescending(d => d.Rating);
                    break;
                case "country_asc":
                    query = query.OrderBy(d => d.Country).ThenBy(d => d.Name);
                    break;
                default: // "name_asc" hoặc mặc định
                    query = query.OrderBy(d => d.Name);
                    break;
            }

            // Đếm tổng số điểm đến để phân trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // Đảm bảo số trang hợp lệ
            if (page < 1)
                page = 1;
            else if (page > totalPages && totalPages > 0)
                page = totalPages;

            // Lấy dữ liệu cho trang hiện tại
            var destinations = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Tạo viewModel với thông tin phân trang
            var viewModel = new DestinationIndexViewModel
            {
                Destinations = destinations,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchString = searchString,
                Country = country,
                MinRating = minRating,
                IsPopular = isPopular,
                SortOrder = sortOrder
            };

            return View(viewModel);
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