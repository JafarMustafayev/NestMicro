using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestStorage.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "MimeTypeId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MimeCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MimeCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MaxUploadSizeInBytes = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MimeTypes_MimeCategories_MimeCategoryId",
                        column: x => x.MimeCategoryId,
                        principalTable: "MimeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_MimeTypeId",
                table: "Files",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MimeTypes_MimeCategoryId",
                table: "MimeTypes",
                column: "MimeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_MimeTypes_MimeTypeId",
                table: "Files",
                column: "MimeTypeId",
                principalTable: "MimeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_MimeTypes_MimeTypeId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "MimeTypes");

            migrationBuilder.DropTable(
                name: "MimeCategories");

            migrationBuilder.DropIndex(
                name: "IX_Files_MimeTypeId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "MimeTypeId",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Files",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
