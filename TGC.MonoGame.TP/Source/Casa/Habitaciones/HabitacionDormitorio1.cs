using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio1 : IHabitacion{
        private const int Size = 5;
        public HabitacionDormitorio1(Vector3 posicionInicial):base(Size,Size,posicionInicial){
        Piso.Azul();
        AddPared(Pared.Abajo (Size, Size, posicionInicial));
        AddPared(Pared.Derecha(Size, Size, posicionInicial));
        AddPuerta(Puerta.Arriba(1f, Size, posicionInicial));

        var carpintero = new ElementoBuilder();

        carpintero.Modelo(TGCGame.GameContent.M_Organizador)
            .ConPosicion(800f,300f)
            .ConEscala(15f);
            AddElemento(carpintero.BuildMueble());

        carpintero.Modelo(TGCGame.GameContent.M_Cajonera)
            .ConPosicion(800f,4600f)
            .ConRotacion(0f,MathHelper.Pi, 0f)
            .ConEscala(15f);
            AddElemento(carpintero.BuildMueble());
        carpintero.Modelo(TGCGame.GameContent.M_CamaMarinera)
            .ConPosicion(3400f,600f)
            .ConRotacion(0f,MathHelper.Pi,0f)
            .ConEscala(8f);
            AddElemento(carpintero.BuildMueble());

        #region LEGOS

        var traslacionLegos = new Vector2(2500f,2500f);
        float random1, random2, random3;
        Vector2 ubicacionSet = new Vector2(1000f,1000f), 
                desplazamientoRandom = ubicacionSet; 
        Vector3 rotacionRandom;
        
        carpintero.Modelo(TGCGame.GameContent.M_Lego);
            for(int i=0; i<100; i++){
                random1 = (Random.Shared.NextSingle());
                random2 = (Random.Shared.NextSingle()-0.5f);
                random3 = (Random.Shared.NextSingle()-0.5f);

                desplazamientoRandom += new Vector2((2000f*MathF.Cos(random1*MathHelper.TwoPi))*random2,2000f*(MathF.Sin(random1*MathHelper.TwoPi)*random2));
                rotacionRandom = new Vector3(0f,MathHelper.Pi*random3, 0f);
                
                //var randomColor = new Vector3( i%3%2*115 , (i+1)%3%2*115 , (i+2)%3%2*115 );
                
                carpintero
                    .ConPosicion(desplazamientoRandom.X,desplazamientoRandom.Y)
                    .ConRotacion(rotacionRandom.X,rotacionRandom.Y,rotacionRandom.Z)
                    .ConEscala(5f);
                    AddElemento(carpintero.BuildMueble());
            }
            traslacionLegos.X -= 1000f;
            traslacionLegos.Y -= 1000f;
            carpintero
                .ConPosicion(traslacionLegos.X,traslacionLegos.Y)
                .ConAltura(300f)
                .ConRotacion(MathHelper.PiOver2,MathHelper.PiOver4, 0f)
                .ConEscala(35f);
                AddElemento(carpintero.BuildMueble());
            carpintero.Reset();

            carpintero
                .ConPosicion(traslacionLegos.X,traslacionLegos.Y +700f)
                .ConEscala(35f);
                AddElemento(carpintero.BuildMueble());
            carpintero.Reset();
                
            carpintero
                .ConPosicion(traslacionLegos.X,traslacionLegos.Y+1200f)
                .ConAltura(150f)
                .ConRotacion(MathHelper.PiOver4/2,MathHelper.PiOver4/2, 0f)
                .ConEscala(35f);
                AddElemento(carpintero.BuildMueble());
            carpintero.Reset();
        #endregion
        
        }
    }    
}