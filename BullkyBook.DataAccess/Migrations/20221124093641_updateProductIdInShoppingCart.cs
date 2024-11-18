using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreTest2.Migrations
{
    public partial class updateProductIdInShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_productModels_ProductId",
                table: "shoppingCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "shoppingCarts",
                newName: "Pro_Id");

            migrationBuilder.RenameIndex(
                name: "IX_shoppingCarts_ProductId",
                table: "shoppingCarts",
                newName: "IX_shoppingCarts_Pro_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_productModels_Pro_Id",
                table: "shoppingCarts",
                column: "Pro_Id",
                principalTable: "productModels",
                principalColumn: "pro_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_productModels_Pro_Id",
                table: "shoppingCarts");

            migrationBuilder.RenameColumn(
                name: "Pro_Id",
                table: "shoppingCarts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_shoppingCarts_Pro_Id",
                table: "shoppingCarts",
                newName: "IX_shoppingCarts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_productModels_ProductId",
                table: "shoppingCarts",
                column: "ProductId",
                principalTable: "productModels",
                principalColumn: "pro_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
