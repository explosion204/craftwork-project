using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "92c095fd-9fb8-4f3d-bf3a-0fab4d6a8779");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "2aba90cd-32d7-4769-a96b-7cd2cb84ecba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c453eb67-e1d2-4038-b402-48d0f9270041", "AQAAAAEAACcQAAAAEDICwojolxCVbrIp4xxXlf+QMyTltrMdKTMyruV6yK4pXSBFySMD9RVxB1J+5fJ10Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                column: "ConcurrencyStamp",
                value: "c25d7d5d-842f-41f8-bc27-867aec849d33");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                column: "ConcurrencyStamp",
                value: "433b64fa-8d18-4df7-8d05-8085eb0f0a51");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "aa808b8b-691e-4b25-bd72-6f378e1e549a", "AQAAAAEAACcQAAAAEABRLuenWWqVoxHYcE9h0/hI3ZgKc3ylxoddTzTLEy/Xl0EP9IeJHMp+or32GKsN8Q==" });
        }
    }
}
