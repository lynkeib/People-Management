using Microsoft.EntityFrameworkCore.Migrations;

namespace leave_management.Data.Migrations
{
    public partial class AdjustRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "LeaveRequestVM",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestComments",
                table: "LeaveRequestVM",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "LeaveRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestComments",
                table: "LeaveRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "LeaveRequestVM");

            migrationBuilder.DropColumn(
                name: "RequestComments",
                table: "LeaveRequestVM");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "RequestComments",
                table: "LeaveRequests");
        }
    }
}
