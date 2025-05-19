using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelWebsite.Migrations
{
    /// <inheritdoc />
    public partial class FixProfilePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa cột hiện tại
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            // Tạo lại cột với kiểu dữ liệu đúng
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột kiểu byte[]
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            // Tạo lại cột kiểu string
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
