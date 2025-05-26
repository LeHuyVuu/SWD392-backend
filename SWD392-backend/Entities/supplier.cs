using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

public partial class supplier
{
    [Key]
    public int id { get; set; }

    public int user_id { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    [StringLength(255)]
    public string slug { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime registered_at { get; set; }

    public bool is_verified { get; set; }

    public string description { get; set; } = null!;

    [StringLength(255)]
    public string image_url { get; set; } = null!;

    [InverseProperty("supplier")]
    public virtual ICollection<order> orders { get; set; } = new List<order>();

    [InverseProperty("supplier")]
    public virtual ICollection<product> products { get; set; } = new List<product>();

    [ForeignKey("user_id")]
    [InverseProperty("suppliers")]
    public virtual user user { get; set; } = null!;
}
