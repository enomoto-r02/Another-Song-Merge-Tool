using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Manager;
using Microsoft.Extensions.Configuration;

namespace Another_Song_Merge_Tool
{
    public class AppConfig
    {
        public static string FILE_INI = "Another Song Merge Tool.ini";

        static AppConfig Instance;
        public Config Config { get; set; }

        public DivaModManagerConfig DivaModManager { get; set; }

        public AppConfig() { }
        public static AppConfig Get()
        {
            if (Instance != null) return Instance;

            if (File.Exists(AppConfig.FILE_INI) == false)
            {
                return null;
            }
            else
            {

                Instance = new ConfigurationBuilder()
                .AddIniFile(AppConfig.FILE_INI)
                .Build()
                .Get<AppConfig>();
            }
            return Instance;
        }
    }
}
