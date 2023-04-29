using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class Televisor : IElemento{
        private Vector3 Position;
        private new Model Model => TGCGame.GameContent.M_Televisor1;

        public Televisor(Vector3 Position, float rotacion):base(Position, new Vector3(0f,rotacion,0f)) {
            this.Position = Position;
            World =  Matrix.CreateScale(10f) *
                    Matrix.CreateRotationY(rotacion) *
                    Matrix.CreateTranslation(Position);
                    
            var bShader = TGCGame.GameContent.E_SpiralShader;
            foreach(var mesh in Model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                meshPart.Effect=bShader;
        }
        
        public override void Draw(Matrix view, Matrix projection) 
        {
            var bShader = TGCGame.GameContent.E_SpiralShader;
            bShader.Parameters["Texture"]?.SetValue(TGCGame.GameContent.T_PisoMadera);
            bShader.Parameters["Filter"]?.SetValue(TGCGame.GameContent.T_MeshFilter);
            bShader.Parameters["World"].SetValue(World);
            
            foreach(var mesh in Model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                mesh.Draw();
        }
    }
}