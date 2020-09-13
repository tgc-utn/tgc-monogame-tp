using System;
using TGC.MonoGame.InsaneGames.Entities;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;

        private Random Random;
        public Map(Room[] rooms, Enemy[] enemies) 
        {
            Rooms = rooms;
            Enemies = enemies;
            Random = new Random();
        }

        public override void Initialize(TGCGame game)
        {
            base.Initialize(game);
            foreach (var room in Rooms)
                room.Initialize(game);

            Array.ForEach(Enemies, (enemy) => {
                enemy.Initialize(game);
                while(true)
                {
                    var room = Rooms[Random.Next(0, Rooms.Length)];
                    if(!room.Spawnable)
                        continue;
                    var spawn = room.SpawnableSpace().GetSpawnPoint(enemy.floorEnemy);
                    enemy.position ??= Matrix.CreateTranslation(spawn);
                    break;
                }
            });
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var room in Rooms)
                room.Draw(gameTime);

            foreach (var enemy in Enemies)
                enemy.Draw(gameTime);
        }
        public override void Load()
        {
            foreach (var room in Rooms)
                room.Load();

            foreach (var enemy in Enemies)
                enemy.Load();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var room in Rooms)
                room.Update(gameTime);

            foreach (var enemy in Enemies)
                enemy.Update(gameTime);
        }
    }
}