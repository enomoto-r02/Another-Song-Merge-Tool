using Microsoft.Extensions.Configuration;
using ModDbMerge2.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModDbMerge2
{
    public class AppConfig
    {
        static AppConfig Instance;

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
