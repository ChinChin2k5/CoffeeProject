using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangesHashPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$YNNv6xrFsG0gsSeIrEL1hen9sHlivBHkSecqdNdlSWJ899ckl2rSO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$wE5YhV5qX/k.y3k2W2H36.rC.5QO.60b377L963G1nQe.c3K/g/Cq");
        }
    }
}
