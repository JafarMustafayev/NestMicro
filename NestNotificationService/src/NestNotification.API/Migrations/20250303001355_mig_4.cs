using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestNotification.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EmailTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "EmailTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "EmailTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhoCreated",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "EmailQueues",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "WhoCreated",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "EmailQueues");
        }
    }
}
