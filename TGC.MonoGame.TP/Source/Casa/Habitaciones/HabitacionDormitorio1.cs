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

        AddElemento(new Mueble(TGCGame.GameContent.M_Organizador, new Vector3(800f,0f,300f), new Vector3(0f,0f,0f), 15f));
        AddElemento(new Mueble(TGCGame.GameContent.M_Cajonera, new Vector3(800f,0f,4600f), new Vector3(0f,MathHelper.Pi,0f), 15f));
        AddElemento(new Mueble(TGCGame.GameContent.M_CamaMarinera, new Vector3(3400f,0f,600f), new Vector3(0f,MathHelper.Pi,0f), 8f));
        AddElemento(new Lego(new Vector3(2500f,300f, 2500f), new Vector3(MathHelper.PiOver2,MathHelper.PiOver4, 0f), Color.Red.ToVector3(), 35f));
        AddElemento(new Lego(new Vector3(2500f,0f, 3200f), new Vector3(0f,0f, 0f), Color.Green.ToVector3(), 35f));
        AddElemento(new Lego(new Vector3(2500f,150f, 3700f), new Vector3(MathHelper.PiOver4/2,MathHelper.PiOver4/2, 0f), Color.Blue.ToVector3(), 35f));
        }
    }    
}