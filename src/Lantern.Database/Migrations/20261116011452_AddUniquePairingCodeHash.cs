using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lantern.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePairingCodeHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PairingCodes_CodeHash",
                table: "PairingCodes",
                column: "CodeHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PairingCodes_CodeHash",
                table: "PairingCodes");
        }
    }
}
