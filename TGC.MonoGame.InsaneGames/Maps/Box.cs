using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Box : Room
    {
        private Vector3 BottomVertex, UpVertex;
        private Dictionary<WallId, Wall> Walls = new Dictionary<WallId, Wall>();
        private SpawnableSpace SpawnSpace { get; set; }
        public Box(Dictionary<WallId, BasicEffect> effects, Vector3 size, Vector3 center, bool spawnable = true, Dictionary<WallId, (float, float)> textureRepeats = null)
        {
            Vector2 floorSize = new Vector2(size.X, size.Z),
                    sideWallSize = new Vector2(size.Y, size.Z),
                    frontWallSize = new Vector2(size.X, size.Y);
            float xLength = size.X / 2,
                  yLength = size.Y / 2,
                  zLength = size.Z / 2;
            
            textureRepeats ??= new Dictionary<WallId, (float, float)>();

            var allWalls = new Wall[] { 
                Wall.CreateFloor(floorSize, new Vector3(center.X, center.Y - yLength, center.Z)),
                Wall.CreateSideWall(sideWallSize, new Vector3(center.X + xLength, center.Y, center.Z), true),
                Wall.CreateSideWall(sideWallSize, new Vector3(center.X - xLength, center.Y, center.Z)),
                Wall.CreateFrontWall(frontWallSize, new Vector3(center.X, center.Y, center.Z - zLength)),
                Wall.CreateFrontWall(frontWallSize, new Vector3(center.X, center.Y, center.Z + zLength), back: true),
                Wall.CreateFloor(floorSize, new Vector3(center.X, center.Y + yLength, center.Z), ceiling: true)
            };
            
            (float, float) textureRepeat;
            foreach(var key in effects.Keys)
            {
                allWalls[(int)key].Effect = effects[key];
                if(textureRepeats.TryGetValue(key, out textureRepeat))
                    allWalls[(int)key].TextureRepeat = textureRepeat;
                Walls.Add(key, allWalls[(int)key]);
                
            }

            var spawnableArea = !spawnable ? new (Vector3, Vector3)[0] : new (Vector3, Vector3)[] {(size, center)};
            SpawnSpace = new SpawnableSpace(spawnableArea);
            
            Spawnable = spawnable;

            BottomVertex = new Vector3(center.X - xLength, center.Y - yLength, center.Z - zLength);
            UpVertex = new Vector3(center.X + xLength, center.Y + yLength, center.Z + zLength);
        }

        public override void Initialize(TGCGame game)
        {
            foreach (var wall in Walls.Values)
                wall.Initialize(game);
            
            base.Initialize(game);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var wall in Walls.Values)
                wall.Draw(gameTime);
        }

        public override SpawnableSpace SpawnableSpace()
        {
            return SpawnSpace;           
        }

        public override bool IsInRoom(Vector3 point)
        {
            return BottomVertex.X < point.X && point.X < UpVertex.X &&
                    BottomVertex.Y < point.Y && point.Y < UpVertex.Y &&
                    BottomVertex.Z < point.Z && point.Z < UpVertex.Z;
        }

        public override Wall CollidesWithWall(Vector3 lowerPoint, Vector3 higherPoint)
        {
            foreach(var wall in Walls.Values)
                if(wall.Collides(lowerPoint, higherPoint)) return wall;
            return null;
        }
    }
}