using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;

public class Player
{
    public Vector3 SpherePosition;
    public float Yaw { get; private set; }
    private readonly Matrix _sphereScale;
    private float _pitch;
    private float _roll;
    private float _speed;
    private float _pitchSpeed; 
    private float _yawSpeed;
    private float _jumpSpeed;
    private bool _isJumping;
    private bool _onGround;
    public BoundingSphere BoundingSphere;

    public Player(Matrix sphereScale, Vector3 spherePosition, BoundingSphere boundingSphere)
    {
        _sphereScale = sphereScale;
        SpherePosition = spherePosition;
        BoundingSphere = boundingSphere;
    }

    public float MaxSpeed = 180f;
    private const float PitchMaxSpeed = 15f;
    private const float YawMaxSpeed = 5.8f;
    public float Acceleration = 60f;
    private const float PitchAcceleration = 5f;
    private const float YawAcceleration = 5f;
    private const float Gravity = 175f;
    public float MaxJumpHeight = 35f;

    public Matrix Update(float time, KeyboardState keyboardState)
    {
        HandleJumping(keyboardState);
        HandleFalling(time);
        HandleYaw(time, keyboardState);
        var rotationY = Matrix.CreateRotationY(Yaw);
        var forward = rotationY.Forward;
        HandleMovement(time, keyboardState, forward);
        var rotationX = Matrix.CreateRotationX(_pitch);
        var translation = Matrix.CreateTranslation(SpherePosition);
        return _sphereScale * rotationX * rotationY * translation;
    }

    private void HandleJumping(KeyboardState keyboardState)
    {
        if(keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
        {
            _isJumping = true;
            _jumpSpeed += CalculateJumpSpeed();
            Console.WriteLine("salto!");
        }
    }

    private void HandleFalling(float time)
    {
        if (!_onGround)
        {
            var newYPosition = CalculateFallPosition(time);
            SpherePosition = newYPosition;
        }
    }

    private void EndJump()
    {
        _isJumping = false;
        _jumpSpeed = 0;
    }
    
    private Vector3 CalculateFallPosition(float time)
    {
        _jumpSpeed -= Gravity * time;
        var newYPosition = SpherePosition.Y + _jumpSpeed * time;
        return new Vector3(SpherePosition.X, newYPosition, SpherePosition.Z);
    }

    private float CalculateJumpSpeed()
    {
        return (float)Math.Sqrt(2 * MaxJumpHeight * Math.Abs(Gravity));
    }

    private void HandleYaw(float time, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            _yawSpeed += YawAcceleration * time;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            _yawSpeed -= YawAcceleration * time;
        }
        else
        {
            DecelerateYaw(time);
        }

        _yawSpeed = MathHelper.Clamp(_yawSpeed, -YawMaxSpeed, YawMaxSpeed);
        Yaw += _yawSpeed * time;
    }

    private void DecelerateYaw(float time)
    {
        var yawDecelerationDirection = Math.Sign(_yawSpeed) * -1;
        _yawSpeed += YawAcceleration * time * yawDecelerationDirection;
    }

    private void HandleMovement(float time, KeyboardState keyboardState, Vector3 forward)
    {
        if (keyboardState.IsKeyDown(Keys.W))
        {
            _speed += Acceleration * time;
            _pitchSpeed -= PitchAcceleration * time;
        }
        else if (keyboardState.IsKeyDown(Keys.S))
        {
            _speed -= Acceleration * time;
            _pitchSpeed += PitchAcceleration * time;
        }
        else
        {
            var decelerationDirection = Math.Sign(_speed) * -1;
            var pitchDecelerationDirection = Math.Sign(_pitchSpeed) * -1;
            _speed += Acceleration * time * decelerationDirection;
            _pitchSpeed += PitchAcceleration * time * pitchDecelerationDirection;
        }

        _pitchSpeed = MathHelper.Clamp(_pitchSpeed, -PitchMaxSpeed, PitchMaxSpeed);
        _pitch += _pitchSpeed * time;
        _speed = MathHelper.Clamp(_speed, -MaxSpeed, MaxSpeed);
        SolveYCollisions();
        SpherePosition += forward * time * _speed;
        BoundingSphere.Center = SpherePosition;
    }

    private void SolveYCollisions()
    {
        _onGround = false;
        
        foreach(var collider in TGCGame.Colliders)
        {
            if (BoundingSphere.Intersects(collider) && _jumpSpeed <= 0)
            {
                SpherePosition.Y = collider.Max.Y + BoundingSphere.Radius;
                _onGround = true;
                EndJump();
                break;
            }
        }

        foreach (var orientedBoundingBox in TGCGame.OrientedColliders)
        {
            if (orientedBoundingBox.Intersects(BoundingSphere, out _, out var normal) && _jumpSpeed <= 0)
            {
                Console.WriteLine("colision rampa!");
                var rotationMatrix = orientedBoundingBox.Orientation;
                var movementDirection = Vector3.TransformNormal(normal, Matrix.Invert(rotationMatrix));
                var newPosition = SpherePosition + movementDirection;
                SpherePosition = newPosition;
                _onGround = true;
                EndJump();
                break;
            }
        }
    }
}