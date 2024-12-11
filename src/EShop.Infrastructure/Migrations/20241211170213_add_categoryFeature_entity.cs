using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_categoryFeature_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryFeature");

            migrationBuilder.CreateTable(
                name: "CategoryFeatures",
                columns: table => new
                {
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    FeatureId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFeatures", x => new { x.CategoryId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_CategoryFeatures_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFeatures_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFeatures_FeatureId",
                table: "CategoryFeatures",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryFeatures");

            migrationBuilder.CreateTable(
                name: "CategoryFeature",
                columns: table => new
                {
                    CategoriesId = table.Column<long>(type: "bigint", nullable: false),
                    FeaturesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFeature", x => new { x.CategoriesId, x.FeaturesId });
                    table.ForeignKey(
                        name: "FK_CategoryFeature_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFeature_Feature_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Feature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFeature_FeaturesId",
                table: "CategoryFeature",
                column: "FeaturesId");
        }
    }
}
