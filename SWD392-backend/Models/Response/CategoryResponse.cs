using System.ComponentModel.DataAnnotations;

namespace SWD392_backend.Models.Response
{
    public class CategoryResponse
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public string image_url { get; set; } = null!;
    }
}
