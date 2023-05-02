using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionDormitorio2 : IHabitacion{
        public const int Size = 5;
        public HabitacionDormitorio2(float posicionX, float posicionZ) : base(Size,Size,new Vector3(posicionX,0f,posicionZ)){

            var posicionInicial = new Vector3(posicionX,0f,posicionZ);

            Amueblar();
        }
        
        public override void DrawElementos(){
            var tShader = TGCGame.GameContent.E_TextureShader;
            var bShader = TGCGame.GameContent.E_BasicShader;
            foreach(var e in Elementos){
                switch(e.GetTag()){
                    case "Alfil":
                    case "Torre":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_PisoMadera);      
                    break;
                    case "Sillon":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
                    break;
                    case "Dragon":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Dragon);
                    break;
                    case "Dragona":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.MediumVioletRed.ToVector3());
                    break;
                    case "Cama":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_PisoMaderaClaro);
                    break;
                    case "Juego":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());
                    break;
                    case "Puff":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                    break;
                    case "Armario1":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_PisoMadera);
                    break;

                    default:
                    break;
                }
                e.Draw();
            }
        }

        private void Amueblar(){
            var ubicacionSet = Vector3.Zero;
            var RotacionSet = new Vector3(0f, MathHelper.PiOver4,0f);

            var carpintero = new ElementoBuilder();
            var tShader = TGCGame.GameContent.E_TextureShader;

            carpintero.Modelo(TGCGame.GameContent.M_Dragon)
                .ConPosicion(400f, 400f)
                .ConShader(tShader)
                .ConAltura(2000f)
                .ConRotacion(MathHelper.PiOver4,MathHelper.PiOver4,0f);
                AddElemento(carpintero.BuildMueble());

          /*  carpintero.Modelo(TGCGame.GameContent.M_Cama)
                .ConPosicion(1000f, 2000f)
                .ConAltura(0f)
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Juego)
                .ConPosicion(3000f, 400f)
                .ConAltura(500f)
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Puff)
                .ConPosicion(4000f, 300f)
                .ConAltura(0f)
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Armario1)
                .ConPosicion(4700f, 2000f)
                .ConAltura(500f)
                .ConRotacion(0f,-MathHelper.Pi,0f);
                AddElemento(carpintero.BuildMueble());  */
                
            carpintero.Modelo(TGCGame.GameContent.M_Dragona)
                .ConPosicion(400f, 4500f)
                .ConAltura(2000f)
                .ConRotacion(MathHelper.PiOver4,MathHelper.PiOver4*3,0f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Sillon)
                .ConEscala(100f)

                .ConPosicion(ubicacionSet.X+50f, ubicacionSet.Z+2150f)
                .ConRotacion(RotacionSet.X,MathHelper.PiOver2+RotacionSet.Y,0+RotacionSet.Z);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(0f,0f)
                .ConRotacion(RotacionSet.X,0,RotacionSet.Z);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(ubicacionSet.X+2050f, ubicacionSet.Z+50f)
                .ConRotacion(RotacionSet.X,RotacionSet.Y-MathHelper.PiOver2,RotacionSet.Z);
                AddElemento(carpintero.BuildMueble());

            ubicacionSet = new Vector3(200f,0f,200f);
            #region Set Ajedrez
            carpintero.Modelo(TGCGame.GameContent.M_Torre)
                .ConShader(TGCGame.GameContent.E_TextureShader)
                
                .ConPosicion(ubicacionSet.X - 200f, ubicacionSet.Z - 200f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(ubicacionSet.X + 400f, ubicacionSet.Z + 400f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_Alfil)
                .ConShader(TGCGame.GameContent.E_TextureShader)

                .ConPosicion(ubicacionSet.X, ubicacionSet.Z +400f);
                AddElemento(carpintero.BuildMueble());
                
                carpintero
                .ConPosicion(ubicacionSet.X +400f, ubicacionSet.Z);
                AddElemento(carpintero.BuildMueble());
            #endregion
            
        }
    }    
}