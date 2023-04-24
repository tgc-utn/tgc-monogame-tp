using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionPrincipal : IHabitacion{
        private const int Size = 10;
        public HabitacionPrincipal(Vector3 posicionInicial):base(Size,Size,posicionInicial){

            AddPared(Pared.Izquierda (Size, Size, posicionInicial));
            AddPuerta(Puerta.Arriba(3f, Size, posicionInicial));
            AddPared(Pared.Abajo(Size, Size, posicionInicial));
            AddPuerta(Puerta.Derecha(7f, Size, posicionInicial));
            

            #region CargaMueblesDinámicos
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;
                //AddDinamico(new EnemyCar(escala, posicionesAutosIA, Vector3.Zero));
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
            // SI QUERÉS PONER EL TELEVISOR ACÁ, 
            // ELIMINA LOS TELEVISORES DEL DRAW EN EL TGCGAME.CS
            //AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(7500f,150f,300f), 10f));
            AddElemento( new Televisor(new Vector3(7500f,150f,300f), 0f) );
            AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(7950f,50f,350f), new Vector3(0f,MathHelper.Pi,0f)));          

            AddElemento( new Mueble(TGCGame.GameContent.M_Sofa, new Vector3(8000f, 0f, 9350f), new Vector3(0f, MathHelper.Pi, 0f)));
            AddElemento( new Mueble(TGCGame.GameContent.M_Mesita, new Vector3(8000f, 0f, 8200f), new Vector3(0f, MathHelper.Pi, 0f), 20f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(8000f, 390f, 8150f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Cafe, new Vector3(8050f, 390f, 8350f), new Vector3(-MathHelper.PiOver2,0f,0f), 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Aparador, new Vector3(750f,-400f,8500f), new Vector3(0f,MathHelper.PiOver2,0f), 15f));
            AddElemento( new Televisor(new Vector3(150f, 650f, 8800f), MathHelper.PiOver2) );
            #endregion

        }
    }    
}