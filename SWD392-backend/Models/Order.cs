using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public double Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public double ShippingPrice { get; set; }

    public string Address { get; set; } = null!;

    public int SupplierId { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? DeliveriedAt { get; set; }

    public virtual ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
