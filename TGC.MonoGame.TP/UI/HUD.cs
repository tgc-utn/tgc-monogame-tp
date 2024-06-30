using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.UI
{
    internal class HUD
    {
        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font, float gameTimer, float score)
        {
            spriteBatch.Begin();

            // Timer
            var timerText = gameTimer.ToString(@"mm\:ss");
            var timerSize = font.MeasureString(timerText);
            var timerPosition = TextHelper.CenterText(graphicsDevice, font, timerText, 1f, 10);
            spriteBatch.DrawString(font, timerText, timerPosition, Color.White);

            // Score
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);

            spriteBatch.End();
        }

    }
}
