namespace TGC.MonoGame.InsaneGames.Maps
{
    abstract class Room : IDrawable 
    {
        public bool Spawnable { get; protected set; }

        abstract public SpawnableSpace SpawnableSpace();
    }
}