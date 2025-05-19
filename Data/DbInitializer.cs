using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelWebsite.Models;

namespace TravelWebsite.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Tạo database nếu chưa tồn tại
            context.Database.Migrate();

            // Thêm các vai trò
            var roles = new[] { "Admin", "User", "Manager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Kiểm tra và sửa lỗi tài khoản admin
            await EnsureAdminAccount(userManager);

            // Kiểm tra xem đã có dữ liệu trong bảng Destinations chưa
            if (await context.Destinations.AnyAsync())
            {
                return; // Cơ sở dữ liệu đã có dữ liệu, không cần thêm dữ liệu mẫu
            }

            // Thêm dữ liệu mẫu cho Destinations
            var destinations = new[]
            {
                new Destination
                {
                    Name = "Đà Nẵng",
                    Description = "Thành phố biển xinh đẹp với bãi biển Mỹ Khê nổi tiếng",
                    Country = "Việt Nam",
                    City = "Đà Nẵng", 
                    ImageUrl = "https://images.unsplash.com/photo-1560804255-e8a0205c9d02",
                    Rating = 4.8m,
                    IsPopular = true,
                    IsActive = true
                },
                new Destination
                {
                    Name = "Hạ Long",
                    Description = "Vịnh Hạ Long - Di sản thiên nhiên thế giới với hàng nghìn hòn đảo đá vôi",
                    Country = "Việt Nam",
                    City = "Quảng Ninh",
                    ImageUrl = "https://images.unsplash.com/photo-1528127269322-539801943592",
                    Rating = 4.9m,
                    IsPopular = true,
                    IsActive = true
                },
                new Destination
                {
                    Name = "Hội An",
                    Description = "Phố cổ Hội An với đèn lồng và kiến trúc truyền thống",
                    Country = "Việt Nam",
                    City = "Quảng Nam",
                    ImageUrl = "https://images.unsplash.com/photo-1564322955382-5dfa5635c6b4",
                    Rating = 4.7m,
                    IsPopular = true,
                    IsActive = true
                },
                new Destination
                {
                    Name = "Huế",
                    Description = "Cố đô Huế với các di tích lịch sử và văn hóa",
                    Country = "Việt Nam",
                    City = "Thừa Thiên Huế",
                    ImageUrl = "https://images.unsplash.com/photo-1558182722-3ad16906387c",
                    Rating = 4.5m,
                    IsPopular = true,
                    IsActive = true
                },
                new Destination
                {
                    Name = "Đà Lạt",
                    Description = "Thành phố ngàn hoa với khí hậu mát mẻ quanh năm",
                    Country = "Việt Nam",
                    City = "Lâm Đồng",
                    ImageUrl = "https://images.unsplash.com/photo-1570794742805-8bad969c3b89",
                    Rating = 4.6m,
                    IsPopular = true,
                    IsActive = true
                },
                new Destination
                {
                    Name = "Nha Trang",
                    Description = "Thiên đường biển với bãi cát trắng và nước trong xanh",
                    Country = "Việt Nam",
                    City = "Khánh Hòa",
                    ImageUrl = "https://images.unsplash.com/photo-1551374139-45bde722ef61",
                    Rating = 4.4m,
                    IsPopular = true,
                    IsActive = true
                }
            };

            await context.Destinations.AddRangeAsync(destinations);
            await context.SaveChangesAsync();

            // Thêm dữ liệu mẫu cho Tours
            var tours = new[]
            {
                new Tour
                {
                    Name = "Khám phá Đà Nẵng 3 ngày 2 đêm",
                    Description = "Tour trọn gói tham quan Đà Nẵng, Bà Nà Hills và cầu Vàng",
                    Price = 3500000,
                    Duration = 3,
                    DestinationId = destinations[0].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1596422846543-75c6fc197f11",
                    IsPopular = true,
                    IsActive = true
                },
                new Tour
                {
                    Name = "Du ngoạn Vịnh Hạ Long",
                    Description = "Tour du thuyền ngắm cảnh Vịnh Hạ Long - kỳ quan thiên nhiên thế giới",
                    Price = 4000000,
                    Duration = 2,
                    DestinationId = destinations[1].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1573709582428-79fd50b4fa23",
                    IsPopular = true,
                    IsActive = true
                },
                new Tour
                {
                    Name = "Khám phá Phố cổ Hội An",
                    Description = "Tour tham quan phố cổ Hội An, làng gốm Thanh Hà và làng rau Trà Quế",
                    Price = 2500000,
                    Duration = 2,
                    DestinationId = destinations[2].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1601189496316-1f4eee6b8572",
                    IsPopular = true,
                    IsActive = true
                },
                new Tour
                {
                    Name = "Khám phá Cố đô Huế",
                    Description = "Tour tham quan Đại Nội, chùa Thiên Mụ và lăng tẩm các vua nhà Nguyễn",
                    Price = 2800000,
                    Duration = 2,
                    DestinationId = destinations[3].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1583449243494-2730de8bffc3",
                    IsPopular = true,
                    IsActive = true
                },
                new Tour
                {
                    Name = "Đà Lạt mộng mơ",
                    Description = "Tour khám phá thành phố Đà Lạt với các điểm tham quan nổi tiếng",
                    Price = 3200000,
                    Duration = 3,
                    DestinationId = destinations[4].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1561542141-0fe60782fc2d",
                    IsPopular = true,
                    IsActive = true
                },
                new Tour
                {
                    Name = "Khám phá biển Nha Trang",
                    Description = "Tour tham quan 4 đảo và tắm biển Nha Trang",
                    Price = 2900000,
                    Duration = 2,
                    DestinationId = destinations[5].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1590100614275-818a732310c8",
                    IsPopular = true,
                    IsActive = true
                }
            };

            await context.Tours.AddRangeAsync(tours);
            await context.SaveChangesAsync();
        }

        private static async Task EnsureAdminAccount(UserManager<ApplicationUser> userManager)
        {
            // Thông tin tài khoản admin
            var adminEmail = "admin@travelwebsite.com";
            var adminUserName = "admin";
            var adminPassword = "Admin@123";

            // Tìm kiếm bằng email
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // Nếu không tìm thấy, tạo mới
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "System",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Thêm admin vào các vai trò
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "User");
                }
                else
                {
                    // Ghi log lỗi nếu cần
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.WriteLine($"Không thể tạo tài khoản admin: {errors}");
                }
            }
            else
            {
                // Kiểm tra và cập nhật username nếu cần
                if (adminUser.UserName != adminUserName)
                {
                    adminUser.UserName = adminUserName;
                    await userManager.UpdateAsync(adminUser);
                }

                // Đảm bảo admin có thể đăng nhập
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                var resetResult = await userManager.ResetPasswordAsync(adminUser, token, adminPassword);

                if (resetResult.Succeeded)
                {
                    Console.WriteLine("Đã cập nhật mật khẩu cho tài khoản admin");
                }

                // Đảm bảo admin có đủ vai trò
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                if (!await userManager.IsInRoleAsync(adminUser, "User"))
                {
                    await userManager.AddToRoleAsync(adminUser, "User");
                }
            }
        }
    }
} 