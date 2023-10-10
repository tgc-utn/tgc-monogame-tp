using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP;

public class MovingPlatform
{
    public Matrix World;
    public Vector3 Position;
    public Vector3 PreviousPosition;
    public BoundingBox MovingBoundingBox;
    private Vector3 _direction = Vector3.Forward;
    private readonly Vector3 _scale;

    private const float MaxHorizontalSpeed = 1.3f;

    public MovingPlatform(Matrix world, Vector3 scale, Vector3 position, BoundingBox movingBoundingBox)
    {
        World = world;
        Position = position;
        MovingBoundingBox = movingBoundingBox;
        _scale = scale;
    }

    public void Update()
    {
        SolveXCollisions();
        var increment = _direction * MaxHorizontalSpeed;
        PreviousPosition = Position;
        Position += increment;
        MovingBoundingBox = new BoundingBox(MovingBoundingBox.Min + increment, MovingBoundingBox.Max + increment);
        World = Matrix.CreateScale(_scale) * Matrix.CreateTranslation(Position);
    }

    private void SolveXCollisions()
    {
        foreach (var boundingBox in Prefab.PlatformAbb)
        {
            if (!MovingBoundingBox.Intersects(boundingBox)) continue;
            _direction *= -1;
            break;

        }
    }
}