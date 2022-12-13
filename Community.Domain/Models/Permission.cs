using System.ComponentModel.DataAnnotations;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    public class Permission : Entity
    {
        public enum ResourceType
        {
            Staff,
            Role,
            Customer
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
