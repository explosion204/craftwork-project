using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class fifth_product_created_field_removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "4be5b890-997b-47be-97bf-88e726c6d1ec");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "69d1cfc1-f84f-43f3-8f9f-84508c934370", "AQAAAAEAACcQAAAAEC4fJnVDSquBo+VKrFI3HuZN8AApW2Y7++KSd5LHT2LdS2JFWSuQ8fPJaX0UotSJNA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "ec7b00a1-a989-4dd9-af66-1cabfd537345");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cfc95bb2-7146-454c-b9b1-93027b332a13", "AQAAAAEAACcQAAAAEMID0H0Ua9NMrvU8GQ7+gyl9v5b7WCDBIIEnRdrWRO9/Zh+TPT8k3g7VgwcI1kATxA==" });
        }
    }
}
