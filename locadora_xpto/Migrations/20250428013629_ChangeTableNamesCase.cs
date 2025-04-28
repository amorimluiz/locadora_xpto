using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace locadora_xpto.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNamesCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alugueis_clientes_ClienteId",
                table: "Alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_Alugueis_veiculos_VeiculoId",
                table: "Alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Alugueis_AluguelId",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alugueis",
                table: "Alugueis");

            migrationBuilder.RenameTable(
                name: "Pagamentos",
                newName: "pagamentos");

            migrationBuilder.RenameTable(
                name: "Alugueis",
                newName: "alugueis");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_AluguelId",
                table: "pagamentos",
                newName: "IX_pagamentos_AluguelId");

            migrationBuilder.RenameIndex(
                name: "IX_Alugueis_VeiculoId",
                table: "alugueis",
                newName: "IX_alugueis_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_Alugueis_ClienteId",
                table: "alugueis",
                newName: "IX_alugueis_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pagamentos",
                table: "pagamentos",
                column: "PagamentoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_alugueis",
                table: "alugueis",
                column: "AluguelId");

            migrationBuilder.AddForeignKey(
                name: "FK_alugueis_clientes_ClienteId",
                table: "alugueis",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_alugueis_veiculos_VeiculoId",
                table: "alugueis",
                column: "VeiculoId",
                principalTable: "veiculos",
                principalColumn: "VeiculoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pagamentos_alugueis_AluguelId",
                table: "pagamentos",
                column: "AluguelId",
                principalTable: "alugueis",
                principalColumn: "AluguelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alugueis_clientes_ClienteId",
                table: "alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_alugueis_veiculos_VeiculoId",
                table: "alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_pagamentos_alugueis_AluguelId",
                table: "pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pagamentos",
                table: "pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_alugueis",
                table: "alugueis");

            migrationBuilder.RenameTable(
                name: "pagamentos",
                newName: "Pagamentos");

            migrationBuilder.RenameTable(
                name: "alugueis",
                newName: "Alugueis");

            migrationBuilder.RenameIndex(
                name: "IX_pagamentos_AluguelId",
                table: "Pagamentos",
                newName: "IX_Pagamentos_AluguelId");

            migrationBuilder.RenameIndex(
                name: "IX_alugueis_VeiculoId",
                table: "Alugueis",
                newName: "IX_Alugueis_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_alugueis_ClienteId",
                table: "Alugueis",
                newName: "IX_Alugueis_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos",
                column: "PagamentoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alugueis",
                table: "Alugueis",
                column: "AluguelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alugueis_clientes_ClienteId",
                table: "Alugueis",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alugueis_veiculos_VeiculoId",
                table: "Alugueis",
                column: "VeiculoId",
                principalTable: "veiculos",
                principalColumn: "VeiculoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Alugueis_AluguelId",
                table: "Pagamentos",
                column: "AluguelId",
                principalTable: "Alugueis",
                principalColumn: "AluguelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
