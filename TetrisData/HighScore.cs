
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisData
{
    public class HighScore
    {
        public string ScoreString;
        public string Name;
        public int ScoreInt;
        public bool Checked = false;

        public HighScore()
        {

        }

        public HighScore(string Username, int Score)
        {
            this.Name = Username;
            this.ScoreInt = Score;
            if (ScoreString == null)
            {
                this.ScoreString = Score.ToString();
            }
        }
    }
}
