namespace Another_Song_Merge_Tool.Manager
{
    public class DivaModManagerConfig
    {

        public static string FILE_CONFIG = "Config.json";
        public string Path { get; set; }
        public string ConfigPath { get { return this.Path + "\\" + FILE_CONFIG; } }

        public DivaModManagerConfig()
        {
        }
    }
}
