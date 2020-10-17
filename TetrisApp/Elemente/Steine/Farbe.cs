
namespace TetrisApp.Elements.Steine
{
        /// <summary>
        ///  Klasse um Farben zu definieren
        /// </summary>
  public struct Color
    {
            
       
            #region VARS
           public byte Red;
           public byte Green;
           public byte Blue;
           public byte? Opacity;
           #endregion VARS

            #region CONS
            public Color(byte r, byte g, byte b)                           // Konstruktor ohne Transparenz (R,G,B)
            {
                this.Red = r;
                this.Green = g;
                this.Blue = b;
                this.Opacity = null;
            }

            public Color(byte r, byte g, byte b, byte? o = null)           // Konstruktor mit Transparenz (R,G,B,O)
            {
                this.Red = r;
                this.Green = g;
                this.Blue = b;
                this.Opacity = o;
            }
            #endregion CONS
    }
}
