using Microsoft.EntityFrameworkCore.Migrations;

namespace Craftwork_Project.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                column: "ConcurrencyStamp",
                value: "7d9077a4-9e39-4392-824f-3527f6720fc7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a91aa5e4-804a-44e1-8ccc-2b34622e3c8b", "AQAAAAEAACcQAAAAEPD7gOqeG6IMOlzx04hjcrwZiy+ybltLiImLLdcGe/d/VvQljGro4TdqwLt4ctTtug==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
