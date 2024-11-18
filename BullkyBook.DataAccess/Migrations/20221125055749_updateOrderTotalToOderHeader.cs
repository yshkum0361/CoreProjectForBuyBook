using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class updateOrderTotalToOderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_AppUserId",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<double>(
                name: "OrderTotal",
                table: "orderHeaders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_AppUserId",
                table: "orderHeaders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_AppUserId",
                table: "orderHeaders");

            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_AppUserId",
                table: "orderHeaders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
