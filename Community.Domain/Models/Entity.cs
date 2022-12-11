using System.ComponentModel.DataAnnotations;

namespace Community.Domain.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
