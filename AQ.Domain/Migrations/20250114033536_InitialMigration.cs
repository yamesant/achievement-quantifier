using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AQ.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE TABLE IF NOT EXISTS AchievementClass (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Unit TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Achievement (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AchievementClassId INTEGER NOT NULL,
                    CompletedDate TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    FOREIGN KEY (AchievementClassId) REFERENCES AchievementClass(Id)
                );
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropTable(
                name: "AchievementClass");
        }
    }
}
