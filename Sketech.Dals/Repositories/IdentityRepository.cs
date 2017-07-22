using Sketech.Dals.Extensions;
using Sketech.Dals.Helper;
using Sketech.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketech.Dals.Repositories
{
    public class IdentityRepository : RepositoryBase
    {
        public IdentityRepository(SkRepositorySession session) : base(session)
        {
        }

        #region Roles

        private async Task<string> ConvertToRole(SqlDataReader data)
        {
            return await data.GetValueAsync<string>("Rolename");
        }

        public async Task<DataAccessResult<IEnumerable<string>>> GetRoles(string applicationName)
        {
            return await ExecuteAdataAccessAsync<IEnumerable<string>>(async response =>
            {
                var query = new StringBuilder();
                query.Append(@"SELECT Rolename");
                query.Append(" FROM dbo.vwRoles");
                query.Append($" WHERE ApplicationName = '{applicationName}'");
                query.Append(" ORDER BY Rolename");

                var returnData = await SelectDbViewAsync(query.ToString(), ConvertToRole);
                var result = returnData.ToList();
                response.TotalResultCount = result.Count;
                return result;
            });

        }

        #endregion

        #region UsersInRoles
        private async Task<UsersInRoles> ConvertToUsersInRoles(SqlDataReader data)
        {
            return new UsersInRoles
            {
                Role = await data.GetValueAsync<string>("Rolename"),
                UserId = await data.GetValueAsync<Guid>("UserId"),
                Username = await data.GetValueAsync<string>("Username")
            };
        }

        public async Task<DataAccessResult<IEnumerable<UsersInRoles>>> GetUsersInRoles(string applicationName, string userName, string roleName)
        {
            return await ExecuteAdataAccessAsync<IEnumerable<UsersInRoles>>(async response =>
            {
                var query = new StringBuilder();
                query.Append(@"SELECT UserId, Username, Rolename");
                query.Append(" FROM dbo.vwUsersInRoles");
                query.Append($" WHERE ApplicationName = '{applicationName}'");
                if (!string.IsNullOrEmpty(userName))
                {
                    query.Append($" AND Username = '{userName}'");
                }
                if (!string.IsNullOrEmpty(roleName))
                {
                    query.Append($" AND RoleName = '{roleName}'");
                }
                query.Append(" ORDER BY Username, Rolename");

                var returnData = await SelectDbViewAsync(query.ToString(), ConvertToUsersInRoles);
                var result = returnData.ToList();
                response.TotalResultCount = result.Count;
                return result;
            });
        }
        #endregion

        #region UserAction
        public async Task<DataAccessResult<bool>> CheckUserForAction(string username, string controllerName, string actionName)
        {
            return await ExecuteAdataAccessAsync<bool>(async response =>
            {
                var hasPermission = AdoHelper.CreateSqlParameter("@HasPermission", false, ParameterDirection.Output);
                var parameters = new List<SqlParameter>
                {
                    AdoHelper.CreateSqlParameter("@Username", username),
                    AdoHelper.CreateSqlParameter("@Controller", controllerName),
                    AdoHelper.CreateSqlParameter("@Action", actionName),
                    hasPermission
                };
                await ExecuteStoredProcedureAsync("dbo.uspCheckUserForAction", parameters);

                var result = Convert.ToBoolean(hasPermission.Value);
                response.TotalResultCount = 1;
                return result;
            });
        }
        #endregion
    }
}
