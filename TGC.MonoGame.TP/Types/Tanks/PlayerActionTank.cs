using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Types.Tanks;

public class PlayerActionTank : ActionTank
{
    public Point _center;
    public float _sensitivityX = 0.0018f;
    public float _sensitivityY = 0.002f;
    public PlayerActionTank(int team, GraphicsDeviceManager graphicsDevice) : base(team)
    {
        _center = new Point(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2);
    }

    public override void Update(GameTime gameTime, Tank tank)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        KeySense(tank);
        ProcessMouse(elapsedTime, tank);
    }
    
    public void KeySense(Tank tank)
    {
        // Avanzo
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            tank.Velocidad += tank.Acceleration;

        // Retrocedo
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            tank.Velocidad -= tank.Acceleration;

        // Giro izq
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            tank.Angle += tank.RotationSpeed;

        // Giro der
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            tank.Angle -= tank.RotationSpeed;
    }
    
    public void ProcessMouse(float elapsedTime, Tank tank)
    {
        var delta = Mouse.GetState().Position - _center;
        Mouse.SetPosition(_center.X, _center.Y);
        var rotationX = delta.X * _sensitivityX * elapsedTime;
        var rotationY = delta.Y * _sensitivityY * elapsedTime;

        // canion
        tank.pitch = Math.Clamp(tank.pitch - rotationY, -8f, 10f);

        // Torreta
        tank.yaw = Math.Clamp(tank.yaw - rotationX, -90.0f, 90.0f);

        tank.UpdateRotations();
        
        // Disparo
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !tank.hasShot)
        {
            var bulletPosition = tank.Position; //TODO por ahi es la position del cannon
            var yawRadians = MathHelper.ToRadians(tank.yaw);
            var pitchRadians = MathHelper.ToRadians(tank.pitch);
            var bulletDirection = Vector3.Transform(
                Vector3.Transform(
                            tank.cannonBone.Transform.Forward,
                            Matrix.CreateFromYawPitchRoll(yawRadians, pitchRadians,0f)
                        ),
                        Matrix.CreateRotationY(tank.Angle));
            var bullet = new Bullet(
                tank.BulletModel,
                tank.BulletEffect,
                tank.BulletReference,
                Matrix.CreateFromYawPitchRoll(yawRadians,-pitchRadians,0f),
                Matrix.CreateRotationY(tank.Angle),
                bulletPosition,
                bulletDirection);
            tank.Bullets.Add(bullet);
            tank.hasShot = true;
            tank.shootTime = 0.25f;
        }
    }
}