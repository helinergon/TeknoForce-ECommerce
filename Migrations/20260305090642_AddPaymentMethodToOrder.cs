using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknoForce.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJ1L7qdKj4Guux4gjhwhh0b5sfkNHKWSYu7f4UZ6eDu9qWUDBGot46QW8y3pvJd5PA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEC7ShM1n0ak39mp20Rm7vCfdDJOeyRWHjg1C4GsMjRkJMf8PAW6vuQabt4e6XBKlMQ==");
        }
    }
}
