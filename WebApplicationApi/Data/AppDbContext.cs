using Microsoft.EntityFrameworkCore;

using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;

namespace WebApplicationApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public AppDbContext() { }

    public DbSet<ProductModel> Products { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<BookingModel> Bookings { get; set; }
    public DbSet<StoreItemModel> BookStore { get; set; }
    public DbSet<RoleModel> Roles { get; set; }
    public DbSet<BookingStatusModel> BookingStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingStatusModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<BookingStatusModel>().HasData(
            new BookingStatusModel { Id = (int)BookingStatus.Submitted, Name = BookingStatus.Submitted.ToString() },
            new BookingStatusModel { Id = (int)BookingStatus.Approved, Name = BookingStatus.Approved.ToString() },
            new BookingStatusModel { Id = (int)BookingStatus.InDelivery, Name = BookingStatus.InDelivery.ToString() },
            new BookingStatusModel { Id = (int)BookingStatus.Cancelled, Name = BookingStatus.Cancelled.ToString() },
            new BookingStatusModel { Id = (int)BookingStatus.Rejected, Name = BookingStatus.Rejected.ToString() }
        );

        modelBuilder.Entity<RoleModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RoleModel>().HasData(
            new RoleModel { Id = (int)Role.Admin, Name = Role.Admin.ToString() },
            new RoleModel { Id = (int)Role.Manager, Name = Role.Manager.ToString() },
            new RoleModel { Id = (int)Role.Customer, Name = Role.Customer.ToString() }
        );

        modelBuilder.Entity<ProductModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.Author)
                .HasColumnName("author");
            entity.Property(e => e.Price)
                .HasColumnName("price");
            entity.Property(e => e.ImagePath)
                .HasColumnName("image_path");
        });

        modelBuilder.Entity<StoreItemModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<ProductModel>()
                .WithMany()
                .HasForeignKey(e => e.ProductId);

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");
            entity.Property(e => e.AvailableQuantity)
                .HasColumnName("available_qty");
            entity.Property(e => e.BookedQuantity)
                .HasColumnName("booked_qty");
            entity.Property(e => e.SoldQuantity)
                .HasColumnName("sold_qty");
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<RoleModel>()
                .WithMany()
                .HasForeignKey(e => e.RoleId);

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Login)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");
            entity.Property(e => e.Address)
                .HasColumnName("address");
            entity.Property(e => e.Phone)
                .HasColumnName("phone");
            entity.Property(e => e.Password)
                .HasColumnName("password");
        });

        modelBuilder.Entity<BookingModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<ProductModel>()
                .WithMany()
                .HasForeignKey(e => e.ProductId);
            entity.HasOne<BookingStatusModel>()
                .WithMany()
                .HasForeignKey(e => e.StatusId);
            entity.HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");
            entity.Property(e => e.StatusId)
                .HasColumnName("status_id");
            entity.Property(e => e.Date)
                .HasColumnName("date");
            entity.Property(e => e.DeliveryAddress)
                .HasColumnName("delivery_address");
            entity.Property(e => e.Time)
                .HasColumnName("time");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");
        });
    }
}
