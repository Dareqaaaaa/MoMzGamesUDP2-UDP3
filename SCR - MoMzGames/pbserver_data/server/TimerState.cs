using System;
using System.Threading;

namespace Core.server
{
    public class TimerState
    {
        public Timer Timer = null;
        public DateTime EndDate = new DateTime();
        private object sync = new object();
        public void StartJob(int period, TimerCallback callback)
        {
            lock (sync)
            {
                Timer = new Timer(callback, this, period, -1);
                EndDate = DateTime.Now.AddMilliseconds(period);
             //   Logger.warning("Job started.");
            }
        }
        /// <summary>
        /// Retorna a quantia de segundos restantes para executar o trabalho.
        /// </summary>
        /// <returns></returns>
        public int getTimeLeft()
        {
            if (Timer == null)
                return 0;
            TimeSpan span = EndDate - DateTime.Now;
            int seconds = (int)span.TotalSeconds;
            if (seconds < 0)
                return 0;
            return seconds;
        }
    }
}