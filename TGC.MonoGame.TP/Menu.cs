using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TGC.MonoGame.TP
{
    internal class Menu
    {
        private TGCGame Game;
        private Model Barco;
        private SpriteBatch spriteBatch;
        public Texture2D botonesOff;
        public Texture2D botonesOn;
        public Texture2D botonesCurrentPlay;
        public Texture2D botonesCurrentExit;
        private SpriteFont font;
        public Song Song { get; set; }
        private SoundEffect soundButtom { get; set; }
        public Menu(TGCGame game)
        {
            Game = game;
            Barco = Game.Content.Load<Model>(TGCGame.ContentFolder3D + "Barco");
            botonesOff = Game.Content.Load<Texture2D>("Textures/" + "color");
            botonesOn = Game.Content.Load<Texture2D>("Textures/" + "roughness");
            botonesCurrentPlay = botonesOff;
            botonesCurrentExit = botonesOff;
            font = Game.Content.Load<SpriteFont>("SpriteFonts/Text");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            soundButtom = Game.Content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "botones2");
            Song = Game.Content.Load<Song>(TGCGame.ContentFolderMusic + "Menu");
            MediaPlayer.IsRepeating = true;
        }

        public void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.ocean.Draw(gameTime, Game.Camera.View, Game.Camera.Projection, Game);
            Barco.Draw(
                Matrix.CreateRotationY( (float)Game.ElapsedTime) * Matrix.CreateScale(0.01f) *
                Matrix.CreateTranslation(new Vector3(500,0,0)), Game.Camera.View, Game.Camera.Projection);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            //spriteBatch.DrawString();
            spriteBatch.Draw(botonesCurrentPlay,
                new Rectangle(250, 50,
                    200, 100), Color.White);
            spriteBatch.DrawString(font, "Play", new Vector2(285, 60), Color.White);
            spriteBatch.Draw(botonesCurrentExit,
                new Rectangle(800, 50,
                    200, 100), Color.White);
            spriteBatch.DrawString(font, "Exit", new Vector2(850, 60), Color.White);
            spriteBatch.End();
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        public void Update(GameTime gameTime)
        {
            var position = Mouse.GetState().Position;
            if (position.X > 800 && position.X < 1000 && position.Y > 50 && position.Y < 150)
            {
                botonesCurrentExit = botonesOn;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    soundButtom.Play();
                    Game.Exit();
                }
            }
            else
            {
                botonesCurrentExit = botonesOff;
            }
            if (position.X > 250 && position.X < 450 && position.Y > 50 && position.Y < 150)
            {
                botonesCurrentPlay = botonesOn;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    MediaPlayer.Stop();
                    soundButtom.Play();
                    Game.GameState = "PLAY";
                    Game.Camera.Menu = false;
                }
            }
            else
            {
                botonesCurrentPlay = botonesOff;
            }
            Game.Camera.Update(gameTime);
        }
    }
}