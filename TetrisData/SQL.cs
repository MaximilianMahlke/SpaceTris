using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows;

namespace TetrisData
{

    public class SQL //: IExportImport
    {
        #region VARS
        public string ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\maximilian\\Desktop\\"
           + "Projects\\TetrisData\\LocalDB\\Tetris_DB.mdf;Integrated Security=True; Connect Timeout = 30; MultipleActiveResultSets=true"; // = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        public SqlConnection Connection;
        ConnectionStringSettings setting;
        Configuration config;


        public SqlDataAdapter da1;
        public SqlDataAdapter da2;
        public DataSet ds = new DataSet();


        public SqlDataReader Reader;
        int Index_ID;
        int? ID_of_Player;
        SortedDictionary<int, string> PlayerAndId = new SortedDictionary<int, string>();
        List<int> Scores = new List<int>();
        List<int> PlayerID = new List<int>();
        List<string> Names = new List<string>();
        List<string> Highscores = new List<string>();
        string AllHighScores;
        #endregion VARS



        #region CONS
        public SQL()
        {
        }
        #endregion CONS


        #region METHS


        #region Get_CommandStrings-Methoden


        /// <Cmd_String für: Löscht ALLE Tabellen>
        ///  Cmd_String für: Löscht ALLE Tabellen
        /// </summary>
        /// <returns></returns>
        public string Cmd_Clear_AllTables()
        {
            return "TRUNCATE TABLE Player; " + "TRUNCATE TABLE Score; " + "TRUNCATE TABLE TempID; ";
        }


        /// <Cmd_String für: Löscht Tabelle Player>
        ///  Cmd_String für: Löscht Tabelle Player
        /// </summary>
        /// <returns></returns>
        public string Cmd_Clear_Player()
        {
            return "TRUNCATE TABLE Player; ";
        }


        /// <// Cmd_String für: Löscht Tabelle Score>
        ///  Cmd_String für: Löscht Tabelle Score
        /// </summary>
        /// <returns></returns>
        public string Cmd_Clear_Score()
        {
            return "TRUNCATE TABLE Score; ";
        }


        /// <Cmd_String für: Löscht Tabelle TempID>
        /// Cmd_String für: Löscht Tabelle TempID
        /// </summary>
        /// <returns></returns>
        public string cmd_Clear_TempID()
        {
            return "TRUNCATE TABLE TempID; ";
        }


        /// <Cmd_String für: Fügt Username zu Tabelle>
        /// Cmd_String für: Fügt Username zu Tabelle
        /// </summary>
        /// <param name="playername"></param>
        /// <returns></returns>
        public string Cmd_Add_NAME(string playername)
        {
            return "INSERT INTO Player (Name) " + "OUTPUT INSERTED.PlayerID INTO TempID (TempPlayerID) VALUES('" + playername + "'); ";
        }


        /// <Cmd_String für: Fügt Punkte und PlayerID zu Score zu>
        /// Cmd_String für: Fügt Punkte und PlayerID zu Score zu
        /// </summary>
        /// <param name="score"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string Cmd_Add_ScoreANDPlayerID(int score, int Id)
        {
            return "INSERT INTO Score (Scores, PlayerID) " + "VALUES('" + score + "', '" + Id + "'); ";
        }


        /// <Cmd_String für: Holt sich die PlayerID>
        /// Cmd_String für: Holt sich die PlayerID
        /// </summary>
        /// <returns></returns>
        public string Cmd_Get_PlayerID()
        {
            return "SELECT TempPlayerID FROM TempID WHERE Id = 1; ";
        }


        /// < Cmd_String für: Prüft ob Playername bereits vorhanden ist>
        /// Cmd_String für: Prüft ob Playername bereits vorhanden ist
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public string Cmd_CheckNameExists(string Username)
        {
            return "SELECT PlayerID FROM Player WHERE Name = '" + Username + "'; ";
        }


        public string Cmd_GetScoreAndPlayerID()
        {
            return "SELECT Scores, PlayerID FROM Score ORDER BY Scores DESC; ";
        }

        public string Cmd_GetPlayerNamesAndIds()
        {
            return "SELECT PlayerID, Name FROM Player; ";
        }

        /// < Lädt Highscores aus Datenbank und zeigt sie an >
        /// Lädt Highscores aus Datenbank und zeigt sie an 
        /// </summary>
        public void DownloadData()
        {
            this.Connect();

            this.Reader = this.GetSqlCommand(this.Cmd_GetScoreAndPlayerID()).ExecuteReader();
            while (this.Reader.Read())
            {
                this.Scores.Add(this.Reader.GetInt32(0));
                this.PlayerID.Add(this.Reader.GetInt32(1));
            }
            this.Reader.Close();

            this.Reader = this.GetSqlCommand(this.Cmd_GetPlayerNamesAndIds()).ExecuteReader();
            while (this.Reader.Read())
            {
                this.PlayerAndId.Add(this.Reader.GetInt32(0), this.Reader.GetString(1));
            }
            this.Reader.Close();
            this.Disconnect();
        }


        //public List<string> ShowHighscore()
        //{
        //    Highscores.Add("                            TOP 15 HIGHSCORES                    (SQL) " + System.Environment.NewLine );

        //    for (int i = 0; i < this.PlayerAndId.Count; i++)
        //    {
        //        this.Highscores.Add(i + ". | Score: " + Scores[i] + " | Player: " +  this.GetNameFromId(PlayerID[i]) + Environment.NewLine);

        //        if (i == 14)
        //        {
        //            return Highscores;
        //        }
        //    }
        //    return Highscores;
        //}

        public string ShowHighscore()
        {
            AllHighScores = "                            TOP 15 HIGHSCORES                    (SQL) " + System.Environment.NewLine;

            for (int i = 0; i < this.PlayerAndId.Count; i++)
            {
                AllHighScores += (i+1 + ". \t| Score: " + Scores[i] + " \t| Player: " + this.GetNameFromId(PlayerID[i]) + Environment.NewLine);

                if (i == 15)
                {
                    return AllHighScores;
                }
            }
            return AllHighScores;
        }
        #endregion Get_CommandStrings-Methoden

        public string GetNameFromId(int Index)
        {
            string outputstring;
            this.PlayerAndId.TryGetValue(Index, out outputstring);
            return outputstring;
        }

        /// <Füllt SQL-Datenbank mit: Score, Name>
        /// Master-Methode, erstellt sich aus Cmd_Strings diverse SQL-Befehle
        /// </summary>
        /// <param name="username"></param>
        /// <param name="score"></param>
        public void UpdateData(string username, int score)
        {
            this.Connect();
            this.GetSqlCommand(this.cmd_Clear_TempID()).ExecuteNonQuery();                         // Löscht die TempID-DB

            this.Reader = this.GetSqlCommand(this.Cmd_CheckNameExists(username)).ExecuteReader();  // Check ob Name bereits vorhanden
            while (this.Reader.Read())
            {
                this.ID_of_Player = this.Reader.GetInt32(0);
            }
            this.Reader.Close();

            if (this.ID_of_Player == null)                                                         // wenn nein
            {
                this.GetSqlCommand(this.Cmd_Add_NAME(username)).ExecuteNonQuery();                 // Schreibt den Namen in dbo.Player und PlayerID in TempID

                this.Reader = GetSqlCommand(this.Cmd_Get_PlayerID()).ExecuteReader();              // Einlesen PlayerID 
                while (this.Reader.Read())
                {
                    this.Index_ID = this.Reader.GetInt32(0);                                       // Speichern in Klassenvariable
                }
                this.Reader.Close();

                this.GetSqlCommand(this.Cmd_Add_ScoreANDPlayerID(score, Index_ID)).ExecuteNonQuery();     //  füge Score + PlayerID hinzu
            }
            else                                                                                    // wenn doch schon vorhanden
            {
                this.GetSqlCommand(this.Cmd_Add_ScoreANDPlayerID(score, (int)this.ID_of_Player)).ExecuteNonQuery();    //füge "ALTE" ID und Score ein
            }

            this.GetSqlCommand(cmd_Clear_TempID()).ExecuteNonQuery();
            this.Disconnect();
        }



        public SqlDataAdapter GetSqlAdapter(string commandString)
        {
            SqlDataAdapter command = new SqlDataAdapter(commandString, this.Connection);
            return command;
        }

        public SqlCommand GetSqlCommand(string commandString)
        {
            SqlCommand command = new SqlCommand(commandString, this.Connection);
            return command;
            //   command.Parameters.Add() anstelle der "score" 
        }


        public void Connect()
        {
            this.setting = ConfigurationManager.ConnectionStrings["Tetris_DB"];
            this.setting = new ConnectionStringSettings();
            this.setting.Name = "Tetris_DB";
            this.setting.ConnectionString = this.ConnectionString;                       // Connection- String zuweisen (Verbindungsinformationen)
            this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Add(setting);
            config.Save();

            try
            {
                this.Connection = new SqlConnection(setting.ConnectionString);                            // Connection-Object erzeugen
                this.Connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void Disconnect()
        {
            this.Connection.Close();
        }







        public SortedDictionary<int, string> ImportData()
        {
            return null;
        }


        public void ExportData()
        {

        }


        public void PrepareExport()
        {

        }





        #endregion METHS

        /* SQL Test-Query
         
SELECT * FROM Player; SELECT * FROM Score; SELECT * FROM TempID;
--TRUNCATE TABLE Player; TRUNCATE TABLE Score; TRUNCATE TABLE TempID; 

--INSERT INTO Score (SCoreID, Score, PlayerID) VALUES ('1','444', '1'); 
--INSERT INTO Player (Name) VALUES ('MAXI');

--SELECT * FROM Score ORDER BY Scores DESC;
         
         */

        #region alt

        /*
        public void SqlCommandMaster(string commandString, bool CommandIsQuery)
        {
            this.Connect();
            this.command = this.GetSqlCommand(commandString);

            if (Reader != null)
            {
                Reader.Close();
                //  this.Disconnect();
            }

            if (CommandIsQuery == false)
            {
                int i = command.ExecuteNonQuery();
            }
            else
            {
                if (ChooseReader == "Score")
                {
                    Reader = Reader_Score;
                }

                if (ChooseReader == "Name")
                {
                    Reader_Name = command.ExecuteReader();
                }

                Reader = command.ExecuteReader();

                Reader.Read();
                Reader.Close();
            }
            if (Reader != null)
            {
                Reader.Close();
                this.Disconnect();
            }
        }

        public string cmd_Clear_Name()
        {
            ChooseReader = "Name";
            return "DELETE FROM dbo.Player";
        }

        public string cmd_Clear_Score()
        {
            ChooseReader = "Score";
            return "DELETE FROM dbo.Score";
        }

        //public string Cmd_GetHighscore()
        //{
        //    // return "DELETE FROM dbo.Player;" + "DELETE FROM dbo.Score";
        //    return "SELECT Name FROM dbo.Player; " + "SELECT Score FROM dbo.Score;";
        //}

        public string Cmd_GetHighscore_SCORE()
        {
            ChooseReader = "Score";
            return "SELECT Score FROM dbo.Score ORDER BY Score DESC";
        }

        public string Cmd_GetHighscore_NAME()
        {
            ChooseReader = "Name";
            return "SELECT Name FROM dbo.Player";
        }

        //public string Cmd_AddHighscore(string playername, int score)
        //{
        //    return "INSERT INTO Score (Score) " + "VALUES('" + score + "');";
        //}

        public string Cmd_AddHighscore_SCORE(int score)
        {
            return "INSERT INTO Score (Score) " + "VALUES('" + score + "')";
        }

        public string Cmd_AddHighscore_NAME(string playername)
        {
            return "INSERT INTO Player (Name) " + "VALUES('" + playername + "');";
        }

        public SqlCommand GetSqlCommand(string commandString)
        {
            command = new SqlCommand(commandString, this.Connection);
            return command;
        }


        public void Connect()
        {
            this.setting = ConfigurationManager.ConnectionStrings["Tetris_DB"];
            this.setting = new ConnectionStringSettings();
            this.setting.Name = "Tetris_DB";
            this.setting.ConnectionString = this.ConnectionString;                       // Connection- String zuweisen (Verbindungsinformationen)
            this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Add(setting);
            config.Save();

            try
            {
                this.Connection = new SqlConnection(setting.ConnectionString);                            // Connection-Object erzeugen
                this.Connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        public void Disconnect()
        {
            this.Connection.Close();
        }



        public SortedDictionary<int, string> ImportData()
        {
            return null;
        }


        public void ExportData()
        {

        }


        public void PrepareExport()
        {

        }




        */
        #endregion alt
    }
}
