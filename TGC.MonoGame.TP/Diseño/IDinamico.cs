using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP{
    public interface IDinamico  {
        public abstract void Update(GameTime gameTime, KeyboardState keyboard);
    }
}