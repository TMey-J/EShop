using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_relation_for_color_to_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ColorId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "ColorProduct",
                columns: table => new
                {
                    ColorsId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorProduct", x => new { x.ColorsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ColorProduct_Color_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorProduct_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorProduct_ProductsId",
                table: "ColorProduct",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorProduct");

            migrationBuilder.AddColumn<long>(
                name: "ColorId",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColorId",
                table: "Product",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id");
        }
    }
}
