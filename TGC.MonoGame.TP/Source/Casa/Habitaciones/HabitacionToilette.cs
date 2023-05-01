using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionToilette : IHabitacion{
        public const int Size = 4;
        public HabitacionToilette(float posicionX, float posicionZ):base(Size,Size,new Vector3(posicionX,0f,posicionZ)){
 
            var posicionInicial = new Vector3(posicionX,0f,posicionZ);
                       
            Amueblar();
        }

        
        public override void DrawElementos(){
            var bShader = TGCGame.GameContent.E_BasicShader;
            var mShader = TGCGame.GameContent.E_TextureMirror;
            foreach(var e in Elementos){
                switch(e.GetTag()){
                    case "Bacha":
                    case "Inodoro":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
                    break;
                    case "Baniera":
                        mShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Ladrillos);
                    break;

                    default:
                    break;
                }
                e.Draw();
            }
        }
        
        private void Amueblar(){
            var carpintero = new ElementoBuilder();
            var mShader = TGCGame.GameContent.E_TextureMirror;

            carpintero.Modelo(TGCGame.GameContent.M_Inodoro)
                .ConPosicion(1500f, 500f)
                .ConRotacion(-MathHelper.PiOver2,0,0)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
            

            carpintero.Modelo(TGCGame.GameContent.M_Baniera)
                .ConPosicion(1500f, 3000)
                .ConShader(mShader)
                .ConRotacion(0f, MathHelper.Pi, 0f)
                .ConEscala(60f);
                AddElemento(carpintero.BuildMueble());
            

            carpintero.Modelo(TGCGame.GameContent.M_Bacha)
                .ConPosicion(3000f, -100f)
                .ConRotacion(-MathHelper.PiOver2, 0f, 0f)
                .ConAltura(1000f)
                .ConEscala(18f);
                AddElemento(carpintero.BuildMueble());
            
        }
    }    
}