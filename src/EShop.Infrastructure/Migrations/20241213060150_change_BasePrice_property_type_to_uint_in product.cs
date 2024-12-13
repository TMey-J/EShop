using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_BasePrice_property_type_to_uint_inproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BasePrice",
                table: "Product",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BasePrice",
                table: "Product",
                type: "float",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
