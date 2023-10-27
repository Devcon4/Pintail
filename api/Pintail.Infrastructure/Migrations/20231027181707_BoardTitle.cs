using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pintail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BoardTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Board",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "Board");
        }
    }
}
