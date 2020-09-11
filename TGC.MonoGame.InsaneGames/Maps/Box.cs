using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Box : Room
    {
        private Wall[] Walls;
        public Box(BasicEffect effect, Vector3 size, Vector3 center)
        {
            Vector2 floorSize = new Vector2(size.X, size.Z),
                    sideWallSize = new Vector2(size.Y, size.Z),
                    frontWallSize = new Vector2(size.X, size.Y);
            float xLength = size.X / 2,
                  yLength = size.Y / 2,
                  zLength = size.Z / 2;


            Walls = new Wall[] { 
                Wall.CreateFloor(effect, floorSize, new Vector3(center.X, center.Y - yLength, center.Z), Color.White),
                Wall.CreateSideWall(effect, sideWallSize, new Vector3(center.X + xLength, center.Y, 0), Color.Violet),
                Wall.CreateSideWall(effect, sideWallSize, new Vector3(center.X - xLength, center.Y, 0), Color.Violet),
                Wall.CreateFrontWall(effect, frontWallSize, new Vector3(0, center.Y, center.Z - zLength), Color.SkyBlue),
                Wall.CreateFrontWall(effect, frontWallSize, new Vector3(0, center.Y, center.Z + zLength), Color.SkyBlue),
                Wall.CreateFloor(effect, floorSize, new Vector3(center.X, center.Y + yLength, center.Z), Color.Yellow)
            };
        }

        public override void Initialize(TGCGame game)
        {
            foreach (var wall in Walls)
                wall.Initialize(game);
            
            base.Initialize(game);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var wall in Walls)
                wall.Draw(gameTime);
        }
    }
}