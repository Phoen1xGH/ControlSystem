using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUpdatesInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "UpdatesInfo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "UpdatesInfo");
        }
    }
}
