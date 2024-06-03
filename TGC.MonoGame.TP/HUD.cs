using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP;

namespace TGC.MonoTP
{
    public class HUD 
    {

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        private SpriteFont SpriteFont;
        private SpriteBatch SpriteBatch;
        private Texture2D texture;      
        private GraphicsDevice GraphicsDevice;
        private ContentManager Content;

        public HUD(ContentManager content , GraphicsDevice graphicsDevice)
        {
            this.Content = content;
            this.GraphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            //SpriteBatch.Begin();
            //SpriteBatch.DrawString(SpriteFont, "DERBY GAMES", new Vector2(300, 500), Color.White);
            //SpriteBatch.End();

            GraphicsDevice.Clear(Color.Black);
            DrawCenterTextY("DERBY GAMES", 100, 5);
            DrawCenterTextY("Controles -  WASD", 300, 1);
            DrawCenterTextY("SALTO - Space", 400, 1);
            DrawCenterTextY("God Mode  -  G", 500, 1);
            DrawCenterTextY("Presione SPACE para comenzar...", 600, 1);
        }

        public void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void LoadContent()
        {
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "CascadiaCodePL");
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "CarCrash");

        }

        public void UnloadContent()
        {
        }

        public void DrawCenterText(string msg, float escala)
        {
            var W = GraphicsDevice.Viewport.Width;
            var H = GraphicsDevice.Viewport.Height;
            var size = SpriteFont.MeasureString(msg) * escala;
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, DepthStencilState.Default, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, (H - size.Y) / 2, 0));
            SpriteBatch.DrawString(SpriteFont, msg, new Vector2(0, 0), Color.YellowGreen);
            SpriteBatch.End();
        }

        public void DrawCenterTextY(string msg, float Y, float escala)
        {
            var W = GraphicsDevice.Viewport.Width;
            var H = GraphicsDevice.Viewport.Height;
            var size = SpriteFont.MeasureString(msg) * escala;
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, DepthStencilState.Default, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            SpriteBatch.DrawString(SpriteFont, msg, new Vector2(0, 0), Color.YellowGreen);
            SpriteBatch.End();
        }

        public void DrawRightText(string msg, float Y, float escala)
        {
            var W = GraphicsDevice.Viewport.Width;
            var H = GraphicsDevice.Viewport.Height;
            var size = SpriteFont.MeasureString(msg) * escala;
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, DepthStencilState.Default, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation(W - size.X - 20, Y, 0));
            SpriteBatch.DrawString(SpriteFont, msg, new Vector2(0, 0), Color.YellowGreen);
            SpriteBatch.End();
        }

        public void GameOver()
        {
            DrawCenterText("GAME OVER", 5);

        }
    }

}
