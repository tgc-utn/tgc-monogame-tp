using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames
{
    interface IDrawable 
    {
        public void Draw(GameTime gameTime) {}
        public void Update(GameTime gameTime) {}
        public void Load() {}
    }
}