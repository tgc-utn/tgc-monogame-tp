using TGC.MonoGame.InsaneGames.Entities;
using Microsoft.Xna.Framework;
using TGC.MonoGame.InsaneGames.Collectibles;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;
        private Collectible[] Collectibles;
        public Map(Room[] rooms, Enemy[] enemies, Collectible[] collectibles) 
        {
            Rooms = rooms;
            Enemies = enemies;
            Collectibles = collectibles;
        }

        public override void Initialize(TGCGame game)
        {
            base.Initialize(game);
            foreach (var room in Rooms)
                room.Initialize(game);

            foreach (var enemy in Enemies)
                enemy.Initialize(game);

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
    }
}