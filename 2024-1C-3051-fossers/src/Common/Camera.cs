using Microsoft.Xna.Framework;
using WarSteel.Entities;

namespace WarSteel.Common;

public class Camera
{
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }
    public Vector3 UpVector { get; private set; } = Vector3.Up;
    public Vector3 RelativePosition { get; }

    private const float defaultNearPlaneDistance = 0.1f;
    private const float defaultFarPlaneDistance = 1000f;
    private const float defaultFOV = MathHelper.PiOver2;

    public Camera(Vector3 initialPosition, float aspectRatio, float fov = defaultFOV, float nearPlaneDistance = defaultNearPlaneDistance, float farPlaneDistance = defaultFarPlaneDistance)
    {
        RelativePosition = initialPosition;
        Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
    }

    public void Follow(Entity entity)
    {
        Vector3 realPosition = Vector3.Transform(RelativePosition, entity.Transform.World);
        View = Matrix.CreateLookAt(realPosition, entity.Transform.Pos, UpVector);
    }
}
