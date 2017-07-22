using System;
using System.Collections.Generic;
using Sketech.Infrastructure.Logging;
using System.Data.SqlClient;
using Sketech.Dals.Helper;
using System.Threading;
using Sketech.Infrastructure.Configurations;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sketech.Entities.Logging;
using System.Collections;
using System.Linq;
using Sketech.Dals.Extensions;

namespace Sketech.Dals.Logging
{
    public class SkDbLogger : SkLoggerBase
    {
        private SqlConnection GetLoggingConnection()
        {
            var connStr = SkConfiguration.Current.ConnectionStrings.Logging;

            // create sql connection and return
            var conn = new SqlConnection(connStr);
            return conn;
        }

        #region Audit Log

        protected override async Task LogAuditEventAsync(string eventName, string detail, Dictionary<string, string> props)
        {
            try
            {
                var user = Thread.CurrentPrincipal.Identity.Name;
                var eventDetail = detail;
                if(props != null && props.Count > 0)
                {
                    var propStr = JsonConvert.SerializeObject(props);
                    eventDetail += $"\n{propStr}";
                }

                var parms = new List<SqlParameter>
                {
                    AdoHelper.CreateSqlParameter<string>("@EventName", eventName),
                    AdoHelper.CreateSqlParameter<string>("@EventDetail", eventDetail),
                    AdoHelper.CreateSqlParameter<string>("@ActionUser", user)
                };

                using (var conn = GetLoggingConnection())
                {
                    await AdoHelper.ExecuteStoredProcedureAsync(conn, "dbo.uspWriteAuditLog", parms);
                }
            }
            catch
            {

            }
        }

        private async Task<object> ConvertToAuditLogEntry(SqlDataReader data)
        {
            return new AuditLogEntry
            {
                Id = await data.GetValueAsync<Guid>("Id"),
                EventDetail = await data.GetValueAsync<string>("EventDetail"),
                EventName = await data.GetValueAsync<string>("EventName"),
                ActionUser = await data.GetValueAsync<string>("ActionUser"),
                Timestamp = await data.GetValueAsync<DateTime>("Timestamp")
            };
        }

        public async Task<IList<AuditLogEntry>> GetAuditLogs(int skip = 0, int take = 100)
        {
            try
            {
                var user = Thread.CurrentPrincipal.Identity.Name;

                var parms = new List<SqlParameter>
                {
                    AdoHelper.CreateSqlParameter("@Skip", skip),
                    AdoHelper.CreateSqlParameter("@Take", take),
                };

                var converters = new List<Func<SqlDataReader, Task<object>>> { ConvertToAuditLogEntry };

                using (var conn = GetLoggingConnection())
                {
                    var data = await AdoHelper.ExecuteStoredProcedureAsync(conn, "dbo.uspGetAuditLog", parms, converters);
                    var entries = data[0].Cast<AuditLogEntry>().ToList();

                    return entries;
                }
            }
            catch
            {

            }

            return null;
        }

        #endregion

        #region System Log

        protected override async Task LogSystemEventAsync(SkLogLevel logLevel, string message, string detail)
        {
            try
            {
                var user = Thread.CurrentPrincipal.Identity.Name;
                var parms = new List<SqlParameter>
                {
                    AdoHelper.CreateSqlParameter<string>("@LogLevel", logLevel.ToString()),
                    AdoHelper.CreateSqlParameter<string>("@LogMessage", message),
                    AdoHelper.CreateSqlParameter<string>("@LogDetail", detail),
                    AdoHelper.CreateSqlParameter<string>("@ActionUser", user)
                };

                using (var conn = GetLoggingConnection())
                {
                    await AdoHelper.ExecuteStoredProcedureAsync(conn, "dbo.uspWriteSystemLog", parms);
                }
            }
            catch
            {

            }
        }

        #endregion
    }
}
