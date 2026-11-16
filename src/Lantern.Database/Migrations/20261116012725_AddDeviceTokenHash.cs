using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lantern.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceTokenHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TokenHash",
                table: "Devices",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "Devices");
        }
    }
}
