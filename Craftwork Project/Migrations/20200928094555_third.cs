using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InCart",
                table: "PurchaseDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PurchaseDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "3e886308-5305-4e8c-b14c-ba23908cd77e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "304311bb-665b-4284-a093-af6f0062d239", "AQAAAAEAACcQAAAAEE6Zj7kkRsaQ0hVKxPs5Ysg2WnCoZt96ewAbRRNwT7KZ1Xtc7roopSfpEa78jVpslA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PurchaseDetails");

            migrationBuilder.AddColumn<bool>(
                name: "InCart",
                table: "PurchaseDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
    }
}
