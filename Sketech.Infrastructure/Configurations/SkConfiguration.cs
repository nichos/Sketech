namespace Sketech.Infrastructure.Configurations
{
    public class SkConfiguration
    {
        private static SkConfiguration _instance;

        public static SkConfiguration Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SkConfiguration();
                }
                return _instance;
            }
        }

        private SkConfiguration()
        {
            ConnectionStrings = new SkConnectionStrings();
            AppSettings = new SkAppSettings();
        }

        public SkConnectionStrings ConnectionStrings { get; private set; }
        public SkAppSettings AppSettings { get; private set; }

    }
}
