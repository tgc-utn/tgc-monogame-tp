using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionToilette : IHabitacion{
        private const int Size = 4;
        public HabitacionToilette(Vector3 posicionInicial):base(Size,Size,posicionInicial){

            Piso.Banio();
            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPared(Pared.Arriba(Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));

            var carpintero = new ElementoBuilder();

            carpintero.Modelo(TGCGame.GameContent.M_Inodoro)
                .ConPosicion(1500f, 500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
            

            carpintero.Modelo(TGCGame.GameContent.M_Baniera)
                .ConPosicion(1500f, 3400)
                .ConRotacion(0f, MathHelper.Pi, 0f)
                .ConEscala(3f);
                AddElemento(carpintero.BuildMueble());
            

            carpintero.Modelo(TGCGame.GameContent.M_Baniera)
                .ConPosicion(3000f, -100f)
                .ConRotacion(-MathHelper.PiOver2, 0f, 0f)
                .ConAltura(1000f)
                .ConEscala(2f);
                AddElemento(carpintero.BuildMueble());
            
        }
    }    
}