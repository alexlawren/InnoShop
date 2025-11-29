using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.ProductInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_IsAviable_To_IsAvailable_In_Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAviable",
                table: "Products",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Products",
                newName: "IsAviable");
        }
    }
}
