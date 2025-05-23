using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class ProductReview
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Content { get; set; } = null!;

    public int Rating { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
