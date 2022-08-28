using Core.models.account.players;
using Core.models.randombox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
    public class RandomBoxXML
    {
        private static SortedList<int, RandomBoxModel> boxes = new SortedList<int, RandomBoxModel>();
        public static void LoadBoxes()
        {
            DirectoryInfo folder = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\data\cupons");
            if (!folder.Exists)
                return;
            foreach (FileInfo file in folder.GetFiles())
            {
                try
                {
                    LoadBox(int.Parse(file.Name.Substring(0, file.Name.Length - 4)));
                }
                catch { }
            }
        }
        private static void LoadBox(int id)
        {
            string path = "data/cupons/" + id + ".xml";
            if (File.Exists(path))
                parse(path, id);
            else
                Logger.warning("[RandomBoxXML] Não existe o arquivo: " + path);
        }
        private static void parse(string path, int cupomId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length == 0)
                    Logger.warning("[RandomBoxXML] O arquivo está vazio: " + path);
                else
                {
                    try
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                XmlNamedNodeMap xml2 = xmlNode1.Attributes;
                                RandomBoxModel box = new RandomBoxModel
                                {
                                    itemsCount = int.Parse(xml2.GetNamedItem("count").Value)
                                };
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("item".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        ItemsModel item = null;
                                        int itemId = int.Parse(xml.GetNamedItem("id").Value);
                                        uint count = uint.Parse(xml.GetNamedItem("count").Value);
                                        if (itemId > 0)
                                        {
                                            item = new ItemsModel(itemId)
                                            {
                                                _name = "Randombox",
                                                _equip = int.Parse(xml.GetNamedItem("equip").Value),
                                                _count = count
                                            };
                                        }
                                        box.items.Add(new RandomBoxItem
                                        {
                                            index = int.Parse(xml.GetNamedItem("index").Value),
                                            percent = int.Parse(xml.GetNamedItem("percent").Value),
                                            special = bool.Parse(xml.GetNamedItem("special").Value),
                                            count = count,
                                            item = item
                                        });
                                    }
                                }
                                box.SetTopPercent();
                                boxes.Add(cupomId, box);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.error("[Box: " + cupomId + "] " + ex.ToString());
                    }
                }
                fileStream.Dispose();
                fileStream.Close();
            }
        }
        public static bool ContainsBox(int id)
        {
            return boxes.ContainsKey(id);
        }
        public static RandomBoxModel getBox(int id)
        {
            try
            {
                return boxes[id];
            }
            catch 
            { 
                return null; 
            }
        }
    }
}