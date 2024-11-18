using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class UpdateAppUserForCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_companyModels_Comp_Id",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_companyModels_Comp_Id",
                table: "AspNetUsers",
                column: "Comp_Id",
                principalTable: "companyModels",
                principalColumn: "Comp_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_companyModels_Comp_Id",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_companyModels_Comp_Id",
                table: "AspNetUsers",
                column: "Comp_Id",
                principalTable: "companyModels",
                principalColumn: "Comp_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
