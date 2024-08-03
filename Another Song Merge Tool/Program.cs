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

        // ex_songのマージ対象外とするpv_id   
        public static readonly List<string> SKIP_EXSONG_MERGE_LIST = new(){
            "pv_262",   // ピアノ×フォルテ×スキャンダル
            "pv_621",   // Nostalogic
        };

        static void Main(string[] args)
        {
            var new_file_name = FileUtil.Backup("mod_pv_db.txt");
            if (string.IsNullOrEmpty(new_file_name) == false)
            {
                Console.WriteLine("[ Backup ]");
                Console.WriteLine(" - {0} Complete.", new_file_name);
                Console.WriteLine();
            }

            CombinePvNo combine_pvno = new CombinePvNo();
            combine_pvno.Load();
            Console.WriteLine(combine_pvno.ToString());

            DivaModManager dmm = new(appConfig);
            Console.WriteLine(dmm.ToStringMods());

            Console.WriteLine("[ Generating ]");
            Console.WriteLine(" - mod_pv_db.txt Generating...");

            dmm.LoadPvDb();

            Mod merge_mod = dmm.Composition();

            Output(dmm, merge_mod, combine_pvno.PvNos);

            Console.WriteLine(" - mod_pv_db.txt Generation Complete.");
            Console.WriteLine();
#if DEBUG
            System.Diagnostics.Process.Start("EXPLORER.EXE", "mod_pv_db.txt");
#endif
            Console.WriteLine();
            Console.WriteLine("Complete!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
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

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "mod_pv_db.txt", false);
        }
    }
}