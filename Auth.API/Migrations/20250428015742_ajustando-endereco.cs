using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class ajustandoendereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "EnderecoPrincipal",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "endereco");

            migrationBuilder.DropColumn(
                name: "TipoEndereco",
                table: "endereco");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnderecoPrincipal",
                table: "endereco",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "endereco",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "endereco",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "endereco",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoEndereco",
                table: "endereco",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
