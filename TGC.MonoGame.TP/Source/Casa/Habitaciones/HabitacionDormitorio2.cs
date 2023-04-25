using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio2 : IHabitacion{
        private const int Size = 5;
        public HabitacionDormitorio2(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        Piso.Azul();
        AddPared(Pared.Arriba (Size, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        AddPuerta(Puerta.Abajo(0f, Size, posicionInicial));
        AddPared(Pared.Izquierda(Size-4, Size, posicionInicial + new Vector3(0f, 0f, 4000f)));
       
        var centro = getCenter()-posicionInicial;
        var ubicacionSet = centro - new Vector3(1200f,0f,1200f);
        var RotacionSet = new Vector3(0f, MathHelper.PiOver4,0f);

        AddElemento(new Mueble(TGCGame.GameContent.M_Dragon, new Vector3(400f,700f,400f), new Vector3(MathHelper.PiOver4,MathHelper.PiOver4,0f)));
        AddElemento(new Mueble(TGCGame.GameContent.M_Dragona,new Vector3(400f,700f,4500f), new Vector3(MathHelper.PiOver4,MathHelper.PiOver4*3,0f)));
        
        #region Set Sillones
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, ubicacionSet + new Vector3(50f,0f,2150f), new Vector3(0,MathHelper.PiOver2,0)+RotacionSet, 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, ubicacionSet + Vector3.Zero ,RotacionSet, 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, ubicacionSet + new Vector3(2050f,0f,50f), new Vector3(0,-MathHelper.PiOver2,0)+RotacionSet, 10f));
        #endregion

        ubicacionSet = centro - new Vector3(200f,0f,200f);
        #region Set Ajedrez
            AddElemento(new Mueble(TGCGame.GameContent.M_Torre , ubicacionSet));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , ubicacionSet + new Vector3(0f,0f,400f)));
            AddElemento(new Mueble(TGCGame.GameContent.M_Torre , ubicacionSet + new Vector3(400f,0f,400f)));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , ubicacionSet + new Vector3(400f,0f,0f)));
        #endregion
        
        }


    }    
}