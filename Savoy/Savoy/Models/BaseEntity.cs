namespace Savoy.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool SoftDelete { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime Updated { get; set; } = DateTime.UtcNow.AddHours(4);

    }
}
