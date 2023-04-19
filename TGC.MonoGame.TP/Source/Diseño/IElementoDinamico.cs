using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public abstract class IElementoDinamico : IElemento, IDinamico  {
        public IElementoDinamico(Vector3 posicionInicial, Vector3 rotacion) : base(posicionInicial, rotacion){}
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}