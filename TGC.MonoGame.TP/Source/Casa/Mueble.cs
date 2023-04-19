using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Mueble : IElemento
    {
        private Vector3 Position;

        public Mueble(Model Model, float escala, Vector3 posicionInicial, Vector3 rotacion) : base(posicionInicial, rotacion){
            World *= Matrix.CreateScale(escala);

            this.Model = Model;
            //no se si es necesario guardarla
            Position = posicionInicial;
        }

        
    }
}
