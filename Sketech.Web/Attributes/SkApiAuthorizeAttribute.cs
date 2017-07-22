using Sketech.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sketech.Web.Attributes
{
    public class SkApiAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (!string.IsNullOrEmpty(Roles))
            {
                return;
            }

            var filterInfos = actionContext.ActionDescriptor.GetFilterPipeline().Where(o => o.Instance.GetType().Name == GetType().Name);
            if (filterInfos.Count() > 1)
            {
                var filter = filterInfos.FirstOrDefault(o => ReferenceEquals((SkAuthorizeAttribute)o.Instance, this));
                if (filter != null & filter.Scope == FilterScope.Controller)
                {
                    return;
                }
            }

            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = actionContext.ActionDescriptor.ActionName;
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
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}