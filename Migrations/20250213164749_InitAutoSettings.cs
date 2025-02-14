using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LolApp.Migrations
{
    /// <inheritdoc />
    public partial class InitAutoSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutoAcceptQueue = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoRequeue = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSelectChampion = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoRandomChampionSkin = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoRerollAram = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoSettings");
        }
    }
}
