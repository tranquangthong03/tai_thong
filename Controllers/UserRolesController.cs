using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelWebsite.Models;
using TravelWebsite.ViewModels;

namespace TravelWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            
            foreach (var user in users)
            {
                var viewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                
                userRolesViewModel.Add(viewModel);
            }
            
            return View(userRolesViewModel);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.UserId = userId;
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            
            ViewBag.UserName = user.UserName;
            
            var model = new List<ManageUserRolesViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();
            
            foreach (var role in roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                
                model.Add(userRolesViewModel);
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Không thể xóa vai trò hiện tại của người dùng");
                return View(model);
            }
            
            result = await _userManager.AddToRolesAsync(user, 
                model.Where(x => x.Selected).Select(y => y.RoleName));
                
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Không thể thêm các vai trò đã chọn cho người dùng");
                return View(model);
            }
            
            return RedirectToAction("Index");
        }
    }
} 