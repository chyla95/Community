namespace Community.API.Dtos.Employee.Role
{
#pragma warning disable CS8618
    public class RoleRequestDto
    {
        public string Name { get; set; }
        public bool IsAdministrator { get; set; }
        public bool CanManageRoles { get; set; }
        public bool CanManageEmployees { get; set; }
        public bool CanManageCustomers { get; set; }
    }
#pragma warning restore CS8618
}
