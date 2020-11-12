using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftworkProject.Infrastructure.Migrations
{
    public partial class seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Reviews",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "540431bf-5b15-408e-b785-e3368f164b4c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "db0461c7-46c2-48c5-9a87-8d903ff22934");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b0135de2-eba0-4805-8b6c-4fd15a1a1772", "AQAAAAEAACcQAAAAEKOVjQ7/5SpuOy9+tFALXW9oeF0uXkEB+cl1j+y1qd46f9BlhN6VoYafwQVrAg4r5Q==" });
        }
    }
}
