using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changecommentdescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Comment",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Comment",
                type: "ntext",
                nullable: false,
                defaultValue: "");
        }
    }
}
