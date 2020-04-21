using Microsoft.EntityFrameworkCore.Migrations;

namespace leave_management.Data.Migrations
{
    public partial class AdjustRequest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_LeaveId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "LeaveId",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LeaveRequestVM",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LeaveRequestVM");

            migrationBuilder.AddColumn<int>(
                name: "LeaveId",
                table: "LeaveRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveId",
                table: "LeaveRequests",
                column: "LeaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveId",
                table: "LeaveRequests",
                column: "LeaveId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
