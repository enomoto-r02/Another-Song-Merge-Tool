using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Manager;
using Another_Song_Merge_Tool.Util;
using NaturalSort.Extension;
using System;
using System.Text;

namespace Another_Song_Merge_Tool
{
    class Program
    {
        static readonly AppConfig appConfig = AppConfig.Get();

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
            else if (File.Exists(appConfig.DivaModManager.ConfigPath) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.DivaModManager.ConfigPath + "\" in "+ AppConfig.FILE_INI + " is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + appConfig.DivaModManager.ConfigPath + "\" in \"" + AppConfig.FILE_INI + "\" is Not Found.");

                return;
            }
            else if (appConfig.Toml == null)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Section [Toml] in \"" + AppConfig.FILE_INI + "\" is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("Section [Toml] in \\\"\" + AppConfig.FILE_INI + \"\\\" is Not Found.");

                return;
            }
            else if (File.Exists(appConfig.Toml.Config) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + appConfig.Toml.Config + "\" is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + appConfig.Toml.Config + "\" is Not Found.");

                return;
            }

            ConfigToml toml = new(appConfig);

            if (toml.Dll == null)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "\"dll\" description in "+ appConfig.Toml.Config +" is Not Found.");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("\"dll\" description in " + appConfig.Toml.Config + " is Not Found.");

                return;
            }
            if (File.Exists(toml.Dll[0]) == false)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "File \"" + toml.Dll[0] + "\" in "+ appConfig.Toml.Config + " is Not Found");
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool Failed.");
                ToolUtil.ErrorLog("File \"" + toml.Dll[0] + "\" in " + appConfig.Toml.Config + " is Not Found");

                return;
            }

            FileUtil.CreateFolder("rom");

            if (appConfig.Config.BackupPvDb)
            {
                var new_file_name = FileUtil.Backup("./rom/"+Mod.FILE_PV_MOD);
                if (File.Exists(new_file_name) == true)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "{0} Backup Complete.", Path.GetFileName(new_file_name));
                }
            }
            else
            {
                var del_file_name = FileUtil.Delete("./rom/" + Mod.FILE_PV_MOD);
                if (File.Exists(del_file_name) == false)
                {
                    Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "{0} Delete Complete.", Path.GetFileName(del_file_name));
                }
            }

            CombinePvNo combine_pvno = new CombinePvNo();
            combine_pvno.Load();
            if (string.IsNullOrEmpty(combine_pvno.ToString())) {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + combine_pvno.ToString());
            }

            DivaModManager dmm = new(appConfig);
            if (dmm.Mods.Count == 0)
            {
                Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "There are no mods to merge.");
                return;
            }

            Console.WriteLine(dmm.ToStringMods());

            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_PV_MOD + " Generating...");

            dmm.LoadPvDb();

            Mod merge_mod = dmm.Composition();

            Output(dmm, merge_mod, combine_pvno.PvNos);

            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_PV_MOD +" Generation Complete.");
            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + "Tool End.");
            //Console.WriteLine();
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }

        private static void Output(DivaModManager dmm, Mod merge_mod, List<string> combine_pv_nos)
        {
            List<string> outputs = new();
            foreach (var song in merge_mod.Pv_Db.Songs)
            {
                foreach (var song_line in song.Lines)
                {
                    outputs.Add(song_line.ToString());
                }
            }
            foreach (var another in dmm.Add_AnotherSong)
            {
                if (dmm.Add_AnotherSong.Where(x => x.Pv_No == another.Pv_No).Count() > 1)
                {
                    if (combine_pv_nos.Count > 0 && combine_pv_nos.Contains(another.Pv_No) == false)
                    {
                        continue;
                    }
                    outputs.Add(another.ToString(appConfig.Config, dmm.Add_AnotherSong));
                }
            }
            dmm.ToStringLengthLine(outputs, combine_pv_nos);

            outputs = outputs.OrderBy(x => x.ToString(), StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                if (string.IsNullOrEmpty(output.ToString()) == false)
                {
                    sb.AppendLine(output.ToString());
                }
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "./rom/"+ Mod.FILE_PV_MOD, false);
        }
    }
}