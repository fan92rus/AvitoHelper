using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AvitoHelper.Migrations
{
    public partial class subscribe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_Orders_OrderId",
                table: "Word");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_Orders_OrderId1",
                table: "Word");

            migrationBuilder.RenameColumn(
                name: "OrderId1",
                table: "Word",
                newName: "Orderid1");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Word",
                newName: "Orderid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Word",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Word_OrderId1",
                table: "Word",
                newName: "IX_Word_Orderid1");

            migrationBuilder.RenameIndex(
                name: "IX_Word_OrderId",
                table: "Word",
                newName: "IX_Word_Orderid");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Product",
                newName: "Orderid");

            migrationBuilder.RenameIndex(
                name: "IX_Product_OrderId",
                table: "Product",
                newName: "IX_Product_Orderid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "id");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DayEndSubscribe",
                table: "Users",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Purchaces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "uId",
                table: "Purchaces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Orders_Orderid",
                table: "Product",
                column: "Orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Word_Orders_Orderid",
                table: "Word",
                column: "Orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Word_Orders_Orderid1",
                table: "Word",
                column: "Orderid1",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Orders_Orderid",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_Orders_Orderid",
                table: "Word");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_Orders_Orderid1",
                table: "Word");

            migrationBuilder.DropColumn(
                name: "DayEndSubscribe",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Purchaces");

            migrationBuilder.DropColumn(
                name: "uId",
                table: "Purchaces");

            migrationBuilder.RenameColumn(
                name: "Orderid1",
                table: "Word",
                newName: "OrderId1");

            migrationBuilder.RenameColumn(
                name: "Orderid",
                table: "Word",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Word",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Word_Orderid1",
                table: "Word",
                newName: "IX_Word_OrderId1");

            migrationBuilder.RenameIndex(
                name: "IX_Word_Orderid",
                table: "Word",
                newName: "IX_Word_OrderId");

            migrationBuilder.RenameColumn(
                name: "Orderid",
                table: "Product",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Orderid",
                table: "Product",
                newName: "IX_Product_OrderId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Orders_OrderId",
                table: "Product",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Word_Orders_OrderId",
                table: "Word",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Word_Orders_OrderId1",
                table: "Word",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
