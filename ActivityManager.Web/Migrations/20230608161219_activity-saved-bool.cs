using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityManager.Web.Migrations
{
    /// <inheritdoc />
    public partial class activitysavedbool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "Activity",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "Activity");
        }
    }
}
