using System.ComponentModel.DataAnnotations.Schema;
using Community.Domain.Models.Abstract;

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
            bool isAdministrator = Roles.Any(r => r.IsAdministrator);
            if (isAdministrator) return true;
            return false;
        }

        public bool HasPermission(Permission permission)
        {
            bool hasPermission = permission switch
            {
                Permission.ManageRoles => Roles.Any(r => r.CanManageRoles),
                Permission.CanManageEmployees => Roles.Any(r => r.CanManageEmployees),
                Permission.CanManageCustomers => Roles.Any(r => r.CanManageCustomers),
                _ => throw new NotImplementedException("Missing mapping for this permission!"),
            };
            return hasPermission;
        }
    }
#pragma warning restore CS8618
}
