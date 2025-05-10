using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class remvoendoenderecos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuario_endereco_EnderecoPrincipalId",
                table: "usuario");

            migrationBuilder.DropTable(
                name: "endereco");

            migrationBuilder.DropIndex(
                name: "IX_usuario_EnderecoPrincipalId",
                table: "usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpresaId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_endereco_empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "empresa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_endereco_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_EnderecoPrincipalId",
                table: "usuario",
                column: "EnderecoPrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_endereco_EmpresaId",
                table: "endereco",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_endereco_UsuarioId",
                table: "endereco",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_endereco_EnderecoPrincipalId",
                table: "usuario",
                column: "EnderecoPrincipalId",
                principalTable: "endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
