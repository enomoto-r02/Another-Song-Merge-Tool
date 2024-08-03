namespace Another_Song_Merge_Tool.DIVA
{
    public class PvDb
    {
        public string Mod_Name { get; set; }
        public string Pv_Db_Name { get; set; }
        public int Pv_Db_Priority { get; set; }

        public string Song_File_Name { get; set; }
        public List<Song> Base_Songs_Data { get; set; }
        public List<Song> Songs { get; set; }

        public PvDb(string mod_name)
        {
            this.Mod_Name = mod_name;
            this.Base_Songs_Data = [];
            this.Songs = [];
        }

        public PvDb(string mod_name, string pv_db_name, int pv_db_priority)
        {
            this.Mod_Name = mod_name;
            this.Pv_Db_Name = pv_db_name;
            this.Pv_Db_Priority = pv_db_priority;
            this.Base_Songs_Data = [];
            this.Songs = [];
        }

        public void Load(List<Song> addAnotherSong, List<string> song_no_cnt, int priority, string path)
        {
            // mod_pv_db.txtを全行読み込む
            if (string.IsNullOrEmpty(path) == false && File.Exists(path) == true)
            {
                Song base_song = new Song();

                foreach (var line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrEmpty(line.Replace("\t", "")) || line.StartsWith('#'))
                    {
                        continue;
                    }

                    SongLine song_line = new(priority, this.Pv_Db_Priority, line);

                    if (song_line.Parameters.Length == 0)
                    {
                        continue;
                    }

                    // 別の曲になった
                    if (song_line.Pv_No != base_song.Pv_No)
                    {
                        this.Base_Songs_Data.Add(base_song);
                        base_song = new Song();
                    }

                    if (string.IsNullOrEmpty(base_song.Pv_No)) { 
                        base_song.Pv_No = song_line.Pv_No;
                    }
                    if (song_line.Parameters[0] == "another_song")
                    {
                        base_song.Is_Another_Song = true;
                        song_line.Is_Del_Line = true;
                    }
                    if (song_line.Parameters[0] == "ex_song")
                    {
                        base_song.Is_ExSong = true;
                        song_line.Is_Del_Line = true;
                    }
                    else if (song_line.Parameters[0] == "song_name")
                    {
                        base_song.Name = song_line.Value;
                    }
                    else if (song_line.Parameters[0] == "song_name_en")
                    {
                        base_song.Name_En = song_line.Value;
                    }

                    base_song.Lines.Add(song_line);
                }
            }

            // オリジナル楽曲をanother_songの0番目に設定
            foreach (var song in Base_Songs_Data)
            {
                foreach (var song_pv_grp in song.Lines.GroupBy(x => x.Pv_No))
                {
                    Song add_another = new();
                    add_another.Pv_No = song_pv_grp.Key;

                    add_another.Another_No = song_no_cnt.Where(x => x == add_another.Pv_No).Count();

                    foreach (var song_line in song.Lines.Where(x => x.Pv_No == song_pv_grp.Key))
                    {
                        if (song_line.Parameters[0] == "song_name")
                        {
                            add_another.Name = song_line.Value;
                        }
                        if (song_line.Parameters[0] == "song_name_en")
                        {
                            add_another.Name_En = song_line.Value;
                        }
                        if (song_line.Parameters[0] == "song_file_name")
                        {
                            add_another.Song_File_Name = song_line.Value;
                        }
                        if (song_line.Parameters.Length > 2)
                        {
                            if (song_line.Parameters[0] == "another_song" && song_line.Parameters[2] == "vocal_chara_num")
                            {
                                add_another.Vocal_Chara_Num = song_line.Value;
                            }
                            else if (song_line.Parameters[0] == "another_song" && song_line.Parameters[2] == "vocal_disp_name")
                            {
                                add_another.Vocal_Disp_Name = song_line.Value;
                            }
                            else if (song_line.Parameters[0] == "another_song" && song_line.Parameters[2] == "vocal_disp_name_en")
                            {
                                add_another.Vocal_Disp_Name_En = song_line.Value;
                            }
                            else if (song_line.Parameters[0] == "another_song" && song_line.Parameters[1] == "length")
                            {
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(add_another.Name)
                            && !string.IsNullOrEmpty(add_another.Name_En)
                            && !string.IsNullOrEmpty(add_another.Song_File_Name)
                            && !string.IsNullOrEmpty(add_another.Vocal_Disp_Name)
                            && !string.IsNullOrEmpty(add_another.Vocal_Disp_Name_En))
                        {
                            break;
                        }
                    }

                    // song_file_nameが重複しない場合はオリジナル楽曲をanother_songに楽曲を追加
                    if (addAnotherSong.Where(x => x.Song_File_Name == add_another.Song_File_Name).Count() == 0)
                    {
                        if (add_another.Another_No == 0)
                        {
                            //if (string.IsNullOrEmpty(add_another.Vocal_Disp_Name)) { add_another.Vocal_Disp_Name = "オリジナル"; }
                            //if (string.IsNullOrEmpty(add_another.Vocal_Disp_Name_En)) { add_another.Vocal_Disp_Name_En = "Original"; }
                            add_another.Vocal_Disp_Name = "オリジナル";
                            add_another.Vocal_Disp_Name_En = "Original";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(add_another.Vocal_Disp_Name)) { add_another.Vocal_Disp_Name = Path.GetFileName(Song_File_Name); }
                            if (string.IsNullOrEmpty(add_another.Vocal_Disp_Name_En)) { add_another.Vocal_Disp_Name_En = Path.GetFileName(Song_File_Name); }
                        }

                        addAnotherSong.Add(add_another);
                        song_no_cnt.Add(add_another.Pv_No);
                    }
                }
            }

            // ex_song楽曲をanother_songに設定
            foreach (Song base_song in Base_Songs_Data)
            {
                if (base_song.Is_ExSong)
                {
                    Song another = new();
                    another.Pv_No = base_song.Pv_No;
                    another.Name = base_song.Name;
                    another.Name_En = base_song.Name_En;

                    another.Another_No = song_no_cnt.Where(x => x == another.Pv_No).Count();
                    var another_no_now = 0;
                    var another_no_next = another_no_now;

                    foreach (var line in base_song.Lines)
                    {
                        if (line.Parameters[0] == "ex_song")
                        {
                            if (line.Parameters.Length > 2)
                            {
                                another_no_next = int.Parse(line.Parameters[1]);

                                // 次のNoに変わった
                                if (another_no_now != another_no_next)
                                {
                                    this.Add_AnotherSong_Validate(addAnotherSong, another, song_no_cnt);

                                    another = new();
                                    another.Pv_No = base_song.Pv_No;
                                    another.Another_No = song_no_cnt.Where(x => x == another.Pv_No).Count();
                                    another.Name = base_song.Name;
                                    another.Name_En = base_song.Name_En;

                                    // 最初の楽曲分減算
                                    another_no_now = song_no_cnt.Where(x => x == another.Pv_No).Count()-1;
                                    another_no_next = another_no_now;
                                }
                                if (line.Parameters[2] == "chara")
                                {
                                    another.Vocal_Disp_Name = line.Value;
                                    another.Vocal_Disp_Name_En = line.Value;
                                }
                                else if (line.Parameters[2] == "file")
                                {
                                    another.Song_File_Name = line.Value;
                                }
                            }
                            else if (line.Parameters[1] == "length")
                            {
                                this.Add_AnotherSong_Validate(addAnotherSong, another, song_no_cnt);
                            }
                        }
                    }
                }
            }

            // 先頭のAnother_Song記載を読み込む
            foreach (Song song in Base_Songs_Data)
            {
                foreach (var song_pv_grp in song.Lines.GroupBy(x => x.Pv_No))
                {
                    Song another = new();
                    another.Pv_No = song_pv_grp.Key;

                    another.Another_No = song_no_cnt.Where(x => x == another.Pv_No).Count();
                    var another_no_now = 0;
                    var another_no_next = another_no_now;

                    foreach (var song_line in song.Lines.Where(x => x.Pv_No == song_pv_grp.Key))
                    {
                        if (song_line.Parameters[0] == "another_song")
                        {
                            if (song_line.Parameters.Length > 2)
                            {
                                another_no_next = int.Parse(song_line.Parameters[1]);

                                // 次の曲に変わった
                                if (another_no_now != another_no_next)
                                {
                                    this.Add_AnotherSong_Validate(addAnotherSong, another, song_no_cnt);

                                    another = new();
                                    another.Pv_No = song_pv_grp.Key;
                                    another.Another_No = song_no_cnt.Where(x => x == another.Pv_No).Count();
                                    another_no_now++;
                                }
                                if (song_line.Parameters[2] == "name") { another.Name = song_line.Value; }
                                else if (song_line.Parameters[2] == "name_en") { another.Name_En = song_line.Value; }
                                else if (song_line.Parameters[2] == "song_file_name") { another.Song_File_Name = song_line.Value; }
                                else if (song_line.Parameters[2] == "vocal_chara_num") { another.Vocal_Chara_Num = song_line.Value; }
                                else if (song_line.Parameters[2] == "vocal_disp_name") { another.Vocal_Disp_Name = song_line.Value; }
                                else if (song_line.Parameters[2] == "vocal_disp_name_en") { another.Vocal_Disp_Name_En = song_line.Value; }
                            }
                            // another_song.length
                            else
                            {
                                this.Add_AnotherSong_Validate(addAnotherSong, another, song_no_cnt);

                                another = new();
                                another.Pv_No = song_pv_grp.Key;
                                another.Another_No = song_no_cnt.Where(x => x == another.Pv_No).Count();
                                another_no_now++;
                            }
                        }
                    }
                }
            }
        }

        private void Add_AnotherSong_Validate(List<Song> addAnotherSong, Song another, List<string> song_no_cnt)
        {
            if (!string.IsNullOrEmpty(another.Name)
                && !string.IsNullOrEmpty(another.Name_En)
                && !string.IsNullOrEmpty(another.Song_File_Name))
            {
                if (string.IsNullOrEmpty(another.Vocal_Disp_Name)) { another.Vocal_Disp_Name = Path.GetFileName(Song_File_Name); }
                if (string.IsNullOrEmpty(another.Vocal_Disp_Name_En)) { another.Vocal_Disp_Name_En = Path.GetFileName(Song_File_Name); }

                // song_file_nameが重複しない場合は楽曲をanother_songに楽曲を追加
                if (addAnotherSong.Where(x => x.Song_File_Name == another.Song_File_Name).Count() == 0)
                {
                    addAnotherSong.Add(another);
                    song_no_cnt.Add(another.Pv_No);
                }
            }
        }
    }
}
