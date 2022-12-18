using System.Security.Claims;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Community.API.Filters
{
    public class CustomerAuthorizationFilter : IAsyncActionFilter
    {
        private readonly IContextAccessor _contextAccessor;
        private readonly ICustomerService _customerService;

        public CustomerAuthorizationFilter(IContextAccessor contextAccessor, ICustomerService customerService)
        {
            _contextAccessor = contextAccessor;
            _customerService = customerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IEnumerable<Claim> claims = _contextAccessor.GetAuthorizationClaims();
            if (!claims.Any()) throw new HttpUnauthorizedException();

            Claim? userIdClaim = claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new HttpUnauthorizedException("Invalid claims!");

            int userId = int.Parse(userIdClaim.Value);
            Customer? customer = await _customerService.GetAsync(userId);
            if (customer == null) throw new HttpUnauthorizedException("Invalid user!");

            bool isAuthorized = false;
            if (!isAuthorized)
            {
                if (customer != null) isAuthorized = true;
            }

            if (!isAuthorized) throw new HttpForbiddenException();
            _contextAccessor.SetUser(customer);

            await next();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomerAuthorizationAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IContextAccessor? contextAccessor = serviceProvider.GetService<IContextAccessor>();
            if (contextAccessor == null) throw new NullReferenceException(nameof(contextAccessor));

            ICustomerService? customerService = serviceProvider.GetService<ICustomerService>();
            if (customerService == null) throw new NullReferenceException(nameof(customerService));

            CustomerAuthorizationFilter authorizationFilter = new(contextAccessor, customerService);
            return authorizationFilter;
        }
    }
}
