using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestStorage.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileCategory",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityId",
                table: "Files",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityType",
                table: "Files",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileCategory",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RelatedEntityId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RelatedEntityType",
                table: "Files");
        }
    }
}
