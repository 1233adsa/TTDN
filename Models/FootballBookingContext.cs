using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TTDN.Models;

public partial class FootballBookingContext : DbContext
{
    public FootballBookingContext()
    {
    }

    public FootballBookingContext(DbContextOptions<FootballBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<FieldSchedule> FieldSchedules { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__46A222CDA411E071");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("role")
                .HasDefaultValue("user");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone")
                .HasDefaultValue("0");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B1928A0C9E");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.BookingDate).HasColumnName("booking_date");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Account).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Bookings__accoun__412EB0B6");

            entity.HasOne(d => d.Field).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FieldId)
                .HasConstraintName("FK__Bookings__field___4222D4EF");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.FieldId).HasName("PK__Fields__1BB6F43E853A85A4");

            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.FieldName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("field_name");
        });

        modelBuilder.Entity<FieldSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__FieldSch__C46A8A6F22392731");

            entity.ToTable("FieldSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.BookingDate).HasColumnName("booking_date");
            entity.Property(e => e.BookingId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("booking_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Booking).WithMany(p => p.FieldSchedules)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__FieldSche__booki__4D94879B");

            entity.HasOne(d => d.Field).WithMany(p => p.FieldSchedules)
                .HasForeignKey(d => d.FieldId)
                .HasConstraintName("FK__FieldSche__field__4CA06362");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__ED1FC9EAA301BDF7");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.Method)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("method");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Payments__bookin__47DBAE45");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.PriceId).HasName("PK__Prices__1681726DFBC52325");

            entity.Property(e => e.PriceId).HasColumnName("price_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.Price1)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Field).WithMany(p => p.Prices)
                .HasForeignKey(d => d.FieldId)
                .HasConstraintName("FK__Prices__field_id__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
