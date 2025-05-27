namespace cybersoft_final_project.Models.Request;

public class UpdateProductRequest
{
    public string name { get; set; } = null!;
    public double price { get; set; }
    public string description { get; set; } = null!;
    public int stock_in_quantity { get; set; }
    public string sku { get; set; } = null!;
    public int categoryId { get; set; }
    public int supplierId { get; set; }
}