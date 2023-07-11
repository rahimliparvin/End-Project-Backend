namespace Savoy.Models
{
    public class WishlistItem : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
