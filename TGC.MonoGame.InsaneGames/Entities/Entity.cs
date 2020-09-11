using Microsoft.Xna.Framework;
namespace TGC.MonoGame.InsaneGames.Entities
{
    public abstract class Entity : IDrawable 
    {
        protected TGCGame Game { get; }

        protected Entity(TGCGame game) 
        {
            Game = game;
        }

        abstract public void Draw(GameTime gameTime);
        abstract public void Update(GameTime gameTime);
        abstract public void Load();

    }
}