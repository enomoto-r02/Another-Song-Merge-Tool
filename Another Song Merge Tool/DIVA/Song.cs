﻿using Another_Song_Merge_Tool.Util;
using System.Text;

namespace Another_Song_Merge_Tool.DIVA
{
    public class Song
    {
        public string Pv_No { get; set; }
        public string Name { get; set; }
        public string Name_En { get; set; }
        public string Song_File_Name { get; set; }

        public List<SongLine> Lines { get; set; }

        public int Another_No { get; set; }
        public string Vocal_Chara_Num { get; set; }
        public string Vocal_Disp_Name { get; set; }
        public string Vocal_Disp_Name_En { get; set; }
        public List<Performer> Performer { get; set; }

        public Song()
        {
            this.Lines = [];
            this.Performer = [];
        }

        public string ToStringPvDb(Config Config, List<Song> Add_AnotherSong)
        {
            var hoge = Add_AnotherSong.OrderBy(x => x.Pv_No);

            StringBuilder ret = new StringBuilder();

            if (string.IsNullOrEmpty(this.Name) == false || string.IsNullOrEmpty(this.Name_En) == false)
            {
                if (string.IsNullOrEmpty(this.Name) == false)
                {
                    var prefix = "";
                    var suffix = "";

                    if (Config.AnotherSongMarkPrefix == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No).Count() > 0)
                    {
                        prefix = Config.AnotherSongMarkPrefixStr;
                    }
                    if (Config.AnotherSongMarkSuffix == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No).Count() > 0)
                    {
                        suffix = Config.AnotherSongMarkSuffixStr;
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name") + "=" + prefix + this.Name + suffix);
                }
                if (string.IsNullOrEmpty(this.Name_En) == false)
                {
                    var prefix = "";
                    var suffix = "";

                    if (Config.AnotherSongMarkPrefix == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No).Count() > 0)
                    {
                        prefix = Config.AnotherSongMarkPrefixStr;
                    }
                    if (Config.AnotherSongMarkSuffix == true && Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No).Count() > 0)
                    {
                        suffix = Config.AnotherSongMarkSuffixStr;
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "name_en") + "=" + prefix + this.Name_En + suffix);
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
                    var view = DivaUtil.GetChara(Vocal_Disp_Name);
                    if (string.IsNullOrEmpty(view))
                    {
                        view = this.Vocal_Disp_Name;
                        if (string.IsNullOrEmpty(view))
                        {
                            view = "none";
                        }
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + view);
                }
                else
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + Path.GetFileName(this.Song_File_Name));
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name_En) == false)
                {
                    var view = DivaUtil.GetCharaEn(Vocal_Disp_Name_En);
                    if (string.IsNullOrEmpty(view))
                    {
                        view = this.Vocal_Disp_Name;
                        if (string.IsNullOrEmpty(view))
                        {
                            view = "none";
                        }
                    }
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + view);
                }
                else
                {
                    ret.AppendLine(string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + Path.GetFileName(this.Song_File_Name));
                }
            }
            else
            {
                return "Song#ToString() : Length";
            }

            return ret.ToString();
        }

        public string ToStringPvField()
        {
            StringBuilder ret = new StringBuilder();

            if (string.IsNullOrEmpty(this.Pv_No) == false)
            {
                foreach (var line in this.Lines)
                {
                    ret.AppendLine(line.ToString());
                }
            }

            return ret.ToString();
        }

        public string GetPerformerChara()
        {
            StringBuilder sb = new();

            for (var i = 0; i < this.Performer.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append("/");
                }
                Performer p = this.Performer[i];
                //sb.Append(p.Chara);
                sb.Append(p.ViewCharaValue());
            }

            return sb.ToString();
        }

        public string GetPerformerCharaEn()
        {
            StringBuilder sb = new();

            for (var i = 0; i < this.Performer.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append("/");
                }
                Performer p = this.Performer[i];
                //sb.Append(p.Chara);
                sb.Append(p.ViewCharaValueEn());
            }

            return sb.ToString();
        }
    }
}
