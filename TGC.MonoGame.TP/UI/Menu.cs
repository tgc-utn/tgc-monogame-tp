using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.UI
{
    internal class Menu
    {
        const float LineSpacing = 25f;

        public static void Update()
        {

        }

        public static void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font, MenuOption selected)
        {

            spriteBatch.Begin();

            var position = TextHelper.CenterText(graphicsDevice, font, "Resume", 1f);
            spriteBatch.DrawString(font, "Resume", position, selected == MenuOption.Resume ? Color.White : Color.Gray);
            
            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Restart", position, selected == MenuOption.Restart ? Color.White : Color.Gray);
            
            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "God Mode", position, selected == MenuOption.GodMode ? Color.White : Color.Gray);

            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Select stage", position, selected == MenuOption.SelectStage ? Color.White : Color.Gray);

            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Exit", position, selected == MenuOption.Exit ? Color.White : Color.Gray);

            spriteBatch.End();

        }
    }
}
