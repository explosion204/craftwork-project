using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftworkProject.Infrastructure.Migrations
{
    public partial class _9th : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "c9aef233-916e-415f-8044-0c976f9c299f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "980aea04-75ae-471f-a188-0d3ec1e76c67");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d7fd81b9-4c40-4c61-b413-f4993ef9f58e", "AQAAAAEAACcQAAAAECwrrDW3TwGqUYUayeazTMdrGxyWPyR1FCoKfJQCgHtt5LyTU0bnFLBrYzeu0GaVjQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "e52054bc-9397-40ce-b8e2-d92bfe7643bc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "a529f2cc-98ef-49d3-b79c-529138d337fe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8b63183d-ffca-4f98-999b-1c6ce22c0da9", "AQAAAAEAACcQAAAAEIbLQQl9O/IMm83TjAQ9CBsHpOAhOEbJsD5N28mQyy3cP75wv29DR72JjmwVMeL4CA==" });
        }
    }
}
