using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class HabitacionOficina : IHabitacion{
        private const int Size = 5;
        public HabitacionOficina(Vector3 posicionInicial):base(Size,Size,posicionInicial){
            Piso = Piso.Texturado(TGCGame.GameContent.T_PisoMaderaClaro);

            AddPared(Pared.Abajo (Size, Size, posicionInicial));
            AddPared(Pared.Derecha(Size, Size, posicionInicial));
            AddPuerta(Puerta.Arriba(2f, Size, posicionInicial));

            Amueblar();
        }
        public override void DrawElementos(){
            var bShader = TGCGame.GameContent.E_BasicShader;
            var tShader = TGCGame.GameContent.E_TextureShader;
            foreach(var e in Elementos){
                switch(e.GetTag()){
                    case "SillaOficina":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_SillaOficina);
                    break;
                    case "Escritorio":
                        tShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_PisoMadera);
                    break;
                    case "Cafe-Rojo":
                        bShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
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

            carpintero.Modelo(TGCGame.GameContent.M_SillaOficina)
                .ConPosicion(4000f, 1000f)
                .ConShader(tShader)
                .ConRotacion(-MathHelper.PiOver2,-MathHelper.PiOver4,0f)
                .ConEscala(10f);
                AddElemento(carpintero.BuildMueble());

            carpintero.Modelo(TGCGame.GameContent.M_CafeRojo)
                .ConPosicion(4000f, 1000f)
                .ConRotacion(-MathHelper.PiOver2,0f,0f)
                .ConEscala(10f)
                .ConAltura(500f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Planta)
                .ConPosicion(500f,500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Escritorio)
                .ConPosicion(3500f, 1500f)
                .ConShader(tShader)
                .ConRotacion(0f, MathHelper.Pi, 0f)
                .ConEscala(170f)
                .ConAltura(50f);
                AddElemento(carpintero.BuildMueble());
            
            carpintero.Modelo(TGCGame.GameContent.M_Plantis)
                .ConPosicion(4700f, 3500f)
                .ConEscala(15f);
                AddElemento(carpintero.BuildMueble());
        }
    }    
}