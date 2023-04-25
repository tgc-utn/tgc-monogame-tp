using System;
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
       
        #region LEGOS!
            Vector3 traslacionLegos = new Vector3(2500f,0f,2500f);
            Vector3 desplazamientoRandom, rotacionRandom;
            float random1, random2, random3;
            Lego lego;
            for(int i=0; i<100; i++){
                random1 = (Random.Shared.NextSingle());
                random2 = (Random.Shared.NextSingle()-0.5f);
                random3 = (Random.Shared.NextSingle()-0.5f);
                desplazamientoRandom = new Vector3((2000f*MathF.Cos(random1*MathHelper.TwoPi))*random2,0f,2000f*(MathF.Sin(random1*MathHelper.TwoPi)*random2));
                rotacionRandom = new Vector3(0f,MathHelper.Pi*random3, 0f);
                var randomColor = new Vector3( i%3%2*115 , (i+1)%3%2*115 , (i+2)%3%2*115 );
                
                lego = new Lego(desplazamientoRandom+traslacionLegos, rotacionRandom, randomColor, 5f);
                AddElemento(lego);
            }
            traslacionLegos += new Vector3(-1000f,0f,-1000f);
            AddElemento(new Lego(traslacionLegos + new Vector3(0f,300f, 0f), new Vector3(MathHelper.PiOver2,MathHelper.PiOver4, 0f), Color.Red.ToVector3(), 35f));
            AddElemento(new Lego(traslacionLegos + new Vector3(0f,0f, 700f), new Vector3(0f,0f, 0f), Color.Green.ToVector3(), 35f));
            AddElemento(new Lego(traslacionLegos + new Vector3(0f,150f, 1200f), new Vector3(MathHelper.PiOver4/2,MathHelper.PiOver4/2, 0f), Color.Blue.ToVector3(), 35f));
        #endregion
        
        }
    }    
}