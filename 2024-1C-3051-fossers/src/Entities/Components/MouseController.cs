using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;

namespace WarSteel.Entities;

class MouseController : Component
{

    private float _sensitivity;

    private float _smoothing = 0.5f;

    private float _pitch = 0;
    private float _yaw = 0;

    private Vector2 _mousePosition;


    public MouseController(float sensitivity)
    {
        _sensitivity = sensitivity;
        _mousePosition = MousePosition() * _sensitivity;
    }


    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        Vector2 mouseDelta = MousePosition() * _sensitivity;

        Vector2 delta = (mouseDelta - _mousePosition) * (1 - _smoothing);
        _mousePosition += delta;

        float pitch = delta.Y;
        float yaw = delta.X;

        _pitch += pitch;
        _yaw += yaw;

        _pitch = MathHelper.Clamp(_pitch,-(float)Math.PI/2f,(float)Math.PI/2f);

        



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
