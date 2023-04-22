using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Mueble : IElemento
    {
        private Vector3 Position = Vector3.Zero;

        public Mueble(Model Model, Vector3 posicionInicial, Vector3 rotacion, float escala = 1f) : base(posicionInicial, rotacion, escala){
            this.Model = Model;
            Position = posicionInicial;
        }
        
        // Hola Seba... Las dos clases siguientes son para probar los modelos mas rapido... en el origen y escala 1
        // Holi n_n
        public Mueble(Model Model) : base(Vector3.Zero, Vector3.Zero, 1f){
            this.Model = Model;
        }
        public Mueble(Model Model, Vector3 posicionInicial, float escala = 1f) : base(posicionInicial, Vector3.Zero, escala){
            this.Model = Model;
            Position = posicionInicial;
        }
    }
}
