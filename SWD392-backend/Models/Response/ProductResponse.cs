namespace SWD392_backend.Models.Response
{
    public class ProductResponse
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public DateTime created_at { get; set; }
        public double price { get; set; }
        public string description { get; set; } = null!;
        public int stock_in_quantity { get; set; }
        public double rating_average { get; set; }
        public string sku { get; set; } = null!;
        public double discount_price { get; set; }
        public double discount_percent { get; set; }
        public int sold_quantity { get; set; }
        public int available_quantity { get; set; }
        public bool is_active { get; set; }
        public bool is_sale { get; set; }
        public string slug { get; set; } = null!;
        public CategoryResponse categories { get; set; }
        public SupplierrResponse supplier { get; set; }
    }
}
