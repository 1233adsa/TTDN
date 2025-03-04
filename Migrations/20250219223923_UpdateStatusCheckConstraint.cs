using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDN.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa ràng buộc cũ
            migrationBuilder.Sql("ALTER TABLE Bookings DROP CONSTRAINT IF EXISTS CK__Bookings__status__403A8C7D;");

            // Thêm ràng buộc mới với giá trị mở rộng
            migrationBuilder.Sql("ALTER TABLE Bookings ADD CONSTRAINT Bookings_status_check CHECK (status IN ('pending', 'confirmed', 'cancelled', 'refunded'));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nếu muốn rollback, khôi phục lại ràng buộc cũ
            migrationBuilder.Sql("ALTER TABLE Bookings DROP CONSTRAINT IF EXISTS CK__Bookings__status__403A8C7D;");
            migrationBuilder.Sql("ALTER TABLE Bookings ADD CONSTRAINT CK__Bookings__status__403A8C7D CHECK (status IN ('pending', 'confirmed', 'cancelled'));");
        }

    }
}
