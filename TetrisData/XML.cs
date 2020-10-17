using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;





namespace TetrisData
{

    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> original;

        public ReverseComparer(IComparer<T> original)
        {
            // TODO: Validation
            this.original = original;
        }

        public int Compare(T left, T right)
        {
            return original.Compare(right, left);
        }
    }


    public class XML // : IExportImport
    {
        #region VARS
        public SortedDictionary<int,  string> Dict_HighScores = new SortedDictionary<int, string>(  new ReverseComparer<int>(Comparer<int>.Default));
        public HighScore Highscore;
        public List<HighScore> Old_List = new List<HighScore>();
        public List<HighScore> ListOF_AllHighscores = new List<HighScore>();
        public string StringOfAllHighScores = "\t TOP 15 | HIGHSCORES:                                (XML)" + System.Environment.NewLine;

        #endregion VARS

        #region CONS
        public XML()
        {
        }
        #endregion CONS

        #region METHS
        public void PrepareExport()
        {
        }

        public void PrepareImport()
        {
        }


        public void ImportData()
        {
            XmlReader Reader = XmlReader.Create("C:\\Users\\Public\\HighScores2_XML.xml");

            while (Reader.Read())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {
                    switch (Reader.Name)
                    {
                        case "Highscore":
                            this.Highscore = new HighScore();
                            break;
                     
                        case "Name":
                            this.Highscore.Name = Reader.ReadElementContentAsString();
                            break;

                        case "Score":
                            this.Highscore.ScoreString = Reader.ReadElementContentAsString();

                            this.Old_List.Add(Highscore);
                            break;
                    }
                }

            }
            Reader.Close();
        }


        public void Add_Current_Score(string Username, int Score)
        {
            HighScore Current_Score = new HighScore(Username, Score);
            this.Old_List.Add(Current_Score);
        }

        public void SortXmlList()
        {
            
            foreach (HighScore hs in Old_List)
            {
                hs.ScoreInt = Int32.Parse(hs.ScoreString);      // Score StirngToInt-Parse
                if (this.Dict_HighScores.ContainsKey(hs.ScoreInt))     
                {
                    this.Dict_HighScores[hs.ScoreInt] += ", " + hs.Name; 
                }
                else
                {
                    this.Dict_HighScores.Add(hs.ScoreInt, hs.Name);
                }  
            }
        }


        public void UpdateAndShowData()
        {
            int counter = 1;
            XElement Highscores = new XElement("Highscores");

            foreach (KeyValuePair<int, string> hs in this.Dict_HighScores)
            {
                // UPDATE:
                Highscores.Add(new XElement("Highscore",
                                  new XElement("Name", hs.Value),
                                  new XElement("Score", hs.Key))
                                      );
                // SHOW:
                this.StringOfAllHighScores += System.Environment.NewLine +
                         counter + ". \t| Score: " + hs.Key +
                         "\t | Player: " + hs.Value;
                counter++;
            }
            Highscores.Save("C:\\Users\\Public\\Highscores2_XML.xml");
        }

        
        
        
        #endregion METHS
    }
}
/*  Ekel-Code
  bool CanBeAdded = true;
            List<HighScore> New_List = new List<HighScore>();
            HighScore Dummy = new HighScore("|", 0);
            New_List.Add(Dummy);
            for (int i = 1400; i > 0; i--)
            {
                foreach (HighScore Old_HS in this.Old_List)            // Für jedes Old_HS in Old_List
                {
                    //  if (i == 1401)
                    {
                        Old_HS.ScoreInt = Int32.Parse(Old_HS.ScoreString);      // Parse String-Scores zu Int-Score
                    }
                    if (Old_HS.ScoreInt == i && Old_HS.Checked == false)         // Wenn Old_HS-Punkte getroffen (also = i)
                    {
                        foreach (HighScore New_HS in New_List)                   // Dann prüfe für alle New_HS in New_List
                        {
                            if (New_HS.ScoreInt == Old_HS.ScoreInt)       // Ob sie bereits ein New_HS mit dem Score des Old_HS hat
                            {                                                   //Wenn JA
                                CanBeAdded = false;
                                New_HS.Name += ", " + Old_HS.Name;      //Füge nur den Playernamen hinzu
                                Old_HS.Checked = true;
                                i++;
                                break;
                            }
                        }
                        if (CanBeAdded)
                        {
                            New_List.Add(Old_HS);
                          continue;
                        }
                    }
                    
                }
            }
            string s = "test;";
            New_List.Remove(Dummy);*/