using TetrisApp.Elements.Steine;

namespace TetrisApp.Elements.Fields
{
    public class BoardField : Field
    {
        #region VARS
       public Color? Color = null;                                      // Jedes BoardField auf dem Spielfeld soll wissen von welcher Farbe 
       public Block block;                                              // und von welchem Block es belegt ist
        #endregion VARS

        #region CONS
        public BoardField(short xt, short yt, Color? farbe = null)
        {
            x = xt; 
            y = yt;
            this.Color = farbe;
        }
        #endregion CONS
       
        #region METHS

        /// <summary>
        /// leert genau EIN BoardFeld
        /// </summary>
        public void Clear()
        {
            this.Color = null;
            this.block = null;
        }


        public override string ToString()                               // überschriebt die ToString()-Methode nach meinen Wünschen
        {
            return ("X: "+ x +" ||  Y: "+ y );
        }
        #endregion METHS
    }
}
