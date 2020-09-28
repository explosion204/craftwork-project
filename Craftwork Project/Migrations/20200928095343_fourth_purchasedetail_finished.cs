using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class fourth_purchasedetail_finished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Confirmed",
                table: "PurchaseDetails",
                newName: "Finished");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Finished",
                table: "PurchaseDetails",
                newName: "Confirmed");

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
    }
}
