namespace Savoy.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get;set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}
