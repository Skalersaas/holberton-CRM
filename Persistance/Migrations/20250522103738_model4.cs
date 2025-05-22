using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class model4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "AdmissionChanges");

            migrationBuilder.CreateTable(
                name: "ChangeTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: false),
                    PreValue = table.Column<string>(type: "text", nullable: false),
                    PostValue = table.Column<string>(type: "text", nullable: false),
                    AdmissionChangeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeTemplate_AdmissionChanges_AdmissionChangeId",
                        column: x => x.AdmissionChangeId,
                        principalTable: "AdmissionChanges",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeTemplate_AdmissionChangeId",
                table: "ChangeTemplate",
                column: "AdmissionChangeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeTemplate");

            migrationBuilder.AddColumn<JsonDocument>(
                name: "Data",
                table: "AdmissionChanges",
                type: "jsonb",
                nullable: false);
        }
    }
}
