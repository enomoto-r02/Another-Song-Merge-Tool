using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Song_Merge_Tool.DIVA
{
    public class PvField
    {
        public string Mod_Name { get; set; }
        public string Field_Name { get; set; }
        public string Field_Path { get; set; }
        public int Field_Priority { get; set; }
        public List<Song> Songs { get; set; }

        public PvField(string mod_name, int field_priority)
        {
            this.Mod_Name = mod_name;
            this.Field_Priority = field_priority;
            this.Songs = new();
        }

        public void Load(List<Song> allFieldSong)
        {
            // mod_pv_field.txtを全行読み込む
            if (string.IsNullOrEmpty(this.Field_Path) == false && File.Exists(this.Field_Path) == true)
            {
                Song song = new Song();

                foreach (var line in File.ReadAllLines(this.Field_Path))
                {
                    if (string.IsNullOrEmpty(line.Replace("\t", "")) || line.StartsWith('#'))
                    {
                        continue;
                    }

                    SongLine song_line = new(this.Field_Priority, this.Field_Priority, line);

                    if (song_line.Parameters.Length == 0)
                    {
                        continue;
                    }

                    // 別の曲になった
                    if (string.IsNullOrEmpty(song.Pv_No) == false && song_line.Pv_No != song.Pv_No)
                    {
                        Add_Song_Validate(allFieldSong, song);
                        song = new Song();
                        //song.Pv_No = song_line.Pv_No;
                    }

                    song.Pv_No = song_line.Pv_No;

                    //if (string.IsNullOrEmpty(song.Pv_No))
                    //{
                    //    song.Pv_No = song_line.Pv_No;
                    //}

                    song.Lines.Add(song_line);
                }

                Add_Song_Validate(allFieldSong, song);
            }
            else
            {
                return;
            }
        }

        public void Add_Song_Validate(List<Song> allFieldSong, Song song)
        {
            if (string.IsNullOrEmpty(song.Pv_No) == false)
            {
                if (allFieldSong.Where(x => x.Pv_No == song.Pv_No).Count() == 0)
                {
                    Songs.Add(song);
                    allFieldSong.Add(song);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach(var song in this.Songs)
            {
                sb.AppendLine(song.ToStringPvField());
            }
            return sb.ToString();
        }
    }
}
