﻿using System;
using System.Collections.Generic;
using System.Threading;
using TetrisApp.Elements.Fields;



namespace TetrisApp.Elements.Steine
{
    public class Block
    {
        #region VARS
        public int Rotation { get; set; }                                           // Ausrichtung des SpielBLocks
        public BlockForm form;
        public Game game;
        public int CurrentPos_x;
        public int CurrentPos_y;
        public List<BoardField> BoardPositions = new List<BoardField>();            // Diese Liste bekommt BoardFields
        public bool SpecialEvent_1 = false;
        #endregion VARS

        #region CONST
        public Block(BlockForm blockform, Game spiel)
        {
            this.form = blockform;
            this.game = spiel;
        }
        #endregion CONST

        #region METHS
        /// <summary>
        /// Aktueller Stein inkl. Rotation wird in das Spielfeld (Board) übertragen, um mit ihm spielen zu können
        /// </summary>
        public bool TransferToGameBoard(Boolean OnlyFillBoardPositions = false)
        {
            bool Field_Is_In_Array = true;

            foreach (BlockField blockfield in this.form.List_BlockVarianten[Rotation])
            {
                if (Field_Is_In_Array == false)
                {
                    break;
                }
                int x = CurrentPos_x + blockfield.x;
                int y = CurrentPos_y + blockfield.y;

                if (x < 0 || x > (this.game.Config.Width_GameBoard - 1))                            // Prüft jedes BLockField in der Liste ob es überhaupt im Spielfeld befindet (X-Achse)
                {
                    Field_Is_In_Array = false;
                    break;
                }
               
                if (y < 0 || y > this.game.Config.Height_GameBoard - 1)
                {
                    Field_Is_In_Array = false;
                    break;
                }

                if (Field_Is_In_Array)
                    BoardPositions.Add(game.Board.Array_Board[x, y]);

                if (game.Board.Array_Board[x, y].block == null)
                {
                    if (OnlyFillBoardPositions == false && Field_Is_In_Array == true)
                    {
                        game.Board.Array_Board[x, y].block = this;
                        game.Board.Array_Board[x, y].Color = this.form.Farbe;
                    }
                }
                if (Field_Is_In_Array == false)
                {
                    BoardPositions.Clear();
                }
            }
            return Field_Is_In_Array;
        }


        /// <summary>
        ///  Schreibt den übernächsten Block in das Vorschaufenster
        /// </summary>
        public void TransferToPreviewBoard()
        {
            foreach (BlockField blockfieldUI in this.form.List_BlockVarianten[Rotation])
            {
                this.game.BoardPreview.Array_Board[blockfieldUI.x + 1, blockfieldUI.y + 1].block = this;
                this.game.BoardPreview.Array_Board[blockfieldUI.x + 1, blockfieldUI.y + 1].Color = this.form.Farbe;
            }
        }


        /// <summary>
        /// Prüft, ob sich der aktuelle Block bzgl.  WÄNDEN +  ANDERE BLÖCKEN bewegen darf in der vorgegebenen Richtung (enum)          
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool CheckIsMoveable(enum_Direction Direction)
        {
            bool AllowMove = true;
            switch (Direction)
            {
                case enum_Direction.LEFT:
                    foreach (BoardField boardfield in BoardPositions)
                    {
                        // Prüfe ob das links-liegende BoardFeld sich bereits in der untersten Zeile befindet
                        if (boardfield.x - 1 < 0)
                        {
                            AllowMove = false;
                            break;
                        }
                        // prüfe ob auf dem links-liegendem Feld ein anderer Block liegt
                        if (this.game.Board.Array_Board[boardfield.x - 1, boardfield.y].block != boardfield.block &&
                            this.game.Board.Array_Board[boardfield.x - 1, boardfield.y].block != null)
                        {
                            //if (this.game.Board.Array[boardfield.x - 1, boardfield.y].Color != null)
                            {
                                // kann sich nicht bewegen, weil anderer Block blockiert
                                AllowMove = false;
                                break;
                            }
                        }
                    }
                    break;

                case enum_Direction.RIGHT:
                    foreach (BoardField boardfield in BoardPositions)
                    {
                        // Prüfe ob das rechts-liegende BoardFeld sich bereits in der untersten Zeile befindet
                        if (boardfield.x + 1 > this.game.Config.Width_GameBoard - 1)
                        {
                            AllowMove = false;
                            break;
                        }

                        // prüfe ob auf dem rechts-liegendem Feld ein anderer Block liegt
                        if (this.game.Board.Array_Board[boardfield.x + 1, boardfield.y].block != boardfield.block &&
                            this.game.Board.Array_Board[boardfield.x + 1, boardfield.y].block != null)
                        {
                            //if (this.game.Board.Array[boardfield.x + 1, boardfield.y].Color != null)
                            {
                                // kann sich nicht bewegen, weil anderer Block blockiert
                                AllowMove = false;
                                break;
                            }
                        }
                    }
                    break;


                case enum_Direction.DOWN:
                    foreach (BoardField boardfield in BoardPositions)
                    {
                        // Prüfe ob das darunter liegende BoardFeld sich bereits in der untersten Zeile befindet
                        if (boardfield.y + 1 > this.game.Config.Height_GameBoard - 1)
                        {
                            AllowMove = false;
                            break;
                        }
                        // prüfe ob auf dem darunter liegendem Feld ein anderer Block liegt
                        if (this.game.Board.Array_Board[boardfield.x, boardfield.y + 1].block != boardfield.block &&
                            this.game.Board.Array_Board[boardfield.x, boardfield.y + 1].block != null)
                        {
                            //if (this.game.Board.Array[boardfield.x, boardfield.y + 1].Color != null)
                            {
                                // kann sich nicht bewegen, weil anderer Block blockiert
                                AllowMove = false;
                                break;
                            }
                        }
                    }
                    break;


                // Prüft ob sich der Block drehen kann
                case enum_Direction.NO_DIRECTION:
                    Block testBlock = CopyBlock(this, true);
                    bool TransferToBoardWasSuccessful = testBlock.TransferToGameBoard(OnlyFillBoardPositions: true); // + bool merken, Per TransferToBoard NUR die BoardPositions übergeben lassen OHNE ins Array zu schreiben                                         
                    if (TransferToBoardWasSuccessful == false)
                    {
                        AllowMove = false;
                    }
                    else // Transfer was successfull
                    {
                        foreach (BoardField boardfield in testBlock.BoardPositions)
                        {
                            if (boardfield.x < 0 || boardfield.x > this.game.Config.Width_GameBoard - 1)
                            {
                                AllowMove = false;
                            }
                            if (boardfield.y < 0 || boardfield.y > this.game.Config.Height_GameBoard - 1)
                            {
                                AllowMove = false;
                            }
                            // prüfe ob der temporäre Block auf Blöcke oder Wände trifft
                            if (this.game.Board.Array_Board[boardfield.x, boardfield.y].block != this
                                && this.game.Board.Array_Board[boardfield.x, boardfield.y].block != null)
                            {
                                //if (this.game.Board.Array[boardfield.x, boardfield.y].Color != null)
                                {
                                    // kann sich nicht bewegen, weil anderer Block blockiert
                                    AllowMove = false;
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
            return AllowMove;
        }


        /// <summary>
        /// Hinweis: TransferToBoard() wird nicht aufgerufen
        /// </summary>
        /// <param name="Block"></param>
        /// <param name="AndNextRotation"></param>
        /// <returns></returns>
        public static Block CopyBlock(Block Block, bool AndNextRotation = false)
        {
            Block testBlock = new Block(Block.form, Block.game)      // Kopie wird angelegt, und zwar mit der aktuell eigenen Form
            {
                Rotation = Block.Rotation,                           // aktuelle Rotation übergeben
                CurrentPos_x = Block.CurrentPos_x,                   // Pos x kopieren
                CurrentPos_y = Block.CurrentPos_y,                   // Pos y kopieren
            };
            if (AndNextRotation)
            {
                testBlock.GetNextRotation();   // genau einmal weiter drehen
            }
            return testBlock;
        }


        public bool Rotate(Boolean UserPress = false)
        {
            if (this.CheckIsMoveable(enum_Direction.NO_DIRECTION) == true)
            {
                this.Clear_Block(this, game.Board);
                GetNextRotation();
                BoardPositions.Clear();
                this.TransferToGameBoard();
                if (UserPress)
                    this.game.LastBlockMoveByUser = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Move_right(Boolean UserPress = false)
        {
            if (this.CheckIsMoveable(enum_Direction.RIGHT) == true)
            {
                this.Clear_Block(this, game.Board);
                this.CurrentPos_x += 1;
                BoardPositions.Clear();
                this.TransferToGameBoard();
                if (UserPress)
                    this.game.LastBlockMoveByUser = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Move_left(Boolean UserPress = false)
        {
            if (this.CheckIsMoveable(enum_Direction.LEFT) == true)
            {
                this.Clear_Block(this, game.Board);
                this.CurrentPos_x -= 1;
                BoardPositions.Clear();
                this.TransferToGameBoard();
                if (UserPress)
                    this.game.LastBlockMoveByUser = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Move_down(Boolean UserPress = false)
        {
            if (this.CheckIsMoveable(enum_Direction.DOWN) == true)
            {
                this.Clear_Block(this, game.Board);
                this.CurrentPos_y += 1;
                BoardPositions.Clear();
                this.TransferToGameBoard();
                if (UserPress)
                    this.game.LastBlockMoveByUser = DateTime.Now;
                return true;
            }
            else
            {
                Thread thread = this.game.KillAllFullLines_async();   //    this.game.KillLine_y(this.game.GlowLines(this.game.WichLineIsFull()));
                this.game.Config.Score += this.game.Config.Config_ScoreForLanding;

                if (this.CurrentPos_y <= 0)
                {
                    this.game.GameOver();
                    return false;
                }
                else
                {
                    TetrisApp.Tools.Utils.SpawnBlock(this.game);
                    return false;
                }
            }
        }


        public void Clear_Block(Block Block, Board Board)
        {
            foreach (BoardField boardfeld in Block.BoardPositions)
            {
                Board.Array_Board[boardfeld.x, boardfeld.y].Color = null;
                Board.Array_Board[boardfeld.x, boardfeld.y].block = null;
            }
        }


        public void GetNextRotation()
        {
            if (this.Rotation < this.form.List_BlockVarianten.Count - 1)
            {
                this.Rotation++;
            }
            else
            {
                this.Rotation = 0;
            }
        }
        #endregion METHS




    }
}
