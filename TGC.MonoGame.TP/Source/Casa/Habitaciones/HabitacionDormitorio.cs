using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio : IHabitacion{
        private const int Size = 6;
        public HabitacionDormitorio(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        }
    }    
}