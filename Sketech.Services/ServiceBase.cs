using Sketech.Dals;
using Sketech.Infrastructure.ExceptionHandlering;
using System;
using System.Threading.Tasks;

namespace Sketech.Services
{
    public class ServiceBase
    {
        public SkRepositorySession GetRepositorySession()
        {
            return new SkRepositorySession();
        }

        protected ServiceResponse<T> ExecuteService<T>(Func<ServiceResponse<T>, T> func, T defaultValue = default(T))
        {
            var response = new ServiceResponse<T>();
            var result = defaultValue;
            try
            {
                result = func(response);
                response.Value = result;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Value = defaultValue;
                SkExceptionHandler.HandleServiceException(ex);
            }

            return response;
        }

        protected async Task<ServiceResponse<T>> ExecuteServiceAsync<T>(Func<ServiceResponse<T>, Task<T>> func, T defaultValue = default(T))
        {
            var response = new ServiceResponse<T>();
            var result = defaultValue;
            try
            {
                result = await func(response);
                response.Value = result;
            }
            catch(Exception ex)
            {
                response.Error = ex;
                response.Value = defaultValue;
                SkExceptionHandler.HandleServiceException(ex);
            }

            return response;
        }
    }
}
