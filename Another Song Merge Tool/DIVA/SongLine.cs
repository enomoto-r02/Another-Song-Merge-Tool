using Another_Song_Merge_Tool.Util;
using System.Linq;
using System.Xml;

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

        // override用コンストラクタ
        public SongLine(string line)
        {
            try
            {
                if (string.IsNullOrEmpty(line))
                {
                    throw new Exception();
                }

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

                this.Is_Del_Line = false;
            }
            catch(Exception e)
            {
                this.Pv_No = "";
                this.Parameters = Array.Empty<string>();
                this.Value = "";
                this.Is_Del_Line = true;
            }
        }

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
            if (this.Is_Del_Line == false && 
                (!string.IsNullOrEmpty(this.Pv_No) || !string.IsNullOrEmpty(string.Join("", this.Parameters)) || !string.IsNullOrEmpty(this.Value)))
            {
                ret += this.Pv_No + "." + string.Join(".", this.Parameters) + "=" + this.Value;
            }

            return ret;
        }

        public void OverRide(SongLine override_line)
        {
            if (this.Pv_No == override_line.Pv_No && string.Join(".", this.Parameters) == string.Join(".", override_line.Parameters))
            {
                this.Value = override_line.Value;
            }
        }
    }
}
