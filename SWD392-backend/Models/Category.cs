using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
