using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionOficina : IHabitacion{
        public HabitacionOficina(int ancho, int alto, Vector3 posicionInicial):base(ancho,alto,posicionInicial){
            Piso.Oficina();
            AddPared(Pared.Abajo (ancho, alto, posicionInicial));
            AddPared(Pared.Arriba(ancho, alto, posicionInicial));
            AddPared(Pared.Derecha(ancho, alto, posicionInicial));
            AddPuerta(Puerta.Izquierda(1f, ancho, posicionInicial));

            AddElemento(new Mueble(TGCGame.GameContent.M_SillaOficina,new Vector3(1000f,0f,1000f), new Vector3(-MathHelper.PiOver2,MathHelper.PiOver4,0f), 10f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(1000f,500f,1000f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
        
            AddElemento(new Mueble(TGCGame.GameContent.M_Planta, new Vector3(500f,0f,500f), new Vector3(0f,0f,0f), 15f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Escritorio, new Vector3(1500f,0f,1500f), new Vector3(0f, 0f, 0f), 170f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Plantis, new Vector3(4800f,0f,4000f), new Vector3(0f,0f,0f), 15f));

        }
    }    
}