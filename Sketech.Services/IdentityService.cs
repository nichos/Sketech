using Sketech.Dals.Repositories;
using Sketech.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sketech.Services
{
    public class IdentityService : ServiceBase
    {
        public async Task<ServiceResponse<IEnumerable<string>>> GetRoles(string applicationName)
        {
            return await ExecuteServiceAsync<IEnumerable<string>>(async response =>
            {
                using (var session = GetRepositorySession())
                {
                    var repo = new IdentityRepository(session);
                    var data = await repo.GetRoles(applicationName);

                    response.TotalResultCount = data.TotalResultCount;
                    return data.Value;
                }
            });
        }

        public async Task<ServiceResponse<IEnumerable<UsersInRoles>>> GetUsersInRoles(string applicationName, string username, string rolename)
        {
            return await ExecuteServiceAsync<IEnumerable<UsersInRoles>>(async response =>
            {
                using (var session = GetRepositorySession())
                {
                    var repo = new IdentityRepository(session);
                    var data = await repo.GetUsersInRoles(applicationName, username, rolename);

                    response.TotalResultCount = data.TotalResultCount;
                    return data.Value;
                }
            });
        }

        public async Task<ServiceResponse<bool>> CheckUserForAction(string username, string controllerName, string actionName)
        {
            return await ExecuteServiceAsync<bool>(async response =>
            {
                using (var session = GetRepositorySession())
                {
                    var repo = new IdentityRepository(session);
                    var data = await repo.CheckUserForAction(username, controllerName, actionName);

                    response.TotalResultCount = data.TotalResultCount;
                    return data.Value;
                }
            });
        }
    }
}
