using System.ComponentModel.DataAnnotations;

namespace Community.Domain.Models
{
#pragma warning disable CS8618
    public class Role : Entity
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        public IEnumerable<Staff> Staff { get; set; }
    }
#pragma warning restore CS8618
}
