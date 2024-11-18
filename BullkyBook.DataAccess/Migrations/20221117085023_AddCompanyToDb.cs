using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class AddCompanyToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companyModels",
                columns: table => new
                {
                    Comp_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comp_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comp_StAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comp_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comp_State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyModels", x => x.Comp_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companyModels");
        }
    }
}
