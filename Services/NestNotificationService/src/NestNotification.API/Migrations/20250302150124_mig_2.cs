using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NestNotification.API.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_EmailTemplates_TemplateId",
                table: "EmailLogs");

            migrationBuilder.DropTable(
                name: "FailedEmails");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_TemplateId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "HtmlBody",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "To",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "WhoCreated",
                table: "EmailTemplates",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "EmailTemplates",
                newName: "IsHtml");

            migrationBuilder.RenameColumn(
                name: "WhoCreated",
                table: "EmailLogs",
                newName: "ToEmail");

            migrationBuilder.RenameColumn(
                name: "SentDate",
                table: "EmailLogs",
                newName: "SentAt");

            migrationBuilder.RenameColumn(
                name: "IsSuccess",
                table: "EmailLogs",
                newName: "IsHtml");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "EmailQueues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHtml = table.Column<bool>(type: "bit", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAttempt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueues", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueues");

            migrationBuilder.RenameColumn(
                name: "IsHtml",
                table: "EmailTemplates",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "EmailTemplates",
                newName: "WhoCreated");

            migrationBuilder.RenameColumn(
                name: "ToEmail",
                table: "EmailLogs",
                newName: "WhoCreated");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "EmailLogs",
                newName: "SentDate");

            migrationBuilder.RenameColumn(
                name: "IsHtml",
                table: "EmailLogs",
                newName: "IsSuccess");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "HtmlBody",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "EmailTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmailLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EmailLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "EmailLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "EmailLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                table: "EmailLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FailedEmails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastAttempt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoCreated = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailedEmails_EmailTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_TemplateId",
                table: "EmailLogs",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FailedEmails_TemplateId",
                table: "FailedEmails",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_EmailTemplates_TemplateId",
                table: "EmailLogs",
                column: "TemplateId",
                principalTable: "EmailTemplates",
                principalColumn: "Id");
        }
    }
}
