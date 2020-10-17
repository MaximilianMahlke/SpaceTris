﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Timers;
using System.Diagnostics;


namespace TetrisGui.Tools
{
   public class Timer
    {
        #region VARS
        private DispatcherTimer GameTimer;
        private MainWindow Window;
        private int Counter;



        #endregion VARS

        /// <summary>
        /// Konstruktor für TimerKlasse  
        /// </summary>
        /// <param name="dispatcher2"></param>
       public Timer(Dispatcher dispatcher2, MainWindow window)
        {
            this.Window = window;

            this.GameTimer = new DispatcherTimer(DispatcherPriority.Input, dispatcher2);
            this.GameTimer.Interval = TimeSpan.FromMilliseconds(15);
            this.GameTimer.Tick += new EventHandler(this.UIaction);
            this.GameTimer.Start();

        }




        #region METH
        private void UIaction(object o, EventArgs args)
        {
            Counter++;
            this.Window.Background = new SolidColorBrush(Color.FromRgb((byte)(Counter % 95), (byte)(Counter % 225), (byte)(Counter % 185)));
        }


        /// <summary>
        /// Tickt alle 1000ms und prüft per Sync_Methoden nach Neuigkeiten und aktualisiert per Tock()-Methode, orientiert sich an [GameCounter
        /// </summary>



        public void Time()
        {

            DateTime Start = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(1);
            }
            DateTime Stop = DateTime.Now;
            TimeSpan Result = Stop.Subtract(Start);
            Window.label_Anzeige.Content = Result;
        }



        public void Tick()
        {
            Tick(null);
        }

        private int counter = 0;
        private Dispatcher dispatcher;
        private MainWindow mainWindow;
        public void Tick(DateTime? ZielZeit)
        {
            counter++;
            DateTime Jetzt = DateTime.Now;

            if (ZielZeit == null) // noch kein Ziel, also berechnen
            {
                ZielZeit = Jetzt.AddMilliseconds(1000.0);
            }

            if (Jetzt < ZielZeit) // Ziel noch nicht überschritten (Jetzt kleiner)
            {
                Thread.Sleep(1); // Dirty Fix, damit "er" sich kurz ausruht
                Tick(ZielZeit);
            }
            else
            {
                NextTick();
                Tick();
            }
        }





        public void NextTick()
        {
            int Count = 0;
            String Tick;


            if (Count != 100000)
            {
                Tick = "TICK:  " + Count;
                Window.label_Anzeige.Content = Tick;
                Count++;
            }
            else
            {
                Kill();
            }

        }


        public void Kill()
        {
            Window.label_Anzeige.Content = "Killed";
        }

        public void StartTimer()
        {
            int counter = 0;
            System.Timers.Timer NT = new System.Timers.Timer();
            NT.AutoReset = true;
            NT.Interval = 1000.0;
            //  this.Tack += new EventHandler(Tack);
            NT.Start();
            Window.label_Anzeige.Content = "| NR: " + counter;
            counter++;

            Debug.WriteLine("");

        }

        public void Tack(object sender, EventArgs e)
        {

        }






        #endregion METH
    }
}
