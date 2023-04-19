using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public interface IDinamico  {
        // En vez de GameTime que sea un tiempo global
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}