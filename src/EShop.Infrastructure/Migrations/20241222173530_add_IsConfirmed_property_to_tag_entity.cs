using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_IsConfirmed_property_to_tag_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Tag",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Tag");
        }
    }
}
