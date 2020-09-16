using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Box : Room
    {
        private Wall[] Walls;

        private SpawnableSpace SpawnSpace { get; set; }
        public Box(Dictionary<WallId, BasicEffect> effects, Vector3 size, Vector3 center, bool spawnable = true)
        {
            Vector2 floorSize = new Vector2(size.X, size.Z),
                    sideWallSize = new Vector2(size.Y, size.Z),
                    frontWallSize = new Vector2(size.X, size.Y);
            float xLength = size.X / 2,
                  yLength = size.Y / 2,
                  zLength = size.Z / 2;

            var allWalls = new Wall[] { 
                Wall.CreateFloor(floorSize, new Vector3(center.X, center.Y - yLength, center.Z)),
                Wall.CreateSideWall(sideWallSize, new Vector3(center.X + xLength, center.Y, center.Z), true),
                Wall.CreateSideWall(sideWallSize, new Vector3(center.X - xLength, center.Y, center.Z)),
                Wall.CreateFrontWall(frontWallSize, new Vector3(center.X, center.Y, center.Z - zLength)),
                Wall.CreateFrontWall(frontWallSize, new Vector3(center.X, center.Y, center.Z + zLength), true),
                Wall.CreateFloor(floorSize, new Vector3(center.X, center.Y + yLength, center.Z), true)
            };
            
            Wall[] WallsToKeep = new Wall[effects.Keys.Count];
            int i = 0;
            foreach(var key in effects.Keys)
            {
                allWalls[(int)key].Effect = effects[key];
                WallsToKeep[i] = allWalls[(int)key];
                i++;
            }
            Walls = WallsToKeep;

            var spawnableArea = !spawnable ? new (Vector3, Vector3)[0] : new (Vector3, Vector3)[] {(size, center)};
            SpawnSpace = new SpawnableSpace(spawnableArea);
            
            Spawnable = spawnable;
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

        public override SpawnableSpace SpawnableSpace()
        {
            return SpawnSpace;           
        }
    }
}