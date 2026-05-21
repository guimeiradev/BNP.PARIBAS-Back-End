using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bnp.Paribas.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    CodProduto = table.Column<string>(type: "varchar(20)", nullable: false),
                    DesProduto = table.Column<string>(type: "varchar(100)", nullable: true),
                    StaStatus = table.Column<char>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.CodProduto);
                    table.CheckConstraint("CK_Produto_StaStatus", "StaStatus IN ('A', 'I')");
                });

            migrationBuilder.CreateTable(
                name: "ProdutoCosif",
                columns: table => new
                {
                    CodProduto = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodCosif = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodClassificacao = table.Column<string>(type: "varchar(20)", nullable: true),
                    StaStatus = table.Column<char>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoCosif", x => new { x.CodProduto, x.CodCosif });
                    table.CheckConstraint("CK_ProdutoCosif_CodClassificacao", "CodClassificacao IN ('Normal', 'MTM')");
                    table.CheckConstraint("CK_ProdutoCosif_StaStatus", "StaStatus IN ('A', 'I')");
                    table.ForeignKey(
                        name: "FK_ProdutoCosif_Produto_CodProduto",
                        column: x => x.CodProduto,
                        principalTable: "Produto",
                        principalColumn: "CodProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentoManual",
                columns: table => new
                {
                    DatMes = table.Column<int>(type: "INTEGER", nullable: false),
                    DatAno = table.Column<int>(type: "INTEGER", nullable: false),
                    NumLancamento = table.Column<long>(type: "INTEGER", nullable: false),
                    CodProduto = table.Column<string>(type: "varchar(20)", nullable: false),
                    CodCosif = table.Column<string>(type: "varchar(20)", nullable: false),
                    ValValor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DesDescricao = table.Column<string>(type: "varchar(100)", nullable: false),
                    DatMovimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CodUsuario = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoManual", x => new { x.DatMes, x.DatAno, x.NumLancamento });
                    table.ForeignKey(
                        name: "FK_MovimentoManual_ProdutoCosif_CodProduto_CodCosif",
                        columns: x => new { x.CodProduto, x.CodCosif },
                        principalTable: "ProdutoCosif",
                        principalColumns: new[] { "CodProduto", "CodCosif" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoManual_CodProduto_CodCosif",
                table: "MovimentoManual",
                columns: new[] { "CodProduto", "CodCosif" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentoManual");

            migrationBuilder.DropTable(
                name: "ProdutoCosif");

            migrationBuilder.DropTable(
                name: "Produto");
        }
    }
}
