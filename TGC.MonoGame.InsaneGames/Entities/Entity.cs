using Microsoft.Xna.Framework;
namespace TGC.MonoGame.InsaneGames.Entities
{
    abstract class Entity : IDrawable 
    {
        public Matrix? position { get; set; }
    }
}