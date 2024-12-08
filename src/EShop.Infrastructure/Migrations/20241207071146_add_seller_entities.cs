using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_seller_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellerBase",
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
                    table.PrimaryKey("PK_SellerBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerBase_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerBase_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualSeller",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CartOrShebaNumber = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    AboutSeller = table.Column<string>(type: "ntext", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualSeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualSeller_SellerBase_SellerId",
                        column: x => x.SellerId,
                        principalTable: "SellerBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalSeller",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    IsRealPerson = table.Column<bool>(type: "bit", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RegisterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EconomicCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    SignatureOwners = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CompanyType = table.Column<byte>(type: "tinyint", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalSeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalSeller_SellerBase_SellerId",
                        column: x => x.SellerId,
                        principalTable: "SellerBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualSeller_SellerId",
                table: "IndividualSeller",
                column: "SellerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalSeller_SellerId",
                table: "LegalSeller",
                column: "SellerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_CityId",
                table: "SellerBase",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerBase_UserId",
                table: "SellerBase",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndividualSeller");

            migrationBuilder.DropTable(
                name: "LegalSeller");

            migrationBuilder.DropTable(
                name: "SellerBase");
        }
    }
}
