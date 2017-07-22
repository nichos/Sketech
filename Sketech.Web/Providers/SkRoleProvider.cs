using Sketech.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;

namespace Sketech.Web.Providers
{
    public class SkRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                return "SkProtoType";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var users = GetUsersInRole(roleName);
            return users.Where(o => o.StartsWith(usernameToMatch)).ToArray();
        }

        public override string[] GetAllRoles()
        {
            var service = new IdentityService();
            var result = new string[] { };
            Task.Run(async () =>
            {
                var roles = await service.GetRoles(this.ApplicationName);
                result = roles.Value.ToArray();
            }).Wait();

            return result;
        }

        public override string[] GetRolesForUser(string username)
        {
            var service = new IdentityService();
            var result = new string[] { };
            Task.Run(async () =>
            {
                var roles = await service.GetUsersInRoles(this.ApplicationName, username, null);
                result = roles.Value.Select(o => o.Role).ToArray();
            }).Wait();

            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var service = new IdentityService();
            var result = new string[] { };
            Task.Run(async () =>
            {
                var roles = await service.GetUsersInRoles(this.ApplicationName, null, roleName);
                result = roles.Value.Select(o => o.Username).ToArray();
            }).Wait();

            return result;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var service = new IdentityService();
            var result = false;
            Task.Run(async () =>
            {
                var roles = await service.GetUsersInRoles(this.ApplicationName, username, roleName);
                result = roles.TotalResultCount > 0;
            }).Wait();

            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            var roles = GetAllRoles();
            return roles != null && roles.Contains(roleName);
        }
    }
}