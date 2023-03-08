using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Catalog.Db.Migrations
{
    /// <inheritdoc />
    public partial class ChannelsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BarCatalog",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarCatalogChannel",
                columns: table => new
                {
                    BarCatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarCatalogChannel", x => new { x.BarCatalogId, x.ChannelId });
                    table.ForeignKey(
                        name: "FK_BarCatalogChannel_BarCatalog_BarCatalogId",
                        column: x => x.BarCatalogId,
                        principalTable: "BarCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarCatalogChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FooCatalogChannel",
                columns: table => new
                {
                    FooCatalogId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooCatalogChannel", x => new { x.FooCatalogId, x.ChannelId });
                    table.ForeignKey(
                        name: "FK_FooCatalogChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FooCatalogChannel_FooCatalog_FooCatalogId",
                        column: x => x.FooCatalogId,
                        principalTable: "FooCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarCatalogChannel_ChannelId",
                table: "BarCatalogChannel",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_FooCatalogChannel_ChannelId",
                table: "FooCatalogChannel",
                column: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarCatalogChannel");

            migrationBuilder.DropTable(
                name: "FooCatalogChannel");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BarCatalog");
        }
    }
}
