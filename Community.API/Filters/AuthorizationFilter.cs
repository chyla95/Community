using System.Security.Claims;
using Community.API.Utilities;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using static Community.Domain.Models.Employee;

namespace Community.API.Filters
{
    public class AuthorizationFilter : IAsyncActionFilter
    {
        private readonly IContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;

        public IEnumerable<Permission>? Permissions { get; set; }
        public bool OnlyAdministrators { get; set; }


        public AuthorizationFilter(IContextAccessor contextAccessor,IEmployeeService employeeService)
        {
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IEnumerable<Claim> claims = _contextAccessor.GetAuthorizationClaims();
            if (!claims.Any()) throw new HttpUnauthorizedException();

            Claim? userIdClaim = claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new HttpUnauthorizedException("Invalid claims!");

            int userId = int.Parse(userIdClaim.Value);
            Employee? employee = await _employeeService.GetAsync(userId);
            if (employee == null) throw new HttpUnauthorizedException("Invalid user!");

            bool isAuthorized = false;
            if (!isAuthorized)
            {
                if (OnlyAdministrators && employee.IsAdministrator()) isAuthorized = true;
            }
            if (!isAuthorized)
            {
                if (!OnlyAdministrators && employee.IsAdministrator()) isAuthorized = true;
            }
            if (!isAuthorized)
            {
                if (Permissions.IsNullOrEmpty()) isAuthorized = true;
            }
            if (!isAuthorized)
            {
                bool hasAllPermissions = true;
                foreach (Permission permission in Permissions!)
                {
                    if (!employee.HasPermission(permission)) hasAllPermissions = false;
                }
                if (hasAllPermissions) isAuthorized = true;
            }

            if(!isAuthorized) throw new HttpForbiddenException();
            _contextAccessor.SetUser(employee);

            await next();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizationAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable { get; set; }
        public IEnumerable<Permission>? Permissions { get; private set; }
        public bool OnlyAdministrators { get; set; }

        public AuthorizationAttribute() { }
        public AuthorizationAttribute(Permission permission)
        {
            Permissions = new HashSet<Permission> { permission };
        }
        public AuthorizationAttribute(Permission[] permissions)
        {
            Permissions = new HashSet<Permission>(permissions);
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IContextAccessor? contextAccessor = serviceProvider.GetService<IContextAccessor>();
            if (contextAccessor == null) throw new NullReferenceException(nameof(contextAccessor));

            IEmployeeService? employeeService = serviceProvider.GetService<IEmployeeService>();
            if (employeeService == null) throw new NullReferenceException(nameof(employeeService));

            AuthorizationFilter authorizationFilter = new(contextAccessor, employeeService)
            {
                Permissions = Permissions,
                OnlyAdministrators = OnlyAdministrators
            };
            return authorizationFilter;
        }
    }
}
