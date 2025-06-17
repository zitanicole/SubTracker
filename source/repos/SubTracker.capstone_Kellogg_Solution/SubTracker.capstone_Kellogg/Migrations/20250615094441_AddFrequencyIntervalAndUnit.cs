using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubTracker.capstone_Kellogg.Migrations
{
    /// <inheritdoc />
    public partial class AddFrequencyIntervalAndUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Frequency",
                table: "Autopayments",
                newName: "FrequencyUnit");

            migrationBuilder.AddColumn<int>(
                name: "FrequencyInterval",
                table: "Autopayments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrequencyInterval",
                table: "Autopayments");

            migrationBuilder.RenameColumn(
                name: "FrequencyUnit",
                table: "Autopayments",
                newName: "Frequency");
        }
    }
}
