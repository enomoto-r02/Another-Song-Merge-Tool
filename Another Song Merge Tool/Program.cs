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
            Console.WriteLine("[Another Song Merge Tool] Tool Start.");
            FileUtil.CreateFolder("rom");

            if (appConfig.Config.BackupPvDb)
            {
                var new_file_name = FileUtil.Backup("./rom/mod_pv_db.txt");
                if (File.Exists(new_file_name) == true)
                {
                    Console.WriteLine("[Another Song Merge Tool] {0} Backup Complete.", Path.GetFileName(new_file_name));
                    //Console.WriteLine();
                }
            }
            else
            {
                var del_file_name = FileUtil.Delete("./rom/mod_pv_db.txt");
                if (File.Exists(del_file_name) == false)
                {
                    Console.WriteLine("[Another Song Merge Tool] {0} Delete Complete.", Path.GetFileName(del_file_name));
                    //Console.WriteLine();
                }
            }

            CombinePvNo combine_pvno = new CombinePvNo();
            combine_pvno.Load();
            if (string.IsNullOrEmpty(combine_pvno.ToString())) {
                Console.WriteLine("[Another Song Merge Tool] " + combine_pvno.ToString());
            }

            DivaModManager dmm = new(appConfig);
            Console.WriteLine(dmm.ToStringMods());

            Console.WriteLine("[Another Song Merge Tool] mod_pv_db.txt Generating...");

            dmm.LoadPvDb();

            Mod merge_mod = dmm.Composition();

            Output(dmm, merge_mod, combine_pvno.PvNos);

            Console.WriteLine("[Another Song Merge Tool] mod_pv_db.txt Generation Complete.");
            //Console.WriteLine();
            Console.WriteLine("[Another Song Merge Tool] Tool End.");
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

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "./rom/mod_pv_db.txt", false);
        }
    }
}