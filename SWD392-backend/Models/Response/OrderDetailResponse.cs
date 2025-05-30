namespace SWD392_backend.Models.Response;

public class OrderDetailResponse
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double DiscountPercent { get; set; }
    public string Note { get; set; } = null!;
}