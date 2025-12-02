using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaMantencion.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Camionetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Patente = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Marca = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Anio = table.Column<int>(type: "INTEGER", nullable: false),
                    Kilometraje = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaUltimaMantencion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camionetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CamionetaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Accion = table.Column<string>(type: "TEXT", nullable: false),
                    Motivo = table.Column<string>(type: "TEXT", nullable: true),
                    KilometrajeAnterior = table.Column<int>(type: "INTEGER", nullable: true),
                    KilometrajeNuevo = table.Column<int>(type: "INTEGER", nullable: true),
                    EstadoAnterior = table.Column<string>(type: "TEXT", nullable: true),
                    EstadoNuevo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historial_Camionetas_CamionetaId",
                        column: x => x.CamionetaId,
                        principalTable: "Camionetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mantenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CamionetaId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TipoMantencion = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    KilometrajeRegistrado = table.Column<int>(type: "INTEGER", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mantenciones_Camionetas_CamionetaId",
                        column: x => x.CamionetaId,
                        principalTable: "Camionetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Camionetas",
                columns: new[] { "Id", "Anio", "Estado", "FechaRegistro", "FechaUltimaMantencion", "Kilometraje", "Marca", "Modelo", "Patente" },
                values: new object[,]
                {
                    { 1, 2022, "Disponible", new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5647), null, 15000, "Toyota", "Hilux", "ABCD12" },
                    { 2, 2023, "Disponible", new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5651), null, 8000, "Ford", "Ranger", "EFGH34" },
                    { 3, 2021, "Disponible", new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5654), null, 25000, "Chevrolet", "Colorado", "IJKL56" },
                    { 4, 2022, "Disponible", new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5693), null, 18000, "Nissan", "Frontier", "MNOP78" },
                    { 5, 2023, "Disponible", new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5696), null, 5000, "Mitsubishi", "L200", "QRST90" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Camionetas_Patente",
                table: "Camionetas",
                column: "Patente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Historial_CamionetaId",
                table: "Historial",
                column: "CamionetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenciones_CamionetaId",
                table: "Mantenciones",
                column: "CamionetaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historial");

            migrationBuilder.DropTable(
                name: "Mantenciones");

            migrationBuilder.DropTable(
                name: "Camionetas");
        }
    }
}
