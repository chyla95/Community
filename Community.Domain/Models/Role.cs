using System.ComponentModel.DataAnnotations;
using Community.Domain.Models.Abstract;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    public class Role : Entity
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
