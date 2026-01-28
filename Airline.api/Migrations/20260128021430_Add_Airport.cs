using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Airline.Migrations
{
    /// <inheritdoc />
    public partial class Add_Airport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "FromAirportId",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToAirportId",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IATACode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FromAirportId",
                table: "Routes",
                column: "FromAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ToAirportId",
                table: "Routes",
                column: "ToAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airports_FromAirportId",
                table: "Routes",
                column: "FromAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airports_ToAirportId",
                table: "Routes",
                column: "ToAirportId",
                principalTable: "Airports",
                principalColumn: "AirportId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airports_FromAirportId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airports_ToAirportId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FromAirportId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ToAirportId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "FromAirportId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ToAirportId",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
