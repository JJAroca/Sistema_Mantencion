using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaMantencion.Migrations
{
    /// <inheritdoc />
    public partial class FixHistorialKilometraje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 30, 22, 23, 7, 823, DateTimeKind.Local).AddTicks(9355));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 30, 22, 23, 7, 823, DateTimeKind.Local).AddTicks(9359));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 30, 22, 23, 7, 823, DateTimeKind.Local).AddTicks(9361));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 30, 22, 23, 7, 823, DateTimeKind.Local).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 30, 22, 23, 7, 823, DateTimeKind.Local).AddTicks(9367));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5647));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5651));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5654));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5693));

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 25, 22, 23, 19, 668, DateTimeKind.Local).AddTicks(5696));
        }
    }
}
