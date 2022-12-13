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
        public bool IsAdministrator { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanManageEmployees { get; set; }
        public bool CanManageCustomers { get; set; }
    }
#pragma warning restore CS8618
}
