using Sketech.Infrastructure.Configurations;
using Sketech.Infrastructure.Ioc;
using Sketech.Infrastructure.Logging;
using System;

namespace Sketech.Infrastructure.ExceptionHandlering
{
    public static class SkExceptionHandler
    {
        private static void HandleExceptioin(Exception ex, SkExceptionHandleAction action)
        {
            //No action
            if(action== SkExceptionHandleAction.None)
            {
                return;
            }

            //log exception
            if(action == SkExceptionHandleAction.LogAndRethrow)
            {
                var logger = SkServiceLocator.Get<SkLoggerBase>();
                if (logger != null)
                {
                    logger.LogError(ex);
                }
            }

            // throw current exception
            if(action == SkExceptionHandleAction.Rethrow || action == SkExceptionHandleAction.LogAndRethrow)
            {
                throw ex;
            }
        }

        public static void HandleDataAccessException(Exception ex)
        {
            HandleExceptioin(ex, SkConfiguration.Current.AppSettings.DataAccessExceptionHandleAction);
        }

        public static void HandleServiceException(Exception ex)
        {
            HandleExceptioin(ex, SkConfiguration.Current.AppSettings.DataAccessExceptionHandleAction);
        }

        public static void HandleWebException(Exception ex)
        {
            HandleExceptioin(ex, SkConfiguration.Current.AppSettings.DataAccessExceptionHandleAction);
        }
    }
}
