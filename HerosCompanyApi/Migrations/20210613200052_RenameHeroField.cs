using Microsoft.EntityFrameworkCore.Migrations;

namespace HerosCompanyApi.Migrations
{
    public partial class RenameHeroField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinedAt",
                table: "Heros",
                newName: "StartedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Heros",
                newName: "JoinedAt");
        }
    }
}
