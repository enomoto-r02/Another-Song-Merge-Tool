using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Song_Merge_Tool.Util
{
    public static class ToolUtil
    {
        public static string FILE_LOG = "Another Song Merge Tool.log";

        public static string CONSOLE_PREFIX = "[Another Song Merge Tool] ";
        public static string LOG_PREFIX = "["+ DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "] ";

        public static void DebugLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Debug] " + str + "\n", FILE_LOG, true);
        }

        public static void InfoLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Infomation] " + str + "\n", FILE_LOG, true);
        }

        public static void WarnLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Warning] " + str + "\n", FILE_LOG, true);
        }

        public static void ErrorLog(string str)
        {
            FileUtil.WriteFile_UTF_8_NO_BOM(LOG_PREFIX + "[Error] " + str + "\n", FILE_LOG, true);
        }
    }
}
