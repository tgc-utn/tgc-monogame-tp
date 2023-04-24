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
        {/* 
            Matrix ScreenWorld =    Matrix.CreateScale(500f, 0f, 1000f) * 
                                    Matrix.CreateRotationZ(MathHelper.PiOver2) * 
                                    Matrix.CreateTranslation(new Vector3(50f, 200f, -500f)) * //Fix: Centrado en el televisor
                                    Matrix.CreateTranslation(Position); 

            TGCGame.GameContent.E_SpiralShader.Parameters["World"].SetValue(ScreenWorld); 
            TGCGame.GameContent.G_Quad.Draw(TGCGame.GameContent.E_SpiralShader);
 */
            //Model.Draw(Televisor,View,Projection);
            var bShader = TGCGame.GameContent.E_SpiralShader;
            bShader.Parameters["World"].SetValue(World);
            
            foreach(var mesh in Model.Meshes)
            foreach(var meshPart in mesh.MeshParts)
                mesh.Draw();
        }
    }
}