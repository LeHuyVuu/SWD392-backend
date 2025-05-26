namespace cybersoft_final_project.Models.Request;

public class OrderRequest
{
    public List<OrderDetailRequest> OrderDetails { get; set; } = new();
}