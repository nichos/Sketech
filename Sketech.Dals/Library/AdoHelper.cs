using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sketech.Dals.Library
{
    public class AdoHelper
    {
        public static SqlParameter CreateSqlParameter<T>(string parmName, T data,
            ParameterDirection direction = ParameterDirection.Input, SqlDbType? dbType = null)
        {
            SqlParameter parm;
            if (data == null)
            {
                var type = typeof(T);
                var underlyType = Nullable.GetUnderlyingType(type);
                if (underlyType != null)
                {
                    type = underlyType;
                }

                var typeName = type.Name;
                switch (typeName)
                {
                    case "Guid":
                        parm = new SqlParameter(parmName, SqlDbType.UniqueIdentifier, 16);
                        break;
                    case "Int32":
                        parm = new SqlParameter(parmName, SqlDbType.Int, 4);
                        break;
                    case "Int16":
                        parm = new SqlParameter(parmName, SqlDbType.SmallInt, 2);
                        break;
                    case "Int64":
                        parm = new SqlParameter(parmName, SqlDbType.BigInt, 8);
                        break;
                    case "Decimal":
                        parm = new SqlParameter(parmName, SqlDbType.Decimal, 18);
                        break;
                    case "Byte":
                        parm = new SqlParameter(parmName, SqlDbType.TinyInt, 1);
                        break;
                    case "Boolean":
                        parm = new SqlParameter(parmName, SqlDbType.Bit, 1);
                        break;
                    case "String":
                        parm = new SqlParameter(parmName, SqlDbType.NVarChar, -1);
                        break;
                    case "DateTime":
                        parm = new SqlParameter(parmName, SqlDbType.DateTime);
                        break;
                    case "DataTable":
                        parm = new SqlParameter(parmName, SqlDbType.Structured);
                        break;
                    default:
                        throw new Exception("Can't set sqldbtype in sql parametr");
                }
                parm.Value = DBNull.Value;
            }
            else
            {
                parm = new SqlParameter(parmName, data);
            }

            parm.Direction = direction;
            if (dbType.HasValue)
            {
                parm.SqlDbType = dbType.Value;
            }

            return parm;
        }

        #region Execute StoreProcedure

        public static async Task ExecuteStoredProcedureAsync(SqlConnection conn, string spName, IList<SqlParameter> parameters, int commandTimeout = 300)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = commandTimeout;
                if (conn.State ==
                    ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }

                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    cmd.Parameters.Clear();
                    throw;
                }
            }
        }

        public static async Task<IList<IEnumerable<object>>> ExecuteStoredProcedureAsync(SqlConnection conn, string spName, IList<SqlParameter> parameters,
            IList<Func<SqlDataReader, Task<object>>> setters, int commandTimeout = 300)
        {
            var resultSet = 0;
            var result = new List<IEnumerable<object>>();
            if (setters == null)
            {
                await ExecuteStoredProcedureAsync(conn, spName, parameters, commandTimeout);
                return null;
            }

            var enumerable =
                setters as Func<SqlDataReader, Task<object>>[] ??
                setters.ToArray();
            if (!enumerable.Any())
            {
                await ExecuteStoredProcedureAsync(conn, spName, parameters, commandTimeout);
                return null;
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = commandTimeout;
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (resultSet < setters.Count)
                    {
                        var returnData = new List<object>();
                        var setter = enumerable[resultSet];
                        while (await reader.ReadAsync())
                        {
                            var data = await setter(reader);
                            returnData.Add(data);
                        }
                        result.Add(returnData);
                        resultSet++;
                        if (resultSet < setters.Count)
                        {
                            await reader.NextResultAsync();
                        }
                    }
                }
            }

            return result;

        }
        #endregion

        #region SelectDbView
        public static async Task<IEnumerable<T>> SelectDbViewAsync<T>(SqlConnection conn, string sqlStatement,
            Func<SqlDataReader, Task<T>> setter, SqlParameter[] parameters = null, int commandtimeout = 300)
        {
            var result = new List<T>();

            using (var cmd = new SqlCommand(sqlStatement, conn))
            {
                cmd.CommandTimeout = commandtimeout;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStatement;
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = await setter(reader);
                            result.Add(data);
                        }
                    }
                }
                catch
                {
                    cmd.Parameters.Clear();
                    throw;
                }
            }

            return result;
        }
        #endregion
    }
}
