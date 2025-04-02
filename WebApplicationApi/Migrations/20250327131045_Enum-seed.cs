using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class Enumseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingStatus_status_id",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStatus",
                table: "BookingStatus");

            migrationBuilder.RenameTable(
                name: "BookingStatus",
                newName: "BookingStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStatuses",
                table: "BookingStatuses",
                column: "id");

            migrationBuilder.InsertData(
                table: "BookingStatuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Submitted" },
                    { 2, "Rejected" },
                    { 3, "Approved" },
                    { 4, "Cancelled" },
                    { 5, "InDelivery" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Manager" },
                    { 3, "Customer" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingStatuses_status_id",
                table: "Bookings",
                column: "status_id",
                principalTable: "BookingStatuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingStatuses_status_id",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingStatuses",
                table: "BookingStatuses");

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "BookingStatuses",
                newName: "BookingStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingStatus",
                table: "BookingStatus",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingStatus_status_id",
                table: "Bookings",
                column: "status_id",
                principalTable: "BookingStatus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
