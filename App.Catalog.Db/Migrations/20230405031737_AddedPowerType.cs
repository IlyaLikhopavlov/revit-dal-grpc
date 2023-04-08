using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Catalog.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddedPowerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PowerTypeId",
                table: "FooCatalog",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PowerTypeId",
                table: "BarCatalog",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PowerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FooCatalog_PowerTypeId",
                table: "FooCatalog",
                column: "PowerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCatalog_PowerTypeId",
                table: "BarCatalog",
                column: "PowerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BarCatalog_PowerTypes_PowerTypeId",
                table: "BarCatalog",
                column: "PowerTypeId",
                principalTable: "PowerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FooCatalog_PowerTypes_PowerTypeId",
                table: "FooCatalog",
                column: "PowerTypeId",
                principalTable: "PowerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarCatalog_PowerTypes_PowerTypeId",
                table: "BarCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_FooCatalog_PowerTypes_PowerTypeId",
                table: "FooCatalog");

            migrationBuilder.DropTable(
                name: "PowerTypes");

            migrationBuilder.DropIndex(
                name: "IX_FooCatalog_PowerTypeId",
                table: "FooCatalog");

            migrationBuilder.DropIndex(
                name: "IX_BarCatalog_PowerTypeId",
                table: "BarCatalog");

            migrationBuilder.DropColumn(
                name: "PowerTypeId",
                table: "FooCatalog");

            migrationBuilder.DropColumn(
                name: "PowerTypeId",
                table: "BarCatalog");
        }
    }
}
