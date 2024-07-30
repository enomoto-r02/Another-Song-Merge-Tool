using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModDbMerge2.DIVA
{
    public class SongLine
    {
        public string Pv_No {  get; set; }
        public string[] Parameters { get; set; }
        public string Value {  get; set; }
        public bool Is_Del_Line { get; set; }
        public int Priority { get; set; }
        public int Pv_Db_Priority { get; set; }
        public bool Is_Another_Song;

        public SongLine(int priority, int pv_db_priority, string line)
        {
            var param = line.Split("=");
            if (param.Length > 1)
            {
                this.Value = param[1];
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
            //if (string.IsNullOrEmpty(this.Value) == false && this.Is_Del_Line == false)
            if (this.Is_Del_Line == false)
            {
                ret += this.Pv_No + "." + string.Join(".", this.Parameters) + "=" + this.Value;
            }

            return ret;
        }

        //public string ToString(List<string> addSongs)
        //{
        //    string ret = "";

        //    if (this.Parameters.Length > 2
        //        && this.Parameters[0] == "another_song"
        //        && (this.Parameters[2] == "name" || this.Parameters[2] == "name_en")
        //        && addSongs.Where(x => x == this.Pv_No).Count() > 1)
        //    {
        //        this.Value = "★" + this.Value;
        //    }

        //    if (string.IsNullOrEmpty(this.Value) == false && this.Is_Del_Line == false)
        //    {
        //        ret += this.Pv_No + "." + string.Join(".", this.Parameters) + "=" + this.Value;
        //    }

        //    return ret;
        //}

    }
}
