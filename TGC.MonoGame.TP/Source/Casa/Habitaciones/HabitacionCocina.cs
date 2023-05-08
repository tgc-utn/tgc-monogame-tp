using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionCocina : IHabitacion{
        public const int Size = 6;
        public HabitacionCocina(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){
            Piso.ConTextura(TGCGame.GameContent.T_PisoCeramica, 10);

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);
            
            Amueblar();
        }
        public override void DrawElementos(){
            var mShader = TGCGame.GameContent.E_TextureMirror;
            var bShader = TGCGame.GameContent.E_BasicShader;
            var tShader = TGCGame.GameContent.E_TextureShader;
            foreach(var mueble in Elementos){
                switch(mueble.GetTag()){
                    case "Olla":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.DarkSlateBlue.ToVector3());
                    break;
                    case "Botella":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.LightBlue.ToVector3());
                    break;
                    case "ParedCocina":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Ladrillos);
                    break;
                    default:
                        mShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Marmol);
                    break;
                }
                mueble.Draw();
            }
        }
        private void Amueblar(){
            var carpintero = new ElementoBuilder();
            var alturaMesada = 600f;
            var mShader = TGCGame.GameContent.E_TextureMirror;
            var tShader = TGCGame.GameContent.E_TextureShader;

            carpintero.Modelo(TGCGame.GameContent.M_Mesada)
                .ConPosicion(100f,150f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConShader(mShader)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_MesadaLateral2)
                .ConPosicion(100f,1000f)
                .ConShader(mShader)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_PlatosApilados)
                .ConPosicion(300f,500f)
                .ConAltura(alturaMesada)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_MesadaCentral)
                .ConPosicion(100f,1500f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConShader(mShader)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Olla)
                .ConPosicion(400f,1500f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(2350f,400f)
                .ConAltura(alturaMesada);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Plato)
                .ConPosicion(500f,1200f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(500f,1600f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
                carpintero
                .ConPosicion(500f,1800f)
                .ConAltura(alturaMesada*2)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_MesadaLateral)
                .ConPosicion(100f,2100f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConShader(mShader)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_PlatoGrande)
                .ConPosicion(300f,2300f)
                .ConAltura(alturaMesada+100f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Botella)
                .ConPosicion(1200f,200f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Maceta2)
                .ConPosicion(300f,2400f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta3)
                .ConPosicion(300f,2900f)
                .ConAltura(alturaMesada+100f)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_ParedCocina)
                .ConPosicion(100f,150f)
                .ConRotacion(0f,MathHelper.PiOver2,0f)
                .ConShader(tShader)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Cocine)
                .ConPosicion(2500f,500f)
                .ConAltura(500f)
                .ConEscala(2f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Alacena)
                .ConPosicion(3000f,0f)
                .ConAltura(alturaMesada*2)
                .ConShader(mShader)
                .ConEscala(20f);
                AddElemento(carpintero.BuildMueble());

                carpintero
                .ConPosicion(200f,3900f)
                .ConRotacion(0f,MathHelper.PiOver2,0f);
                AddElemento(carpintero.BuildMueble());
                            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta)
                .ConPosicion(5400f,5400f)
                .ConEscala(60f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta4)
                .ConPosicion(400f,5400f)
                .ConEscala(60f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Maceta4)
                .ConPosicion(400f,5400f)
                .ConAltura(700f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}