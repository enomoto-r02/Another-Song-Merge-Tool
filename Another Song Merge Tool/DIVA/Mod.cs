namespace Another_Song_Merge_Tool.DIVA
{
    public class Mod
    {
        public string Name { get; set; }
        public string Folder_Path { get; set; }
        public PvDb Pv_Db { get; set; }
        public int Priority { get; set; }
        public int Pv_Db_Priority { get; set; }
        public bool Enabled { get; set; }
        public string Pv_Db_Path { get; set; }
        public string Pv_Db_Name { get; set; }

        public Mod()
        {
            this.Pv_Db = new PvDb(this.Name);
            this.Priority = -1;
            this.Pv_Db_Priority = -1;

        }

        public Mod(int priority, string name, string enabled, string folder_path) : this()
        {
            this.Priority = priority;
            this.Name = name;
            this.Enabled = bool.Parse(enabled);
            this.Folder_Path = folder_path;
            this.Pv_Db_Priority = -1;

            var mod_pv_path = folder_path + "\\rom\\" + "mod_pv_db.txt";
            var pv_path = folder_path + "\\rom\\" + "pv_db.txt";

            if (File.Exists(mod_pv_path))
            {
                this.Pv_Db_Path = mod_pv_path;
                this.Pv_Db_Name = "mod_pv_db.txt";
            }
            else if (File.Exists(pv_path))
            {
                this.Pv_Db_Path = pv_path;
                this.Pv_Db_Name = "pv_db.txt";
            }
            else
            {
                this.Pv_Db_Path = "";
                this.Pv_Db_Name = "";
            }
        }

        public void LoadPvDb(List<Song> addAnotherSong, List<string> song_no_cnt, int now_pv_db_priority)
        {
            this.Pv_Db = new PvDb(this.Name, this.Pv_Db_Name, now_pv_db_priority);
            this.Pv_Db.Load(addAnotherSong, song_no_cnt, this.Priority, this.Pv_Db_Path);
            this.Pv_Db_Priority = this.Pv_Db.Pv_Db_Priority;
        }
    }
}
