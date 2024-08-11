using Another_Song_Merge_Tool.Util;
using System.Xml.Linq;

namespace Another_Song_Merge_Tool.DIVA
{
    public class StageLogic
    {
        public string Mod_Name { get; set; }
        public string Stage_Bin_Name { get; set; }
        public string Stage_Bin_Path { get; set; }
        public string Stage_Xml_Name { get; set; }
        public string Stage_Xml_Path { get; set; }
        public int Stage_Priority { get; set; }
        public List<StageData> Stages { get; set; }

        public StageLogic(string mod_name, int stage_priority)
        {
            this.Mod_Name = mod_name;
            this.Stage_Priority = stage_priority;
            this.Stages = new();
        }

        public void Init(AppConfig appConfig, string stageDataBinName)
        {
            if (string.IsNullOrEmpty(this.Stage_Bin_Path) == false && File.Exists(this.Stage_Bin_Path))
            {
                File.Copy(this.Stage_Bin_Path, stageDataBinName, true);
            }

            // DataBaseConverter.exeでmod_stage_data.binを変換
            appConfig.MikuMikuLibrary.Load(appConfig, stageDataBinName);
        }

        public void Load(AppConfig appConfig, string stageDataBinName)
        {
            var stageDataXmlName = Path.ChangeExtension(stageDataBinName, "xml");

            if (string.IsNullOrEmpty(this.Stage_Bin_Path) == false)
            {
                // Another Song Merge Toolフォルダにコピー
                if (string.IsNullOrEmpty(this.Stage_Bin_Path) || string.IsNullOrEmpty(stageDataBinName))
                {
                    return;
                }
                else
                {
                    File.Copy(this.Stage_Bin_Path, stageDataBinName, true);
                }

                // DataBaseConverter.exeでmod_stage_data.binをxmlに変換
                appConfig.MikuMikuLibrary.Load(appConfig, stageDataBinName);

                StageLogic.Merge(Mod.FILE_STAGE_XML_MOD, stageDataXmlName, Mod.FILE_STAGE_XML_MOD);

                if (string.IsNullOrEmpty(stageDataXmlName) == false && File.Exists(stageDataXmlName))
                {
                    File.Delete(stageDataXmlName);
                }
                else
                {
                    ToolUtil.ErrorLog("File \"" + stageDataXmlName + "\" is Not Found.");
                    return;
                }

                if (string.IsNullOrEmpty(stageDataBinName) == false && File.Exists(stageDataBinName))
                {
                    File.Delete(stageDataBinName);
                }
                else
                {
                    ToolUtil.ErrorLog("File \"" + stageDataBinName + "\" is Not Found.");
                    return;
                }
            }
        }

        public static void End(AppConfig appConfig)
        {
            // DataBaseConverter.exeでmod_stage_data.binをxmlに変換
            appConfig.MikuMikuLibrary.Load(appConfig, Mod.FILE_STAGE_XML_MOD);

            if (File.Exists(Mod.FILE_STAGE_BIN_MOD))
            {
                File.Move(Mod.FILE_STAGE_BIN_MOD, "./rom/" + Mod.FILE_STAGE_BIN_MOD, true);
            }
            else
            {
                ToolUtil.ErrorLog("File \"" + Mod.FILE_STAGE_BIN_MOD + "\" is Not Found.");
                return;
            }
            if (File.Exists(Mod.FILE_STAGE_XML_MOD))
            {
                File.Delete(Mod.FILE_STAGE_XML_MOD);
            }
            else
            {
                ToolUtil.ErrorLog("File \"" + Mod.FILE_STAGE_XML_MOD + "\" is Not Found.");
                return;
            }
        }

        public static void Merge(string basePath, string targetPath, string savePath)
        {
            var doc1 = XDocument.Load(basePath);
            var doc2 = XDocument.Load(targetPath);

            var stages = doc2.Root.Elements("Stages").Elements("Stage");

            foreach (var stage in stages)
            {
                var stage_name = stage.Element("Name").Value;
                var hoge = doc1.Descendants("Name").Where(x => x.Value == stage_name);
                if (hoge.Count() == 0)
                {
                    doc1.Root.Elements("Stages").Last().Add(stage);
                }
            }

            doc1.Save(savePath);
        }
    }
}
