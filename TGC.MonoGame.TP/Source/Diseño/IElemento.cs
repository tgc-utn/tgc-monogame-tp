using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    /// Crea un elemento estático en la posición del vector PosicionInicial
    public abstract class IElemento  {
        public Matrix World = Matrix.Identity;
        protected Model Model;
        public Vector3 PosicionInicial {get; set;}
        public Vector3 rotacion;
        //AGREGAR ESCALA
        //AGREGAR EFFECT
        
        public IElemento(Vector3 posicionInicial, Vector3 rotacion){
            PosicionInicial = posicionInicial;
            Rotar(rotacion);
            Trasladar(posicionInicial);
        }
        public IElemento(Vector3 posicionInicial, Vector3 rotacion, float escala){
            Escalar(escala);

            PosicionInicial = posicionInicial;
            Rotar(rotacion);
            Trasladar(posicionInicial);
        }

    
        private void Escalar(float escala){
            World *= Matrix.CreateScale(escala);
        }
        private void Rotar(Vector3 rotacion){
            World *= Matrix.CreateRotationX(rotacion.X) * Matrix.CreateRotationY(rotacion.Y) * Matrix.CreateRotationZ(rotacion.Z);
        }
        private void Trasladar(Vector3 posicionInicial){
            World = World * Matrix.CreateTranslation(posicionInicial);
        }
    
        public void newPosicionInicial(Vector3 posicionInicial){
            PosicionInicial = posicionInicial;
            World *= Matrix.CreateTranslation(posicionInicial);
        }
        public void Draw (Matrix view, Matrix projection){
            Model.Draw(World,view,projection);
        }
    }
}