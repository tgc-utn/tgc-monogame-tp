using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class MouseController : IComponent
{

    private float _sensitivity;

    private float _smoothing = 0.5f;

    private float _pitch = 0;
    private float _yaw = MathHelper.PiOver2;

    public float Pitch
    {
        get => _pitch;
    }
    public float Yaw
    {
        get => _yaw;
    }

    private Vector2 _mousePosition;

    public MouseController(float sensitivity)
    {
        _sensitivity = sensitivity;
        _mousePosition = MousePosition() * _sensitivity;
    }

    public void Initialize(Entity self, Scene scene) { }

    public void Destroy(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        Vector2 mouseDelta = MousePosition() * _sensitivity;

        Vector2 delta = (mouseDelta - _mousePosition) * (1 - _smoothing);
        _mousePosition += delta;

        float pitch = delta.Y;
        float yaw = delta.X;

        _pitch += pitch;
        _yaw += yaw;

        _pitch = MathHelper.Clamp(_pitch, 0.01f, MathHelper.PiOver2 - 0.05f);

        float radius = self.Transform.Pos.Length();

        Vector3 newPosition = new Vector3(
            radius * (float)(Math.Sin(_pitch) * Math.Cos(_yaw)),
            radius * (float)Math.Cos(_pitch),
            radius * (float)(Math.Sin(_pitch) * Math.Sin(_yaw))
        );

        self.Transform.Pos = newPosition;
    }



    private Vector2 MousePosition()
    {
        MouseState mouseState = Mouse.GetState();
        return mouseState.Position.ToVector2();
    }

}
