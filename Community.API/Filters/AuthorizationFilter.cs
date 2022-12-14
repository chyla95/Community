using System.Diagnostics;
using System.Security;
using Community.API.Utilities.Accessors;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using static Community.Domain.Models.Employee;

namespace Community.API.Filters
{
    public class AuthorizationFilter : IActionFilter
    {
        private readonly IContextAccessor _contextAccessor;

        public IEnumerable<Permission>? Permissions { get; set; }
        public bool OnlyAdministrators { get; set; }


        public AuthorizationFilter(IContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool isUserAuthenticated = _contextAccessor.IsUserAuthenticated<Employee>();
            if (!isUserAuthenticated) throw new HttpUnauthorizedException();

            Employee user = _contextAccessor.GetUser<Employee>();

            if (OnlyAdministrators && !user.IsAdministrator()) throw new HttpForbiddenException();
            if (!OnlyAdministrators && user.IsAdministrator()) return;

            if (Permissions.IsNullOrEmpty()) return;
            foreach (Permission permission in Permissions!)
            {
                if (!user.HasPermission(permission)) throw new HttpForbiddenException();
            }

            return;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
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

            AuthorizationFilter authorizationFilter = new(contextAccessor)
            {
                Permissions = Permissions,
                OnlyAdministrators = OnlyAdministrators
            };
            return authorizationFilter;
        }
    }
}
