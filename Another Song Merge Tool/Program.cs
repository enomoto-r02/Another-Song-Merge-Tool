using ModDbMerge2.Util;
using ModDbMerge2.DIVA;
using ModDbMerge2.Manager;
using System.Text;
using NaturalSort.Extension;
using System.Collections.Generic;

namespace ModDbMerge2
{
    class Program
    {
        static readonly AppConfig Config = AppConfig.Get();

        static void Main(string[] args)
        {
            var new_file_name = FileUtil.Backup("mod_pv_db.txt");
            if (string.IsNullOrEmpty(new_file_name) == false)
            {
                Console.WriteLine("{0} 退避完了", new_file_name);
            }

            Console.WriteLine("mod_pv_db.txt 生成中...");

            DivaModManager dmm = new(Config);

            foreach (var item in dmm.Mods)
            {
                Console.WriteLine(" - " + item.Name);
            }
            Console.WriteLine();

            dmm.ReadPvDb();

            Mod merge_mod = dmm.Composition();

            Program.Output(dmm, merge_mod);
#if DEBUG
            System.Diagnostics.Process.Start("EXPLORER.EXE", "mod_pv_db.txt");
#endif

            Console.WriteLine("mod_pv_db.txt 生成完了");
            Console.WriteLine();
            Console.WriteLine("いずれかのキーを押すと終了します。");
            Console.ReadKey();
        }

        private static void Output(DivaModManager dmm, Mod merge_mod)
        {
            List<string> outputs = new();
            foreach (var song_line in merge_mod.Pv_Db.Song_Lines)
            {
                outputs.Add(song_line.ToString());
            }
            foreach (var another in dmm.Add_AnotherSong)
            {
                outputs.Add(another.ToString());
                //outputs.Add(another.ToString(dmm.Add_AnotherSong));
            }
            dmm.ToStringLengthLine(outputs);

            outputs = outputs.OrderBy(x => x.ToString(), StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var output in outputs)
            {
                if (string.IsNullOrEmpty(output.ToString()) == false)
                {
                    sb.AppendLine(output.ToString());
                }
            }

            FileUtil.WriteFile(sb.ToString(), "mod_pv_db.txt", false);
        }
    }
}