
using System;
using TetrisApp.Elements.Fields;
namespace TetrisApp.Tools.Interaction
{
   
    /// <summary>
    /// Diese Klasse befindet sich im Core-Projekt und erbt von dem Interface IGuiInteraction
    /// Eine wietere Klasse erbt hiervon, befindet sich aber im GUI-Projekt und ermöglicht somit Kommunikation
    /// </summary>
    
    // abstract BEI EINER KLASSE bedeutet nur, dass diese Klasse nicht instanziiert werden kann
    public abstract class GuiInteraction : IGuiInteraction        
    {
        
        // virtual bedeutet, dass man zwar Code schreiben kann, und dass diese Methode überschrieben werden DARF


        public virtual void AnimationSpeed(enum_SpeedRatio SpeedRatio)
        { 

        }
        
        public virtual void Refresh_StatisticData()
        {
           
        }

        // abstract BEI EINER METHODE bedeuetet, dass diese Methode ÜBERSCHRIEBEN WERDEN MUSS!!!
        public abstract void LineKilled(int LineIndex);

        public abstract void CodeRed();

        public abstract void LineBlockKilled(BoardField boardField, TimeSpan Duration);
      
       
    }
}

