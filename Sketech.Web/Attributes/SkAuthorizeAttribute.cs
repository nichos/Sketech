using Sketech.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sketech.Web.Attributes
{
    public class SkAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(Roles))
            {
                return;
            }

            var filters = FilterProviders.Providers.GetFilters(filterContext.Controller.ControllerContext, filterContext.ActionDescriptor).Where(o => o.Instance.GetType().Name == GetType().Name).ToList();

            if (filters.Count() > 1)
            {
                var filter = filters.FirstOrDefault(o => ReferenceEquals((SkAuthorizeAttribute)o.Instance, this));
                if (filter != null & filter.Scope == FilterScope.Controller)
                {
                    return;
                }
            }

            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;
            var userName = Thread.CurrentPrincipal.Identity.Name;
            var service = new IdentityService();
            var hasPermission = false;
            Task.Run(async () =>
            {
                var response = await service.CheckUserForAction(userName, controllerName, actionName);
                hasPermission = response.Value;
            }).Wait();

            if (!hasPermission)
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}