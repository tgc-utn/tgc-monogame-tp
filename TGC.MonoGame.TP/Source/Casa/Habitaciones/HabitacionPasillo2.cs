using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo2 : IHabitacion{
        private const int Size = 4;
        public HabitacionPasillo2(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        
        Piso.Alfombrado();
        AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        
        AddElemento( new Mueble(TGCGame.GameContent.M_Aparador, new Vector3(3300f,-400f,3400f), new Vector3(0f,-MathHelper.PiOver2,0f), 15f));

        }

    }
}