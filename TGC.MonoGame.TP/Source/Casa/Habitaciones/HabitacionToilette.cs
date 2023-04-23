using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionToilette : IHabitacion{
        private const int Size = 4;
        public HabitacionToilette(Vector3 posicionInicial):base(Size,Size,posicionInicial){

            Piso.Banio();
            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPared(Pared.Arriba(Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
            AddPuerta(Puerta.Abajo(2f, Size, posicionInicial));

            
            AddElemento(new Mueble(TGCGame.GameContent.M_Inodoro, new Vector3(1500f, 0f, 500f), new Vector3(0f, 0f, 0f), 15f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Baniera, new Vector3(1500f, 0f, 3400f), new Vector3(0f, MathHelper.Pi, 0f), 3f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Bacha, new Vector3(3000f, 1000f, -100f), new Vector3(-MathHelper.PiOver2, 0f, 0f), 2f));
        }
    }    
}