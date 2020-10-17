using System;
using System.Runtime.InteropServices;

namespace TetrisApp.Tools
{
    public class Timer_Core
    {
        #region VARS
        private int TimerId;
        private TimerEventHandler TimerHandler;  // NOTE: declare at class scope so garbage collector doesn't release it!!!
        private DateTime TimerStarted;
        private int MillisecondIntervall;
        public Boolean Enabled;
        public delegate void TickDelegate();
        public event TickDelegate Tick;
        #endregion VARS

        #region CONS
        public Timer_Core()
        {
        }
        #endregion CONS


        #region METHS
        /// <summary>
        /// Startet den System-Timer
        /// </summary>
        /// <param name="MillisecondInterval"></param>
        public void StartSystemTimer(int MillisecondInterval)
        {
            Enabled = true;
            this.MillisecondIntervall = MillisecondInterval;
            timeBeginPeriod(MillisecondInterval);
            TimerHandler = new TimerEventHandler(TimerCallback);
            TimerId = timeSetEvent(MillisecondInterval, 0, TimerHandler, IntPtr.Zero, EVENT_TYPE);
            TimerStarted = DateTime.Now;
        }

        public void Stop()
        {
            Enabled = false;
            TimerId = 0;
            int err = timeKillEvent(TimerId);
            timeEndPeriod(MillisecondIntervall);
            System.Threading.Thread.Sleep(100); // Ensure callbacks are drained
        }

        private void TimerCallback(int id, int msg, IntPtr user, int dw1, int dw2)
        {
            if (Tick != null)   // Wenn sich jemand von Außen auf das Event verknüpft, wird das Event ausgelöst (Invoke)
                Tick.Invoke();
        }

        // P/Invoke declarations
        private delegate void TimerEventHandler(int id, int msg, IntPtr user, int dw1, int dw2);
        private const int TIME_PERIODIC = 1;
        private const int EVENT_TYPE = TIME_PERIODIC;// + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerEventHandler handler, IntPtr user, int eventType);
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);
        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);
        #endregion METH
    }
}
