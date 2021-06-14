using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HerosCompanyApi.Migrations
{
    public partial class AddedHeroColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                table: "Heros",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeTrained",
                table: "Heros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastTimeTrainingAmount",
                table: "Heros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeTrained",
                table: "Heros");

            migrationBuilder.DropColumn(
                name: "LastTimeTrainingAmount",
                table: "Heros");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedAt",
                table: "Heros",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
