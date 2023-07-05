namespace Savoy.Models
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
        public bool HoverImage { get; set; } = false;
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
