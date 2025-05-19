using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelWebsite.Models;

namespace TravelWebsite.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Khởi tạo các vai trò mặc định
            string[] roleNames = { "Admin", "User", "Manager" };
            
            foreach (var roleName in roleNames)
            {
                // Kiểm tra vai trò đã tồn tại chưa
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Tạo vai trò mới nếu chưa tồn tại
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            // Kiểm tra tài khoản admin đã tồn tại chưa
            var adminUser = await userManager.FindByEmailAsync("admin@travelwebsite.com");
            
            if (adminUser == null)
            {
                // Tạo tài khoản admin mới
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@travelwebsite.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                // Tạo tài khoản admin với mật khẩu mặc định
                var result = await userManager.CreateAsync(user, "Admin@123");
                
                if (result.Succeeded)
                {
                    // Gán vai trò admin cho tài khoản
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        public static async Task SeedDefaultDataAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Khởi tạo vai trò
            await SeedRolesAsync(roleManager);
            
            // Khởi tạo tài khoản admin
            await SeedAdminAsync(userManager);
        }
    }
} 