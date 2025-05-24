using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class OrdersDetail
{
    public int Id { get; set; }

    public Guid OrderId { get; set; }

    public int ProductAttributeId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public double DiscountPercent { get; set; }

    public string Note { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ProductAttribute ProductAttribute { get; set; } = null!;
}
