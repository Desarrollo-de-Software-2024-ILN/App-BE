using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySeries.Migrations
{
    /// <inheritdoc />
    public partial class fixserieidtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appseries_AppWatchlist_WatchlistId",
                table: "Appseries");

            migrationBuilder.DropTable(
                name: "AppWatchlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appseries",
                table: "Appseries");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Appseries");

            migrationBuilder.RenameTable(
                name: "Appseries",
                newName: "AppSeries");

            migrationBuilder.RenameIndex(
                name: "IX_Appseries_WatchlistId",
                table: "AppSeries",
                newName: "IX_AppSeries_WatchlistId");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalTemporadas",
                table: "AppSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSeries",
                table: "AppSeries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppListasDeSeguimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaModificacion = table.Column<DateOnly>(type: "date", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppListasDeSeguimiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppNotificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Msj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNotificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTemporadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FechaLanzamiento = table.Column<DateOnly>(type: "date", maxLength: 128, nullable: false),
                    NumTemporada = table.Column<int>(type: "int", nullable: false),
                    SerieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTemporadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTemporadas_AppSeries_SerieID",
                        column: x => x.SerieID,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalificacionNota = table.Column<float>(type: "real", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCalificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerieId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calificacion_AppSeries_SerieId",
                        column: x => x.SerieId,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppEpisodios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumEpisodio = table.Column<int>(type: "int", nullable: false),
                    FechaEstreno = table.Column<DateOnly>(type: "date", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TemporadaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEpisodios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEpisodios_AppTemporadas_TemporadaID",
                        column: x => x.TemporadaID,
                        principalTable: "AppTemporadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppEpisodios_TemporadaID",
                table: "AppEpisodios",
                column: "TemporadaID");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemporadas_SerieID",
                table: "AppTemporadas",
                column: "SerieID");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_SerieId",
                table: "Calificacion",
                column: "SerieId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_WatchlistId",
                table: "AppSeries",
                column: "WatchlistId",
                principalTable: "AppListasDeSeguimiento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_WatchlistId",
                table: "AppSeries");

            migrationBuilder.DropTable(
                name: "AppEpisodios");

            migrationBuilder.DropTable(
                name: "AppListasDeSeguimiento");

            migrationBuilder.DropTable(
                name: "AppNotificacion");

            migrationBuilder.DropTable(
                name: "Calificacion");

            migrationBuilder.DropTable(
                name: "AppTemporadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSeries",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "TotalTemporadas",
                table: "AppSeries");

            migrationBuilder.RenameTable(
                name: "AppSeries",
                newName: "Appseries");

            migrationBuilder.RenameIndex(
                name: "IX_AppSeries_WatchlistId",
                table: "Appseries",
                newName: "IX_Appseries_WatchlistId");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Appseries",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appseries",
                table: "Appseries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppWatchlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWatchlist", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Appseries_AppWatchlist_WatchlistId",
                table: "Appseries",
                column: "WatchlistId",
                principalTable: "AppWatchlist",
                principalColumn: "Id");
        }
    }
}
