using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.UI
{
    internal class TitleScreen : Text
    {

        const string titleText = "Marble It Down!";
        const float titleScale = 5f;

        const string pressStartText = "Press any key";
        const float pressStartScale = 1f;

        public override void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();

            var titlePosition = CenterText(graphicsDevice, font, titleText, titleScale);
            DrawStringWithShadow(spriteBatch, font, pressStartText, titlePosition, Color.Yellow, titleScale);

            var pressStartPosition = CenterText(graphicsDevice, font, titleText, titleScale) + new Vector2(0,200);
            DrawStringWithShadow(spriteBatch, font, pressStartText, pressStartPosition, Color.White, pressStartScale); 

            spriteBatch.End();
        }

    }
}
