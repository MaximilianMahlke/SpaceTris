﻿using System.Windows;
using System.Windows.Controls;
using TetrisApp;


namespace TetrisGui.GUI
{
    public class BoardUI
    {
        #region VARS

        private MainWindow Window;
        private Game Game;
        public Grid Grid;
        public BoardFeldUI[,] Array_UI_Board;
        #endregion


        #region CONS
        public BoardUI(MainWindow Window, Game Game, Grid Grid)
        {
            this.Window = Window;
            this.Game = Game;
            this.Grid = Grid;
        }
        #endregion CONS


        #region METHS
        // Nimmt ein MainWindow und ein Spiel und legt auf dem Grid_Board Colums und Rows an
        public void ErstelleGrid(short width, short heigth)
        {
            for (short i = 0; i < width; i++)
            {
                this.Grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            for (short j = 0; j < heigth; j++)
            {
                this.Grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
        }
        // Befüllt das angelegte Grid_Boarder mit Bordern
        public void GenerateBoardFeldArray(short width, short height)
        {
            this.Array_UI_Board = new BoardFeldUI[width, height];

            for (short x = 0; x < width; x++)
            {
                for (short y = 0; y < height; y++)
                {
                    BoardFeldUI BFUI = new BoardFeldUI(this, x, y);

                    Array_UI_Board[x, y] = BFUI;
                }
            }
        }
    }
}
        #endregion METHS




