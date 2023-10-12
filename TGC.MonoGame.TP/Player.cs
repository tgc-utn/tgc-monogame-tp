using System;
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Collisions;

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

    public Player(Matrix sphereScale, Vector3 spherePosition, BoundingSphere boundingSphere, float yaw)
    {
        _sphereScale = sphereScale;
        SpherePosition = spherePosition;
        BoundingSphere = boundingSphere;
        Yaw = yaw;
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
        var translation = Matrix.CreateTranslation(BoundingSphere.Center);
        RestartPosition(keyboardState);
        Console.WriteLine(_onGround);
        return _sphereScale * rotationX * rotationY * translation;
    }

    private void RestartPosition(KeyboardState keyboardState)
    {
        if (!(BoundingSphere.Center.Y <= -150f) && !keyboardState.IsKeyDown(Keys.R)) return;
        BoundingSphere.Center = TGCGame.InitialSpherePosition; // TODO: checkpoint
        Yaw = TGCGame.InitialSphereYaw;
        SetSpeedToZero();
    }

    private void SetSpeedToZero()
    {
        _pitchSpeed = 0;
        _speed = 0;
        _jumpSpeed = 0;
    }

    private void HandleJumping(KeyboardState keyboardState)
    {
        if(keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
        {
            StartJump();
        }
    }

    private void HandleFalling(float time)
    {
        if (_onGround) return;
        var newYPosition = CalculateFallPosition(time);
        BoundingSphere.Center = newYPosition;
    }
    
    private void StartJump()
    {
        _isJumping = true;
        _jumpSpeed += CalculateJumpSpeed();
    }

    private void EndJump()
    {
        _isJumping = false;
        _jumpSpeed = 0;
    }
    
    private Vector3 CalculateFallPosition(float time)
    {
        _jumpSpeed -= Gravity * time;
        var newYPosition = BoundingSphere.Center.Y + _jumpSpeed * time;
        return new Vector3(BoundingSphere.Center.X, newYPosition, BoundingSphere.Center.Z);
    }

    private float CalculateJumpSpeed()
    {
        return (float)Math.Sqrt(2 * MaxJumpHeight * Math.Abs(Gravity));
    }

    private void HandleYaw(float time, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            AccelerateYaw(YawAcceleration, time);
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            AccelerateYaw(-YawAcceleration, time);
        }
        else
        {
            DecelerateYaw(time);
        }

        AdjustYawSpeed(time);
    }

    private void AdjustYawSpeed(float time)
    {
        _yawSpeed = MathHelper.Clamp(_yawSpeed, -YawMaxSpeed, YawMaxSpeed);
        Yaw += _yawSpeed * time;
    }

    private void AccelerateYaw(float yawAcceleration, float time)
    {
        _yawSpeed += yawAcceleration * time;
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
            Accelerate(Acceleration, time);
            AcceleratePitch(PitchAcceleration, time);
        }
        else if (keyboardState.IsKeyDown(Keys.S))
        {
            Accelerate(-Acceleration, time);
            AcceleratePitch(-PitchAcceleration, time);
        }
        else
        {
            Decelerate(time);
            DeceleratePitch(time);
        }

        AdjustPitchSpeed(time);
        AdjustSpeed(time, forward);
        SolveYCollisions();
        UpdateSpherePosition(BoundingSphere.Center);
    }

    private void UpdateSpherePosition(Vector3 newPosition)
    {
        SpherePosition = newPosition;
    }

    private void AdjustSpeed(float time, Vector3 forward)
    {
        _speed = MathHelper.Clamp(_speed, -MaxSpeed, MaxSpeed);
        BoundingSphere.Center += forward * time * _speed;
    }

    private void AdjustPitchSpeed(float time)
    {
        _pitchSpeed = MathHelper.Clamp(_pitchSpeed, -PitchMaxSpeed, PitchMaxSpeed);
        _pitch += _pitchSpeed * time;
    }

    private void AcceleratePitch(float pitchAcceleration,float time)
    {
        _pitchSpeed -= pitchAcceleration * time;
    }
    
    private void DeceleratePitch(float time)
    {
        var pitchDecelerationDirection = Math.Sign(_pitchSpeed) * -1;
        _pitchSpeed += PitchAcceleration * time * pitchDecelerationDirection;
    }

    private void Accelerate(float acceleration, float time)
    {
        _speed += acceleration * time;
    }

    private void Decelerate(float time)
    {
        var decelerationDirection = Math.Sign(_speed) * -1;
        _speed += Acceleration * time * decelerationDirection;
    }

    private void SolveYCollisions()
    {
        var sphereCenter = BoundingSphere.Center;
        var radius = BoundingSphere.Radius;
        
        _onGround = false;
        
        foreach(var collider in Prefab.PlatformAabb)
        {
            if (!collider.Intersects(BoundingSphere) || !(_jumpSpeed <= 0f)) continue;
            
            var closestPoint = BoundingVolumesExtensions.ClosestPoint(collider, BoundingSphere.Center);
            var distance = Vector3.Distance(closestPoint, BoundingSphere.Center);

            if (!(distance <= BoundingSphere.Radius)) continue;
            var newPosition = SolveCollisionPosition(sphereCenter, closestPoint, radius, distance);
            BoundingSphere.Center = newPosition;
            _onGround = true;
            EndJump();
        }

        foreach (var movingPlatform in Prefab.MovingPlatforms)
        {
            var collider = movingPlatform.MovingBoundingBox; 
            
            if (!collider.Intersects(BoundingSphere) || !(_jumpSpeed <= 0f)) continue;
            
            var closestPoint = BoundingVolumesExtensions.ClosestPoint(collider, BoundingSphere.Center);
            var distance = Vector3.Distance(closestPoint, BoundingSphere.Center);

            if (!(distance <= BoundingSphere.Radius)) continue;
            var platformMovement = movingPlatform.Position - movingPlatform.PreviousPosition;
            var newPosition = SolveCollisionPosition(sphereCenter, closestPoint, radius, distance);
            BoundingSphere.Center = newPosition;
            BoundingSphere.Center += platformMovement;
            _onGround = true;
            EndJump();

        }

        foreach (var orientedBoundingBox in Prefab.RampObb)
        {
            if (!orientedBoundingBox.Intersects(BoundingSphere, out _, out _) || !(_jumpSpeed <= 0f)) continue;
            
            var closestPoint = orientedBoundingBox.ClosestPoint(BoundingSphere.Center);
            var distance = Vector3.Distance(closestPoint, BoundingSphere.Center);

            if (!(distance <= BoundingSphere.Radius)) continue;
            var newPosition = SolveCollisionPosition(sphereCenter, closestPoint, radius, distance);
            BoundingSphere.Center = newPosition;
            _onGround = true;
            EndJump();
        }
    }

    private static Vector3 SolveCollisionPosition(Vector3 currentPosition, Vector3 closestPoint, float radius, float distance)
    {
        var penetration = radius - distance;
        var direction = Vector3.Normalize(currentPosition - closestPoint);
        return currentPosition + direction * penetration;
    }
}