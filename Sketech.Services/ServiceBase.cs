using Sketech.Dals;

namespace Sketech.Services
{
    public class ServiceBase
    {
        public SkRepositorySession GetRepositorySession()
        {
            return new SkRepositorySession();
        }
    }
}
