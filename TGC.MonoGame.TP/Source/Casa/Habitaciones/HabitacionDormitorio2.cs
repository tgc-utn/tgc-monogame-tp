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
<<<<<<< HEAD
        
       

        
=======
       
        var centro = getCenter()-posicionInicial;

        AddElemento(new Mueble(TGCGame.GameContent.M_Dragon, new Vector3(400f,700f,400f), new Vector3(MathHelper.PiOver4,MathHelper.PiOver4,0f)));
        AddElemento(new Mueble(TGCGame.GameContent.M_Dragona,new Vector3(400f,700f,4500f), new Vector3(MathHelper.PiOver4,MathHelper.PiOver4*3,0f)));
>>>>>>> f7c719bd067c7063dd1767e318ac8569e68305f0
        }
    }    
}