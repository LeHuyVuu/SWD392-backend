using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392_backend.Migrations
{
    /// <inheritdoc />
    public partial class V1_AddDefaultValueToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "sold_quantity",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "rating_average",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "discount_percent",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "sold_quantity",
                table: "products",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "rating_average",
                table: "products",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "discount_percent",
                table: "products",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.0);
        }
    }
}
