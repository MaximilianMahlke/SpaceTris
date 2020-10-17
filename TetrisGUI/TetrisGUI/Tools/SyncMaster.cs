﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TetrisApp;
using TetrisGui.GUI;
using TetrisGui.Tools.Interaction;



namespace TetrisGui.Tools
{
    public class SyncMaster
    {


        #region VARS
        public Game Game;
        public BoardUI Board_UI_GameBoard;
        public BoardUI Board_UI_PreviewBoard;
        public MainWindow Window;
        public BGAnimation BgAnimation;

        #endregion VARS

        #region CONST

        public SyncMaster(Game Spiel, BoardUI Board_UI_GameBoard, BoardUI Board_UI_Preview, MainWindow Window)
        {
            // TODO: Complete member initializations
            this.Game = Spiel;
            this.Board_UI_GameBoard = Board_UI_GameBoard;
            this.Board_UI_PreviewBoard = Board_UI_Preview;
            this.Window = Window;
        }

        #endregion CONST

        #region METH

        public void Sync_CoreData()
        {
        }


        // Skaliert das Spielfeld entsprechend der Fenstergröße
        private void ScaleBoard()
        {
            int AnzSpalten = this.Game.Config.Width_GameBoard;
            int AnzZeilen = this.Game.Config.Height_GameBoard;
            double GridWidth = this.Window.Grid_GameBoardOuter.ActualWidth;
            double GridHeight = this.Window.Grid_GameBoardOuter.ActualHeight;

            double BreitenVerhaeltnis_Grid = GridWidth / GridHeight;
            double BreitenVerhaeltnis_App = (double)AnzSpalten / AnzZeilen;

            if (BreitenVerhaeltnis_App > BreitenVerhaeltnis_Grid)
            {
                this.Window.Grid_GameBoard.MaxWidth = GridWidth;
                this.Window.Grid_GameBoard.MaxHeight = GridWidth / BreitenVerhaeltnis_App;

                //this.Window.Grid_GameBoard.MaxWidth = GridWidth;
                //this.Window.Grid_GameBoard.MaxHeight = GridHeight * BreitenVerhaeltnis_App;
                // Szenario 1 - Höhe wird beschränkt => Breite dehnt sich maximal aus
                //this.Window.Grid_GameBoard.MaxHeight = .....;
            }
            else
            {
                this.Window.Grid_GameBoard.MaxHeight = GridHeight;
                this.Window.Grid_GameBoard.MaxWidth = GridHeight * BreitenVerhaeltnis_App;
                // Szenario 2 - Breite wird beschränkt => Höhe dehnt sich maximal aus
                //this.Window.Grid_GameBoard.MaxWidth = .....;
            }
        }
        // Skaliert das Vorschaufenster entsprechend der Fenstergröße
        private void ScaleBoard_Preview()
        {
            //int AnzSpalten = this.Game.Config.Width_Preview;
            //int AnzZeilen = this.Game.Config.Height_Preview;
            //double GridWidth = this.Window.Grid_PreviewOuter.ActualWidth;
            //double GridHeight = this.Window.Grid_PreviewOuter.ActualHeight;
            //double BreitenVerhaeltnis_Grid = GridWidth / GridHeight;
            //double BreitenVerhaeltnis_App = (double)AnzSpalten / AnzZeilen;
            //if (BreitenVerhaeltnis_App > BreitenVerhaeltnis_Grid)
            //{
            //    this.Window.Grid_PreviewBoard.MaxWidth = GridWidth;
            //    this.Window.Grid_PreviewBoard.MaxHeight = GridWidth / BreitenVerhaeltnis_App;
            //}
            //else
            //{
            //    this.Window.Grid_PreviewBoard.MaxHeight = GridHeight;
            //    this.Window.Grid_PreviewBoard.MaxWidth = GridHeight * BreitenVerhaeltnis_App;
            //    // Szenario 2 - Breite wird beschränkt => Höhe dehnt sich maximal aus
            //    //this.Window.Grid_GameBoard.MaxWidth = .....;
            //}
        }

        /// <summary>
        ///  Synchronisiert Boards
        /// </summary>
        public void Sync_Refresh()
        {
            if (this.Game.GameRunning == true)
                Sync_CoreData();
            ScaleBoard();
            ScaleBoard_Preview();
            this.Game.GuiInteraction.Refresh_StatisticData();

            if (((Gui_Interaction)this.Game.GuiInteraction).ActionQueue.Count > 0)
            {
                // Kopie von der Warteschleife erzeugen (nur die Verweise rübershiften)
                List<Action> ActionQueue_Copy = ((Gui_Interaction)this.Game.GuiInteraction).ActionQueue.ToList();

                // originale Liste leeren => Kopie bleibt unangetastet
                ((Gui_Interaction)this.Game.GuiInteraction).ActionQueue.Clear();

                // Wartschleife mit Aktionen in der Kopie durchlaufen und die jeweilige Aktion durchführen mit invoke
                foreach (Action action in ActionQueue_Copy)
                {
                    action.Invoke();
                }
            }


            // Für GameBoard
            for (short x = 0; x < Game.Config.Width_GameBoard; x++)
            {
                for (short y = 0; y < Game.Config.Height_GameBoard; y++)
                {
                    if (this.Game.Board.Array_Board[x, y].Color != null)
                    {
                        this.Board_UI_GameBoard.Array_UI_Board[x, y].Border.Background =
                            new LinearGradientBrush(
                                startColor:
                                Color.FromArgb(
                                            (this.Game.Board.Array_Board[x, y].Color.Value.Opacity != null ?
                                             (byte)(this.Game.Board.Array_Board[x, y].Color.Value.Opacity.Value * 0.99) :
                                            (byte)(255 * 0.99)),
                                        this.Game.Board.Array_Board[x, y].Color.Value.Red,
                                        this.Game.Board.Array_Board[x, y].Color.Value.Green,
                                        this.Game.Board.Array_Board[x, y].Color.Value.Blue
                                        ),

                                endColor: Color.FromArgb(
                                            (this.Game.Board.Array_Board[x, y].Color.Value.Opacity != null ?
                                            (byte)(this.Game.Board.Array_Board[x, y].Color.Value.Opacity.Value * 0.77) :
                                            (byte)(255 * 0.77)),
                                        this.Game.Board.Array_Board[x, y].Color.Value.Red,
                                        this.Game.Board.Array_Board[x, y].Color.Value.Green,
                                        this.Game.Board.Array_Board[x, y].Color.Value.Blue
                                        ),

                                        angle: 90
                                );
                        //    new SolidColorBrush(Color.FromArgb(
                        //    (this.Game.Board.Array_Board[x, y].Color.Value.Opacity != null ? 
                        //    this.Game.Board.Array_Board[x, y].Color.Value.Opacity.Value : (byte)255),
                        //this.Game.Board.Array_Board[x, y].Color.Value.Red,
                        //this.Game.Board.Array_Board[x, y].Color.Value.Green,
                        //this.Game.Board.Array_Board[x, y].Color.Value.Blue
                        //));
                    }
                    else
                    {
                        this.Board_UI_GameBoard.Array_UI_Board[x, y].Border.ClearValue(Border.BackgroundProperty);
                    }
                }
            }

            // Für PreviewBoard
            for (short x = 0; x < Game.Config.Width_Preview; x++)
            {
                for (short y = 0; y < Game.Config.Height_Preview; y++)
                {
                    if (this.Game.BoardPreview.Array_Board[x, y].Color != null)
                    {
                        this.Board_UI_PreviewBoard.Array_UI_Board[x, y].Border.Background =
                            new LinearGradientBrush(
                                startColor:
                                Color.FromArgb(
                                            (this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity != null ?
                                             (byte)(this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity.Value * 0.95) :
                                            (byte)(255 * 0.95)),
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Red,
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Green,
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Blue
                                        ),

                                endColor: Color.FromArgb(
                                            (this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity != null ?
                                            (byte)(this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity.Value * 0.77) :
                                            (byte)(255 * 0.77)),
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Red,
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Green,
                                        this.Game.BoardPreview.Array_Board[x, y].Color.Value.Blue
                                        ),

                                        angle: 90
                                );
                        //    new SolidColorBrush(Color.FromArgb(
                        //       (this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity != null ? this.Game.BoardPreview.Array_Board[x, y].Color.Value.Opacity.Value : (byte)255),
                        //this.Game.BoardPreview.Array_Board[x, y].Color.Value.Red,
                        //this.Game.BoardPreview.Array_Board[x, y].Color.Value.Green,
                        //this.Game.BoardPreview.Array_Board[x, y].Color.Value.Blue
                        //));
                    }
                    else
                    {
                        this.Board_UI_PreviewBoard.Array_UI_Board[x, y].Border.ClearValue(Border.BackgroundProperty);
                    }
                }
            }
        }
        
        #endregion


      
    }
}
