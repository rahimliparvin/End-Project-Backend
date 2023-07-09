using System.ComponentModel.DataAnnotations;

namespace Savoy.ViewModels.Account
{
    public class LoginsVM
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
