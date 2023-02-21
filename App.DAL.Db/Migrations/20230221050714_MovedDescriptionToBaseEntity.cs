using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.Db.Migrations
{
    /// <inheritdoc />
    public partial class MovedDescriptionToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foo_Description",
                table: "BaseEntity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foo_Description",
                table: "BaseEntity",
                type: "TEXT",
                nullable: true);
        }
    }
}
