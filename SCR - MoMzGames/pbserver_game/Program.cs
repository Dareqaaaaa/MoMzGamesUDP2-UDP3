/*
 * Arquivo: Program.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/03/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.filters;
using Core.managers;
using Core.managers.events;
using Core.managers.server;
using Core.models.account.players;
using Core.server;
using Core.sql;
using Core.xml;
using CrashReporterDotNET;
using Game.data.sync;
using Game.data.managers;
using Game.data.xml;
using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Game
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
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            Console.Title = "Point Blank - Game";
            Logger.StartedFor = "game";
            Logger.checkDirectorys();
            StringUtil header = new StringUtil();
            header.AppendLine(@"     /-??-/_____________________________\-??-\");
            header.AppendLine(@"    /-??-/       Point Blank GAME        \-??-\");
            header.AppendLine(@"   /-??-/          by MoMz Games          \-??-\");
            header.AppendLine(@"/-??-/_______________________________________\-??-\");
            string date = ComDiv.GetLinkerTime(Assembly.GetExecutingAssembly(), null).ToString("dd/MM/yyyy HH:mm");
            header.AppendLine("|-??-|__________| Release date |______________|-??-|");
            header.AppendLine("|-??-|         '" + date + "'             |-??-|");
            header.AppendLine("|-??-|   Sintam inveja, não nos atinge        |-??-|");
            header.AppendLine("|-??-|________________________________________|-??-|");
            Logger.info(header.getString());
            ConfigGS.Load();
           // if (ComDiv.gen5(Console.ReadLine()) == "33a4a02e0eeec4b13b00f23a380014f0")
           // {
                Console.Clear();
                DateTime LiveDate = ComDiv.GetDate();
                //if (LiveDate == new DateTime() || long.Parse(LiveDate.ToString("yyMMddHHmmss")) >= 190203220000)
               //     return;
                BasicInventoryXML.Load();
                ServerConfigSyncer.GenerateConfig(ConfigGS.configId);
                ServersXML.Load();
                ChannelsXML.Load(ConfigGS.serverId);
                EventLoader.LoadAll();
                TitlesXML.Load();
                TitleAwardsXML.Load();
                ClanManager.Load();
                NickFilter.Load();
                MissionCardXML.LoadBasicCards(1);
                BattleServerXML.Load();
                RankXML.Load();
                RankXML.LoadAwards();
                ClanRankXML.Load();
                MissionAwards.Load();
                MissionsXML.Load();
                Translation.Load();
                ShopManager.Load(1);
                ClassicModeManager.LoadList();
                RandomBoxXML.LoadBoxes();
                CupomEffectManager.LoadCupomFlags();
                bool check = true;
                foreach (string msg in args)
                    if (ComDiv.gen5(msg) == "fb7af2d57e4c4be822ebc028d4c7d2c5")
                        check = true;
                Game_SyncNet.Start();
                if (check)
                {
                    bool started = GameManager.Start();
                    Logger.warning("[Aviso] Padrão de textos: " + ConfigGB.EncodeText.EncodingName);
                    Logger.warning("[Aviso] Modo atual: " + (ConfigGS.isTestMode ? "Testes" : "Público"));
                    Logger.warning(StartSuccess());
                    if (started)
                        LoggerGS.updateRAM();
                }
           // }
            Process.GetCurrentProcess().WaitForExit();
        }
        private static string StartSuccess()
        {
            if (Logger.erro)
                return "[Aviso] Falha na inicialização.";
            return "[Aviso] Servidor ativo. (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")";
        }
        public static bool Create(int rank, ItemsModel msg)
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@rank", rank);
                    command.Parameters.AddWithValue("@id", msg._id);
                    command.Parameters.AddWithValue("@name", msg._name);
                    command.Parameters.AddWithValue("@count", (int)msg._count);
                    command.Parameters.AddWithValue("@equip", msg._equip);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO info_rank_awards(rank_id,item_id,item_name,item_count,item_equip)VALUES(@rank,@id,@name,@count,@equip)";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}