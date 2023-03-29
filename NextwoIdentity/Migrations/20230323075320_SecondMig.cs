using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextwoIdentity.Migrations
{
    /// <inheritdoc />
    public partial class SecondMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorys",
                columns: table => new
                {
                    CategoreyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoreyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorys", x => x.CategoreyId);
                });

            migrationBuilder.CreateTable(
                name: "Prodect",
                columns: table => new
                {
                    ProdectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoreyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodect", x => x.ProdectId);
                    table.ForeignKey(
                        name: "FK_Prodect_Categorys_CategoreyId",
                        column: x => x.CategoreyId,
                        principalTable: "Categorys",
                        principalColumn: "CategoreyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prodect_CategoreyId",
                table: "Prodect",
                column: "CategoreyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prodect");

            migrationBuilder.DropTable(
                name: "Categorys");
        }
    }
}
