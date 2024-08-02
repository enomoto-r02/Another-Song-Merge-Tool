using System.Linq;

namespace Another_Song_Merge_Tool.DIVA
{
    public class SongLine
    {
        public string Pv_No { get; set; }
        public string[] Parameters { get; set; }
        public string Value { get; set; }
        public bool Is_Del_Line { get; set; }
        public int Priority { get; set; }
        public int Pv_Db_Priority { get; set; }
        public bool Is_Another_Song;

        public SongLine(int priority, int pv_db_priority, string line)
        {
            var param = line.Split("=");
            if (param.Length > 1)
            {
                this.Value = String.Join("=", param.Where((item, index) => index != 0));
            }
            else
            {
                this.Value = "";
            }
            var song_param = param[0].Split(".");
            this.Pv_No = song_param[0];
            this.Parameters = song_param.Skip(1).ToArray();

            this.Priority = priority;
            this.Pv_Db_Priority = pv_db_priority;
            this.Is_Del_Line = false;
        }

        public override string ToString()
        {
            string ret = "";
            if (this.Is_Del_Line == false)
            {
                ret += this.Pv_No + "." + string.Join(".", this.Parameters) + "=" + this.Value;
            }

            return ret;
        }
    }
}
