using System;
using System.Windows.Threading;
using TetrisApp;


namespace TetrisGui.Tools
{
    public class Timer_GUI
    {
        #region VARS
        private DispatcherTimer DispatcherTimer;
        private MainWindow Window;
        private SyncMaster SyncM;
        private Game Game;

        #endregion VARS


        #region CONS
        /// <summary>
        /// Konstruktor für TimerKlasse  
        /// </summary>
        /// <param name="dispatcher2"></param>
        public Timer_GUI(Dispatcher dispatcher2, MainWindow window, SyncMaster syncMaster, Game spiel)
        {
            this.Window = window;
            this.SyncM = syncMaster;
            this.Game = spiel;
            this.DispatcherTimer = new DispatcherTimer(DispatcherPriority.Input, dispatcher2);
            this.DispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            this.DispatcherTimer.Tick += new EventHandler(this.UIaction);
            this.DispatcherTimer.Start();
        }
        #endregion CONS


        #region METH

        /// <summary>
        /// Timer führt hede ms die Refresh-Methoden aus
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private void UIaction(object o, EventArgs args)
        {
            SyncM.Sync_Refresh();
            SyncM.Sync_CoreData();
        }

        #endregion METH
    }
}
