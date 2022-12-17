using System.ComponentModel.DataAnnotations.Schema;
using Community.Domain.Models.Abstract;

namespace Community.Domain.Models
{
    [Table(nameof(Customer) + "s")]
    public class Customer : User
    {

    }
}
