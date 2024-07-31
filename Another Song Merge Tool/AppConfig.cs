using Another_Song_Merge_Tool.Manager;
using Microsoft.Extensions.Configuration;

namespace Another_Song_Merge_Tool
{
    public class AppConfig
    {
        static AppConfig Instance;
        public Config Config { get; set; }

        public DivaModManagerConfig DivaModManager { get; set; }

        public AppConfig() { }
        public static AppConfig Get()
        {
            if (Instance != null) return Instance;

            Instance = new ConfigurationBuilder()
                .AddIniFile(".\\Another Song Merge Tool.ini")
                .Build()
                .Get<AppConfig>();

            return Instance;
        }
    }
}
