namespace SWD392_backend.Models.Request
{
    public class UploadMainProductImgRequest
    {
        public int ProductId { get; set; }
        public string ProductSlug { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId {  get; set; }
        public string ContentType { get; set; }
    }
}
