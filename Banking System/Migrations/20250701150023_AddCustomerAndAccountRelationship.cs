using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking_System.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAndAccountRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "TbAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TbCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAccount_CustomerId",
                table: "TbAccount",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbAccount_TbCustomer_CustomerId",
                table: "TbAccount",
                column: "CustomerId",
                principalTable: "TbCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbAccount_TbCustomer_CustomerId",
                table: "TbAccount");

            migrationBuilder.DropTable(
                name: "TbCustomer");

            migrationBuilder.DropIndex(
                name: "IX_TbAccount_CustomerId",
                table: "TbAccount");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TbAccount");
        }
    }
}
