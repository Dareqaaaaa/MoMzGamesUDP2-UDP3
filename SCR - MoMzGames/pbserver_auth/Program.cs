/*
 * Arquivo: Program.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/03/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.configs;
using Auth.data.sync;
using Core;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.server;
using Core.xml;
using CrashReporterDotNET;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Core.models.account.players;

namespace Auth
{
    public class Programm
    {
        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportCrash(e.Exception);
        }

        public static void ReportCrash(Exception exception, string developerMessage = "")
        {
            var reportCrash = new ReportCrash("muawayatuatu156@gmail.com")
            {
                DeveloperMessage = developerMessage
            };

            reportCrash.Send(exception);
        }
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            ReportCrash((Exception)unhandledExceptionEventArgs.ExceptionObject);
            Environment.Exit(0);
        }
        private static void v1(ItemsModel item)
        {
            ItemsModel m2 = new ItemsModel(item) { _objId = item._objId };
        }
        private static void v2(ItemsModel item)
        {
            ItemsModel m2 = item.DeepCopy();
        }
        private static void bb(out byte[] a1)
        {
            a1 = new byte[144];
            using (SendGPacket p1 = new SendGPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    //SLOT slot = room._slots[i];
                    p1.writeH(0 + (i * 2), (ushort)10);
                    p1.writeH(32 + (i * 2), (ushort)20);
                    p1.writeH(64 + (i * 2), (ushort)30);
                    p1.writeH(96 + (i * 2), (ushort)40);
                    p1.writeC(128 + (i), (byte)50);
                }
                p1.mstream.ToArray();
            }
        }
        private static void bb(out byte[] a1, out byte[] a2, out byte[] a3, out byte[] a4, out byte[] a5)
        {
            a1 = new byte[32]; //2
            a2 = new byte[32]; //2
            a3 = new byte[32]; //2
            a4 = new byte[32]; //2
            a5 = new byte[16]; //1
            using (SendGPacket p1 = new SendGPacket())
            using (SendGPacket p2 = new SendGPacket())
            using (SendGPacket p3 = new SendGPacket())
            using (SendGPacket p4 = new SendGPacket())
            using (SendGPacket p5 = new SendGPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    //SLOT slot = room._slots[i];
                    p1.writeH((ushort)10);
                    p2.writeH((ushort)20);
                    p3.writeH((ushort)30);
                    p4.writeH((ushort)40);
                    p5.writeC((byte)50);
                }
                p1.mstream.ToArray();
                p2.mstream.ToArray();
                p3.mstream.ToArray();
                p4.mstream.ToArray();
                p5.mstream.ToArray();
            }
        }
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Console.Title = "Point Blank - Auth";
            Logger.StartedFor = "auth";
            Logger.checkDirectorys();
            StringUtil header = new StringUtil();
            header.AppendLine(@"     /-??-/_____________________________\-??-\");
            header.AppendLine(@"    /-??-/       Point Blank AUTH        \-??-\");
            header.AppendLine(@"   /-??-/         by MoMz Games           \-??-\");
            header.AppendLine(@"/-??-/_______________________________________\-??-\");
            string date = ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly(), null).ToString("dd/MM/yyyy HH:mm");
            header.AppendLine("|-??-|__________| Release date |______________|-??-|");
            header.AppendLine("|-??-|         '" + date + "'             |-??-|");
            header.AppendLine("|-??-|   Sintam inveja, não nos atinge        |-??-|");
            header.AppendLine("|-??-|________________________________________|-??-|");
            Logger.info(header.getString());
            ConfigGA.Load();
            ConfigMaps.Load();
            ServerConfigSyncer.GenerateConfig(ConfigGA.configId);
            EventLoader.LoadAll();         
            DirectXML.Start();
            BasicInventoryXML.Load();
            ServersXML.Load();               
            MissionCardXML.LoadBasicCards(2);
            MapsXML.Load();
            ShopManager.Load(2);
            CupomEffectManager.LoadCupomFlags();
            MissionsXML.Load();
            bool check = true;
            foreach (string msg in args)
                if (ComDiv.gen5(msg) == "f2c076c9e8cd34ce4cd122f3d9ae1b28")
                    check = true;
            DateTime LiveDate = ComDiv.GetDate();
            bool item2 = LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 190203220000;
           // if (!LoggerGA.test(check, date, args, item2, LiveDate)) item2 = true;
            if (item2)
                return;
            Auth_SyncNet.Start();
            if (check)
            {
                bool started = LoginManager.Start();
                Logger.warning("[Aviso] Padrão de textos: " + ConfigGB.EncodeText.EncodingName);
                Logger.warning("[Aviso] Modo atual: " + (ConfigGA.isTestMode ? "Testes" : "Público"));
                Logger.warning(StartSuccess());
                if (started)
                    LoggerGA.updateRAM2();
            }
            Process.GetCurrentProcess().WaitForExit();
        }
        private static string StartSuccess()
        {
            if (Logger.erro)
                return "[Aviso] Falha na inicialização.";
            return "[Aviso] Servidor ativo. (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")";
        }
    }
}