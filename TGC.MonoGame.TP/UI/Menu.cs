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
        Option selected;

        public enum Option
        {
            Resume,
            Restart,
            GodMode,
            SelectStage,
            Exit
        }

        public void Update()
        {
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
        {

            spriteBatch.Begin();

            var position = TextHelper.CenterText(graphicsDevice, font, "Resume", 1f);
            spriteBatch.DrawString(font, "Resume", position, selected == Option.Resume ? Color.White : Color.Gray);
            
            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Restart", position, selected == Option.Restart ? Color.White : Color.Gray);
            
            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "God Mode", position, selected == Option.GodMode ? Color.White : Color.Gray);

            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Select stage", position, selected == Option.SelectStage ? Color.White : Color.Gray);

            position.Y += LineSpacing;
            spriteBatch.DrawString(font, "Exit", position, selected == Option.Exit ? Color.White : Color.Gray);

            spriteBatch.End();

        }
    }
}
