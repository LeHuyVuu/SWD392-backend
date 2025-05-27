using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392_backend.Migrations
{
    /// <inheritdoc />
    public partial class V2_AddIsMainToProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_main",
                table: "product_images",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_main",
                table: "product_images");
        }
    }
}
