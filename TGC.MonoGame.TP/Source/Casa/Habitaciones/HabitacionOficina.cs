using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionOficina : IHabitacion{
        private const int Size = 5;
        public HabitacionOficina(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            Piso.Oficina();
            AddPared(Pared.Abajo (Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
            AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));

            var carpintero = new ElementoBuilder();
            
            carpintero.Modelo(TGCGame.GameContent.M_SillaOficina)
                .ConPosicion(4000f, 1000f)
                .ConRotacion(-MathHelper.PiOver2,-MathHelper.PiOver4,0f)
                .ConEscala(10f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Cafe)
                .ConPosicion(4000f, 1000f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
                .ConEscala(10f)
                .ConAltura(500f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Planta)
                .ConPosicion(500f,500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Escritorio)
                .ConPosicion(3500f, 1500f)
                .ConRotacion(0f, MathHelper.Pi, 0f)
                .ConEscala(170f)
                .ConAltura(500f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Plantis)
                .ConPosicion(4700f, 3500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}