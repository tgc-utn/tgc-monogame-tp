using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo1 : IHabitacion{
        private const int Size = 4;
        public HabitacionPasillo1(Vector3 posicionInicial):base(Size,Size,posicionInicial){  
            Piso.Alfombrado();

            AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));
        }

    }
}