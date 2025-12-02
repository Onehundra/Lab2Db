using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab2Db.Models;

public partial class Lab2DbContext : DbContext
{
    public Lab2DbContext()
    {
    }

    public Lab2DbContext(DbContextOptions<Lab2DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butiker> Butikerna { get; set; }

    public virtual DbSet<Böcker> Böckerna { get; set; }

    public virtual DbSet<Författare> Författarna { get; set; }

    public virtual DbSet<Kunder> Kunderna { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldon { get; set; }

    public virtual DbSet<Ordrar> Ordrarna { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=Kebab\\SQLEXPRESS;Initial Catalog=Lab1;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Butiker__3214EC272B3027D0");

            entity.ToTable("Butiker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ButikensNamn).HasMaxLength(100);
            entity.Property(e => e.GatuAdress).HasMaxLength(100);
            entity.Property(e => e.PostNummer).HasMaxLength(10);
            entity.Property(e => e.Stad).HasMaxLength(100);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Böcker__3BF79E0332167351");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.FörfattareId).HasColumnName("FörfattareID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Språk).HasMaxLength(100);
            entity.Property(e => e.Titel).HasMaxLength(100);

            entity.HasOne(d => d.Författare).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörfattareId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Böcker_Författare");
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC27A4FA820D");

            entity.ToTable("Författare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(100);
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.KundId).HasName("PK__Kunder__F2B5DEAC73461941");

            entity.ToTable("Kunder");

            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.Efternamn).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(100);
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn });

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LagerSaldo_Butik");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LagerSaldo_Bok");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Ordrar__C3905BAFDAA16A9E");

            entity.ToTable("Ordrar");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.OrderDatum).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Kund).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.KundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordrar_Kunder");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
