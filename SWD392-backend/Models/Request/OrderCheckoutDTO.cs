namespace cybersoft_final_project.Models.Request;

public class OrderCheckoutDTO
{
    public int SupplierId { get; set; }
    public string Address { get; set; } = "";
    public double ShippingPrice { get; set; }
    public double Total { get; set; }
    public List<OrderDetailDTO> OrderDetails { get; set; } = new();
}