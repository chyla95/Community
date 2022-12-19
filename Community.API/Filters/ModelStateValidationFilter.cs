using Community.API.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Community.API.Filters
{
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ModelState.IsValid) return;

            IEnumerable<string> errorKeys = filterContext.ModelState.Keys;

            IEnumerable<HttpExceptionFieldMessage> exceptions = errorKeys
                .Where(ek => ek != null)
                .SelectMany(ek =>
                {
                    ModelStateEntry modelStateEntry = filterContext.ModelState[ek]!;
                    return modelStateEntry.Errors.Select(me => new HttpExceptionFieldMessage(ek, me.ErrorMessage));
                });

            throw new HttpValidationException(exceptions);
        }
    }
}
