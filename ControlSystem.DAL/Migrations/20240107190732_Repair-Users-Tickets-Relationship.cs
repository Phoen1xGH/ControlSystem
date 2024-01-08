using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RepairUsersTicketsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Tickets_TicketId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_TicketId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "UserAccounts");

            migrationBuilder.CreateTable(
                name: "TicketUserAccount",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "integer", nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketUserAccount", x => new { x.ParticipantsId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_TicketUserAccount_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketUserAccount_UserAccounts_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketUserAccount_TicketId",
                table: "TicketUserAccount",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketUserAccount");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "UserAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_TicketId",
                table: "UserAccounts",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Tickets_TicketId",
                table: "UserAccounts",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
