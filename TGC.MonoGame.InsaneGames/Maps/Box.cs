using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Box : Room
    {
        private Wall[] Walls;

        private SpawnableSpace SpawnSpace { get; set; }
        public Box(BasicEffect effect, Vector3 size, Vector3 center, WallId[] wallsToRemove = null, bool spawnable = true)
        {
            Vector2 floorSize = new Vector2(size.X, size.Z),
                    sideWallSize = new Vector2(size.Y, size.Z),
                    frontWallSize = new Vector2(size.X, size.Y);
            float xLength = size.X / 2,
                  yLength = size.Y / 2,
                  zLength = size.Z / 2;
            wallsToRemove ??= new WallId[0];

            var allWalls = new ArrayList() { 
                Wall.CreateFloor(effect, floorSize, new Vector3(center.X, center.Y - yLength, center.Z), Color.White),
                Wall.CreateSideWall(effect, sideWallSize, new Vector3(center.X + xLength, center.Y, center.Z), Color.Violet, true),
                Wall.CreateSideWall(effect, sideWallSize, new Vector3(center.X - xLength, center.Y, center.Z), Color.Violet),
                Wall.CreateFrontWall(effect, frontWallSize, new Vector3(center.X, center.Y, center.Z - zLength), Color.SkyBlue),
                Wall.CreateFrontWall(effect, frontWallSize, new Vector3(center.X, center.Y, center.Z + zLength), Color.SkyBlue, true),
                Wall.CreateFloor(effect, floorSize, new Vector3(center.X, center.Y + yLength, center.Z), Color.Yellow, true)
            };
            
            Array.Sort(wallsToRemove, (WallId m, WallId n) => n - m);
            Array.ForEach(wallsToRemove, (w) => allWalls.RemoveAt((int)w) );
            Walls = (Wall[]) allWalls.ToArray(typeof(Wall));

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