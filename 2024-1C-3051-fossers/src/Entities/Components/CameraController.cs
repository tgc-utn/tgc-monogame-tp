using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Scenes;

public class CameraController : IComponent
{

    public Transform _transform;
    public Vector3 _offset = new Vector3(0, 600, -800);
    public float _smoothSpeed = 0.125f;
    public float _rotationSpeed = 1f;

    private float _currentYaw = 0f;
    private float _currentPitch = 10f;

    private float _verticalOffset = 500;

    private MouseState _previousMouseState;

    public CameraController(Transform transform)
    {
        _transform = transform;
    }

    public void Initialize(Entity self, Scene scene)
    {
        self.Transform.Position = _transform.Position + _offset;
        self.Transform.Orientation = Quaternion.CreateFromYawPitchRoll(_currentPitch, _currentYaw, 0);
        _previousMouseState = Mouse.GetState();
    }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        MouseState state = Mouse.GetState();

        float deltaX = state.X - _previousMouseState.X; 
        float deltaY = state.Y - _previousMouseState.Y;

        _currentYaw -= deltaX * _rotationSpeed * dt;
        _currentPitch -= deltaY * _rotationSpeed * dt;

        _currentPitch = Math.Clamp(_currentPitch, -40, 80);

        Quaternion rotation = Quaternion.CreateFromYawPitchRoll(_currentYaw,_currentPitch, 0);
        Vector3 desiredPosition = _transform.AbsolutePosition + Vector3.Transform(_offset, Matrix.CreateFromQuaternion(rotation));


        Vector3 smoothedPosition = Vector3.Lerp(self.Transform.Position, desiredPosition, _smoothSpeed);
        self.Transform.Position = smoothedPosition;

        self.Transform.LookAt(_transform.AbsolutePosition + Vector3.Up * _verticalOffset);

        _previousMouseState = state;
    }

    public void Destroy(Entity self, Scene scene) { }


}