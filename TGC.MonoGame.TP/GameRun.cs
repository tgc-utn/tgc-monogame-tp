using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP
{
    internal class GameRun
    {
        private TGCGame Game;
        private float time;
        public GameRun(TGCGame game)
        {
            Game = game;
            time = 0;
        }


        public void Draw(GameTime gameTime)
        {
            
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.Model.Draw(Game.World * Matrix.CreateTranslation(120, 25, 0), Game.Camera.View, Game.Camera.Projection);
            Game.island.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(1000, 0, 0), Game.Camera.View, Game.Camera.Projection);
            Game.Barco3.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-100f, 0, 0), Game.Camera.View, Game.Camera.Projection);
            Game.MainShip.Draw();
            Game.Terreno2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(-550, 50, 0), Game.Camera.View, Game.Camera.Projection);
            Game.Rock.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-800, 20, 0), Game.Camera.View, Game.Camera.Projection);
            Game.island.Draw(Game.World * Matrix.CreateTranslation(200f, -60f, -600), Game.Camera.View, Game.Camera.Projection);
            Game.islandTwo.Draw(Game.World * Matrix.CreateTranslation(-900f, -60f, -1000f), Game.Camera.View, Game.Camera.Projection);
            for (int isla = 0; isla < Game.cantIslas; isla++)
            {
                Game.islands[isla].Draw(Game.World * Matrix.CreateScale(500f) * Matrix.CreateTranslation(Game.posicionesIslas[isla]), Game.Camera.View, Game.Camera.Projection);
            }
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Game.ocean.Draw(gameTime, Game.Camera.View, Game.Camera.Projection, Game);
            if (Game.Camera.CanShoot)
            {
                Game.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                Game.spriteBatch.Draw(Game.Mira,
                    new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - 400, Game.GraphicsDevice.Viewport.Height / 2 - 300,
                        800, 600), Color.White);
                Game.spriteBatch.End();
                Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                Game.GraphicsDevice.BlendState = BlendState.Opaque;
            }
        }

        public void Update(GameTime gameTime)
        {
            Game.MainShip.Update(gameTime);
            Game.Camera.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.GameState = "PAUSE";
                Game.Exit();
            }
        }
    }
}