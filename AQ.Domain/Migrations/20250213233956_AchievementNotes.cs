using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AQ.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AchievementNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Achievement",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Achievement");
        }
    }
}
