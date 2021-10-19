using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP
{
    internal class Menu
    {
        private TGCGame Game;
        private Model Barco;
        private SpriteBatch spriteBatch;
        public Menu(TGCGame game)
        {
            Game = game;
            Barco = Game.Content.Load<Model>(TGCGame.ContentFolder3D + "Barco");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.ocean.Draw(gameTime, Game.Camera.View, Game.Camera.Projection, Game);
            Barco.Draw(
                Matrix.CreateRotationY( (float)Game.ElapsedTime) * Matrix.CreateScale(0.01f) *
                Matrix.CreateTranslation(new Vector3(500,0,0)), Game.Camera.View, Game.Camera.Projection);
        }

        public void Update(GameTime gameTime)
        {
            
            Game.Camera.Update(gameTime);
        }
    }
}