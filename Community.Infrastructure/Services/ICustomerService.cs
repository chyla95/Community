using Community.Domain.Models;

namespace Community.Infrastructure.Services
{
    public interface ICustomerService : IUserService<Customer>
    {
        Task ConvertToEmployee(Customer entityBefore, Employee entityAfter);
    }
}
