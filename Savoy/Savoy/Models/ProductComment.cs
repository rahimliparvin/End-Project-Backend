namespace Savoy.Models
{
    public class ProductComment : BaseEntity
    {
        public string Comment { get;set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
