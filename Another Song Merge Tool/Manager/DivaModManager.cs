﻿using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Util;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Another_Song_Merge_Tool.Manager
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

        public List<Song> AddDbAnotherSong { get; set; }
        public List<string> Add_Song { get; }
        public List<Song> allFieldSong { get; set; }


        public DivaModManager()
        {
            this.Song_no_cnt = [];
            this.AddDbAnotherSong = [];
            this.Add_Song = [];
            this.allFieldSong = [];
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

                if (mod.Db_Priority != -1)
                {
                    mod.Db_Priority = pv_db_priority;
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

        public void LoadPvData(AppConfig appConfig)
        {
            var now_pv_db_priority = 0;
            var is_create_stage_data = false;
            for (var i = 0; i < Mods.Count; i++)
            {
                if (Mods[i].Enabled)
                {
                    Mods[i].LoadPvDb(this.AddDbAnotherSong, this.Song_no_cnt, now_pv_db_priority);
                    if (appConfig.Config.MergePvField)
                    {
                        Mods[i].LoadPvField(this.allFieldSong);

                    }
                    if (appConfig.Config.MergeStageData)
                    {
                        if (is_create_stage_data == false)
                        {
                            is_create_stage_data = Mods[i].InitStageData(appConfig);
                        }
                        else
                        {
                            Mods[i].LoadStageData(appConfig);
                        }
                    }
                    if (Mods[i].Db_Priority >= 0)
                    {
                        now_pv_db_priority++;
                    }

                }
            }
            if (appConfig.Config.MergeStageData)
            {
                Mod.EndStageData(appConfig);
            }

        }

        public string ToStringMods()
        {
            StringBuilder sb = new();

            if (this.Mods.Count > 0)
            {
                sb.AppendLine(ToolUtil.CONSOLE_PREFIX + "Load Mods Folder");

                foreach (var item in this.Mods)
                {
                    sb.AppendLine(ToolUtil.CONSOLE_PREFIX + "- " + item.Name);
                }
            }

            return sb.ToString();
        }

        public void ToStringLengthLine(List<string> addLines, List<string> combine_pv_nos)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pv_no in this.AddDbAnotherSong.GroupBy(x => x.Pv_No))
            {
                if (combine_pv_nos.Count > 0 && combine_pv_nos.Contains(pv_no.Key) == false)
                {
                    continue;
                }
                var cnt = this.AddDbAnotherSong.Where(x => x.Pv_No == pv_no.Key).Count();
                if (cnt > 1)
                {
                    addLines.Add(string.Format("{0}.another_song.length={1}", pv_no.Key, cnt));
                }
            }
        }

        public Mod Composition()
        {
            Mod composition_mod = new Mod();
            Song composition_song = new();

            foreach (var mod in Mods)
            {
                foreach (var song in mod.Pv_Db.Base_Songs_Data)
                {
                    foreach (var pv_no in song.Lines.GroupBy(x => x.Pv_No))
                    {
                        if (string.IsNullOrEmpty(pv_no.Key))
                        {
                            continue;
                        }
                        foreach (var song_line in song.Lines.Where(x => x.Pv_No == pv_no.Key))
                        {
                            // 既に読み込み済みの楽曲
                            if (Add_Song.Contains(song_line.Pv_No))
                            {
                                // 削除
                                song_line.Is_Del_Line = true;
                            }
                            composition_song.Lines.Add(song_line);
                        }

                        // 読み込み済楽曲として設定
                        Add_Song.Add(pv_no.Key);
                    }
                }
            }
            composition_mod.Pv_Db.Songs.Add(composition_song);

            return composition_mod;
        }
    }
}
