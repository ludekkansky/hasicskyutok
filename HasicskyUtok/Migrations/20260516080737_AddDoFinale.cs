using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HasicskyUtok.Migrations
{
    /// <inheritdoc />
    public partial class AddDoFinale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoFinale",
                table: "Stafeta",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoFinale",
                table: "Stafeta");
        }
    }
}
