using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Launcher
{
    public class Version
    {
        public static string Ver;

        static Version()
        {
            Ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string getVersion()
        {
            return Ver;
        }
    }
}
