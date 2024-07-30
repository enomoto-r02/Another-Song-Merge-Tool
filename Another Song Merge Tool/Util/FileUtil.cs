namespace ModDbMerge2.Util
{
    public static class FileUtil
    {
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void WriteFile(string str, string path, Boolean addFlg)
        {
            using (StreamWriter writer = new StreamWriter(
                path,
                addFlg,
                System.Text.Encoding.UTF8
            ))
            {
                writer.Write(str);
                writer.Close();
            }
        }

        public static string ReadFile(string path)
        {
            string ret = "";

            using (StreamReader sr = new StreamReader(
                path,
                System.Text.Encoding.UTF8
            ))
            {
                ret = sr.ReadToEnd();
                sr.Close();
            }

            return ret;
        }

        public static string Backup(string file_name)
        {
            var new_file_name = "";
            if (File.Exists(file_name))
            {
                new_file_name = Path.GetFileNameWithoutExtension(file_name) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(file_name);
                File.Move(file_name, new_file_name);
            }

            return new_file_name;
        }
    }
}
