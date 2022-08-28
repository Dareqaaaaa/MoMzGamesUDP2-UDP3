using Core.models.account.players;
using Core.models.account.title;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Core.xml
{
    public class TitleAwardsXML
    {
        public static List<TitleA> awards = new List<TitleA>();
        public static void Load()
        {
            string path = "data/titles/title_awards2.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.warning("[TitleAwards] Não existe o arquivo: " + path);
        }
        /// <summary>
        /// Gera uma lista das premiações de um título.
        /// </summary>
        /// <param name="titleId">Título</param>
        /// <returns></returns>
        public static List<ItemsModel> getAwards(int titleId)
        {
            List<ItemsModel> items = new List<ItemsModel>();
            lock (awards)
            {
                for (int i = 0; i < awards.Count; i++)
                {
                    TitleA title = awards[i];
                    if (title._id == titleId)
                        items.Add(title._item);
                }
            }
            return items;
        }
        /// <summary>
        /// Checa se existe um item de um título que possua um id igual.
        /// </summary>
        /// <param name="titleId">Título</param>
        /// <param name="itemId">Id do item</param>
        /// <returns></returns>
        public static bool Contains(int titleId, int itemId)
        {
            if (itemId == 0)
                return false;
            for (int i = 0; i < awards.Count; i++)
            {
                TitleA title = awards[i];
                if (title._id == titleId && title._item._id == itemId)
                    return true;
            }
            return false;
        }
        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (fileStream.Length > 0)
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
                                    if ("title".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        int itemId = int.Parse(xml.GetNamedItem("itemid").Value);
                                        awards.Add(new TitleA { _id = int.Parse(xml.GetNamedItem("id").Value), _item = new ItemsModel(itemId, "Title reward", int.Parse(xml.GetNamedItem("equip").Value), uint.Parse(xml.GetNamedItem("count").Value)) });
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