using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio1 : IHabitacion{
        private const int Size = 5;
        public HabitacionDormitorio1(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        Piso.Azul();
        AddPared(Pared.Abajo (Size, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        AddPuerta(Puerta.Arriba(1f, Size, posicionInicial));
        }
    }    
}