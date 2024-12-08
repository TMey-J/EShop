using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_seller_relation_with_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "SellerBase",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SellerProducts",
                columns: table => new
                {
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerProducts", x => new { x.ProductId, x.SellerId });
                    table.ForeignKey(
                        name: "FK_SellerProducts_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerProducts_SellerBase_SellerId",
                        column: x => x.SellerId,
                        principalTable: "SellerBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_ProductId",
                table: "SellerBase",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerProducts_SellerId",
                table: "SellerProducts",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerBase_Product_ProductId",
                table: "SellerBase",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerBase_Product_ProductId",
                table: "SellerBase");

            migrationBuilder.DropTable(
                name: "SellerProducts");

            migrationBuilder.DropIndex(
                name: "IX_SellerBase_ProductId",
                table: "SellerBase");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SellerBase");
        }
    }
}
