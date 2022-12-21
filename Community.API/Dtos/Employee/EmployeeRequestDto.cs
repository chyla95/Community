using System.ComponentModel.DataAnnotations;

namespace Community.API.Dtos.Employee
{
#pragma warning disable CS8618
    public class EmployeeRequestDto
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
