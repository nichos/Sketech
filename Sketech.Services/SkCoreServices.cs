using Sketech.Dals.Logging;
using Sketech.Infrastructure.Ioc;
using Sketech.Infrastructure.Logging;

namespace Sketech.Services
{
    public static class SkCoreServices
    {
        public static void Initialize()
        {
            // Register Logger
            SkServiceLocator.Register<SkLoggerBase>(new SkDbLogger());
        }
    }
}
