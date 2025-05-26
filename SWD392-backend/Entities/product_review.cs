using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

public partial class product_review
{
    [Key]
    public int id { get; set; }

    public int product_id { get; set; }

    public string content { get; set; } = null!;

    public int rating { get; set; }

    public int user_id { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime created_at { get; set; }

    [ForeignKey("product_id")]
    [InverseProperty("product_reviews")]
    public virtual product product { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("product_reviews")]
    public virtual user user { get; set; } = null!;
}
