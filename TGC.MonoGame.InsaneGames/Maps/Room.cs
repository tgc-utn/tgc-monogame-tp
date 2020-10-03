using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Maps
{
    abstract class Room : IDrawable 
    {
        public bool Spawnable { get; protected set; }

        abstract public SpawnableSpace SpawnableSpace();

        abstract public bool IsInRoom(Vector3 center);
        abstract public Wall CollidesWithWall(Vector3 lowerPoint, Vector3 higherPoint);
    }
}