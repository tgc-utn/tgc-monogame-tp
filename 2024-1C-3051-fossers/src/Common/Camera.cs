using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities;
using WarSteel.Scenes;

namespace WarSteel.Common;

public class Camera : Entity
{
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }
    private Entity FollowedEntity;

    private GraphicsDevice _device;

    private const float defaultNearPlaneDistance = 0.1f;
    private const float defaultFarPlaneDistance = 1000f;
    private const float defaultFOV = MathHelper.PiOver2;

    public Camera(Vector3 initialPosition, float aspectRatio,GraphicsDevice device, float fov = defaultFOV, float nearPlaneDistance = defaultNearPlaneDistance, float farPlaneDistance = defaultFarPlaneDistance) : base("camera", Array.Empty<string>(), new Transform(), new Dictionary<Type, IComponent>())
    {
        Transform.Position = initialPosition;
        Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance, farPlaneDistance);
        _device = device;
    }


    public override void Update(GameTime time, Scene scene)
    {
        base.Update(time, scene);
        View = Matrix.CreateLookAt(Transform.AbsolutePosition, Transform.AbsolutePosition + Transform.Forward * 10, Vector3.Up);
    }

}
