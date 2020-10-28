using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Chinchulines.Menu.States
{
    public abstract class Screen
    {
        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected Game _game;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public Screen(Game game, GraphicsDeviceManager graphics, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphics.GraphicsDevice;
            _content = content;
        }

        public abstract void Update(GameTime gameTime);
    }
}
