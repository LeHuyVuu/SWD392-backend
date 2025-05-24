using System;
using System.Collections.Generic;

namespace SWD392_backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public string FullName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
