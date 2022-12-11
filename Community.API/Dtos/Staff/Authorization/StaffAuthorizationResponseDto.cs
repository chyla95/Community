using Community.API.Dtos.Staff.Role;

namespace Community.API.Dtos.Staff.Authorization
{
#pragma warning disable CS8618
    public class StaffAuthorizationResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<RoleResponseDto> Roles { get; set; }
        public string JwtToken { get; set; }
    }
#pragma warning restore CS8618
}
