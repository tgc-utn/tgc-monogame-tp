using TGC.MonoGame.InsaneGames.Entities;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;
        public Map(Room[] rooms, Enemy[] enemies) 
        {
            Rooms = rooms;
            Enemies = enemies;
        }

        public override void Initialize(TGCGame game)
        {
            base.Initialize(game);
            foreach (var room in Rooms)
                room.Initialize(game);

            foreach (var enemy in Enemies)
                enemy.Initialize(game);
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
    }
}