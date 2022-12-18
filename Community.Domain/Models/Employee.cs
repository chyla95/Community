using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.IdentityModel.Tokens;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    [Table(nameof(Employee) + "s")]
    public class Employee : Customer
    {
        public IEnumerable<Role>? Roles { get; set; }

        public enum Permission
        {
            ManageRoles,
            CanManageEmployees,
            CanManageCustomers,
        }

        public bool IsAdministrator()
        {
            if (Roles.IsNullOrEmpty()) return false;
            bool isAdministrator = Roles!.Any(r => r.IsAdministrator);
            if (!isAdministrator) return false;
            return true;
        }

        public bool HasPermission(Permission permission)
        {
            if (Roles.IsNullOrEmpty()) return false;
            bool hasPermission = permission switch
            {
                Permission.ManageRoles => Roles!.Any(r => r.CanManageRoles),
                Permission.CanManageEmployees => Roles!.Any(r => r.CanManageEmployees),
                Permission.CanManageCustomers => Roles!.Any(r => r.CanManageCustomers),
                _ => throw new NotImplementedException("Missing mapping for this permission!"),
            };
            return hasPermission;
        }
    }
#pragma warning restore CS8618
}
