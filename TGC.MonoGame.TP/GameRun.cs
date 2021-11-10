using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;
using Microsoft.Xna.Framework.Media;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NumericVector3 = System.Numerics.Vector3;
namespace TGC.MonoGame.TP
{
    internal class GameRun
    {
        private TGCGame Game;
        private Simulation Simulation { get; set; }
        private BufferPool BufferPool { get; set; }
        private float time;
        public GameRun(TGCGame game)
        {
            Game = game;
            time = 0;
            
            BufferPool = new BufferPool();
            Simulation = Simulation.Create(BufferPool, new NarrowPhaseCallbacks(),
                new PoseIntegratorCallbacks(new NumericVector3(0, -100, 0)), new PositionFirstTimestepper());
            Simulation.Statics.Add(new StaticDescription(new NumericVector3(0, -0.5f, 0),
                new CollidableDescription(Simulation.Shapes.Add(new Box(2000, 1, 2000)), 0.1f)));
            
            /*var boxIndex = Simulation.Shapes.Add(boxShape);
            var position = new NumericVector3(-30 + i * 10 + 1, j * 10 + 1, -40);

            var bodyDescription = BodyDescription.CreateDynamic(position, boxInertia,
                new CollidableDescription(boxIndex, 0.1f), new BodyActivityDescription(0.01f));

            var bodyHandle = Simulation.Bodies.Add(bodyDescription);*/
        }


        public void Draw(GameTime gameTime)
        {
            
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.MainShip.Draw();
            for (int eShip = 0; eShip < Game.CountEnemyShip; eShip++)
            {
                Game.EnemyShips[eShip].Draw();
            }

            
            
            Game.Rock.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-800, 20, 0), Game.Camera.View, Game.Camera.Projection);
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