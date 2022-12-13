using Community.API.Dtos.Employee.Permission;

namespace Community.API.Dtos.Employee.Role
{
#pragma warning disable CS8618
    public class RoleResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdministrator { get; set; }

        public IEnumerable<PermissionResponseDto> Permissions { get; set; }
    }
#pragma warning restore CS8618
}
