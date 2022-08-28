using Core.models.account.mission;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
    public class MissionAwards
    {
        private static List<MisAwards> _awards = new List<MisAwards>();
        public static void Load()
        {
            string path = "data/cards/MissionAwards.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.warning("[MissionAwards] Não existe o arquivo: " + path);
        }

        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                    Logger.warning("[MissionAwards] O arquivo está vazio: " + path);
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
                                    if ("mission".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        int id = int.Parse(xml.GetNamedItem("id").Value);
                                        int blueOrder = int.Parse(xml.GetNamedItem("blueOrder").Value);
                                        int exp = int.Parse(xml.GetNamedItem("exp").Value);
                                        int gp = int.Parse(xml.GetNamedItem("gp").Value);
                                        _awards.Add(new MisAwards(id, blueOrder, exp, gp));
                                    }
                                }
                            }
                        }
                    }
                    catch (XmlException ex)
                    {
                        Logger.error(ex.ToString());
                    }
                }
                fileStream.Dispose();
                fileStream.Close();
            }
        }
        public static MisAwards getAward(int mission)
        {
            lock (_awards)
            {
                for (int i = 0; i < _awards.Count; i++)
                {
                    MisAwards mis = _awards[i];
                    if (mis._id == mission)
                        return mis;
                }
                return null;
            }
        }
    }
}