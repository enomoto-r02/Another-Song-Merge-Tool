using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Manager;
using Another_Song_Merge_Tool.MikuMikuLibrary;
using Another_Song_Merge_Tool.Util;
using NaturalSort.Extension;
using System.Text;

namespace Another_Song_Merge_Tool
{
    class Program
    {
        static readonly AppConfig appConfig = AppConfig.Get();
        static readonly string start_dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");

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
            if (File.Exists(appConfig.MikuMikuLibrary.DataBaseConverter_Path) == false)
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

            if (string.IsNullOrEmpty(combine_pvno.ToString()))
            {
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

            dmm.LoadPvData(appConfig);

            Mod merge_mod = dmm.Composition();

            Output_PvDb(dmm, merge_mod, combine_pvno.PvNos);
            Output_PvField(dmm);

            Console.WriteLine(ToolUtil.CONSOLE_PREFIX + Mod.FILE_PV_MOD + " Generation Complete.");
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

            outputs = outputs.OrderBy(x => x.ToString(), StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                if (string.IsNullOrEmpty(output.ToString()) == false)
                {
                    sb.AppendLine(output.ToString());
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

            outputs = outputs.OrderBy(x => x.ToString(), StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                if (string.IsNullOrEmpty(output.ToString()) == false)
                {
                    sb.AppendLine(output.ToString());
                }
            }

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "./rom/" + Mod.FILE_FIELD_MOD, false);
        }
    }
}