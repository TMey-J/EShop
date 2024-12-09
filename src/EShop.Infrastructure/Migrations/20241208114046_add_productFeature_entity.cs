using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_productFeature_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualSeller_SellerBase_SellerId",
                table: "IndividualSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_LegalSeller_SellerBase_SellerId",
                table: "LegalSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerProducts_SellerBase_SellerId",
                table: "SellerProducts");

            migrationBuilder.DropTable(
                name: "SellerBase");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Feature",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDocumentApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    RejectReason = table.Column<string>(type: "ntext", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seller_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seller_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    FeatureName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeatureValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeature_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeature_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeature_ProductId",
                table: "ProductFeature",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeature_SellerId",
                table: "ProductFeature",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seller_CityId",
                table: "Seller",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Seller_UserId",
                table: "Seller",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualSeller_Seller_SellerId",
                table: "IndividualSeller",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalSeller_Seller_SellerId",
                table: "LegalSeller",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProducts_Seller_SellerId",
                table: "SellerProducts",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualSeller_Seller_SellerId",
                table: "IndividualSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_LegalSeller_Seller_SellerId",
                table: "LegalSeller");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerProducts_Seller_SellerId",
                table: "SellerProducts");

            migrationBuilder.DropTable(
                name: "ProductFeature");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Feature",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "SellerBase",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDocumentApproved = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    RejectReason = table.Column<string>(type: "ntext", nullable: true),
                    ShopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerBase_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerBase_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SellerBase_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_CityId",
                table: "SellerBase",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_ProductId",
                table: "SellerBase",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_UserId",
                table: "SellerBase",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualSeller_SellerBase_SellerId",
                table: "IndividualSeller",
                column: "SellerId",
                principalTable: "SellerBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalSeller_SellerBase_SellerId",
                table: "LegalSeller",
                column: "SellerId",
                principalTable: "SellerBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProducts_SellerBase_SellerId",
                table: "SellerProducts",
                column: "SellerId",
                principalTable: "SellerBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
