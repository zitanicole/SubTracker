using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubTracker.capstone_Kellogg.Migrations
{
    /// <inheritdoc />
    public partial class AddAutopaymentAccountNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Autopayments_AccountId",
                table: "Autopayments",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Autopayments_Accounts_AccountId",
                table: "Autopayments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autopayments_Accounts_AccountId",
                table: "Autopayments");

            migrationBuilder.DropIndex(
                name: "IX_Autopayments_AccountId",
                table: "Autopayments");
        }
    }
}
