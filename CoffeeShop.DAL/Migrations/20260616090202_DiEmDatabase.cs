using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DiEmDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreInventories",
                table: "StoreInventories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreInventories",
                table: "StoreInventories",
                columns: new[] { "StoreId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreInventories",
                table: "StoreInventories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreInventories",
                table: "StoreInventories",
                column: "StoreId");
        }
    }
}
