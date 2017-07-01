using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketech.Infrastructure.Ioc
{
    public static class SkServiceLocator
    {
        private static Dictionary<string, object> _registeredServices = new Dictionary<string, object>();

        public static void Register<T>(object serviceInstance)
        {
            var key = nameof(T);
            if (_registeredServices.ContainsKey(key))
            {
                _registeredServices[key] = serviceInstance;
            }
            else
            {
                _registeredServices.Add(key, serviceInstance);
            }
        }

        public static T Get<T>()
        {
            var key = nameof(T);
            if (_registeredServices.ContainsKey(key))
            {
                return (T)_registeredServices[key];
            }

            return default(T);
        }
    }
}
