using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{

    public class HabitacionConferencias : IHabitacion{
        private const int Size = 10;
        public HabitacionConferencias(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            Piso.Azul();
            AddPared(Pared.Abajo (Size, Size, posicionInicial));
            AddPuerta(Puerta.Izquierda(4f, Size, posicionInicial));
            AddPuerta(Puerta.Derecha(1f, Size, posicionInicial));
            AddPuerta(Puerta.Arriba(1f, Size, posicionInicial));

        }
    }    
}