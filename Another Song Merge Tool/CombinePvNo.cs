using Another_Song_Merge_Tool.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Song_Merge_Tool
{

    public class CombinePvNo
    {
        public static string FILE_COMBINE = "Combine_Pv_No.txt";

        public List<string> PvNos;

        public CombinePvNo() 
        {
            this.PvNos = new();
        }

        public void Load()
        {
            if (File.Exists(FILE_COMBINE))
            {
                foreach(var line in FileUtil.ReadFile(FILE_COMBINE).Split("\r\n").ToList())
                {
                    if (string.IsNullOrEmpty(line.Trim()) == false && line.Trim().StartsWith("#") == false)
                    {
                        PvNos.Add(line);
                    }
                }
            }
            else
            {
                ToolUtil.InfoLog(FILE_COMBINE + " is not Found.");
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (this.PvNos.Count > 0)
            {
                sb.AppendLine(ToolUtil.CONSOLE_PREFIX + FILE_COMBINE);

                foreach (var pv_nos in this.PvNos)
                {
                    sb.AppendLine(ToolUtil.CONSOLE_PREFIX + " - " + pv_nos.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
