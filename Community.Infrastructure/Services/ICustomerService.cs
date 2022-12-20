using Community.Domain.Models;

namespace Community.Infrastructure.Services
{
    public interface ICustomerService : IUserService<Customer>
    {
        Task ConvertToEmployeeAsync(Customer entityBefore, Employee entityAfter);
    }
}
