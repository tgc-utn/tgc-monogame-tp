using System;
using TGC.MonoGame.InsaneGames.Entities.Collectibles;
using TGC.MonoGame.InsaneGames.Entities.Enemies;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;
        private Random Random;
        private Collectible[] Collectibles;
        public Map(Room[] rooms, Enemy[] enemies, Collectible[] collectibles) 
        {
            Rooms = rooms;
            Enemies = enemies;
            Random = new Random();
            Collectibles = collectibles;
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

            foreach (var collectible in Collectibles)
                collectible.Initialize(game);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var room in Rooms)
                room.Draw(gameTime);

            foreach (var enemy in Enemies)
                enemy.Draw(gameTime);

            foreach (var collectible in Collectibles)
                collectible.Draw(gameTime);
        }
        public override void Load()
        {
            foreach (var room in Rooms)
                room.Load();

            foreach (var enemy in Enemies)
                enemy.Load();

            foreach (var collectible in Collectibles)
                collectible.Load();
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