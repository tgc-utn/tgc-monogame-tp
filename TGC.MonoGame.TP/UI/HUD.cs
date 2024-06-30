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
        const float timerScale = 1.5f;

        public static void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font, TimeSpan gameTimer, float score)
        {
            spriteBatch.Begin();

            // Timer
            var timerText = gameTimer.ToString(@"mm\:ss");
            var timerSize = font.MeasureString(timerText);
            var timerPosition = TextHelper.CenterText(graphicsDevice, font, timerText, timerScale, 10);
            TextHelper.DrawString(spriteBatch, font, timerText, timerPosition, Color.White, timerScale);

            // TODO: ¿agregar score?
            // Score
            // TextHelper.DrawString(spriteBatch, font, "Score: " + score, new Vector2(10, 10), Color.White, 1.5f);

            spriteBatch.End();
        }

    }
}
