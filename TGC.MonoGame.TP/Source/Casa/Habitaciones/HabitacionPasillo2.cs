using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionPasillo2 : IHabitacion{
        public const int Size = 4;
        public HabitacionPasillo2(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){
            Piso.ConTextura(TGCGame.GameContent.T_PisoAlfombrado);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

            Amueblar();
        }
        private void Amueblar(){
            var carpintero = new ElementoBuilder();

            // Bugueado
            /* carpintero.Modelo(TGCGame.GameContent.M_Aparador)
                .ConPosicion(3300f, 3400f)
                .ConRotacion(0f,-MathHelper.PiOver2,0f)
                .ConEscala(15f)
                .ConAltura(-400f); */

            //AddElemento(carpintero.BuildMueble());
        }

    }
}