using Microsoft.Extensions.Configuration;
using ModDbMerge2.DIVA;
using System.Linq;
using System.Text;

namespace ModDbMerge2.Manager
{
    public class DivaModManager
    {
        const string CONFIGS = "Configs";
        const string GAME_NAME = "Project DIVA Mega Mix\u002B";
        const string CURRENT_LOAD_OUT = "CurrentLoadout";
        const string LOAD_OUTS = "Loadouts";
        const string MOD_FOLDER = "ModsFolder";

        public List<Mod> Mods { get; }
        public Mod Merge_mod { get; }

        public List<string> Song_no_cnt { get; }

        public List<Song> Add_AnotherSong { get; set; }
        public List<string> Add_Song { get; }


        public DivaModManager()
        {
            this.Song_no_cnt = [];
            this.Add_AnotherSong = [];
            this.Add_Song = [];
        }
        public DivaModManager(AppConfig Config) : this()
        {
            // DMMのConfig.json読み込み
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Config.DivaModManager.ConfigPath, optional: true);

            var build = builder.Build();

            var current_loadout_join = string.Join(":", CONFIGS, GAME_NAME, CURRENT_LOAD_OUT);
            var CurrentLoadout = build[current_loadout_join];

            const string NAME = "name";
            const string ENABLED = "enabled";


            var i = 0;
            var name_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), NAME);
            var enabled_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), ENABLED);
            var mod_folder_join = string.Join(":", CONFIGS, GAME_NAME, MOD_FOLDER);

            List<Mod> mods = new List<Mod>();

            var mod_name = build[name_join];
            var mod_enabled = build[enabled_join];
            var mod_folder = build[mod_folder_join] + "\\" + mod_name;
            var pv_db_priority = 0;

            while (mod_name != null)
            {
                Mod mod = new Mod(i, mod_name, mod_enabled, mod_folder);

                if (mod.Pv_Db_Priority != -1)
                {
                    mod.Pv_Db_Priority = pv_db_priority;
                    pv_db_priority++;
                }
                if (mod.Enabled)
                {
                    mods.Add(mod);
                }

                i++;
                name_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), NAME);
                enabled_join = string.Join(":", CONFIGS, GAME_NAME, LOAD_OUTS, CurrentLoadout, i.ToString(), ENABLED);
                mod_folder = build[mod_folder_join] + "\\" + mod_name;

                mod_name = build[name_join];
                mod_enabled = build[enabled_join];
                mod_folder = build[mod_folder_join] + "\\" + mod_name;
            }

            this.Mods = mods;
        }

        public void ReadPvDb()
        {
            var now_pv_db_priority = 0;
            for (var i = 0; i < Mods.Count; i++)
            {
                if (Mods[i].Enabled)
                {
                    Mods[i].ReadPvDb(this.Add_AnotherSong, this.Song_no_cnt, now_pv_db_priority);
                    if (Mods[i].Pv_Db_Priority >= 0)
                    {
                        now_pv_db_priority++;
                    }
                }
            }
        }

        public string ToStringLengthLine()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pv_no in this.Add_AnotherSong.GroupBy(x => x.Pv_No))
            {
                var cnt = this.Add_AnotherSong.Where(x => x.Pv_No == pv_no.Key).Count();
                sb.AppendLine(string.Format("{0}.another_song.length={1}", pv_no.Key, cnt));
            }


            return sb.ToString();
        }

        public void ToStringLengthLine(List<string> addLines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pv_no in this.Add_AnotherSong.GroupBy(x => x.Pv_No))
            {
                var cnt = this.Add_AnotherSong.Where(x => x.Pv_No == pv_no.Key).Count();
                addLines.Add(string.Format("{0}.another_song.length={1}", pv_no.Key, cnt));
            }
        }

        public Mod Composition_old()
        {
            Mod composition_mod = new Mod();

            foreach (var mod in Mods)
            {
                string song_pv_no = "";
                foreach (var song_data in mod.Pv_Db.Song_Lines)
                {
                    // 既に読み込み済みの楽曲
                    if (Add_Song.Contains(song_data.Pv_No))
                    {
                        // 削除
                        song_data.Is_Del_Line = true;
                    }
                    composition_mod.Pv_Db.Song_Lines.Add(song_data);

                    if (string.IsNullOrEmpty(song_pv_no)) 
                    {
                        song_pv_no = song_data.Pv_No; 
                    }
                }

                // 読み込み済楽曲として設定
                if(string.IsNullOrEmpty(song_pv_no) == false)
                {
                    Add_Song.Add(song_pv_no);
                }
            }

            return composition_mod;
        }

        public Mod Composition()
        {
            Mod composition_mod = new Mod();

            foreach (var mod in Mods)
            {
                foreach (var pv_no in mod.Pv_Db.Song_Lines.GroupBy(x => x.Pv_No))
                {
                    if (string.IsNullOrEmpty(pv_no.Key))
                    {
                        continue;
                    }
                    foreach (var song_line in mod.Pv_Db.Song_Lines.Where(x => x.Pv_No == pv_no.Key))
                    {
                        // 既に読み込み済みの楽曲
                        if (Add_Song.Contains(song_line.Pv_No))
                        {
                            // 削除
                            song_line.Is_Del_Line = true;
                        }
                        composition_mod.Pv_Db.Song_Lines.Add(song_line);
                    }
                    
                    // 読み込み済楽曲として設定
                    Add_Song.Add(pv_no.Key);
                }
            }

            return composition_mod;
        }
    }
}
