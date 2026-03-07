using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeknoForce.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressFieldsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "Orders",
                newName: "Street");

            migrationBuilder.AddColumn<string>(
                name: "AddressNote",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingNo",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEC7ShM1n0ak39mp20Rm7vCfdDJOeyRWHjg1C4GsMjRkJMf8PAW6vuQabt4e6XBKlMQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressNote",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApartmentNo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BuildingNo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Orders",
                newName: "ShippingAddress");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDkYrRspvtKbQCYnw14COrCwOskkrLXY+F6upxuc6itWfh/PoRbSskTZjiUucsTgGw==");
        }
    }
}
