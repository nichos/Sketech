using Sketech.Infrastructure.ExceptionHandlering;
using System;
using System.Configuration;

namespace Sketech.Infrastructure.Configurations
{
    public class SkAppSettings
    {
        public int SqlCommandTimeout
        {
            get
            {
                var result = 300;
                var timeoutStr = ConfigurationManager.AppSettings["Sql.CommandTimeout"];
                if (!string.IsNullOrEmpty(timeoutStr))
                {
                    int timeout;
                    if (int.TryParse(timeoutStr, out timeout))
                    {
                        result = timeout;
                    }
                    else
                    {
                        result = 300;
                    }
                }
                return result;
            }

        }

        private SkExceptionHandleAction GetExceptionActionFromConfig(string config, SkExceptionHandleAction defaultAction)
        {
            var actionStr = ConfigurationManager.AppSettings[config];
            if (!string.IsNullOrEmpty(actionStr))
            {
                SkExceptionHandleAction action;
                if (Enum.TryParse(actionStr, out action))
                {
                    return action;
                }

                return defaultAction;
            }

            return defaultAction;
        }

        public SkExceptionHandleAction ServiceExceptionHandleAction
        {
            get
            {
                return GetExceptionActionFromConfig("Exception.ServiceExceptionAction", SkExceptionHandleAction.LogAndRethrow);
            }
        }

        public SkExceptionHandleAction DataAccessExceptionHandleAction
        {
            get
            {
                return GetExceptionActionFromConfig("Exception.DataAccessExceptionAction", SkExceptionHandleAction.LogAndRethrow);
            }
        }

        public SkExceptionHandleAction WebExceptionHandleAction
        {
            get
            {
                return GetExceptionActionFromConfig("Exception.WebExceptionAction", SkExceptionHandleAction.Log);
            }
        }

    }
}
