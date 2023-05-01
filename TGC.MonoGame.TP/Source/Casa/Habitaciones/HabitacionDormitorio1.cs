using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio1 : IHabitacion{
        public const int Size = 5;

        // / / / / / / / / /
        // LA DE LOS LEGOS
        public HabitacionDormitorio1(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

            Amueblar();
        }
        
        public override void DrawElementos(){
            var bShader = TGCGame.GameContent.E_BasicShader;
            var tShader = TGCGame.GameContent.E_TextureShader;
            
            foreach(var e in Elementos){
                switch(e.GetTag()){
                    case "Lego":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());      
                    break;
                    case "Organizador":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_MaderaNikari);
                    break;
                    case "Cajonera":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_PisoMadera);
                    break;
                    default:
                    break;
                }
                e.Draw();
            }
        }

        private void Amueblar(){
            var carpintero = new ElementoBuilder();
            var tShader = TGCGame.GameContent.E_TextureShader;

            carpintero.Modelo(TGCGame.GameContent.M_Organizador)
                .ConPosicion(800f,300f)
                .ConShader(tShader)
                .ConRotacion(-MathHelper.PiOver2,0f, 0f)
                .ConEscala(1000f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Cajonera)
                .ConPosicion(800f,4600f)
                .ConShader(tShader)
                .ConRotacion(0f,MathHelper.Pi, 0f)
                .ConRotacion(-MathHelper.PiOver2,0f, 0f)
                .ConEscala(1000f);
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