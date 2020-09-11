using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    abstract class Room : IDrawable
    {
        protected TGCGame Game { get; }
        protected Room(TGCGame game)
        {
            Game = game;
        }
        abstract public void Draw(GameTime gameTime);
        abstract public void Update(GameTime gameTime);
        abstract public void Load();
        abstract public void Initialize(Vector3 size, Vector3 center);
    }
}