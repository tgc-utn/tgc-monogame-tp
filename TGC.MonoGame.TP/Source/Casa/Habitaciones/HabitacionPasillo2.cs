using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo2 : IHabitacion{
        private const int Size = 4;
        public HabitacionPasillo2(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            Piso.Alfombrado();

            AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
        
            Amueblar();
        }
        private void Amueblar(){
            var carpintero = new ElementoBuilder();

            carpintero.Modelo(TGCGame.GameContent.M_Aparador)
                .ConPosicion(3300f, 3400f)
                .ConRotacion(0f,-MathHelper.PiOver2,0f)
                .ConEscala(15f)
                .ConAltura(-400f);

            AddElemento(carpintero.BuildMueble());
        }

    }
}