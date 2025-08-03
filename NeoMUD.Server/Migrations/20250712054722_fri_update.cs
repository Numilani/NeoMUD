using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoMUD.Migrations
{
    /// <inheritdoc />
    public partial class fri_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentRoom",
                table: "Characters",
                newName: "CurrentRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentRoomId",
                table: "Characters",
                newName: "CurrentRoom");
        }
    }
}
