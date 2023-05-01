using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo1 : IHabitacion{
        public const int Size = 4;
        public HabitacionPasillo1(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){  
            Piso.ConTextura(TGCGame.GameContent.T_PisoAlfombrado);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

        }

    }
}