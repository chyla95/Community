using System.ComponentModel.DataAnnotations;

namespace Community.API.Dtos.Customer
{
#pragma warning disable CS8618
    public class CustomerRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5), MaxLength(50)]
        public string Password { get; set; }
    }
#pragma warning restore CS8618
}
