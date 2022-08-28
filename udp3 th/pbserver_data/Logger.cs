/*
 * Arquivo: Logger.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 30/11/2017
 * Sintam inveja, não nos atinge
 */

using System;
using System.IO;

namespace Core
{
    public static class Logger
    {
        private static string Date = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        public static string StartedFor = "None";
        private static object Sync = new object();
        public static bool erro;
        public static void LogLogin(string text)
        {
            try
            {
                save("[Date: " + DateTime.Now.ToString("dd/MM/yy HH:mm") + "] Motive: " + text, "login");
            }
            catch
            {
            }
        }
        public static void LogProblems(string text, string problemInfo)
        {
            try
            {
                save("[Data: " + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + "]" + text, problemInfo);
            }
            catch
            {
            }
        }
        public static void LogCMD(string text)
        {
            try
            {
                save(text, "cmd");
            }
            catch
            {
            }
        }
        public static void LogHack(string text)
        {
            try
            {
                save("[" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + "]: " + text, "hack");
            }
            catch
            {
            }
        }
        public static void LogRoom(string text, uint startDate, uint uniqueRoomId)
        {
            try
            {
                save(text, startDate, uniqueRoomId);
            }
            catch
            {
            }
        }
        public static void info(string text)
        {
            write(text, ConsoleColor.Gray);
        }
        public static void warning(string text)
        {
            write(text, ConsoleColor.Yellow);
        }
        public static void error(string text)
        {
            erro = true;
            write(text, ConsoleColor.Red);
        }
        private static void write(string text, ConsoleColor color)
        {
            try
            {
                lock (Sync)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(text);
                    save(text, StartedFor);
                }
            }
            catch
            {
            }
        }
        public static void checkDirectorys()
        {
            try
            {
                if (StartedFor == "auth")
                {
                    if (!Directory.Exists("logs/auth"))
                        Directory.CreateDirectory("logs/auth");
                    if (!Directory.Exists("logs/login"))
                        Directory.CreateDirectory("logs/login");
                }
                else
                {
                    if (!Directory.Exists("logs/cmd"))
                        Directory.CreateDirectory("logs/cmd");
                    if (!Directory.Exists("logs/game"))
                        Directory.CreateDirectory("logs/game");
                    if (!Directory.Exists("logs/rooms"))
                        Directory.CreateDirectory("logs/rooms");
                    if (!Directory.Exists("logs/errorC"))
                        Directory.CreateDirectory("logs/errorC");
                    if (!Directory.Exists("logs/hack"))
                        Directory.CreateDirectory("logs/hack");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void save(string text, uint dateTime, ulong roomId)
        {
            using (FileStream fileStream = new FileStream("logs/rooms/" + dateTime + "-" + roomId + ".log", FileMode.Append))
            using (StreamWriter stream = new StreamWriter(fileStream))
            {
                try
                {
                    if (stream != null)
                        stream.WriteLine(text);
                }
                catch
                {
                }
                stream.Flush();
                stream.Close();
                fileStream.Flush();
                fileStream.Close();
            }
        }
        public static void save(string text, string type)
        {
            using (FileStream fileStream = new FileStream("logs/" + type + "/" + Date + ".log", FileMode.Append))
            using (StreamWriter stream = new StreamWriter(fileStream))
            {
                try
                {
                    if (stream != null)
                        stream.WriteLine(text);
                }
                catch
                {
                }
                stream.Flush();
                stream.Close();
                fileStream.Flush();
                fileStream.Close();
            }
        }
    }
}