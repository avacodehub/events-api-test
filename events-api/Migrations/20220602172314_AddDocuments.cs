using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace events_api.Migrations
{
    public partial class AddDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Event_EventId1",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_EventId1",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "EventId1",
                table: "Photo");

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_EventId",
                table: "Document",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId1",
                table: "Photo",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_EventId1",
                table: "Photo",
                column: "EventId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Event_EventId1",
                table: "Photo",
                column: "EventId1",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
