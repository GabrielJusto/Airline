using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Migrations
{
    /// <inheritdoc />
    public partial class Add_RouteId_On_Fligh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Routes_RouteID",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "RouteID",
                table: "Flights",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_RouteID",
                table: "Flights",
                newName: "IX_Flights_RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Routes_RouteId",
                table: "Flights",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Routes_RouteId",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Flights",
                newName: "RouteID");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_RouteId",
                table: "Flights",
                newName: "IX_Flights_RouteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Routes_RouteID",
                table: "Flights",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "RouteID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
