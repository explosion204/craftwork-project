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
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "da2c0de6-4d3a-4048-9ef8-1d48ff76bb14");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eb4bd5b1-66e2-42c9-bb37-3bbe5baecf4b", "AQAAAAEAACcQAAAAEOzcRsfMKMTuSf5ixWs/AxyfNEqc5oqtgH7kheLxoGukN3HkkTC/edrcGrpuQGV4EQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "aaa3ac34-758c-46fb-9d3b-25673adac4f5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "971d9bb8-b35a-4aa2-891f-a6646819eaa4", "AQAAAAEAACcQAAAAECGD7yVKZnEvPh1rgxDCoL8O/YaZkQc1oZR1446BZhPrVTHK/bha7Y/QW4yXYY7zrA==" });
        }
    }
}
