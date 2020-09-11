using TGC.MonoGame.InsaneGames.Entities;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    class Map : IDrawable
    {
        private Room[] Rooms;
        private Enemy[] Enemies;
        private TGCGame Game;
        public Map(TGCGame game) 
        {
            Game = game;
        }

        public void Initialize(Room[] rooms, Enemy[] enemies)
        {
            Rooms = rooms;
            Enemies = enemies;
        }
        public void Draw(GameTime gameTime)
        {
            foreach (var room in Rooms)
                room.Draw(gameTime);

            foreach (var enemy in Enemies)
                enemy.Draw(gameTime);
        }
        public void Load()
        {
            foreach (var room in Rooms)
                room.Load();

            foreach (var enemy in Enemies)
                enemy.Load();
        }

    }
}