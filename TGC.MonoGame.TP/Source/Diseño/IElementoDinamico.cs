using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public abstract class IElementoDinamico : IElemento, IDinamico  {
        public IElementoDinamico(Vector3 posicionInicial, Vector3 rotacion, float escala = 1f) : base(posicionInicial, rotacion, escala){}
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}