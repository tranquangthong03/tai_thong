# Travel Website

Website du lịch xây dựng trên nền tảng ASP.NET Core MVC.

## Tính năng

- Đăng ký và đăng nhập tài khoản
- Xem danh sách điểm đến và tour du lịch
- Đặt tour du lịch
- Đánh giá tour
- Giao diện tiếng Việt

## Công nghệ sử dụng

- ASP.NET Core 9.0
- Entity Framework Core
- SQL Server
- Identity Framework
- Bootstrap

## Cấu trúc dự án

- **Models**: Chứa các lớp đối tượng dữ liệu
  - ApplicationUser
  - Destination
  - Tour
  - Booking
  - Review
- **ViewModels**: Chứa các lớp dùng để hiển thị và nhận dữ liệu từ View
- **Controllers**: Xử lý logic nghiệp vụ
- **Views**: Giao diện người dùng
- **Data**: Chứa các lớp truy cập dữ liệu
- **Services**: Chứa các dịch vụ
- **Utilities**: Chứa các tiện ích

## Cài đặt và chạy

1. Clone repository
2. Mở project trong Visual Studio
3. Khôi phục các package NuGet
4. Cập nhật chuỗi kết nối trong appsettings.json
5. Chạy migration để tạo cơ sở dữ liệu: `dotnet ef database update`
6. Chạy ứng dụng: `dotnet run`

## Tác giả

- Trần Quang Thông 