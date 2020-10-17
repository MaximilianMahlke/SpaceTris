﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TetrisApp;
using TetrisApp.Elements.Steine;
using TetrisApp.Tools;
using TetrisGui.GUI;
using TetrisGui.Tools;
using TetrisGui.Tools.Interaction;
using TetrisData;

namespace TetrisGui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Game = new Game();

            #region UI
            Game.GuiInteraction = new Gui_Interaction(this, Game);
            this.label_TIME.Content = "PLAYTIME : " + System.Environment.NewLine + "00:00:00";
            this.label_Controls1.Content = "SYSTEM-CONTROLS : " + System.Environment.NewLine + " • ENTER = Start" + System.Environment.NewLine + " • SPACE = Pause" + System.Environment.NewLine +
                                         " • ESCAPE  = Quit ";

            this.label_Controls.Content = "MOVEMENT-CONTROLS : " + System.Environment.NewLine + " • LEFT = Move left" + System.Environment.NewLine +
                                            " • RIGHT = Move right" + System.Environment.NewLine + " • DOWN = Move down" + System.Environment.NewLine + " • UP = Rotate";
            //  Intro();


            this.KeyDown += new KeyEventHandler(Window_KeyDown);
            // Erstelle GameBoard
            Board_UI_GameBoard = new BoardUI(this, Game, Grid_GameBoard);
            Board_UI_GameBoard.ErstelleGrid(this.Game.Config.Width_GameBoard, this.Game.Config.Height_GameBoard);
            Board_UI_GameBoard.GenerateBoardFeldArray(this.Game.Config.Width_GameBoard, this.Game.Config.Height_GameBoard);

            // Erstelle PreviewBoard
            Board_UI_PreviewBoard = new BoardUI(this, Game, Grid_PreviewBoard);
            Board_UI_PreviewBoard.ErstelleGrid(this.Game.Config.Width_Preview, this.Game.Config.Height_Preview);
            Board_UI_PreviewBoard.GenerateBoardFeldArray(this.Game.Config.Width_Preview, this.Game.Config.Height_Preview);

            Sync = new SyncMaster(Game, Board_UI_GameBoard, Board_UI_PreviewBoard, this);
            this.Timer = new Timer_GUI(this.Dispatcher, this, Sync, Game);

            //this.Width = 1468;
            //this.Height = 826;
            // 0,5626702997275204

            this.Width = 1680;
            this.Height = 1680 * ((double)826 / 1468);

            //this.Height = 1050;
            // 0,625
            //    this.Show();
            //System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
            //screen.Bounds
            #endregion UI


        }


        #region VARS
        // Timer für UI
        private BoardUI Board_UI_GameBoard;
        private BoardUI Board_UI_PreviewBoard;
        private Game Game;
        private Timer_GUI Timer;
        public SyncMaster Sync;
        public BGAnimation BackgroundAnimation;
        public static MainWindow MainMainWindow;
        public List<Storyboard> SchlingernStoryboards = new List<Storyboard>();
        public TimeSpan SchlingernDuration = TimeSpan.FromMilliseconds(2000);
        public double SchlingernMargin = 10;
        public dynamic test;
        public Storyboard Storyboard_GameOver;
        SQL Sql = new SQL();
        XML Xml = new XML();
        #endregion VARS


        #region METHS

        public void Intro()
        {
            Thread thread = new Thread(() =>
            {
                Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                int Dauer_Millis = 5000;

                DateTime StartZeit = DateTime.Now;
                DateTime EndZeit = StartZeit.AddMilliseconds(Dauer_Millis);
                byte Dauer = (byte)random.Next(500, 1000);

                SpinWait sw = new SpinWait();

                int count = 0;
                double y = 1;
                double x = 0;

                while (DateTime.Now < EndZeit)
                {
                    if (DateTime.Now < StartZeit.AddMilliseconds(3300))
                    {
                        this.Game.Board.Array_Board[random.Next((int)x), random.Next((int)y)].Color = new TetrisApp.Elements.Steine.Color(
                            (byte)random.Next(256),
                            (byte)random.Next(256),
                            (byte)random.Next(256),
                            (byte)random.Next(256));
                    }

                    else
                    {
                        this.Game.Board.Array_Board[random.Next((int)x), random.Next((int)y)].Color = null;
                    }
                    count++;

                    if (y < 18.9)
                    {
                        y += 0.015;
                    }

                    if (x < 10.9)
                    {
                        x += 0.009;
                    }

                    Dauer = (byte)(Dauer - 1.85);
                    if (Dauer < 13)
                    {
                        Dauer = 14;
                    }

                    for (int i = 0; i < Dauer; i++)
                    {
                        sw.SpinOnce();
                    }
                    //Thread.Sleep(Dauer);
                }
                count = 0;
                this.Game.Board.Clear(this.Game.Board);
            });
            thread.Start();

        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Background
            BackgroundAnimation = new BGAnimation(Grid_Main, Grid_Main, this.Game, this);

            Grid_Main.Background = BackgroundAnimation.InitializeAnimation();
            BackgroundAnimation.StartColorAnimation();
            BackgroundAnimation.StartBlockAnimation(150);

            //{
            //    Storyboard storyboard = new Storyboard();

            //    ThicknessAnimation da = new ThicknessAnimation();
            //    da.From = new Thickness(-10, -10, 0, 0);
            //    da.To = new Thickness(10, 10, 0, 0);
            //    da.RepeatBehavior = RepeatBehavior.Forever;
            //    da.Duration = TimeSpan.FromMilliseconds(1000);

            //    storyboard.AutoReverse = true;
            //    storyboard.Children.Add(da);

            //    Storyboard.SetTarget(da, Grid_MainStructure);
            //    Storyboard.SetTargetProperty(da, new PropertyPath(Grid.MarginProperty));

            //    storyboard.Begin(Grid_MainStructure);
            //}

            // <Grid.RenderTransform>
            //    <TransformGroup>
            //        <ScaleTransform/>
            //        <SkewTransform/>
            //        <RotateTransform/>
            //        <TranslateTransform X="10"/>
            //    </TransformGroup>
            //</Grid.RenderTransform>

            {
                {
                    var tg = new TransformGroup();
                    var translation = new TranslateTransform(0, 0);
                    var translationName = "myTranslation";
                    RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Grid_MainStructure.RenderTransform = tg;
                }

                //{
                //    var translation = new TranslateTransform(0, 0);
                //    var translationName = "myTranslation2";
                //    RegisterName(translationName, translation);
                //    TransformGroup_Cockpit.Children.Add(translation);
                //    //this.Image_Cockpit.RenderTransform = tg;
                //}

                //var anim = new DoubleAnimation(3, 100, new Duration(new TimeSpan(0, 0, 0, 1, 0)))
                //{
                //    EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut }
                //};

                AnimateSchlingern(10, SchlingernDuration, TranslateTransform.YProperty);
                AnimateSchlingern(10, SchlingernDuration, TranslateTransform.XProperty);
            }
        }
        

        private void AnimateSchlingern(double To, TimeSpan Duration, DependencyProperty dp)
        {
            var storyboard = new Storyboard();
            {
                DoubleAnimation da = new DoubleAnimation()
                {
                    To = To,
                    Duration = Duration
                };
                Storyboard.SetTargetName(storyboard, "myTranslation");
                Storyboard.SetTargetProperty(storyboard, new PropertyPath(dp));

                storyboard.Children.Add(da);
            }

            storyboard.Completed += new EventHandler((obj, args) =>
            {
                SchlingernStoryboards.Remove(storyboard);

                Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                double to = (double)r.Next(-(int)(SchlingernMargin * 100), (int)(SchlingernMargin * 100)) / 100;

                AnimateSchlingern(to, TimeSpan.FromMilliseconds(r.Next((int)(SchlingernDuration.TotalMilliseconds * 0.5), (int)SchlingernDuration.TotalMilliseconds)), dp);
            }
            );

            SchlingernStoryboards.Add(storyboard);

            storyboard.Begin(this, true);
        }
        
        // NO USE!
        public void CheckBeforeExport(Game game)
        {
            SaveGame SaveGame = new SaveGame(this.Game);
            //  this.Game = null;
            SaveGame.Json.ExportData(SaveGame, "Game");

            ////  Game Tempgame = game;

            //// GuiInteraction genau wie Rec.Events
            //this.Game.GuiInteraction = null;
            //this.Game.Json.Export(this.Game, "GuiInteraction");
            //this.Game.GuiInteraction = null;

            //// RecurringEvents in eigene Datei Auslagern (wegen Zirkelverweis beim Schreiben)
            ////    this.Game.Json.Export(this.Game.RecurringEvents, "RecurringEvents");

            //this.Game.RecurringEvents = null;

            //this.Game.NextBlock = null;
            //this.Game.CurrentBlock = null;

            ////this.Game.Json.Export(this.Game.CurrentBlock, "CurrrentBlock");
            ////this.Game.Board.Array_Board.
            //// da beide Zirkelverweise nun ausgelagert sind kann das Game-Objekt ausgelagert werden
            //this.Game.Json.Export(this.Game, "Game");
        }


        #endregion METHS


        #region BUTTONS
        private void Button_Start(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false)
            {
                this.Game.Config.UserName = this.textBox_UserName.Text;
                this.textBox_UserName.IsEnabled = false;
                this.Game.StartGame();
                this.button_Start.IsEnabled = false;
                this.button_Pause.IsEnabled = true;

            }
        }


        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false)
            {
                if (this.Game.CurrentBlock.Move_down() == false && this.Game.GameRunning == true)
                {
                    TetrisApp.Tools.Utils.SpawnBlock(this.Game);
                }
            }
        }


        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false)
            {
                this.Game.Board.Clear(this.Game.Board);
            }
        }


        private void Button_Move_left(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false && this.Game.GameRunning == true)
            {
                this.Game.CurrentBlock.Move_left();
            }
        }


        private void Button_Move_right(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false)
            {
                this.Game.CurrentBlock.Move_right();
            }
        }


        private void Button_Rotate(object sender, RoutedEventArgs e)
        {
            if (this.Game.GameIsOver == false)
            {
                this.Game.CurrentBlock.Rotate();
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }


        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Game.Pause();
        }


        private void Button_Save(object sender, RoutedEventArgs e)
        {
            this.Game.SaveGame();
        }


        private void Button_Load(object sender, RoutedEventArgs e)
        {
            this.Game.LoadGame();
        }


        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < this.Game.Config.Width_GameBoard; x++)
            {
                this.Game.Board.Array_Board[x, this.Game.Config.Height_GameBoard - 1].Color = new TetrisApp.Elements.Steine.Color(255, 255, 255);
                Block b = Utils.GenerateBlock(this.Game);
                this.Game.Board.Array_Board[x, this.Game.Config.Height_GameBoard - 1].block = b;
            }
        }


        private void Button_SQL(object sender, RoutedEventArgs e)
        {
            Sql.UpdateData(this.Game.Config.UserName, this.Game.Config.Score);
        }

        private void Button_Click_XML(object sender, RoutedEventArgs e)
        { 
            Xml.ImportData();
            Xml.Add_Current_Score(this.Game.Config.UserName, this.Game.Config.Score);
            Xml.SortXmlList();
            Xml.UpdateAndShowData();
            this.label_HighScore.Content = Xml.StringOfAllHighScores;
            this.border_highscore.Opacity = 1.0;
        }

        private void Button_Click_SQLDownload(object sender, RoutedEventArgs e)
        {
            Sql.DownloadData();
            this.label_HighScore.Content = Sql.ShowHighscore();
            this.border_highscore.Opacity = 1.0;
        }

        //User Input
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Game.LastBlockMoveByUser == null ||
                this.Game.LastBlockMoveByUser < DateTime.Now.Subtract(TimeSpan.FromMilliseconds(50)))
            {

                switch (e.Key)
                {
                    case Key.Left:
                        if (this.Game.GameRunning == true)
                        {
                            this.Game.CurrentBlock.Move_left(true);
                        }
                        break;

                    case (Key.Right):
                        if (this.Game.GameRunning == true)
                        {
                            this.Game.CurrentBlock.Move_right(true);
                        }
                        break;

                    case Key.Down:
                        if (this.Game.GameRunning == true)
                        {
                            this.Game.CurrentBlock.Move_down(true);
                        }
                        break;

                    case Key.Up:
                        if (this.Game.GameRunning == true)
                        {
                            this.Game.CurrentBlock.Rotate(true);
                        }
                        break;

                    case Key.Escape:
                        Process.GetCurrentProcess().Kill();
                        Environment.Exit(0);
                        break;

                    case Key.Space:
                        this.Game.Pause();
                        break;
                }
            }
        }

        #endregion BUTTONS


        private void textBox_UserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.button_Start.IsEnabled = true;
        }

       

     

       

   
    }
}






/* Meine Tabellen:
 * 
 *
 * CREATE TABLE [dbo].[Player] (
    [PlayerID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([PlayerID] DESC),
    CONSTRAINT [FK_Player_Score] FOREIGN KEY ([PlayerID]) REFERENCES [dbo].[Score] ([ScoreID])
);


 * 
 * 
 * CREATE TABLE [dbo].[Score] (
    [ScoreID] INT IDENTITY (1, 1) NOT NULL,
    [Score]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ScoreID] DESC)
);

 * 
 * 
 * FK:
 *  -- CONSTRAINT [FK_Score_To_Player] FOREIGN KEY ([Player]) REFERENCES [Player]([PlayerID])
 * 
 * 
 * 
 * 
 * 
*/