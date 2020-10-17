using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.Reflection;



namespace TetrisData
{
    public class JSON// : IExportImport
    {
        #region VARS
        public SortedDictionary<int, string> HighScores = new SortedDictionary<int, string>();
        public JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();
        public Dictionary<string, object> Dict;
        public object[] objectArray;
        List<object> Scores = new List<object>();
        List<object> Names = new List<object>();
        #endregion VARS

        #region CONS
        public JSON()
        {
        }
        #endregion CONS

        #region METHS
        /// <Importiert Highscore aus Datei und schreibt alles in das Dictionary Highscore_Core>
        /// Importiert Highscore aus Datei und schreibt alles in das Dictionary Highscore_Core
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<int, string> ImportData()
        {
            string path = "C:\\Users\\Public\\" + "HighScore_JSON" + ".txt";
            string ImportString;
            dynamic Dynamic;
            try
            {
                ImportString = System.IO.File.ReadAllText(path);                                                // Datei holen
                Dynamic = (dynamic)JsonSerializer.DeserializeObject(ImportString);                                  // Deserialisieren
            }
            catch
            {
                throw new System.InvalidOperationException("Error - Could not find file in " +
                    path + System.Environment.NewLine +
                    " ||SOURCE:|| SortedDictionary<int, string> ImportData()");
            }
            Dict = (Dictionary<string, object>)Dynamic;
            bool Switch_ForEachLoop = false;

            foreach (KeyValuePair<string, object> ii in Dynamic)
            {
                if (ii.Value.GetType().Equals(typeof(object[])))
                {
                    objectArray = (object[])ii.Value;
                    if (Switch_ForEachLoop == false)
                    {
                        for (int i = 0; i < objectArray.Length; i++)
                        {
                            this.Scores.Add((int)objectArray[i]);
                        }
                    }
                    if (Switch_ForEachLoop == true)
                    {
                        for (int i = 0; i < objectArray.Length; i++)
                        {
                            this.Names.Add(objectArray[i]);
                        }
                    }
                    Switch_ForEachLoop = true;
                }
            }
            for (int i = 0; i < objectArray.Length; i++)
            {
                if ((HighScores.ContainsKey((int)this.Scores[i]) == false))
                {
                    this.HighScores.Add((int)this.Scores[i], (string)this.Names[i]);
                }
            }
            return HighScores;
        }


        public void ExportData<T>(T t, string FileName)
        {
            string path = "C:\\Users\\Public\\" + FileName + ".txt";

            try
            {
                if (Directory.Exists(path) == true)
                {
                    this.ImportData();
                }
                string exportstring = JsonSerializer.Serialize(t);
                System.IO.File.WriteAllText(path, exportstring);
            }
            catch
            {
                throw new System.InvalidOperationException("Error - Could not read/write file in " + path + "SOURCE: void Export<T>(T t, string FileName)");
            }
        }
        #endregion METHS
    }
}
