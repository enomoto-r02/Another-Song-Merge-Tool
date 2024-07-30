using System.Linq;

namespace ModDbMerge2.DIVA
{
    public class Song
    {
        public string Pv_No { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public string Song_File_Name { get; set; }

        public List<SongLine> Lines { get; set; }


        public int Another_No { get; set; }
        public string Vocal_Disp_Name { get; set; }
        public string Vocal_Disp_Name_En { get; set; }

        public override string ToString()
        {
            string ret = "";

            if (string.IsNullOrEmpty(this.Name) == false || string.IsNullOrEmpty(this.Name_en) == false)
            {
                if (string.IsNullOrEmpty(this.Name) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "name") + "=" + this.Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Name_en) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "name_en") + "=" + this.Name_en + "\n";
                }
                if (string.IsNullOrEmpty(this.Song_File_Name) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "song_file_name") + "=" + this.Song_File_Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + this.Vocal_Disp_Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name_En) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + this.Vocal_Disp_Name_En + "\n";
                }
            }
            else
            {
                //return string.Join(".", this.Pv_No, "another_song", "length") + "=" + this.length + "\n";
                return "Song#ToString() : Length";
            }

            return ret;
        }

        public string ToString(List<Song> Add_AnotherSong)
        {
            var hoge = Add_AnotherSong.OrderBy(x => x.Pv_No);


            string ret = "";

            if (string.IsNullOrEmpty(this.Name) == false || string.IsNullOrEmpty(this.Name_en) == false)
            {
                if (string.IsNullOrEmpty(this.Name) == false)
                {
                    var append = "";
                    if (this.Pv_No == "pv_726")
                    {
                        ;
                    }
                    if (Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No && x.Another_No > 0).Count() > 0)
                    {
                        append = "★";
                    }
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "name") + "=" + append + this.Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Name_en) == false)
                {
                    var append = "";
                    if (Add_AnotherSong.Where(x => x.Pv_No == this.Pv_No && x.Another_No > 0).Count() > 0)
                    {
                        append = "★";
                    }
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "name_en") + "=" + append +this.Name_en + "\n";
                }
                if (string.IsNullOrEmpty(this.Song_File_Name) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "song_file_name") + "=" + this.Song_File_Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name") + "=" + this.Vocal_Disp_Name + "\n";
                }
                if (string.IsNullOrEmpty(this.Vocal_Disp_Name_En) == false)
                {
                    ret += string.Join(".", this.Pv_No, "another_song", this.Another_No, "vocal_disp_name_en") + "=" + this.Vocal_Disp_Name_En + "\n";
                }
            }
            else
            {
                //return string.Join(".", this.Pv_No, "another_song", "length") + "=" + this.length + "\n";
                return "Song#ToString() : Length";
            }

            return ret;
        }
    }
}
