using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
    public class MapsXML
    {
        public static List<byte> TagList = new List<byte>();
        public static List<ushort> ModeList = new List<ushort>();
        public static uint maps1, maps2, maps3, maps4;
        public static void Load()
        {
            string path = "data/maps/modes.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.warning("[MapsXML] Não existe o arquivo: " + path);
        }
        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                    Logger.warning("[MapsXML] O arquivo está vazio: " + path);
                else
                {
                    try
                    {
                        int idx = 0, list2 = 1;
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("map".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        uint flag = (uint)(1 << idx++);
                                        int list = list2;
                                        if (idx == 32) { list2++; idx = 0; }
                                        TagList.Add(byte.Parse(xml.GetNamedItem("tag").Value));
                                        ModeList.Add(ushort.Parse(xml.GetNamedItem("mode").Value));
                                        bool enable = bool.Parse(xml.GetNamedItem("enable").Value);
                                        if (!enable)
                                            continue;
                                        if (list == 1) maps1 += flag;
                                        else if (list == 2) maps2 += flag;
                                        else if (list == 3) maps3 += flag;
                                        else if (list == 4) maps4 += flag; 
                                        else Logger.warning("[Lista não definida] Flag: " + flag + "; List: " + list);
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
    }
}