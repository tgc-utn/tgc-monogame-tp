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

    public Player(Matrix sphereScale, Vector3 spherePosition)
    {
        _sphereScale = sphereScale;
        SpherePosition = spherePosition;
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
        if (keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
        {
            _isJumping = true; 
            _jumpSpeed = (float)Math.Sqrt(2 * MaxJumpHeight * Math.Abs(Gravity)); 
        }
        if (_isJumping)
        {
            _jumpSpeed -= Gravity * time;
            var newYPosition = SpherePosition.Y + _jumpSpeed * time;

            if (newYPosition <= 0)
            {
                newYPosition = 0;
                _isJumping = false;
                _jumpSpeed = 0;
            }

            var newPosition = new Vector3(SpherePosition.X, newYPosition, SpherePosition.Z);
            SpherePosition = newPosition;
        }
            
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
            var yawDecelerationDirection = Math.Sign(_yawSpeed) * -1;
            _yawSpeed += YawAcceleration * time * yawDecelerationDirection;
        }
            
        _yawSpeed = MathHelper.Clamp(_yawSpeed, -YawMaxSpeed, YawMaxSpeed);
        Yaw += _yawSpeed * time;

        var rotationY = Matrix.CreateRotationY(Yaw);
        var forward = rotationY.Forward;

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
        _pitch += _pitchSpeed * time;
            
        var rotationX = Matrix.CreateRotationX(_pitch);
        var translation = Matrix.CreateTranslation(SpherePosition);
            
        return _sphereScale * rotationX * rotationY * translation;
    }
}