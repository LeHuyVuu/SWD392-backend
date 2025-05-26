using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

public partial class order
{
    [Key]
    public Guid id { get; set; }

    public double total { get; set; }

    public DateTime created_at { get; set; }

    public int user_id { get; set; }

    public double shipping_price { get; set; }

    [StringLength(255)]
    public string address { get; set; } = null!;

    public int supplier_id { get; set; }

    public DateTime? paid_at { get; set; }

    public DateTime? deliveried_at { get; set; }

    [InverseProperty("product")]
    public virtual ICollection<orders_detail> orders_details { get; set; } = new List<orders_detail>();

    [ForeignKey("supplier_id")]
    [InverseProperty("orders")]
    public virtual supplier supplier { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("orders")]
    public virtual user user { get; set; } = null!;
}
