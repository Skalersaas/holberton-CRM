using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForAdmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmissionNotes_Admissions_Guid",
                table: "AdmissionNotes");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionNotes_AdmissionGuid",
                table: "AdmissionNotes",
                column: "AdmissionGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmissionNotes_Admissions_AdmissionGuid",
                table: "AdmissionNotes",
                column: "AdmissionGuid",
                principalTable: "Admissions",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmissionNotes_Admissions_AdmissionGuid",
                table: "AdmissionNotes");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionNotes_AdmissionGuid",
                table: "AdmissionNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmissionNotes_Admissions_Guid",
                table: "AdmissionNotes",
                column: "Guid",
                principalTable: "Admissions",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
