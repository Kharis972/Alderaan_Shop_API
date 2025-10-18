using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace alderaan_shop.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_UserId1",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_UserId1",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "orders",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserId1",
                table: "orders",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_UserId1",
                table: "orders",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
