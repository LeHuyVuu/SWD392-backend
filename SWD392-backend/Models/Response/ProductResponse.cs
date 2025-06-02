namespace SWD392_backend.Models.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string Slug { get; set; }
        public double RatingAverage { get; set; }
        public bool IsSale { get; set; }
        public int StockInQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoriesId { get; set; }
        public int SupplierId { get; set; }
    }
}
