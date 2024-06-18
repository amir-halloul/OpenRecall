using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenRecall.Cli.Migrations
{
    /// <inheritdoc />
    public partial class ActivityDescriptionVector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionVector",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionVector",
                table: "Activities");
        }
    }
}
