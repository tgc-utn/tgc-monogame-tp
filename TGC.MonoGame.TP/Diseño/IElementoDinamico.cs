using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Design
{
    public abstract class IElementoDinamico : Elemento, IDinamico  {
        public IElementoDinamico(Model modelo, Vector3 posicionInicial, Vector3 rotacion, float escala = 1f) 
        : base(modelo, posicionInicial, rotacion, escala){}
        
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}