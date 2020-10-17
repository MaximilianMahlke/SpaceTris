using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGui.Tools
{
   public class Timer_Sys
   {
       #region VARS
       
       private int mTimerId;
       private TimerEventHandler mHandler;
       private int mTestTick;
       private DateTime mTestStart;
     
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



       #endregion VARS

       #region CONST
       #endregion CONST


       #region METHS

       public void Start_Timer_Sys()  // Startmethode initialisiert unhd startet den Timer 
       {
           timeBeginPeriod(1);
           mHandler = new TimerEventHandler(TimerCallback);
           mTimerId = timeSetEvent(1, 0, mHandler, UIntPtr.Zero, EVENT_TYPE);
       }




       #endregion METHS




   }
}
