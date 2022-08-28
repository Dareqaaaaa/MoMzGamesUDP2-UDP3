/*
 * Arquivo: BattleServerXML.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 09/10/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace Game.data.xml
{
    public static class BattleServerXML
    {
        public static List<BattleServer> Servers = new List<BattleServer>();
        public static void Load()
        {
            string path = "data/battle/battleserverlist.xml";
            if (File.Exists(path))
                parse(path);
            else
                Logger.info("[BattleServerXML] Não existe o arquivo: " + path);
        }
        public static BattleServer GetRandomServer()
        {
            if (Servers.Count == 0)
                return null;
            Random rnd = new Random();
            int idx = rnd.Next(Servers.Count);
            try
            {
                return Servers[idx];
            }
            catch { return null; }
        }
        private static void parse(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (fileStream.Length > 0)
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                XmlNamedNodeMap xml2 = xmlNode1.Attributes;
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("server".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        BattleServer server = new BattleServer(
                                            xml.GetNamedItem("ip").Value,
                                            int.Parse(xml.GetNamedItem("sync").Value))
                                        {
                                            Port = int.Parse(xml.GetNamedItem("port").Value),
                                        };
                                        Servers.Add(server);
                                    }
                                }
                            }
                        }
                    }
                    fileStream.Dispose();
                    fileStream.Close();
                }
            }
            catch (XmlException ex)
            {
                Logger.error("[BattleServerXML] Erro no arquivo: " + path + "\r\n" + ex.ToString());
            }
        }
    }
    public class BattleServer
    {
        public string IP;
        public int Port, SyncPort;
        public IPEndPoint Connection;
        public BattleServer(string ip, int syncPort)
        {
            IP = ip;
            SyncPort = syncPort;
            Connection = new IPEndPoint(IPAddress.Parse(ip), syncPort);
        }
    }
}