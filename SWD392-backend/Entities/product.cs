using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

[Index("name", Name = "products_name_index")]
public partial class product
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime created_at { get; set; }

    public double price { get; set; }

    public string description { get; set; } = null!;

    public int stock_in_quantity { get; set; }

    public double rating_average { get; set; } = 0.0;

    [StringLength(255)]
    public string sku { get; set; } = null!;

    public double discount_price { get; set; }

    public double discount_percent { get; set; } = 0.0;

    public int sold_quantity { get; set; } = 0;

    public int available_quantity { get; set; }

    public bool is_active { get; set; }

    public bool is_sale { get; set; }

    [StringLength(255)]
    public string slug { get; set; } = null!;

    public int categories_id { get; set; }

    public int supplier_id { get; set; }

    [ForeignKey("categories_id")]
    [InverseProperty("products")]
    public virtual category categories { get; set; } = null!;

    [InverseProperty("product")]
    public virtual ICollection<product_attribute> product_attributes { get; set; } = new List<product_attribute>();

    [InverseProperty("products")]
    public virtual ICollection<product_image> product_images { get; set; } = new List<product_image>();

    [InverseProperty("product")]
    public virtual ICollection<product_review> product_reviews { get; set; } = new List<product_review>();

    [ForeignKey("supplier_id")]
    [InverseProperty("products")]
    public virtual supplier supplier { get; set; } = null!;
}
