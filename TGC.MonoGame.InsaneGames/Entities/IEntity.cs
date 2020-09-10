using Microsoft.Xna.Framework;
namespace TGC.MonoGame.InsaneGames.Entities
{
    public interface IEntity
    {
        public void Load();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
        public void Unload();
    }
}