using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

public partial class category
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    [StringLength(255)]
    public string slug { get; set; } = null!;

    [StringLength(255)]
    public string image_url { get; set; } = null!;

    [InverseProperty("categories")]
    public virtual ICollection<product> products { get; set; } = new List<product>();
}
