using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TetrisGui.GUI
{
    public class BoardFeldUI
    {
        #region VARS
        private short x;
        private short y;
        public Border Border;
        public BoardUI Parent_BoardUI;
        #endregion VARS

        #region CONS
        public BoardFeldUI(BoardUI Parent_BoardUI, short x, short y)
        {
            this.x = x;
            this.y = y;
            this.Parent_BoardUI = Parent_BoardUI;

            this.Initialize_UI();
        }
        #endregion CONS

        #region METHS
        public void Initialize_UI()
        {
            this.Border = new Border()
            {
                BorderBrush = new SolidColorBrush(
                    Color.FromArgb(200, 150, 200, 255)
                    )
            };   
            // Neue Instanz von Border wird erstellt
            this.Border.BorderThickness = new Thickness(
                (x == 0 ? 1 : 0),
                (y == 0 ? 1 : 0),
                1,
                1
                );

            Grid.SetRow(Border, y);                                         // Zeilen-Position des Borders wird festgelegt
            Grid.SetColumn(Border, x);                                      // Spalten-Position des Borders wird festgelegt
            this.Parent_BoardUI.Grid.Children.Add(Border);   // Border wird dem Grid hinzugefügt

        }
        #endregion METHS
    }
}
