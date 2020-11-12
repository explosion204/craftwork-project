using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftworkProject.Infrastructure.Migrations
{
    public partial class _8th : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                table: "Products",
                type: "text",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "fe6516ff-6839-4a6a-b6fe-a0c56423a1ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "f8981ec6-2022-4607-98e3-9ab8ae806bff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0849fcf9-c14a-4b6e-8db1-99fade560f70", "AQAAAAEAACcQAAAAEHYZJXXNnAzPejwUH1jVzKWuvm0ehwaVRGirmzC6ojpbGnUb9wv7C0P++w2+b07A+Q==" });
        }
    }
}
