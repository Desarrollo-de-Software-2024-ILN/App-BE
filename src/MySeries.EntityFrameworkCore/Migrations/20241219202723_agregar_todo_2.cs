using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySeries.Migrations
{
    /// <inheritdoc />
    public partial class agregar_todo_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appseries",
                table: "Appseries");

            migrationBuilder.RenameTable(
                name: "Appseries",
                newName: "AppSeries");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "AppSeries",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "AppSeries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppSeries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "AppSeries",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdSerie",
                table: "AppSeries",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WatchlistId",
                table: "AppSeries",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSeries",
                table: "AppSeries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppCalificacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdSerie = table.Column<int>(type: "int", nullable: false),
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCalificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCalificacion_AppSeries_IdSerie",
                        column: x => x.IdSerie,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Msj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AppNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppWatchlists",
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
                    table.PrimaryKey("PK_AppWatchlists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSeries_WatchlistId",
                table: "AppSeries",
                column: "WatchlistId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCalificacion_IdSerie",
                table: "AppCalificacion",
                column: "IdSerie");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSeries_AppWatchlists_WatchlistId",
                table: "AppSeries",
                column: "WatchlistId",
                principalTable: "AppWatchlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSeries_AppWatchlists_WatchlistId",
                table: "AppSeries");

            migrationBuilder.DropTable(
                name: "AppCalificacion");

            migrationBuilder.DropTable(
                name: "AppNotifications");

            migrationBuilder.DropTable(
                name: "AppWatchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSeries",
                table: "AppSeries");

            migrationBuilder.DropIndex(
                name: "IX_AppSeries_WatchlistId",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "IdSerie",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "WatchlistId",
                table: "AppSeries");

            migrationBuilder.RenameTable(
                name: "AppSeries",
                newName: "Appseries");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Appseries",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appseries",
                table: "Appseries",
                column: "Id");
        }
    }
}
