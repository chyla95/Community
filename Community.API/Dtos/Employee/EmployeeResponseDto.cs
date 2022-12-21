using Community.API.Dtos.Role;

namespace Community.API.Dtos.Employee
{
#pragma warning disable CS8618
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<RoleResponseDto>? Roles { get; set; }
    }
#pragma warning restore CS8618
}
