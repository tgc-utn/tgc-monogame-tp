using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Box : DrawableGameComponent
    {
        private Wall[] Walls;

        private BasicEffect Effect { get; set; }
        
        protected new TGCGame Game { get; }

        public Box(TGCGame game) : base(game)
        {
            Game = game;
        }

        public void Initialize(Vector3 size, Vector3 center)
        {
            Effect = new BasicEffect(GraphicsDevice);
            Effect.VertexColorEnabled = true;

            Vector2 floorSize = new Vector2(size.X, size.Z),
                    sideWallSize = new Vector2(size.Y, size.Z),
                    frontWallSize = new Vector2(size.X, size.Y);
            float xLength = size.X / 2,
                  yLength = size.Y / 2,
                  zLength = size.Z / 2;


            Walls = new Wall[] { 
                Wall.CreateFloor(Game, Effect, floorSize, new Vector3(center.X, center.Y - yLength, center.Z), Color.White),
                Wall.CreateSideWall(Game, Effect, sideWallSize, new Vector3(center.X + xLength, center.Y, 0), Color.Violet),
                Wall.CreateSideWall(Game, Effect, sideWallSize, new Vector3(center.X - xLength, center.Y, 0), Color.Violet),
                Wall.CreateFrontWall(Game, Effect, frontWallSize, new Vector3(0, center.Y, center.Z - zLength), Color.SkyBlue),
                Wall.CreateFrontWall(Game, Effect, frontWallSize, new Vector3(0, center.Y, center.Z + zLength), Color.SkyBlue),
                Wall.CreateFloor(Game, Effect, floorSize, new Vector3(center.X, center.Y + yLength, center.Z), Color.Yellow)
            };
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var wall in Walls)
            {
                wall.Draw(gameTime);
            }
        }
    }
}