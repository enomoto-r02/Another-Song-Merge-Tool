using Another_Song_Merge_Tool.Util;

namespace Another_Song_Merge_Tool.DIVA
{

    public class Performer
    {
        public string Chara { get; private set; }
        public string Type { get; set; }
        public int Index { get; set; }

        public Performer()
        {
            this.Index = -1;
        }

        public void SetChara(string target)
        {
            if(DivaUtil.CHARA_STR.ContainsKey(target))
            {
                this.Chara = target;
            }
            else
            {
                this.Chara = "MIK";
            }
        }

        public string ViewCharaValue()
        {
            var ret = "";
            try
            {
                ret = DivaUtil.CHARA_STR[this.Chara];
            }
            catch (Exception)
            {
                ret = "初音ミク";
            }
            return ret;
        }

        public string ViewCharaValueEn()
        {
            var ret = "";
            try
            {
                ret = DivaUtil.CHARA_STR_EN[this.Chara];
            }
            catch (Exception)
            {
                ret = "Miku";
            }
            return ret;
        }
    }
}
