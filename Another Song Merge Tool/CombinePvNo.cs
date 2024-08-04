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
        public readonly string FileName;

        public List<string> PvNos;

        public CombinePvNo() 
        {
            this.FileName = "Combine_Pv_No.txt";
            this.PvNos = new();
        }

        public void Load()
        {
            if (File.Exists(this.FileName))
            {
                foreach(var line in FileUtil.ReadFile(this.FileName).Split("\r\n").ToList())
                {
                    if (string.IsNullOrEmpty(line.Trim()) == false && line.Trim().StartsWith("#") == false)
                    {
                        PvNos.Add(line);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (this.PvNos.Count > 0)
            {
                sb.AppendLine("[Another Song Merge Tool] combine_pv_no.txt");

                foreach (var pv_nos in this.PvNos)
                {
                    sb.AppendLine("[Another Song Merge Tool] - " + pv_nos.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
