using System;
using System.Collections.Generic;
using System.Linq;
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
    private bool _colliding = false;
    private BoundingSphere _boundingSphere;

    public Player(Matrix sphereScale, Vector3 spherePosition, BoundingSphere boundingSphere)
    {
        _sphereScale = sphereScale;
        SpherePosition = spherePosition;
        _boundingSphere = boundingSphere;
    }

    public float MaxSpeed = 180f;
    private const float PitchMaxSpeed = 15f;
    private const float YawMaxSpeed = 5.8f;
    public float Acceleration = 60f;
    private const float PitchAcceleration = 5f;
    private const float YawAcceleration = 5f;
    private const float Gravity = 175f;
    public float MaxJumpHeight = 35f;

    public Matrix Update(float time, KeyboardState keyboardState, BoundingBox[] colliders)
    {
        _colliding = colliders.Any( b => _boundingSphere.Intersects(b));
        
        SpherePosition = CalculateFallPosition(time);
        
        HandleJumping(time, keyboardState, colliders);
        
        HandleYaw(time, keyboardState);
        
        var rotationY = Matrix.CreateRotationY(Yaw);
        var forward = rotationY.Forward;
        
        HandleMovement(time, keyboardState, colliders, forward);
        
        var rotationX = Matrix.CreateRotationX(_pitch);
        var translation = Matrix.CreateTranslation(SpherePosition);
      
        return _sphereScale * rotationX * rotationY * translation;
    }

    private void HandleJumping(float time, KeyboardState keyboardState, BoundingBox[] colliders)
    {
        if (keyboardState.IsKeyDown(Keys.Space) && _onGround)
        {
            StartJump();
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
        
        var newPosition = new Vector3(SpherePosition.X, newYPosition, SpherePosition.Z);
        Console.WriteLine(_jumpSpeed);
        
        return newPosition;
    }

    private void StartJump()
    {
        _jumpSpeed = CalculateJumpSpeed();
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

    private void HandleMovement(float time, KeyboardState keyboardState, BoundingBox[] colliders, Vector3 forward)
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
        _speed = MathHelper.Clamp(_speed, -MaxSpeed, MaxSpeed);
        SpherePosition += forward * time * _speed;

        SpherePosition = SolveYCollisions(SpherePosition, colliders);
        
        _boundingSphere.Center = SpherePosition;

        _pitch += _pitchSpeed * time;
    }

    private Vector3 SolveYCollisions(Vector3 speedVector, BoundingBox[] colliders)
    {
        int index = 0;
        for (; index < colliders.Length; index++)
        {
            if (_boundingSphere.Intersects(colliders[index]) && _jumpSpeed < 0)
            {
                speedVector = new Vector3(speedVector.X, colliders[index].Max.Y + _boundingSphere.Radius, speedVector.Z);
                _onGround = true;
            }
        }

        for (int i = 0; i < TGCGame.OrientedColliders.Length; i++)
        {
            if (TGCGame.OrientedColliders[i].Intersects(_boundingSphere))
            {
                
            }
        }
        
        return speedVector;
    }
}