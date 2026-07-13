using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangesDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LockoutEnd", "OtpCode", "OtpExpiryTime", "PasswordHash", "Role", "StoreId" },
                values: new object[,]
                {
                    { 2, "manager@coffeeshop.com", null, 0, null, "$2a$12$GrzJ9xrUlYji0uo42cbIpuqVK7V3mhj9rXHMOboEdMLrvzveo6Gg6", "Manager", null },
                    { 3, "staff@coffeeshop.com", null, 0, null, "$2a$12$V2kwEWM4EcUnUYAF.8lMvu.ZKW0vwqxXoFqcTjZO10lNWgGaPCfOK", "Staff", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
