using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public double Price { get; set; }

    public string Description { get; set; } = null!;

    public int StockInQuantity { get; set; }

    public double RatingAverage { get; set; }

    public string Sku { get; set; } = null!;

    public double DiscountPrice { get; set; }

    public double DiscountPercent { get; set; }

    public int SoldQuantity { get; set; }

    public int AvailableQuantity { get; set; }

    public bool IsActive { get; set; }

    public bool IsSale { get; set; }

    public string Slug { get; set; } = null!;

    public int CategoriesId { get; set; }

    public int SupplierId { get; set; }

    public virtual Category Categories { get; set; } = null!;

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual Supplier Supplier { get; set; } = null!;
}
