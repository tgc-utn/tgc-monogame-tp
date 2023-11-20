using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.Tanks;

namespace TGC.MonoGame.TP.Cameras;

public class AngularCamera : Camera
{
    public readonly Vector3 DefaultWorldUpVector = Vector3.Up;
    
    public Vector3 Direction { get; set; }
    
    public float Speed { get; set; }
    public float Angle { get; set; }
    
    public AngularCamera(float aspectRatio, Vector3 position, Vector3 targetPosition, float angle) : base(aspectRatio)
    {
        BuildView(position, targetPosition, angle);
    }

    public AngularCamera(float aspectRatio, Vector3 position, Vector3 targetPosition, float angle, float nearPlaneDistance,
        float farPlaneDistance) : base(aspectRatio, nearPlaneDistance, farPlaneDistance)
    {
        BuildView(position, targetPosition, angle);
    }
    
    private void BuildView(Vector3 position, Vector3 targetPosition, float angle)
    {
        var tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(angle));
        var orbitalPosition = tankFrontDirection * 12.5f;
        var upDistance = Vector3.Up * 1f;

        Position = targetPosition + orbitalPosition + upDistance;
        
        FrontDirection = Vector3.Normalize(targetPosition - Position);
        RightDirection = Vector3.Normalize(Vector3.Cross(DefaultWorldUpVector, FrontDirection));
        UpDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, RightDirection));
        View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
    }

    public override void Update(GameTime gameTime, Tank player) {}
}