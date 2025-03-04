using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTDN.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Active" // Bạn có thể đổi giá trị mặc định nếu cần
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Fields"
            );
        }

    }
}
