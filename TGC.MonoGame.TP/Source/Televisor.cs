using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.MonoGame.TP
{
    public class Televisor
    {
        private Vector3 Position;
        private Model Model => TGCGame.GameContent.M_Televisor1;

        public Televisor(Vector3 Position) {
            this.Position = Position;
        }
        
        internal void Draw(Matrix View, Matrix Projection) 
        {
            Matrix ScreenWorld =    Matrix.CreateScale(500f, 0f, 1000f) * 
                                    Matrix.CreateRotationZ(MathHelper.PiOver2) * 
                                    Matrix.CreateTranslation(new Vector3(50f, 200f, -500f)) * //Fix: Centrado en el televisor
                                    Matrix.CreateTranslation(Position); 

            TGCGame.GameContent.E_SpirtalShader.Parameters["World"].SetValue(ScreenWorld); 
            TGCGame.GameContent.G_Quad.Draw(TGCGame.GameContent.E_SpirtalShader);

            Matrix Televisor =  Matrix.CreateScale(10f) *
                                Matrix.CreateRotationY(MathHelper.PiOver2) *
                                Matrix.CreateTranslation(Position);

            Model.Draw(Televisor, View, Projection);
        }
    }
}