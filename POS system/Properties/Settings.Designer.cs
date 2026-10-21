using System.Configuration;

namespace POS_system.Properties
{
    internal sealed class Settings
    {
        private static readonly Settings defaultInstance = new Settings();

        private Settings()
        {
        }

        public static Settings Default
        {
            get { return defaultInstance; }
        }

        public string TEMPOConnectionString
        {
            get { return GetConnectionString("POS_system.Properties.Settings.TEMPOConnectionString"); }
        }

        public string StockDBConnectionString
        {
            get { return GetConnectionString("POS_system.Properties.Settings.StockDBConnectionString"); }
        }

        public string StockDBConnectionString1
        {
            get { return GetConnectionString("POS_system.Properties.Settings.StockDBConnectionString1"); }
        }

        public string StockDBConnectionString2
        {
            get { return GetConnectionString("POS_system.Properties.Settings.StockDBConnectionString2"); }
        }

        public string StockDBConnectionString3
        {
            get { return GetConnectionString("POS_system.Properties.Settings.StockDBConnectionString3"); }
        }

        private static string GetConnectionString(string name)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[name];

            if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                throw new ConfigurationErrorsException(
                    "Connection string '" + name + "' is missing from App.config.");

            return setting.ConnectionString;
        }
    }
}
