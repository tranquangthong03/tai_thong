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
        private const int PageSize = 6; // Số lượng tour mỗi trang

        public TourController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tour
        public async Task<IActionResult> Index(int page = 1, string searchName = "", decimal? minPrice = null, decimal? maxPrice = null, int? destinationId = null, bool? isPopular = null, string sortOrder = "")
        {
            ViewData["CurrentPage"] = page;
            ViewData["SearchName"] = searchName;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["DestinationId"] = destinationId;
            ViewData["IsPopular"] = isPopular;
            ViewData["SortOrder"] = sortOrder;

            // Khởi tạo query
            var query = _context.Tours
                .Include(t => t.Destination)
                .Where(t => t.IsActive);

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(t => 
                    t.Name.Contains(searchName) || 
                    (t.Description != null && t.Description.Contains(searchName)) ||
                    (t.Destination.Name.Contains(searchName)));
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                query = query.Where(t => t.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(t => t.Price <= maxPrice.Value);
            }

            // Lọc theo điểm đến
            if (destinationId.HasValue)
            {
                query = query.Where(t => t.DestinationId == destinationId.Value);
            }

            // Lọc theo phổ biến
            if (isPopular.HasValue && isPopular.Value)
            {
                query = query.Where(t => t.IsPopular);
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(t => t.Name);
                    break;
                case "price_asc":
                    query = query.OrderBy(t => t.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(t => t.Price);
                    break;
                case "duration_asc":
                    query = query.OrderBy(t => t.Duration);
                    break;
                case "duration_desc":
                    query = query.OrderByDescending(t => t.Duration);
                    break;
                default: // "name_asc" hoặc mặc định
                    query = query.OrderBy(t => t.Name);
                    break;
            }

            // Đếm tổng số tour để phân trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // Đảm bảo số trang hợp lệ
            if (page < 1)
                page = 1;
            else if (page > totalPages && totalPages > 0)
                page = totalPages;

            // Lấy dữ liệu cho trang hiện tại
            var tours = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Lấy danh sách điểm đến cho dropdown lọc
            ViewBag.Destinations = await _context.Destinations
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync();

            // Tạo viewModel với thông tin phân trang
            var viewModel = new TourIndexViewModel
            {
                Tours = tours,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchName = searchName,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                DestinationId = destinationId,
                IsPopular = isPopular,
                SortOrder = sortOrder
            };

            return View(viewModel);
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
                
                // Xác định trang để chuyển hướng sau khi tạo tour
                int totalTours = await _context.Tours.Where(t => t.IsActive).CountAsync();
                int targetPage = (int)Math.Ceiling(totalTours / (double)PageSize);
                
                return RedirectToAction(nameof(Index), new { page = targetPage });
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
                
                // Xác định trang chứa tour sau khi sửa
                var tourIndex = await _context.Tours
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.Name)
                    .Select(t => t.Id)
                    .ToListAsync();
                
                var position = tourIndex.IndexOf(id);
                int targetPage = position / PageSize + 1;
                
                return RedirectToAction(nameof(Index), new { page = targetPage });
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