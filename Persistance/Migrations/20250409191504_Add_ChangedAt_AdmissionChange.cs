using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Text.Json;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_ChangedAt_AdmissionChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "AdmissionChanges",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "AdmissionChanges"
            );
        }
    }
}
