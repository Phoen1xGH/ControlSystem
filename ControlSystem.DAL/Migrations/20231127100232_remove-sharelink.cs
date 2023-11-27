using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removesharelink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Links_ShareLinkId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShareLinkId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShareLinkId",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShareLinkId",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShareLinkId",
                table: "Tickets",
                column: "ShareLinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Links_ShareLinkId",
                table: "Tickets",
                column: "ShareLinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
