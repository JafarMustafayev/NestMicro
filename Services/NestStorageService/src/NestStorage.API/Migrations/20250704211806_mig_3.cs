using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestStorage.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromIp",
                table: "Files",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "0.0.0.0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromIp",
                table: "Files");
        }
    }
}
