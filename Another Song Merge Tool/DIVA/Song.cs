﻿using System.Text;

namespace Another_Song_Merge_Tool.DIVA
{
    public class Song
    {
        public string Pv_No { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public string Song_File_Name { get; set; }

        public List<SongLine> Lines { get; set; }


        public int Another_No { get; set; }
        public string Vocal_Chara_Num { get; set; }
        public string Vocal_Disp_Name { get; set; }
        public string Vocal_Disp_Name_En { get; set; }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            if (string.IsNullOrEmpty(this.Name) == false || string.IsNullOrEmpty(this.Name_en) == false)
            {
                if (string.IsNullOrEmpty(this.Name) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name") + "=" + this.Name);
                }
                if (string.IsNullOrEmpty(this.Name_en) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name_en") + "=" + this.Name_en);
                }
                if (string.IsNullOrEmpty(this.Song_File_Name) == false && this.Another_No > 0)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "song_file_name") + "=" + this.Song_File_Name);
                }
                if (string.IsNullOrEmpty(this.Vocal_Chara_Num) == false && this.Another_No > 0)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_chara_num") + "=" + this.Vocal_Chara_Num);
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + this.Vocal_Disp_Name);
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name_En) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + this.Vocal_Disp_Name_En);
                }
            }
            else
            {
                //return string.Join(".", this.Pv_No, "another_song", "length") + "=" + this.length + "\n";
                return "Song#ToString() : Length";
            }

            return ret.ToString();
        }

        public string ToString(Config Config, List<Song> Add_AnotherSong)
        {
            var hoge = Add_AnotherSong.OrderBy(x => x.Pv_No);


            StringBuilder ret = new StringBuilder();

            if (string.IsNullOrEmpty(this.Name) == false || string.IsNullOrEmpty(this.Name_en) == false)
            {
                if (string.IsNullOrEmpty(this.Name) == false)
                {
                    var append = "";
                    if (Config.AnotherSongMark == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No && x.Another_No > 0).Count() > 0)
                    {
                        append = Config.AnotherSongMarkPrefix;
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name") + "=" + append + this.Name);
                }
                if (string.IsNullOrEmpty(this.Name_en) == false)
                {
                    var append = "";
                    if (Config.AnotherSongMark == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No && x.Another_No > 0).Count() > 0)
                    {
                        append = Config.AnotherSongMarkPrefix;
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name_en") + "=" + append + this.Name_en);
                }
                if (string.IsNullOrEmpty(this.Song_File_Name) == false && this.Another_No > 0)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "song_file_name") + "=" + this.Song_File_Name);
                }
                if (string.IsNullOrEmpty(this.Vocal_Chara_Num) == false && this.Another_No > 0)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_chara_num") + "=" + this.Vocal_Chara_Num);
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + this.Vocal_Disp_Name);
                }
                else
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + Path.GetFileName(this.Song_File_Name));
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name_En) == false)
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + this.Vocal_Disp_Name_En);
                }
                else
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + Path.GetFileName(this.Song_File_Name));
                }
            }
            else
            {
                //return string.Join(".", this.Pv_No, "another_song", "length") + "=" + this.length + "\n";
                return "Song#ToString() : Length";
            }

            return ret.ToString();
        }
    }
}
