using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Authentication
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MinLength(5)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Secret { get; set; } = string.Empty;
        public bool Remember { get; set; }
    }
}
