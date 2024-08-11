namespace Another_Song_Merge_Tool.DIVA
{
    public class Mod
    {
        public static string FILE_PV_DB = "pv_db.txt";
        public static string FILE_PV_MOD = "mod_pv_db.txt";
        public static string FILE_PV_MDATA = "mdata_pv_db.txt";

        public static string FILE_FIELD_DB = "pv_field.txt";
        public static string FILE_FIELD_MOD = "mod_pv_field.txt";
        public static string FILE_FIELD_MDATA = "mdata_pv_field.txt";

        public static string FILE_STAGE_BIN_DB = "stage_data.bin";
        public static string FILE_STAGE_BIN_MOD = "mod_stage_data.bin";
        public static string FILE_STAGE_BIN_MDATA = "mdata_stage_data.bin";

        public static string FILE_STAGE_XML_DB = "stage_data.xml";
        public static string FILE_STAGE_XML_MOD = "mod_stage_data.xml";
        public static string FILE_STAGE_XML_MDATA = "mdata_stage_data.xml";

        public string Name { get; set; }
        public string Folder_Path { get; set; }
        public PvDb Pv_Db { get; set; }
        public PvField Pv_Field { get; set; }
        public StageLogic Stage_Data { get; set; }
        public int Mod_Priority { get; set; }
        public int Db_Priority { get; set; }
        public int Field_Priority { get; set; }
        public int Stage_Priority { get; set; }
        public bool Enabled { get; set; }

        public Mod()
        {
            this.Mod_Priority = -1;
            this.Db_Priority = -1;
            this.Field_Priority = -1;
            this.Stage_Priority = -1;

            this.Pv_Db = new(this.Name, this.Db_Priority);
            this.Pv_Field = new(this.Name, this.Field_Priority);
            this.Stage_Data = new(this.Name, this.Stage_Priority);
        }

        public Mod(int priority, string name, string enabled, string folder_path) : this()
        {
            this.Mod_Priority = priority;
            this.Name = name;
            this.Enabled = bool.Parse(enabled);
            this.Folder_Path = folder_path;

            this.Pv_Db = new PvDb(this.Name, this.Mod_Priority);

            var mod_db_path = folder_path + "\\rom\\" + FILE_PV_MOD;
            var db_path = folder_path + "\\rom\\" + FILE_PV_DB;
            var mdata_db_path = folder_path + "\\rom\\" + FILE_PV_MDATA;

            if (File.Exists(mod_db_path))
            {
                this.Pv_Db.Db_Path = mod_db_path;
                this.Pv_Db.Db_Name = FILE_PV_MOD;
            }
            else if (File.Exists(db_path))
            {
                this.Pv_Db.Db_Path = db_path;
                this.Pv_Db.Db_Name = FILE_PV_MOD;
            }
            else if (File.Exists(mdata_db_path))
            {
                this.Pv_Db.Db_Path = mdata_db_path;
                this.Pv_Db.Db_Name = FILE_PV_MDATA;
            }
            else
            {
                this.Pv_Db.Db_Path = "";
                this.Pv_Db.Db_Name = "";
            }

            this.Pv_Field = new PvField(this.Name, this.Mod_Priority);

            var mod_field_path = folder_path + "\\rom\\" + FILE_FIELD_MOD;
            var field_path = folder_path + "\\rom\\" + FILE_FIELD_DB;
            var mdata_field_path = folder_path + "\\rom\\" + FILE_FIELD_MDATA;

            if (File.Exists(mod_field_path))
            {
                this.Pv_Field.Field_Path = mod_field_path;
                this.Pv_Field.Field_Name = FILE_FIELD_MOD;
            }
            else if (File.Exists(field_path))
            {
                this.Pv_Field.Field_Path = field_path;
                this.Pv_Field.Field_Name = FILE_FIELD_DB;
            }
            else if (File.Exists(mdata_field_path))
            {
                this.Pv_Field.Field_Path = mdata_field_path;
                this.Pv_Field.Field_Name = FILE_FIELD_MDATA;
            }
            else
            {
                this.Pv_Field.Field_Path = "";
                this.Pv_Field.Field_Name = "";
            }

            this.Stage_Data = new(this.Name, this.Stage_Priority);

            var mod_stage_path = folder_path + "\\rom\\" + FILE_STAGE_BIN_MOD;
            var stage_path = folder_path + "\\rom\\" + FILE_STAGE_BIN_DB;
            var mdata_stage_path = folder_path + "\\rom\\" + FILE_STAGE_BIN_MDATA;

            if (File.Exists(mod_stage_path))
            {
                this.Stage_Data.Stage_Bin_Path = mod_stage_path;
                this.Stage_Data.Stage_Bin_Name = FILE_STAGE_BIN_MOD;
            }
            else if (File.Exists(stage_path))
            {
                this.Stage_Data.Stage_Bin_Path = stage_path;
                this.Stage_Data.Stage_Bin_Name = FILE_STAGE_BIN_DB;
            }
            else if (File.Exists(mdata_stage_path))
            {
                this.Stage_Data.Stage_Bin_Path = mdata_stage_path;
                this.Stage_Data.Stage_Bin_Name = FILE_STAGE_BIN_MDATA;
            }
            else
            {
                this.Stage_Data.Stage_Bin_Path = "";
                this.Stage_Data.Stage_Bin_Name = "";
            }

        }

        public void LoadPvDb(List<Song> addAnotherSong, List<string> song_no_cnt, int now_pv_db_priority)
        {
            this.Pv_Db.Load(addAnotherSong, song_no_cnt);
            this.Db_Priority = this.Pv_Db.Db_Priority;      // ？
        }

        public void LoadPvField(List<Song> allFieldSong)
        {
            this.Pv_Field.Load(allFieldSong);
        }

        public bool InitStageData(AppConfig appConfig)
        {
            string tmpStageDataName = this.Stage_Data.Stage_Bin_Name;
            if (string.IsNullOrEmpty(tmpStageDataName))
            {
                return false;
            }
            else
            {
                this.Stage_Data.Init(appConfig, tmpStageDataName);
                return true;
            }
        }
        public bool LoadStageData(AppConfig appConfig)
        {
            if (string.IsNullOrEmpty(this.Stage_Data.Stage_Bin_Path) || string.IsNullOrEmpty(this.Stage_Data.Stage_Bin_Path))
            {
                return false;
            }
            else
            {
                string tmpStageDataName = Path.GetFileNameWithoutExtension(Mod.FILE_STAGE_BIN_MOD) + "_" + Program.start_dt + ".bin";
                this.Stage_Data.Load(appConfig, tmpStageDataName);
                return true;
            }
        }

        public static void EndStageData(AppConfig appConfig)
        {
            StageLogic.End(appConfig);
        }
    }
}
