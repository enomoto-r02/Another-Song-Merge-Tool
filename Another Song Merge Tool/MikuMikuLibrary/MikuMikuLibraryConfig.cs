using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Song_Merge_Tool.MikuMikuLibrary
{
    public class MikuMikuLibraryConfig
    {
        public static string FILE_DATABASECONVERTER = "DatabaseConverter.exe";
        public string Path { get; set; }
        public string DataBaseConverter_Path { get { return this.Path + "\\" + FILE_DATABASECONVERTER; } }

        public MikuMikuLibraryConfig()
        {
        }

        public void Load(AppConfig appConfig)
        {
            // 外部EXEを起動する
            var info = new ProcessStartInfo(DataBaseConverter_Path)
            {
                // コマンドラインパラメータを指定する
                ArgumentList =
                {
                    @"mod_stage_data.bin"
                }
            };
            Process? p = Process.Start(info);

            if (p != null)
            {
                // 別EXEが終了するまで待つ場合
                p.WaitForExit();
                Console.WriteLine("DataBaseConverter 実行完了");

                // 別EXEを強制終了する場合
                // System.Threading.Thread.Sleep(5000);
                // p.Kill();

                // 別EXEからプログラムの終了コードを受け取る場合
                //int code = p.ExitCode;
            }
        }
    }
    
}
