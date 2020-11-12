using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftworkProject.Infrastructure.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "a8088049-4d1b-470f-93ca-8fc5aae32736");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "623e8e94-e5b3-41a5-8710-5b33c2fb1221");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34465bdd-e4b9-4bf6-b57f-5d233a9b2d6d", "AQAAAAEAACcQAAAAEEDIc4qGx/sxNULVxrQBjXGZ58d6BYYOB4jRXZXrYA/F86fxWQWs8p3VjSfvIq/70Q==" });
        }
    }
}
