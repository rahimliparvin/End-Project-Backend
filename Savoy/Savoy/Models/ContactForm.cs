namespace Savoy.Models
{
    public class ContactForm : BaseEntity
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Message { get; set; }

    }
}
