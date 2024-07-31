using Another_Song_Merge_Tool.DIVA;
using Another_Song_Merge_Tool.Manager;
using Another_Song_Merge_Tool.Util;
using NaturalSort.Extension;
using System.Text;

namespace Another_Song_Merge_Tool
{
    class Program
    {
        static readonly AppConfig appConfig = AppConfig.Get();

        static void Main(string[] args)
        {
            var new_file_name = FileUtil.Backup("mod_pv_db.txt");
            if (string.IsNullOrEmpty(new_file_name) == false)
            {
                Console.WriteLine("{0} 退避完了", new_file_name);
                Console.WriteLine();
            }

            DivaModManager dmm = new(appConfig);

            foreach (var item in dmm.Mods)
            {
                Console.WriteLine(" - " + item.Name);
            }
            Console.WriteLine();

            Console.WriteLine("mod_pv_db.txt 生成中...");

            dmm.ReadPvDb();

            Mod merge_mod = dmm.Composition();

            Output(dmm, merge_mod);

            Console.WriteLine("mod_pv_db.txt 生成完了");
            Console.WriteLine();
#if DEBUG
            System.Diagnostics.Process.Start("EXPLORER.EXE", "mod_pv_db.txt");
#endif

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
                outputs.Add(another.ToString(appConfig.Config,dmm.Add_AnotherSong));
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

            FileUtil.WriteFile_UTF_8_NO_BOM(sb.ToString(), "mod_pv_db.txt", false);
        }
    }
}