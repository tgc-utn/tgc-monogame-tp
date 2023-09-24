using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Cameras;

public class DebugCamera : Camera
{
    public readonly Vector3 DefaultWorldFrontVector = Vector3.Forward;
    public readonly Vector3 DefaultWorldUpVector = Vector3.Up;
    public float Speed { get; set; }
    public float Angle { get; set; }
    
    public DebugCamera(float aspectRatio, Vector3 position, float speed, float angle) : base(aspectRatio)
    {
        BuildView(position, speed, angle);
    }
    
    public DebugCamera(float aspectRatio, Vector3 position, float speed, float angle, float nearPlaneDistance,
        float farPlaneDistance) : base(aspectRatio, nearPlaneDistance, farPlaneDistance)
    {
        BuildView(position, speed, angle);
    }
    
    private void BuildView(Vector3 position, float speed, float angle)
    {
        Position = position;
        FrontDirection = DefaultWorldFrontVector;
        Speed = speed;
        Angle = angle;
        View = Matrix.CreateLookAt(Position, Position + FrontDirection, DefaultWorldUpVector);
    }
    
    public override void Update(GameTime gameTime, Matrix followedWorld) // no se usa el followed world
    {
        var keyboardState = Keyboard.GetState();
        var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        // Check for input to rotate the camera.
        var pitch = 0f;
        var turn = 0f;

        if (keyboardState.IsKeyDown(Keys.Up))
            pitch += time * Angle;

        if (keyboardState.IsKeyDown(Keys.Down))
            pitch -= time * Angle;

        if (keyboardState.IsKeyDown(Keys.Left))
            turn += time * Angle;

        if (keyboardState.IsKeyDown(Keys.Right))
            turn -= time * Angle;

        RightDirection = Vector3.Cross(DefaultWorldUpVector, FrontDirection);
        var flatFront = Vector3.Cross(RightDirection, DefaultWorldUpVector);

        var pitchMatrix = Matrix.CreateFromAxisAngle(RightDirection, pitch);
        var turnMatrix = Matrix.CreateFromAxisAngle(DefaultWorldUpVector, turn);

        var tiltedFront = Vector3.TransformNormal(FrontDirection, pitchMatrix * turnMatrix);

        // Check angle so we can't flip over.
        if (Vector3.Dot(tiltedFront, flatFront) > 0.001f) FrontDirection = Vector3.Normalize(tiltedFront);

        // Check for input to move the camera around.
        if (keyboardState.IsKeyDown(Keys.W))
            Position += FrontDirection * time * Speed;

        if (keyboardState.IsKeyDown(Keys.S))
            Position -= FrontDirection * time * Speed;

        if (keyboardState.IsKeyDown(Keys.A))
            Position += RightDirection * time * Speed;

        if (keyboardState.IsKeyDown(Keys.D))
            Position -= RightDirection * time * Speed;

        View = Matrix.CreateLookAt(Position, Position + FrontDirection, DefaultWorldUpVector);
    }
}