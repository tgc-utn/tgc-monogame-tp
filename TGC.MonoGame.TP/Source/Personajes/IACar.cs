using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class IACar : Auto
    {
        public IACar(Vector3 posicionInicial, Vector3 rotacion) : base(posicionInicial, rotacion){}

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
        }
    }
}
