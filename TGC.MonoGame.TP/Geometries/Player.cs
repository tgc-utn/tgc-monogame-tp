using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Geometries;

public class Player
{
    public const float Speed = 10.0f;

    public Vector3 position;
    // colision

    public Vector3 direction;

    public Player(Vector3 playerPosition)
    {
        position = playerPosition;
    }
}
