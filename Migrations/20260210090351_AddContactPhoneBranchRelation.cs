using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknoForce.Migrations
{
    /// <inheritdoc />
    public partial class AddContactPhoneBranchRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapEmbed",
                table: "ContactBranches");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ContactBranches",
                newName: "ContactName");

            migrationBuilder.AddColumn<int>(
                name: "ContactBranchId",
                table: "ContactPhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MapIframe",
                table: "ContactBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactPhones_ContactBranchId",
                table: "ContactPhones",
                column: "ContactBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPhones_ContactBranches_ContactBranchId",
                table: "ContactPhones",
                column: "ContactBranchId",
                principalTable: "ContactBranches",
                principalColumn: "ContactBranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactPhones_ContactBranches_ContactBranchId",
                table: "ContactPhones");

            migrationBuilder.DropIndex(
                name: "IX_ContactPhones_ContactBranchId",
                table: "ContactPhones");

            migrationBuilder.DropColumn(
                name: "ContactBranchId",
                table: "ContactPhones");

            migrationBuilder.DropColumn(
                name: "MapIframe",
                table: "ContactBranches");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "ContactBranches",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "MapEmbed",
                table: "ContactBranches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
