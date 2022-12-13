using System.ComponentModel.DataAnnotations;
using Community.Domain.Models.Abstract;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    public class Permission : Entity
    {
        public enum ResourceType
        {
            Employee,
            Role,
            Customer,
        }

        public enum OperationType
        {
            Create,
            Read,
            Update,
            Delete,
        }

        [Required]
        public ResourceType Resource { get; set; }
        [Required]
        public OperationType Operation { get; set; }
    }
#pragma warning restore CS8618
}
