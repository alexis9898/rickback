using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class t2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "characters",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "species",
                table: "characters",
                newName: "Species");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "characters",
                newName: "Image");

            migrationBuilder.AddColumn<int>(
                name: "OriginalId",
                table: "characters",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalId",
                table: "characters");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "characters",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Species",
                table: "characters",
                newName: "species");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "characters",
                newName: "image");
        }
    }
}
