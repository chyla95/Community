using System.ComponentModel.DataAnnotations;

namespace Community.API.Dtos.Role
{
#pragma warning disable CS8618
    public class RoleRequestDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public bool IsAdministrator { get; set; }
        [Required]
        public bool CanManageRoles { get; set; }
        [Required]
        public bool CanManageEmployees { get; set; }
        [Required]
        public bool CanManageCustomers { get; set; }
    }
#pragma warning restore CS8618
}
