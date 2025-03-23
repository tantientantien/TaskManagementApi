using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class Addcolortolabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 6,
                column: "Color",
                value: "#FFFFFF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Labels");
        }
    }
}
