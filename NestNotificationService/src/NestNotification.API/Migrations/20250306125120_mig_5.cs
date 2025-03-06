using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestNotification.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhoCreated",
                table: "EmailTemplates",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "EmailTemplates",
                newName: "WhoCreated");
        }
    }
}
