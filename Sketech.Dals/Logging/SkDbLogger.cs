using System;
using System.Collections.Generic;
using Sketech.Infrastructure.Logging;
using System.Data.SqlClient;
using Sketech.Dals.Helper;
using System.Threading;
using Sketech.Infrastructure.Configurations;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

                var conn = GetLoggingConnection();
                await AdoHelper.ExecuteStoredProcedureAsync(conn, "dbo.uspWriteAuditLog", parms);
            }
            catch
            {

            }
        }

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

                var conn = GetLoggingConnection();
                await AdoHelper.ExecuteStoredProcedureAsync(conn, "dbo.uspWriteSystemLog", parms);
            }
            catch
            {

            }
        }
    }
}
