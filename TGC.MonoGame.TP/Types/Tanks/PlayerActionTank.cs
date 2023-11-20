using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Types.Tanks;

public class PlayerActionTank : ActionTank
{
    public Point _center;
    public float _sensitivityX = 0.0018f;
    public float _sensitivityY = 0.002f;
    public PlayerActionTank(bool isEnemy, GraphicsDeviceManager graphicsDevice)
    {
        _center = new Point(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2);
    }

    public override void Update(GameTime gameTime, Tank tank)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        KeySense(elapsedTime, tank);
        ProcessMouse(elapsedTime, tank);
        tank.TankHud.Update(tank.World, tank.health, tank.shootTime);
    }
    
    public void KeySense(float elapsedTime, Tank tank)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // Avanzo
            tank.Velocidad += tank.Acceleration;
            tank.LeftWheelRotation += elapsedTime * tank.Acceleration;
            tank.RightWheelRotation += elapsedTime * tank.Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            // Retrocedo
            tank.Velocidad -= tank.Acceleration;
            tank.LeftWheelRotation += elapsedTime * (-tank.Acceleration);
            tank.RightWheelRotation += elapsedTime * (-tank.Acceleration);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // Giro izq
            tank.Angle += tank.RotationSpeed;
            tank.LeftWheelRotation += elapsedTime * (-tank.Acceleration);
            tank.RightWheelRotation += elapsedTime * tank.Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            // Giro der
            tank.Angle -= tank.RotationSpeed;
            tank.LeftWheelRotation += elapsedTime * tank.Acceleration;
            tank.RightWheelRotation += elapsedTime * (-tank.Acceleration);
        }
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
            bullet.Speed += tank.Velocidad;
            tank.Bullets.Add(bullet);
            tank.hasShot = true;
            tank.shootTime = 0.25f;
            // Music
            var instance = tank.BulletSoundEffect.CreateInstance();
            //Quizas podriamos hacer que el sonido sea mas fuerte si el tanque esta mas cerca
            instance.Volume = 0.05f;
            instance.Play();
        }
    }
    public override void Respawn(Tank tank)
    {
    }
}