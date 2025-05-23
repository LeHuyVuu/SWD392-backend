using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class ProductAttribute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public virtual ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();

    public virtual Product Product { get; set; } = null!;
}
