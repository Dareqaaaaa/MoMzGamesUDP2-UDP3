/*
 * Arquivo: Logger.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 30/11/2017
 * Sinta inveja, não nos atinge
 */

using System;
using System.IO;

namespace Battle
{
    public static class Logger
    {
        private static string name = "logs/battle/" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".log";
        private static object Sync = new object();
        private static void write(string text, ConsoleColor color)
        {
            try
            {
                lock (Sync)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(text);
                    save(text);
                }
            }
            catch
            {
            }
        }
        public static void error(string text)
        {
            write(text, ConsoleColor.Red);
        }
        public static void warning(string text)
        {
            write(text, ConsoleColor.Yellow);
        }
        public static void info(string text)
        {
            write(text, ConsoleColor.Gray);
        }
        private static void save(string text)
        {
            using (FileStream fileStream = new FileStream(name, FileMode.Append))
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
        public static void checkDirectory()
        {
            if (!Directory.Exists("logs/battle"))
                Directory.CreateDirectory("logs/battle");
        }
    }
}