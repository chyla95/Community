using System.ComponentModel.DataAnnotations;

namespace Community.Domain.Models.Abstract
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
