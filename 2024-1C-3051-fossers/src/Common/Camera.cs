using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Entities;

namespace WarSteel.Common;

public class Camera
{
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }
    public Vector3 UpVector { get; private set; } = Vector3.Up;
    public Vector3 RelativePosition { get; set; }
    public Quaternion orientation = Quaternion.Identity;

    private const float defaultNearPlaneDistance = 0.1f;
    private const float defaultFarPlaneDistance = 1000f;
    private const float defaultFOV = MathHelper.PiOver2;
    private const float rotationSpeed = 1f;
    private const float scalingSpeed = 1;

    private Entity FollowedEntity;

    public Camera(Vector3 initialPosition, float aspectRatio, float fov = defaultFOV, float nearPlaneDistance = defaultNearPlaneDistance, float farPlaneDistance = defaultFarPlaneDistance)
    {
        RelativePosition = initialPosition;
        Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
    }

    public void Follow(Entity entity)
    {
        FollowedEntity = entity;
    }

    public void Update(GameTime time)
    {

        float dt = (float)time.ElapsedGameTime.TotalSeconds;

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, dt * rotationSpeed) * orientation;

        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, -dt * rotationSpeed) * orientation;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Right, dt * rotationSpeed) * orientation;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Right, -dt * rotationSpeed) * orientation;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Q))
        {
            RelativePosition *= (1 + dt * scalingSpeed);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.E))
        {
            RelativePosition *= (1 - dt * scalingSpeed);
        }



        Vector3 realPosition = Vector3.Transform(Vector3.Transform(RelativePosition, Matrix.CreateFromQuaternion(orientation)), FollowedEntity.Transform.World);
        View = Matrix.CreateLookAt(realPosition, FollowedEntity.Transform.Pos, UpVector);
    }
}
