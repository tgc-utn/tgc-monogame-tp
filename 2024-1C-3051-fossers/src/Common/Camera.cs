using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Entities;
using WarSteel.Scenes;

namespace WarSteel.Common;

public class Camera : Entity
{
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }
    private Entity FollowedEntity;

    private const float defaultNearPlaneDistance = 0.1f;
    private const float defaultFarPlaneDistance = 1000f;
    private const float defaultFOV = MathHelper.PiOver2;

    public Camera(Vector3 initialPosition, float aspectRatio, float fov = defaultFOV, float nearPlaneDistance = defaultNearPlaneDistance, float farPlaneDistance = defaultFarPlaneDistance) : base("camera", Array.Empty<string>(), new Transform(), Array.Empty<Component>())
    {
        Transform.Pos = initialPosition;
        Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
    }

    public void Follow(Entity entity)
    {
        FollowedEntity = entity;
    }

    public override void Update(GameTime time, Scene scene)
    {
        base.Update(time, scene);
        Transform.Pos = Vector3.Transform(Transform.Pos, FollowedEntity.Transform.GetWorld());
        View = Matrix.CreateLookAt(Transform.Pos, FollowedEntity.Transform.Pos, Transform.GetWorld().Up);
    }
}
