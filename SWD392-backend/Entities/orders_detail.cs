using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

[Table("orders_detail")]
public partial class orders_detail
{
    [Key]
    public int id { get; set; }

    public Guid product_id { get; set; }

    public int quantity { get; set; }

    public double price { get; set; }

    public double discount_percent { get; set; }

    public string note { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("orders_details")]
    public virtual order product { get; set; } = null!;
}
