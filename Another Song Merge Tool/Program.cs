﻿using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Manager;
using Another_Song_Merge_Tool.Util;
using NaturalSort.Extension;
using System.Runtime.CompilerServices;
using System.Text;

namespace Another_Song_Merge_Tool
{
    class Program
    {
        static readonly AppConfig appConfig = AppConfig.Get();
        public static readonly string start_dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        static void Main(string[] args)
        {
            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Start.");

            if (appConfig == null)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + AppConfig.FILE_INI + "\" is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + AppConfig.FILE_INI + "\" is Not Found.");

                return;
            }
            if (File.Exists(appConfig.DivaModManager.ConfigPath) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.DivaModManager.ConfigPath + "\" in " + AppConfig.FILE_INI + " is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + appConfig.DivaModManager.ConfigPath + "\" in \"" + AppConfig.FILE_INI + "\" is Not Found.");

                return;
            }
            if (File.Exists(ConfigToml.FILE_CONFIG) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + ConfigToml.FILE_CONFIG + "\" is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + ConfigToml.FILE_CONFIG + "\" is Not Found.");

                return;
            }

            ConfigToml toml = new();

            if (toml.Dll == null)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "\"dll\" description in " + ConfigToml.FILE_CONFIG + " is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("\"dll\" description in " + ConfigToml.FILE_CONFIG + " is Not Found.");

                return;
            }
            if (File.Exists(toml.Dll[0]) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + toml.Dll[0] + "\" in " + ConfigToml.FILE_CONFIG + " is Not Found");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + toml.Dll[0] + "\" in " + ConfigToml.FILE_CONFIG + " is Not Found");

                return;
            }
            if (appConfig.Config.MergeStageData && File.Exists(appConfig.MikuMikuLibrary.DataBaseConverter_Path) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.MikuMikuLibrary.DataBaseConverter_Path + "\" in " + ConfigToml.FILE_CONFIG + " is Not Found");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + toml.Dll[0] + "\" in " + ConfigToml.FILE_CONFIG + " is Not Found");

                return;
            }

            FileUtil.CreateFolder("rom");

            if (appConfig.Config.BackupPvDb)
            {
                var new_file_name = FileUtil.Backup("./rom/" + Mod.FILE_PV_MOD);
                if (File.Exists(new_file_name) == true)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "{0} Backup Complete.", Path.GetFileName(new_file_name));
                }
            }

            CombinePvNo combine_pvno = new CombinePvNo();
            combine_pvno.Load();

            if (string.IsNullOrEmpty(combine_pvno.ToString()))
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + combine_pvno.ToString());
            }

            FileUtil.Delete("./rom/" + Mod.FILE_PV_DB);
            FileUtil.Delete("./rom/" + Mod.FILE_PV_MOD);
            FileUtil.Delete("./rom/" + Mod.FILE_PV_MDATA);
            FileUtil.Delete("./rom/" + Mod.FILE_FIELD_DB);
            FileUtil.Delete("./rom/" + Mod.FILE_FIELD_MOD);
            FileUtil.Delete("./rom/" + Mod.FILE_FIELD_MDATA);
            FileUtil.Delete("./rom/" + Mod.FILE_STAGE_BIN_DB);
            FileUtil.Delete("./rom/" + Mod.FILE_STAGE_BIN_MOD);
            FileUtil.Delete("./rom/" + Mod.FILE_STAGE_BIN_MDATA);

            DivaModManager dmm = new(appConfig);

            if (dmm.Mods.Count == 0)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "There are no mods to merge.");

                return;
            }

            Console.WriteLine(dmm.ToStringMods());

            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_PV_MOD + " Generating...");

            dmm.LoadPvData(appConfig);

            Mod merge_mod = dmm.Composition();

            Output_PvDb(dmm, merge_mod, combine_pvno.PvNos);
            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_PV_MOD + " Generation Complete.");


            if (appConfig.Config.MergePvField)
            {
                Output_PvField(dmm);
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_FIELD_MOD + " Generation Complete.");
            }
            if (appConfig.Config.MergeStageData)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_STAGE_BIN_MOD + " Generation Complete.");
            }

            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool End.");
            //Console.WriteLine();
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }

        private static void Output_PvDb(DivaModManager dmm, Mod merge_mod, List<string> combine_pv_nos)
        {
            List<string> outputs = new();
            foreach (var song in merge_mod.Pv_Db.Songs)
            {
                foreach (var song_line in song.Lines)
                {
                    outputs.Add(song_line.ToString());
                }
            }
            foreach (var another in dmm.AddDbAnotherSong)
            {
                if (dmm.AddDbAnotherSong.Where(x => x.Pv_No == another.Pv_No).Count() > 1)
                {
                    if (combine_pv_nos.Count > 0 && combine_pv_nos.Contains(another.Pv_No) == false)
                    {
                        continue;
                    }
                    outputs.Add(another.ToStringPvDb(appConfig.Config, dmm.AddDbAnotherSong));
                }
            }
            dmm.ToStringLengthLine(outputs, combine_pv_nos);

            // ソート
            // Another Song行は数行まとめて出力するため、StringBuilderのToStringしてからソート
            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                var s = output.ToString();
                if (string.IsNullOrEmpty(s) == false)
                {
                    sb.AppendLine(s);
                }
            }

            outputs = sb.ToString().Split("\r\n").ToList();
            outputs = outputs.OrderBy(x => x.ToString(), StringComparer.Ordinal).ToList();
            sb.Clear();

            if (appConfig.Config.OverRidePvDb)
            {
                sb.Append(Override_PvDb(outputs));
            }
            else
            {
                foreach (var output in outputs)
                {
                    var s = output.ToString();
                    if (string.IsNullOrEmpty(s) == false)
                    {
                        sb.AppendLine(s);
                    }
                }
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "./rom/" + Mod.FILE_PV_MOD, false);
        }

        private static void Output_PvField(DivaModManager dmm)
        {
            List<string> outputs = new();
            foreach (var mod in dmm.Mods)
            {
                foreach (var song in mod.Pv_Field.Songs)
                {
                    outputs.Add(song.ToStringPvField());
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                sb.AppendLine(output.ToString());
            }

            outputs = sb.ToString().Split("\r\n").ToList();
            outputs = outputs.OrderBy(x => x.ToString(), StringComparer.Ordinal).ToList();

            sb = new StringBuilder();
            foreach (var output in outputs)
            {
                sb.AppendLine(output.ToString());
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "./rom/" + Mod.FILE_FIELD_MOD, false);
        }
        private static string Override_PvDb(List<string> outputs)
        {
            StringBuilder sb = new StringBuilder();

            // 読み込んだSongLine
            List<SongLine> override_reads = new List<SongLine>();

            // mod_pv_db.txtを全行読み込む
            if (File.Exists(Mod.FILE_PV_MOD_OVERRIDE) == true)
            {
                foreach (var line in File.ReadAllLines(Mod.FILE_PV_MOD_OVERRIDE))
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        continue;
                    }
                    SongLine override_read = new SongLine(line);
                    if (override_read.Is_Del_Line == false)
                    {
                        override_reads.Add(override_read);
                    }
                }
            }

            foreach (var output in outputs)
            {
                SongLine output_line = new SongLine(output);
                if (output_line.Is_Del_Line == false)
                {
                    foreach (var override_line in override_reads)
                    {
                        output_line.OverRide(override_line);
                    }
                }

                var s = output_line.ToString();
                if (string.IsNullOrEmpty(s) == false)
                {
                    sb.AppendLine(s);
                }
            }

            return sb.ToString();
        }
    }
}