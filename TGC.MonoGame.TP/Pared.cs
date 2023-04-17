using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Pared{
        private int Ancho;
        private int Alto;
        private Vector3 PosicionInicial;
        private bool esHorizontal;
        private Effect Efecto;
        public Pared(int Ancho, int Alto, Vector3 PosicionInicial, bool esHorizontal){
            this.Ancho = Ancho;
            this.Alto = Alto;
            this.PosicionInicial = PosicionInicial;
            this.esHorizontal = esHorizontal;
        }

        public void Load(ContentManager Content)
        {
            Efecto = Content.Load<Effect>("Effects/BasicShader");

            Efecto.Parameters["DiffuseColor"].SetValue(new Color(117, 115, 162).ToVector3());
            Matrix Rotacion = esHorizontal ? Matrix.Identity : Matrix.CreateRotationY(MathHelper.PiOver2);
            Efecto.Parameters["World"].SetValue(
                Matrix.CreateScale(Ancho*500f, 0f, Alto*500f) *
                Matrix.CreateRotationX(MathHelper.PiOver2) *
                Rotacion *
                Matrix.CreateTranslation(PosicionInicial));  
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            Efecto.Parameters["View"].SetValue(View);
            Efecto.Parameters["Projection"].SetValue(Projection);
            TGCGame.GeometriesManager.Quad.Draw(Efecto); 
        }
    }
}