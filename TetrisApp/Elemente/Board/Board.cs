﻿//using System.Windows.Threading;


namespace TetrisApp.Elements.Fields
{
    public class Board
    {
        
        #region VARS

        short Breite;
        short Hoehe;
        public BoardField[,] Array_Board = null;                                   // Erstellt ein leeres 2-dim Array (Spielfeld) für den Inhalt: [BoardFeld]
        Game ParentSpiel;                                                          // Legt eine Referenz OHNE INHALT an, welche auf ein [Spiel] zeigt
       #endregion VARS


        #region KONS
        public Board(Game Spiel, short Breite, short Hoehe)                          // Erstellt Kontruktor welcher ein [Spiel] als Argument verlangt
        {
            this.Hoehe = Hoehe;
            this.Breite = Breite;
            this.ParentSpiel = Spiel;                                               // Packt das [Spiel]-Objekt in die Referenz ParentSpiel          
            this.ErstelleSpielfeld();                                               // Nimmt eine Liste aus [Blockformen] und füllt sie mit den vorgebenen Spielfeld 
        }
        #endregion KONS
    
        #region METHS
        /// <summary>
        /// Erstellt das Spielfeld aus 10x18 [Boardfeldern]
        /// </summary>
        public void ErstelleSpielfeld()                                         
        {
            this.Array_Board = new BoardField[Breite, Hoehe];
            for (short x = 0; x < Breite; x++)
            {
                for (short y = 0; y < Hoehe; y++)
                {
                    Array_Board[x, y] = new BoardField(x, y);
                }
            }
        }

        /// <summary>
        ///  leert das Spielfeld
        /// </summary>
        /// <param name="Board"></param>
        public void Clear(Board Board)
        {
            for (short x = 0; x < Board.Breite; x++)
            {
                for (short y = 0; y < Board.Hoehe; y++)
                {
                    Array_Board[x, y] = new BoardField(x, y);
                }
            }
        }
        #endregion METHS
    }
}

