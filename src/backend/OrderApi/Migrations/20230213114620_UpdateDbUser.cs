using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faces_Orders_OrderId",
                table: "Faces");

            migrationBuilder.DropIndex(
                name: "IX_Faces_OrderId",
                table: "Faces");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Faces_OrderId",
                table: "Faces",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faces_Orders_OrderId",
                table: "Faces",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
