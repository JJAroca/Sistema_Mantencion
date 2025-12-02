using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaMantencion.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToCamioneta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Camionetas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Camionetas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "FechaEliminacion", "FechaRegistro" },
                values: new object[] { true, null, new DateTime(2025, 11, 30, 22, 54, 11, 941, DateTimeKind.Local).AddTicks(3103) });

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "FechaEliminacion", "FechaRegistro" },
                values: new object[] { true, null, new DateTime(2025, 11, 30, 22, 54, 11, 941, DateTimeKind.Local).AddTicks(3106) });

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Activo", "FechaEliminacion", "FechaRegistro" },
                values: new object[] { true, null, new DateTime(2025, 11, 30, 22, 54, 11, 941, DateTimeKind.Local).AddTicks(3108) });

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Activo", "FechaEliminacion", "FechaRegistro" },
                values: new object[] { true, null, new DateTime(2025, 11, 30, 22, 54, 11, 941, DateTimeKind.Local).AddTicks(3111) });

            migrationBuilder.UpdateData(
                table: "Camionetas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Activo", "FechaEliminacion", "FechaRegistro" },
                values: new object[] { true, null, new DateTime(2025, 11, 30, 22, 54, 11, 941, DateTimeKind.Local).AddTicks(3114) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Camionetas");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Camionetas");

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
    }
}
