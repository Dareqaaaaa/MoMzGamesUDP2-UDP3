using Core.models.account.rank;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
    public class ClanRankXML
    {
        private static List<RankModel> _ranks = new List<RankModel>();
        public static void Load()
        {
            string path = "data/ranktemplate/rankclantemplate.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.warning("[ClanRankXML] Não existe o arquivo: " + path);
        }
        public static RankModel getRank(int rankId)
        {
            lock (_ranks)
            {
                for (int i = 0; i < _ranks.Count; i++)
                {
                    RankModel rank = _ranks[i];
                    if (rank._id == rankId)
                        return rank;
                }
                return null;
            }
        }
        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                    Logger.warning("[ClanRankXML] O arquivo está vazio: " + path);
                else
                {
                    try
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("rank".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        _ranks.Add(new RankModel(int.Parse(xml.GetNamedItem("id").Value),
                                            int.Parse(xml.GetNamedItem("onNextLevel").Value), 0,
                                            int.Parse(xml.GetNamedItem("onAllExp").Value)));
                                    }
                                }
                            }
                        }
                    }
                    catch (XmlException ex)
                    {
                        Logger.warning(ex.ToString());
                    }
                }
                fileStream.Dispose();
                fileStream.Close();
            }
        }
    }
}