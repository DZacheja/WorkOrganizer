using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrganizer.Migrations
{
    public partial class AddConfirmedPersonToTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmedPersonTaskID",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmedPersonSubtaskID",
                table: "Subtasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ConfirmedPersonTaskID",
                table: "Tasks",
                column: "ConfirmedPersonTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_ConfirmedPersonSubtaskID",
                table: "Subtasks",
                column: "ConfirmedPersonSubtaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Users_ConfirmedPersonSubtaskID",
                table: "Subtasks",
                column: "ConfirmedPersonSubtaskID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_ConfirmedPersonTaskID",
                table: "Tasks",
                column: "ConfirmedPersonTaskID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Users_ConfirmedPersonSubtaskID",
                table: "Subtasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_ConfirmedPersonTaskID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ConfirmedPersonTaskID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Subtasks_ConfirmedPersonSubtaskID",
                table: "Subtasks");

            migrationBuilder.DropColumn(
                name: "ConfirmedPersonTaskID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ConfirmedPersonSubtaskID",
                table: "Subtasks");
        }
    }
}
