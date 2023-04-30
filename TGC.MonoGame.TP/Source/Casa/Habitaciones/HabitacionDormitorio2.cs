using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio2 : IHabitacion{
        private const int Size = 5;
        public HabitacionDormitorio2(Vector3 posicionInicial) : base(Size,Size,posicionInicial){
        
        Piso.Azul();
        AddPared(Pared.Arriba (Size, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        AddPuerta(Puerta.Abajo(0f, Size, posicionInicial));
        AddPared(Pared.Izquierda(Size-4, Size, posicionInicial + new Vector3(0f, 0f, 4000f)));
       
        var centro = getCenter()-posicionInicial;
        var ubicacionSet = centro - new Vector3(1200f,0f,1200f);
        var RotacionSet = new Vector3(0f, MathHelper.PiOver4,0f);

        var carpintero = new ElementoBuilder();
        
        carpintero.Modelo(TGCGame.GameContent.M_Dragon)
            .ConPosicion(400f, 400f)
            .ConRotacion(MathHelper.PiOver4,MathHelper.PiOver4,0f);
            AddElemento(carpintero.BuildMueble());
            
        carpintero.Modelo(TGCGame.GameContent.M_Dragona)
            .ConPosicion(400f, 4500f)
            .ConRotacion(MathHelper.PiOver4,MathHelper.PiOver4*3,0f);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(TGCGame.GameContent.M_Sillon)
            .ConEscala(10f)

            .ConPosicion(ubicacionSet.X+50f, ubicacionSet.Z+2150f)
            .ConRotacion(RotacionSet.X,MathHelper.PiOver2+RotacionSet.Y,0+RotacionSet.Z);
            AddElemento(carpintero.BuildMueble());
            
            carpintero
            .ConPosicion(0f,0f)
            .ConRotacion(RotacionSet.X,0,RotacionSet.Z);
            AddElemento(carpintero.BuildMueble());
            
            carpintero
            .ConPosicion(ubicacionSet.X+2050f, ubicacionSet.Z+50f)
            .ConRotacion(RotacionSet.X,RotacionSet.Y-MathHelper.PiOver2,RotacionSet.Z);
            AddElemento(carpintero.BuildMueble());

        ubicacionSet = centro - new Vector3(200f,0f,200f);
        #region Set Ajedrez
        carpintero.Modelo(TGCGame.GameContent.M_Torre)
            .ConPosicion(ubicacionSet.X - 200f, ubicacionSet.Z - 200f);
            AddElemento(carpintero.BuildMueble());
            
            carpintero
            .ConPosicion(ubicacionSet.X + 400f, ubicacionSet.Z + 400f);

        carpintero.Modelo(TGCGame.GameContent.M_Alfil)
            .ConPosicion(ubicacionSet.X, ubicacionSet.Z +400f);
            AddElemento(carpintero.BuildMueble());
            
            carpintero
            .ConPosicion(ubicacionSet.X +400f, ubicacionSet.Z);
            AddElemento(carpintero.BuildMueble());
        #endregion
        
        }


    }    
}