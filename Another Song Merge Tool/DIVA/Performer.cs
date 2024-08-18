using Another_Song_Merge_Tool.Util;

namespace Another_Song_Merge_Tool.DIVA
{

    public class Performer
    {
        public string Chara { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }

        public Performer()
        {
            this.Index = -1;
        }

        public string ViewChara()
        {
            var ret = "";
            if (string.IsNullOrEmpty(this.Chara) == false)
            {
                try
                {
                    ret = DivaUtil.CHARA_STR[this.Chara];
                }
                catch(KeyNotFoundException)
                {
                    ret = "初音ミク";
                }
            }
            return ret;
        }

        public string ViewCharaEn()
        {
            var ret = "";
            if (string.IsNullOrEmpty(this.Chara) == false)
            {
                try
                {
                    ret = DivaUtil.CHARA_STR_EN[this.Chara];
                }
                catch (KeyNotFoundException)
                {
                    ret = "Miku";
                }
            }
            return ret;
        }
    }
}
