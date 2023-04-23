using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPrincipal : IHabitacion{
        public HabitacionPrincipal(int ancho, int alto, Vector3 posicionInicial):base(ancho,alto,posicionInicial){
            AddPared(Pared.Izquierda (ancho, alto, posicionInicial));
            AddPuerta(Puerta.Arriba(8f, ancho, posicionInicial));
            AddPared(Pared.Abajo(ancho, alto, posicionInicial));
            AddPuerta(Puerta.Derecha(5f, ancho, posicionInicial));

            #region CargaMueblesDinámicos
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion
            
            #region CargaMueblesEstáticos

            AddElemento( new Mueble(TGCGame.GameContent.M_Mesa, new Vector3(4150f,0f,4150f), new Vector3(0, MathHelper.PiOver2, 0), 12f ));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,2950f), Vector3.Zero, 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4150f,370f,5150f), new Vector3(0, MathHelper.Pi, 0), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,3650f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(3650f,370f,4450f), new Vector3(0, MathHelper.PiOver2, 0), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,3650f), new Vector3(0, -MathHelper.PiOver2*0.7f, 0), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Silla, new Vector3(4750f,370f,4450f), new Vector3(0, -MathHelper.PiOver2*1.3f, 0), 10f));

            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(7500f,100f,3500f), new Vector3(0, MathHelper.Pi, 0), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(7500f,150f,300f), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(7950f,50f,350f), new Vector3(0f,MathHelper.Pi,0f)));

            #endregion

        }
    }    
}