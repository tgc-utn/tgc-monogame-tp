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
        AddElemento(new Mueble(TGCGame.GameContent.M_Dragon, centro + new Vector3(0f,700f,0f), new Vector3(MathHelper.PiOver4,MathHelper.PiOver4,0f)));
        }
    }    
}