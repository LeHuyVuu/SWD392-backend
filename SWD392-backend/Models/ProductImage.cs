using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string ProductImageUrl { get; set; } = null!;

    public int ProductsId { get; set; }

    public virtual Product Products { get; set; } = null!;
}
