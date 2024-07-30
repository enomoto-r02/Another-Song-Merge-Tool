namespace ModDbMerge2.Manager
{
    public class DivaModManagerConfig
    {
        public string Path { get; set; }
        public string Config { get; set; }
        public string ConfigPath { get { return this.Path + "\\" + this.Config; } }
        public bool AddPrefix { get; set; }
    }
}
