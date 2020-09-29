using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class second_new_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "75ac4e89-dee5-43ac-98de-55eeaa89075d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"), "179d3e6e-2860-4210-82b5-2e631d80c902", "customer", "CUSTOMER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b017bd04-af7f-4f4d-9f4f-356be776d41e", "AQAAAAEAACcQAAAAEMS48XdrIygusjaUJBx7JnlZc0KBVC1N8xldyJWYsMcxXSXUb9FEPhJSNO1mmI2RXA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "f9849b75-295f-413d-b610-e3a78786d22e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "97e89a71-88c3-4664-a661-1590d5f65b15", "AQAAAAEAACcQAAAAECD3JmddlO7BkmnPTwyKE1BAY36CILj0YyomHKpZupmXFAkI6YzQMRB1LRVwzO12Hw==" });
        }
    }
}
