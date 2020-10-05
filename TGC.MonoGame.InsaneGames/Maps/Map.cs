using System;
using TGC.MonoGame.InsaneGames.Entities.Collectibles;
using TGC.MonoGame.InsaneGames.Entities.Enemies;
using TGC.MonoGame.InsaneGames.Entities;
using Microsoft.Xna.Framework;
using TGC.MonoGame.InsaneGames.Obstacles;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;
        private Random Random;
        private Collectible[] Collectibles;
        private Obstacle[] Obstacles;
        private Player Player;

        public Map(Room[] rooms, Enemy[] enemies, Collectible[] collectibles, Obstacle[] obstacles, Player player) 
        {
            Rooms = rooms;
            Enemies = enemies;
            Random = new Random();
            Collectibles = collectibles;
            Obstacles = obstacles;
            Player = player;
        }

        public override void Initialize(TGCGame game)
        {
            base.Initialize(game);

            Player.Initialize(game);

            foreach (var room in Rooms)
                room.Initialize(game);

            Array.ForEach(Enemies, (enemy) => {
                enemy.Initialize(game);
                while(true)
                {
                    if(!(enemy.position is null)) break;

                    var room = Rooms[Random.Next(0, Rooms.Length)];
                    if(!room.Spawnable)
                        continue;
                    var spawn = room.SpawnableSpace().GetSpawnPoint(enemy.floorEnemy);
                    
                    if(room.CollidesWithWall(enemy.BottomVertex + spawn, enemy.UpVertex + spawn) is null)
                        enemy.position = Matrix.CreateTranslation(spawn);
                    else
                        continue;
                    break;
                }
            });

            foreach (var collectible in Collectibles)
                collectible.Initialize(game);

            foreach (var obstacle in Obstacles)
                obstacle.Initialize(game);
        }

        public override void Draw(GameTime gameTime)
        {
            Player.Draw(gameTime);

            foreach (var room in Rooms)
                room.Draw(gameTime);

            foreach (var enemy in Enemies)
                enemy.Draw(gameTime);

            foreach (var collectible in Collectibles)
                collectible.Draw(gameTime);
            
            foreach (var obstacle in Obstacles)
                obstacle.Draw(gameTime);
        }
        public override void Load()
        {
            Player.Load();

            foreach (var room in Rooms)
                room.Load();

            foreach (var enemy in Enemies)
                enemy.Load();

            foreach (var collectible in Collectibles)
                collectible.Load();

            foreach (var obstacle in Obstacles)
                obstacle.Load();
        }

        public override void Update(GameTime gameTime)
        {
            Player.Update(gameTime);

            foreach (var room in Rooms)
                room.Update(gameTime);

            foreach (var enemy in Enemies)
                enemy.Update(gameTime);
        }
    }
}