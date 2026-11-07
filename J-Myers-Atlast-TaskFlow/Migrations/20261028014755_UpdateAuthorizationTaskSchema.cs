using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace J_Myers_Atlast_TaskFlow.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthorizationTaskSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AuthorizationTasks",
                newName: "TaskId");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "AuthorizationTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuthorizationTasks",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "AuthorizationTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "AuthorizationTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "AuthorizationTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AuthorizationTasks",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationTasks_ProjectId",
                table: "AuthorizationTasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationTasks_Projects_ProjectId",
                table: "AuthorizationTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationTasks_Projects_ProjectId",
                table: "AuthorizationTasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationTasks_ProjectId",
                table: "AuthorizationTasks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuthorizationTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "AuthorizationTasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "AuthorizationTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AuthorizationTasks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AuthorizationTasks");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "AuthorizationTasks",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "AuthorizationTasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
