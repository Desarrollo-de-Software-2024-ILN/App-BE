using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySeries.Migrations
{
    /// <inheritdoc />
    public partial class addwatchlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WatchlistId",
                table: "Appseries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppWatchlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWatchlist", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appseries_WatchlistId",
                table: "Appseries",
                column: "WatchlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appseries_AppWatchlist_WatchlistId",
                table: "Appseries",
                column: "WatchlistId",
                principalTable: "AppWatchlist",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appseries_AppWatchlist_WatchlistId",
                table: "Appseries");

            migrationBuilder.DropTable(
                name: "AppWatchlist");

            migrationBuilder.DropIndex(
                name: "IX_Appseries_WatchlistId",
                table: "Appseries");

            migrationBuilder.DropColumn(
                name: "WatchlistId",
                table: "Appseries");
        }
    }
}
