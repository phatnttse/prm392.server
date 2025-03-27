using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRM392.API.Migrations
{
    /// <inheritdoc />
    public partial class Update_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "Orders",
                newName: "OrderCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderCode",
                table: "Orders",
                newName: "OrderNumber");
        }
    }
}
