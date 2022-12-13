using Community.Domain.Models.Abstract;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    public class Employee : User
    {
        public IEnumerable<Role> Roles { get; set; }
    }
#pragma warning restore CS8618
}
