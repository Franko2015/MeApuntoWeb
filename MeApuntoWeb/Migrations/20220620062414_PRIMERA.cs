using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeApuntoWeb.Migrations
{
    public partial class PRIMERA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEstado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEstado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblLugar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLugar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTipo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organizacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo_usuarioId = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblUsuario_tblTipo_Tipo_usuarioId",
                        column: x => x.Tipo_usuarioId,
                        principalTable: "tblTipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblEvento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_evento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_termino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    LugarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEvento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEvento_tblCategoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "tblCategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEvento_tblEstado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "tblEstado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEvento_tblLugar_LugarId",
                        column: x => x.LugarId,
                        principalTable: "tblLugar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEvento_tblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "tblUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEvento_CategoriaId",
                table: "tblEvento",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEvento_EstadoId",
                table: "tblEvento",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEvento_LugarId",
                table: "tblEvento",
                column: "LugarId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEvento_UsuarioId",
                table: "tblEvento",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUsuario_Tipo_usuarioId",
                table: "tblUsuario",
                column: "Tipo_usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEvento");

            migrationBuilder.DropTable(
                name: "tblCategoria");

            migrationBuilder.DropTable(
                name: "tblEstado");

            migrationBuilder.DropTable(
                name: "tblLugar");

            migrationBuilder.DropTable(
                name: "tblUsuario");

            migrationBuilder.DropTable(
                name: "tblTipo");
        }
    }
}
