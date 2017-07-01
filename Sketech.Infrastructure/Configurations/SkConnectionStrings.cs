using System.Configuration;

namespace Sketech.Infrastructure.Configurations
{
    public class SkConnectionStrings
    {
        public string Primary
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PrimaryConnectionString"].ConnectionString;
            }
        }

        public string Logging
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["LoggingConnectionString"].ConnectionString;
            }
        }
    }
}
