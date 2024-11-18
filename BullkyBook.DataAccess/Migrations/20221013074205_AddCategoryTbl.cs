using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class AddCategoryTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bullkyBookModels",
                columns: table => new
                {
                    book_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    display_Order = table.Column<int>(type: "int", nullable: false),
                    oder_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bullkyBookModels", x => x.book_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bullkyBookModels");
        }
    }
}
