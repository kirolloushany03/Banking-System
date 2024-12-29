using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking_System.Migrations
{
    /// <inheritdoc />
    public partial class UsingIndexOnAccountNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TbAccount_AccountNumber",
                table: "TbAccount",
                column: "AccountNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbAccount_AccountNumber",
                table: "TbAccount");
        }
    }
}
