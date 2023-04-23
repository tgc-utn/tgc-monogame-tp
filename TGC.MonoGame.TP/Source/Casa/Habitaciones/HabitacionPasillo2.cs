using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo2 : IHabitacion{
        private const int Size = 4;
        public HabitacionPasillo2(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        
        Piso.Azul();
        AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        
        }

    }
}