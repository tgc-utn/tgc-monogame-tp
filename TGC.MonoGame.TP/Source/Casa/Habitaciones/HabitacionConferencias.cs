using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{

    public class HabitacionConferencias : IHabitacion{
        public const int Size = 10;
        public HabitacionConferencias(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){
            
            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

        }
    }    
}