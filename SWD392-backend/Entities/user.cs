using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

[Index("email", Name = "users_email_key", IsUnique = true)]
public partial class user
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string username { get; set; } = null!;

    [StringLength(255)]
    public string email { get; set; } = null!;

    [StringLength(255)]
    public string password { get; set; } = null!;

    public string role { get; set; } = null!;

    public DateTime created_at { get; set; }

    [StringLength(255)]
    public string phone { get; set; } = null!;

    [StringLength(255)]
    public string address { get; set; } = null!;

    [StringLength(255)]
    public string? image_url { get; set; }

    public bool is_active { get; set; }

    [StringLength(255)]
    public string full_name { get; set; } = null!;

    [InverseProperty("user")]
    public virtual ICollection<order> orders { get; set; } = new List<order>();

    [InverseProperty("user")]
    public virtual ICollection<product_review> product_reviews { get; set; } = new List<product_review>();

    [InverseProperty("user")]
    public virtual ICollection<supplier> suppliers { get; set; } = new List<supplier>();
}
