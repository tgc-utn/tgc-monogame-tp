using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.UI
{
    public abstract class Text
    {
        public abstract void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font);

        protected Vector2 CenterText(GraphicsDevice graphicsDevice, SpriteFont font, String text, float scale)
        {
            Vector2 size = font.MeasureString(text);
            return new Vector2((graphicsDevice.Viewport.Width - size.X * scale) / 2, (graphicsDevice.Viewport.Height - size.Y * scale) / 2);
        }

        protected void DrawStringWithShadow(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 position, Color color, float scale)
        {
            // shadow
            spriteBatch.DrawString(font, text, position + new Vector2(2, 2), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            // text
            spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

    }
}
