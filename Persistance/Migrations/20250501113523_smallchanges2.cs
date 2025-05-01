using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class smallchanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Students_StudentGuid",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Users_UserGuid",
                table: "Admissions");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Admissions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "StudentGuid",
                table: "Admissions",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Admissions_UserGuid",
                table: "Admissions",
                newName: "IX_Admissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Admissions_StudentGuid",
                table: "Admissions",
                newName: "IX_Admissions_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Students_StudentId",
                table: "Admissions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Users_UserId",
                table: "Admissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Students_StudentId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Users_UserId",
                table: "Admissions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Admissions",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Admissions",
                newName: "StudentGuid");

            migrationBuilder.RenameIndex(
                name: "IX_Admissions_UserId",
                table: "Admissions",
                newName: "IX_Admissions_UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_Admissions_StudentId",
                table: "Admissions",
                newName: "IX_Admissions_StudentGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Students_StudentGuid",
                table: "Admissions",
                column: "StudentGuid",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Users_UserGuid",
                table: "Admissions",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
