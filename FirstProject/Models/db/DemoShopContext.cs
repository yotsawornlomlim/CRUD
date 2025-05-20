using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Models.db;

public partial class DemoShopContext : DbContext
{
    public DemoShopContext()
    {
    }

    public DemoShopContext(DbContextOptions<DemoShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Publish> Publishes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = .\\SQLEXPRESS; Database = DemoShop; Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_Table_3");

            entity.ToTable("Book");

            entity.Property(e => e.BookId)
                .HasMaxLength(50)
                .HasColumnName("BookID");
            entity.Property(e => e.BookName).HasMaxLength(100);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.PublishId).HasColumnName("PublishID");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Category");

            entity.HasOne(d => d.Publish).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Publish");

        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Table_1");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Publish>(entity =>
        {
            entity.HasKey(e => e.PublishId).HasName("PK_Table_2");

            entity.ToTable("Publish");

            entity.Property(e => e.PublishId).HasColumnName("PublishID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ContactName).HasMaxLength(100);
            entity.Property(e => e.PublishName).HasMaxLength(100);
            entity.Property(e => e.Telephone).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
