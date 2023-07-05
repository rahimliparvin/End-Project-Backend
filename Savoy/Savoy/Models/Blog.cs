namespace Savoy.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
