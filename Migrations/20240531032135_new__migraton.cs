using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfApp2.Migrations
{
    /// <inheritdoc />
    public partial class new__migraton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    X = table.Column<double>(type: "double precision", nullable: false),
                    Y = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputTables",
                columns: table => new
                {
                    CityCount = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteCount = table.Column<int>(type: "integer", nullable: false),
                    CoordinatesMatrix = table.Column<string>(type: "text", nullable: false),
                    DisruptionPercentage = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputTables", x => x.CityCount);
                });

            migrationBuilder.CreateTable(
                name: "ResultTables",
                columns: table => new
                {
                    OptimalValue = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptimalRoutes = table.Column<string>(type: "text", nullable: false),
                    InputTableId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultTables", x => x.OptimalValue);
                    table.ForeignKey(
                        name: "FK_ResultTables_InputTables_InputTableId",
                        column: x => x.InputTableId,
                        principalTable: "InputTables",
                        principalColumn: "CityCount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultTables_InputTableId",
                table: "ResultTables",
                column: "InputTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ResultTables");

            migrationBuilder.DropTable(
                name: "InputTables");
        }
    }
}
