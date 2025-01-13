using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_enitties_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerProducts",
                table: "SellerProducts");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "EndOfDiscount",
                table: "Product");

            migrationBuilder.AlterColumn<short>(
                name: "Count",
                table: "SellerProducts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "ColorId",
                table: "SellerProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "BasePrice",
                table: "SellerProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte>(
                name: "DiscountPercentage",
                table: "SellerProducts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfDiscount",
                table: "SellerProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerProducts",
                table: "SellerProducts",
                columns: new[] { "ProductId", "SellerId", "ColorId" });

            migrationBuilder.CreateIndex(
                name: "IX_SellerProducts_ColorId",
                table: "SellerProducts",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProducts_Color_ColorId",
                table: "SellerProducts",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProducts_Color_ColorId",
                table: "SellerProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerProducts",
                table: "SellerProducts");

            migrationBuilder.DropIndex(
                name: "IX_SellerProducts_ColorId",
                table: "SellerProducts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "SellerProducts");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "SellerProducts");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "SellerProducts");

            migrationBuilder.DropColumn(
                name: "EndOfDiscount",
                table: "SellerProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "SellerProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<long>(
                name: "BasePrice",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte>(
                name: "DiscountPercentage",
                table: "Product",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOfDiscount",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerProducts",
                table: "SellerProducts",
                columns: new[] { "ProductId", "SellerId" });

            migrationBuilder.CreateTable(
                name: "productColors",
                columns: table => new
                {
                    ColorId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productColors", x => new { x.ColorId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_productColors_Color_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productColors_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productColors_ProductId",
                table: "productColors",
                column: "ProductId");
        }
    }
}
