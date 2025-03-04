using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDN.Migrations
{
    public partial class UpdateStatusBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa ràng buộc cũ (nếu có)
            migrationBuilder.Sql(@"
                DECLARE @constraintName NVARCHAR(200);
                SELECT @constraintName = name 
                FROM sys.check_constraints 
                WHERE parent_object_id = OBJECT_ID('dbo.Bookings') AND name LIKE 'Bookings_status_check%';

                IF @constraintName IS NOT NULL
                BEGIN
                    EXEC('ALTER TABLE Bookings DROP CONSTRAINT ' + @constraintName);
                END
            ");

            // Thêm ràng buộc mới
            migrationBuilder.Sql("ALTER TABLE Bookings ADD CONSTRAINT Bookings_status_check CHECK (status IN ('pending', 'confirmed', 'cancelled', 'refunded', 'refunding'));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa ràng buộc mới
            migrationBuilder.Sql(@"
                DECLARE @constraintName NVARCHAR(200);
                SELECT @constraintName = name 
                FROM sys.check_constraints 
                WHERE parent_object_id = OBJECT_ID('dbo.Bookings') AND name LIKE 'Bookings_status_check%';

                IF @constraintName IS NOT NULL
                BEGIN
                    EXEC('ALTER TABLE Bookings DROP CONSTRAINT ' + @constraintName);
                END
            ");

            // Khôi phục lại ràng buộc cũ
            migrationBuilder.Sql("ALTER TABLE Bookings ADD CONSTRAINT Bookings_status_check CHECK (status IN ('pending', 'confirmed', 'cancelled', 'refunded'));");
        }
    }
}

