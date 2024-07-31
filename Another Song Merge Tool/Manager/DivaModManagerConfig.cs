namespace Another_Song_Merge_Tool.Manager
{
    public class DivaModManagerConfig
    {
        public string Path { get; set; }
        public string Config { get; set; }
        public string ConfigPath { get { return this.Path + "\\" + this.Config; } }
        public bool AddPrefix { get; set; }
        public bool AnotherSongMark { get; set; }
    }
}
