using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_backend.Entities;

public partial class product_image
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string product_image_url { get; set; } = null!;
    public bool is_main { get; set; }

    public int products_id { get; set; }

    [ForeignKey("products_id")]
    [InverseProperty("product_images")]
    public virtual product products { get; set; } = null!;
}
