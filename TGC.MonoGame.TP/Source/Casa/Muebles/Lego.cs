using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Lego : Mueble
    {   
        private Vector3 Color;
        public Lego(Vector3 posicionInicial, Vector3 rotacion, Vector3 color, float escala = 1f) : base(TGCGame.GameContent.M_Lego,posicionInicial, rotacion, escala){
            Color = color;
        }

        public override void Draw(Matrix view, Matrix projection){
            var bShader = TGCGame.GameContent.E_BasicShader;
            bShader.Parameters["World"].SetValue(World);
            bShader.Parameters["DiffuseColor"].SetValue(Color);

            foreach(var mesh in Model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                mesh.Draw();
        }
    }
}
