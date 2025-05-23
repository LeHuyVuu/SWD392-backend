using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Models;

public partial class DefaultdbContext : DbContext
{

    public DefaultdbContext()
    {
   
    }

    public DefaultdbContext(DbContextOptions<DefaultdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("order_status", new[] { "pending", "preparing", "delivery", "delivered", "returned", "cancelled", "refunding", "refunded" });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveriedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deliveried_at");
            entity.Property(e => e.PaidAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paid_at");
            entity.Property(e => e.ShippingPrice).HasColumnName("shipping_price");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_supplier");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_user");
        });

        modelBuilder.Entity<OrdersDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_detail_pkey");

            entity.ToTable("orders_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductAttributeId).HasColumnName("product_attribute_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrdersDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_detail_order");

            entity.HasOne(d => d.ProductAttribute).WithMany(p => p.OrdersDetails)
                .HasForeignKey(d => d.ProductAttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_detail_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.Name, "products_name_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvailableQuantity).HasColumnName("available_quantity");
            entity.Property(e => e.CategoriesId).HasColumnName("categories_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.DiscountPrice).HasColumnName("discount_price");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsSale).HasColumnName("is_sale");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.RatingAverage).HasColumnName("rating_average");
            entity.Property(e => e.Sku)
                .HasMaxLength(255)
                .HasColumnName("sku");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.SoldQuantity).HasColumnName("sold_quantity");
            entity.Property(e => e.StockInQuantity).HasColumnName("stock_in_quantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Categories).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_suppliers");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_attribute_pkey");

            entity.ToTable("product_attribute");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_attribute_product");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_images_pkey");

            entity.ToTable("product_images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductImageUrl)
                .HasMaxLength(255)
                .HasColumnName("product_image_url");
            entity.Property(e => e.ProductsId).HasColumnName("products_id");

            entity.HasOne(d => d.Products).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_images_products");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_reviews_pkey");

            entity.ToTable("product_reviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_reviews_product");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_reviews_user");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsVerified).HasColumnName("is_verified");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.RegisteredAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registered_at");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_suppliers_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });
        modelBuilder.HasSequence("product_images_seq");
        modelBuilder.HasSequence("products_id_seq");
        modelBuilder.HasSequence("products_images_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
