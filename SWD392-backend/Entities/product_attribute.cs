using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

[Table("product_attribute")]
public partial class product_attribute
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    public int product_id { get; set; }

    public int quantity { get; set; }

    public double price { get; set; }

    [InverseProperty("product_attribute")]
    public virtual ICollection<orders_detail> orders_details { get; set; } = new List<orders_detail>();

    [ForeignKey("product_id")]
    [InverseProperty("product_attributes")]
    public virtual product product { get; set; } = null!;
}
