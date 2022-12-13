using Community.API.Dtos.Employee.Role;

namespace Community.API.Dtos.Employee.Authorization
{
#pragma warning disable CS8618
    public class EmployeeAuthorizationResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<RoleResponseDto> Roles { get; set; }
        public string JwtToken { get; set; }
    }
#pragma warning restore CS8618
}
