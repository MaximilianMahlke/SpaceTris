﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TetrisApp.Elements.Fields;
using TetrisApp.Elements.Steine;
using TetrisApp.Tools;
using TetrisApp.Tools.Interaction;
using TetrisData;

namespace TetrisApp
{
    // NO USE!
    public class SaveGame : Config
    {
        #region VARS
        //public Block CurrentBlock;
        //public Block NextBlock;
        public Config Config;
        //public Board Board;
        //public Board BoardPreview;
        //public Object Window;
        //public Timer_Core Timer_Core;
        public JSON Json;
        //public List<RecurringEvent> RecurringEvents;
        //public List<BlockForm> List_Blockforms;
        //public IGuiInteraction GuiInteraction;
        //public bool GameRunning;
        //public bool GameIsOver;

        //public DateTime StartTime;
        //public DateTime? LastBlockMoveByUser;
        //public DateTime? LastBlockSpawn;

        //public Thread KillLineThread;
        //public TimeSpan MoveDownInterval;
        #endregion VARS


        #region CONS
        public SaveGame()
        {
        }
        #endregion CONS

        #region METHS
        public SaveGame(Game Game)
        {
            //Game.CurrentBlock.game = null;
            //Game.CurrentBlock.form.List_BlockVarianten = null;
            //this.CurrentBlock = Game.CurrentBlock;

            //Game.NextBlock.game = null;
            //Game.NextBlock.form = null;
            //this.NextBlock = Game.NextBlock;


            this.Config = Game.Config;

            //this.Board = Game.Board;
            //this.Board..
            //this.BoardPreview = Game.BoardPreview;
            //this.Window = Game.Window;
            this.Json = Game.Json;
            //this.RecurringEvents = Game.RecurringEvents;
            //this.List_Blockforms = Game.List_BlockForms;
            //this.GuiInteraction = Game.GuiInteraction;
            //this.GameRunning = Game.GameRunning;
            //this.GameIsOver = Game.GameIsOver;

            //this.StartTime = Game.StartTime;
            //this.LastBlockMoveByUser = Game.LastBlockMoveByUser;
            //this.LastBlockSpawn = Game.LastBlockSpawn;

            //this.KillLineThread = Game.KillLineThread;
            //this.MoveDownInterval = Game.MoveDownInterval;
        }

        public void GetHighscore()
        {
        }
        #endregion METHS

    }
}
