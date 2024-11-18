using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class updatedShoppingcartToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_productModels_Pro_Id",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_Pro_Id",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "Pro_Id",
                table: "shoppingCarts");

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_ProductId",
                table: "shoppingCarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_productModels_ProductId",
                table: "shoppingCarts",
                column: "ProductId",
                principalTable: "productModels",
                principalColumn: "pro_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_productModels_ProductId",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_ProductId",
                table: "shoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "Pro_Id",
                table: "shoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_Pro_Id",
                table: "shoppingCarts",
                column: "Pro_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_productModels_Pro_Id",
                table: "shoppingCarts",
                column: "Pro_Id",
                principalTable: "productModels",
                principalColumn: "pro_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
