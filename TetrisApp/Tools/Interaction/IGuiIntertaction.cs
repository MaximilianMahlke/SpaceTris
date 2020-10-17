using System;
using TetrisApp.Elements.Fields;

namespace TetrisApp.Tools.Interaction
{
    public enum enum_SpeedRatio
    {
        Slower = 0,
        Normal = 1,
        Faster = 2,
        UltraFast = 3,
        Refresh = 4
    }


    /// <summary>
    /// Dieses Interface legt die Methoden fest, welche von den Sub-Klassen überschrieben werden
    /// </summary>
    public interface IGuiInteraction
    {
        /// <summary>
        /// Aktuaklisiert GUI-Daten (Score, Lines, Level, Zeit)
        /// </summary>
        void Refresh_StatisticData();

        void LineKilled(int LineIndex);

        void AnimationSpeed(enum_SpeedRatio SpeedRatio);

        void CodeRed();

        void LineBlockKilled(BoardField boardField, TimeSpan Duration);
    }
}
