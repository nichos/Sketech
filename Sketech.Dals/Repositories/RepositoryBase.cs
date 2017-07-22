using Sketech.Dals.Helper;
using Sketech.Infrastructure.ExceptionHandlering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sketech.Dals.Repositories
{
    public abstract class RepositoryBase
    {
        private SkRepositorySession Session { get; }

        protected RepositoryBase(SkRepositorySession session)
        {
            this.Session = session;
        }
        
        private string GetDatabaseConnectionInfo()
        {
            return $"{Session.GetConnection().Database}, {Session.GetConnection().DataSource}";
        }

        private string GenerateErrorMessage(string sqlStatement, IList<SqlParameter> parameters, Exception ex,
           int resultSet)
        {
            var error = GetDatabaseConnectionInfo() + Environment.NewLine + sqlStatement;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    error += Environment.NewLine +
                             $"  {parameter.ParameterName} = {parameter.Value}";
                }
            }

            if (ex.GetType() != typeof(SqlException))
            {
                error += Environment.NewLine + $"  Error in setters[{resultSet}]";
            }
            return error;
        }

        private int GetCommandTimeout(int timeout)
        {
            return timeout < 0 ? Session.SqlCommandTimeout : timeout;
        }

        #region Execute StoreProcedure
        protected async Task ExecuteStoredProcedureAsync(string spName, IList<SqlParameter> parameters, int timeout = -1)
        {
            try
            {
                var conn = Session.GetConnection();

                await AdoHelper.ExecuteStoredProcedureAsync(conn, spName, parameters, GetCommandTimeout(timeout));
            }
            catch (Exception ex)
            {
                var error = GenerateErrorMessage(spName, parameters, ex, 0);
                throw new InvalidOperationException(error, ex);
            }
        }

        protected async Task<IList<IEnumerable<object>>> ExecuteStoredProcedureAsync(string spName, IList<SqlParameter> parameters,
            IList<Func<SqlDataReader, Task<object>>> setters, int timeout = -1)
        {
            try
            {
                timeout = timeout < -1 ? Session.SqlCommandTimeout : timeout;
                var result = new List<IEnumerable<object>>();
                if (setters == null)
                {
                    await ExecuteStoredProcedureAsync(spName, parameters);
                    return null;
                }

                var enumerable =
                    setters as Func<SqlDataReader, Task<object>>[];
                if (!enumerable.Any())
                {
                    await ExecuteStoredProcedureAsync(spName, parameters);
                    return null;
                }

                var conn = Session.GetConnection();
                return await AdoHelper.ExecuteStoredProcedureAsync(conn, spName, parameters, setters, timeout);
            }
            catch (Exception ex)
            {
                var error = GenerateErrorMessage(spName, parameters, ex, 0);
                throw new InvalidOperationException(error, ex);
            }
        }
        #endregion

        #region SelectDbView
        protected async Task<IEnumerable<T>> SelectDbViewAsync<T>(string sqlStatement,
            Func<SqlDataReader, Task<T>> setter, SqlParameter[] parameters = null, int timeout = -1)
        {
            try
            {
                var result = new List<T>();

                var conn = Session.GetConnection();
                return await AdoHelper.SelectDbViewAsync<T>(conn, sqlStatement, setter, parameters, GetCommandTimeout(timeout));
            }
            catch (Exception ex)
            {
                var error = GenerateErrorMessage(sqlStatement, parameters, ex, 0);
                throw new InvalidOperationException(error, ex);
            }
        }
        #endregion

        protected DataAccessResult<T> ExecuteAdataAccess<T>(Func<DataAccessResult<T>, T> func, T defaultValue = default(T))
        {
            var response = new DataAccessResult<T>();
            var result = defaultValue;
            try
            {
                result = func(response);
                response.Value = result;
            }
            catch(Exception ex)
            {
                response.Error = ex;
                response.Value = defaultValue;
                SkExceptionHandler.HandleDataAccessException(ex);
            }

            return response;
        }

        protected async Task<DataAccessResult<T>> ExecuteAdataAccessAsync<T>(Func<DataAccessResult<T>, Task<T>> func, T defaultValue = default(T))
        {
            var response = new DataAccessResult<T>();
            var result = defaultValue;
            try
            {
                result = await func(response);
                response.Value = result;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Value = defaultValue;
                SkExceptionHandler.HandleDataAccessException(ex);
            }

            return response;
        }
    }
}