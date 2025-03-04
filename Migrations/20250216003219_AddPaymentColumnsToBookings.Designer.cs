﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TTDN.Models;

#nullable disable

namespace TTDN.Migrations
{
    [DbContext(typeof(FootballBookingContext))]
    [Migration("20250216003219_AddPaymentColumnsToBookings")]
    partial class AddPaymentColumnsToBookings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TTDN.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("user")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("username");

                    b.Property<string>("phone")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasDefaultValue("0")
                        .HasColumnName("phone");

                    b.HasKey("AccountId")
                        .HasName("PK__Accounts__46A222CDA411E071");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TTDN.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("booking_date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time")
                        .HasColumnName("end_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time")
                        .HasColumnName("start_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("status");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total_price");

                    b.HasKey("BookingId")
                        .HasName("PK__Bookings__5DE3A5B1928A0C9E");

                    b.HasIndex("AccountId");

                    b.HasIndex("FieldId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TTDN.Models.Field", b =>
                {
                    b.Property<int>("FieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FieldId"));

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("field_name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FieldId")
                        .HasName("PK__Fields__1BB6F43E853A85A4");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("TTDN.Models.FieldSchedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("schedule_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("booking_date");

                    b.Property<int?>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("booking_id")
                        .HasDefaultValueSql("(NULL)");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time")
                        .HasColumnName("end_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time")
                        .HasColumnName("start_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("status");

                    b.HasKey("ScheduleId")
                        .HasName("PK__FieldSch__C46A8A6F22392731");

                    b.HasIndex("BookingId");

                    b.HasIndex("FieldId");

                    b.ToTable("FieldSchedule", (string)null);
                });

            modelBuilder.Entity("TTDN.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("payment_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("amount");

                    b.Property<int?>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("method");

                    b.Property<DateTime?>("PaymentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("payment_date")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("status");

                    b.HasKey("PaymentId")
                        .HasName("PK__Payments__ED1FC9EAA301BDF7");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TTDN.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("price_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PriceId"));

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time")
                        .HasColumnName("end_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<decimal>("Price1")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("price");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time")
                        .HasColumnName("start_time");

                    b.HasKey("PriceId")
                        .HasName("PK__Prices__1681726DFBC52325");

                    b.HasIndex("FieldId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("TTDN.Models.Booking", b =>
                {
                    b.HasOne("TTDN.Models.Account", "Account")
                        .WithMany("Bookings")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Bookings__accoun__412EB0B6");

                    b.HasOne("TTDN.Models.Field", "Field")
                        .WithMany("Bookings")
                        .HasForeignKey("FieldId")
                        .HasConstraintName("FK__Bookings__field___4222D4EF");

                    b.Navigation("Account");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("TTDN.Models.FieldSchedule", b =>
                {
                    b.HasOne("TTDN.Models.Booking", "Booking")
                        .WithMany("FieldSchedules")
                        .HasForeignKey("BookingId")
                        .HasConstraintName("FK__FieldSche__booki__4D94879B");

                    b.HasOne("TTDN.Models.Field", "Field")
                        .WithMany("FieldSchedules")
                        .HasForeignKey("FieldId")
                        .HasConstraintName("FK__FieldSche__field__4CA06362");

                    b.Navigation("Booking");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("TTDN.Models.Payment", b =>
                {
                    b.HasOne("TTDN.Models.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId")
                        .HasConstraintName("FK__Payments__bookin__47DBAE45");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TTDN.Models.Price", b =>
                {
                    b.HasOne("TTDN.Models.Field", "Field")
                        .WithMany("Prices")
                        .HasForeignKey("FieldId")
                        .HasConstraintName("FK__Prices__field_id__3D5E1FD2");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("TTDN.Models.Account", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("TTDN.Models.Booking", b =>
                {
                    b.Navigation("FieldSchedules");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("TTDN.Models.Field", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("FieldSchedules");

                    b.Navigation("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
