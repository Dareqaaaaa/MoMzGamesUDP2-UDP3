/*
 * Arquivo: NickFilter.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 26/03/2017
 * Sintam inveja, não nos atinge
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Core.filters
{
    public static class NickFilter
    {
        public static List<string> _filter = new List<string>();
        public static void Load()
        {
            if (File.Exists("data/filters/nicks.txt"))
            {
                string line;
                try
                {
                    using (StreamReader file = new StreamReader("data/filters/nicks.txt"))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            _filter.Add(line);
                        }
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.error("[NickFilter] " + ex.ToString());
                }
            }
            else
                Logger.warning("[Aviso]: O arquivo 1 de filtros não existe.");
        }
    }
}