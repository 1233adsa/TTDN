using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDN.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentColumnsToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueTime",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "DepositAmount",
            table: "Bookings");

            // Xóa cột DueTime
            migrationBuilder.DropColumn(
            name: "DueTime",
            table: "Bookings");
        }
    }
}
