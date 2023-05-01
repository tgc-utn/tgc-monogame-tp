using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionPrincipal : IHabitacion{
        public const int Size = 10;
        public HabitacionPrincipal(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);
            
            Piso.ConTextura(TGCGame.GameContent.T_PisoMadera);
 
            Amueblar();
        }        
        public override void DrawElementos(){
            var tShader = TGCGame.GameContent.E_TextureShader;
            var bShader = TGCGame.GameContent.E_BasicShader;
            var fShader = TGCGame.GameContent.E_BlacksFilter;
            
            foreach(var e in Elementos){
                switch(e.GetTag()){
                    case "Mesa":
                        fShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_MaderaNikari);      
                        fShader.Parameters["Filter"].SetValue(TGCGame.GameContent.T_MeshFilter);      
                    break;
                    case "Chair":
                    case "Mesita":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_MaderaNikari);      
                    break;
                    case "Sofa":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Alfombra);      
                    break;
                    case "Sillon":
                    case "CafeRojo":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
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
            
            carpintero.Modelo(TGCGame.GameContent.M_Mesa)
                .ConPosicion(4150f, 4150f)
                .ConShader(tShader)
                .ConEscala(12f)
                .ConRotacion(0, MathHelper.PiOver2, 0);
                
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Silla)
                .ConShader(tShader)
                .ConAltura(370f)
                .ConEscala(10f)
                .ConRotacion(-MathHelper.PiOver2, 0, 0)
                .ConPosicion(4150f, 2950f);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4150f,5150f)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );

            
                carpintero
                .ConPosicion(3650f,3650f)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );
            
                carpintero
                .ConPosicion(3650f,4450f)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.PiOver2, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4750f,3650f)
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*0.7f, 0);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(4750f,4450f)
                .ConRotacion(-MathHelper.PiOver2, -MathHelper.PiOver2*1.3f, 0);
                AddElemento( carpintero.BuildMueble() );
            
        
            carpintero.Modelo(TGCGame.GameContent.M_Sillon);
                carpintero
                .ConPosicion(7500f,3500)
                .ConAltura(100f)
                .ConRotacion(0, MathHelper.Pi, 0)
                .ConEscala(10f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_MuebleTV);
                carpintero.ConPosicion(7950f,350f)
                .ConRotacion(0, MathHelper.Pi, 0);
                AddElemento( carpintero.BuildMueble() );
            
            
            carpintero.Modelo(TGCGame.GameContent.M_Sofa)
                .ConPosicion(8000f,9350f)
                .ConShader(tShader)
                .ConRotacion(-MathHelper.PiOver2, MathHelper.Pi, 0)
                .ConEscala(1f);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_Mesita)
                .ConEscala(20f)
                .ConShader(tShader)
                .ConPosicion(8000f, 8200f)
                .ConRotacion(-MathHelper.PiOver2, 0, 0);
                AddElemento( carpintero.BuildMueble() );
            

            carpintero.Modelo(TGCGame.GameContent.M_CafeRojo)
                .ConEscala(10f)
                .ConAltura(390f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
          
                .ConPosicion(8000f, 8150f);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConPosicion(8050f, 8350f);
                AddElemento( carpintero.BuildMueble() );
            
            //Está bugueado
            /* carpintero.Modelo(TGCGame.GameContent.M_Aparador)
                .ConEscala(15f)
                .ConPosicion(getCenter().X,getCenter().Z)
                .ConRotacion(MathHelper.Pi,0,-MathHelper.PiOver2)
                .ConShader(tShader)
                .ConAltura(300f);
                AddElemento( carpintero.BuildMueble() ); */

            carpintero.Modelo(TGCGame.GameContent.M_Televisor)
                .ConEscala(10f)
                .ConShader(TGCGame.GameContent.E_SpiralShader)
                .ConPosicion(7500f,350f)
                .ConAltura(100);
                AddElemento( carpintero.BuildMueble() );

                carpintero
                .ConAltura(650f)
                .ConPosicion(150f, 8800f)
                .ConRotacion(0f,MathHelper.PiOver2,0f);
                
                AddElemento( carpintero.BuildMueble() );

            #region Autos Enemigos

                var posicionesAutosIA = new Vector3(1000f,0f,6000f);           
                for(int i=1; i<6; i++){
                    AddDinamico(new EnemyCar(i*1000f,0f,6000f, Vector3.Zero));
                    AddDinamico(new EnemyCar(1001f,0f,i*1000f, Vector3.Zero));
                }
            
            #endregion            
        }
    }    
}