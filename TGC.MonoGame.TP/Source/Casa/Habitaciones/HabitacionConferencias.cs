using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class HabitacionConferencias : IHabitacion{
        public HabitacionConferencias(int ancho, int alto, Vector3 posicionInicial):base(ancho,alto,posicionInicial){
            Piso.Azul();
            AddPared(Pared.Abajo (ancho, alto, posicionInicial));
            AddPuerta(Puerta.Izquierda(5f, ancho, posicionInicial));
            AddPuerta(Puerta.Derecha(1f, ancho, posicionInicial));
            AddPuerta(Puerta.Arriba(3f, ancho, posicionInicial));

            #region Set Televisión, Rack y Sillas
            for(int i = 2000 ; i<1000*6 ; i+=1000){
                AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 7800f ), 10f));
                AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 7100f ), 10f));
                AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 6400f ), 10f));
                AddElemento(new Mueble(TGCGame.GameContent.M_Silla, new Vector3( i , 400f , 5700f ), 10f));
            }
            AddElemento( new Mueble(TGCGame.GameContent.M_MuebleTV,   new Vector3(2750f,50f,9200f)));
            AddElemento( new Mueble(TGCGame.GameContent.M_Televisor1, new Vector3(3200f,150f,9200f),10f));            
            #endregion
            #region Set Sillones
            var RotacionSet = new Vector3(0f, MathHelper.PiOver4,0f);
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(6000f,150f,4000f), new Vector3(0,MathHelper.PiOver2,0)+RotacionSet, 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(5950f,150f,1850f),RotacionSet, 10f));
            AddElemento( new Mueble(TGCGame.GameContent.M_Sillon, new Vector3(8100f,150f,1900f), new Vector3(0,-MathHelper.PiOver2,0)+RotacionSet, 10f));
            #endregion
            
            #region LEGOS!
            Vector3 traslacionLegos = new Vector3(2500f,0f,2500f);
            Vector3 desplazamientoRandom, rotacionRandom;
            float random1, random2, random3;
            Mueble lego;
            for(int i=0; i<200; i++){
                random1 = (Random.Shared.NextSingle());
                random2 = (Random.Shared.NextSingle()-0.5f);
                random3 = (Random.Shared.NextSingle()-0.5f);
                desplazamientoRandom = new Vector3((2000f*MathF.Cos(random1*MathHelper.TwoPi))*random2,0f,2000f*(MathF.Sin(random1*MathHelper.TwoPi)*random2));
                rotacionRandom = new Vector3(0f,MathHelper.Pi*random3, 0f);
                //var randomColor = new Vector3( (random1*1000)%256, (random2*1000)%256, (random3*1000)%256);
                
                lego = new Mueble(TGCGame.GameContent.M_Lego , desplazamientoRandom+traslacionLegos, rotacionRandom, 5f);
                AddElemento(lego);
            }
            traslacionLegos += new Vector3(-1000f,0f,-1000f);
            AddElemento(new Mueble(TGCGame.GameContent.M_Lego , traslacionLegos + new Vector3(0f,300f, 0f), new Vector3(MathHelper.PiOver2,MathHelper.PiOver4, 0f), 35f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Lego , traslacionLegos + new Vector3(0f,0f, 700f), new Vector3(0f,0f, 0f), 35f));
            AddElemento(new Mueble(TGCGame.GameContent.M_Lego , traslacionLegos + new Vector3(0f,150f, 1200f), new Vector3(MathHelper.PiOver4/2,MathHelper.PiOver4/2, 0f), 35f));
            #endregion
            
            #region Set Ajedrez
            var desplazamientoSet = new Vector3(6650f,0f,2650f);
            AddElemento(new Mueble(TGCGame.GameContent.M_Torre , desplazamientoSet));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , desplazamientoSet + new Vector3(0f,0f,400f)));
            AddElemento(new Mueble(TGCGame.GameContent.M_Torre , desplazamientoSet + new Vector3(400f,0f,400f)));
            AddElemento(new Mueble(TGCGame.GameContent.M_Alfil , desplazamientoSet + new Vector3(400f,0f,0f)));

            #endregion
           
        }
    }    
}