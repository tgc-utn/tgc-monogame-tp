using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Tanks;

public class TankPlayer : Tank
{
    public TankPlayer(TankReference model, Vector3 position, GraphicsDeviceManager graphicsDeviceManager) : base(model, position, graphicsDeviceManager)
    {
    }
    
    public override void Update(GameTime gameTime)
    {
        // var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        // KeySense();
        // ProcessMouse(elapsedTime);
        
        base.Update(gameTime);

        // TankHud.Update(World, health, shootTime);
    }
    
    public void KeySense()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // Avanzo
            Velocidad += Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            // Retrocedo
            Velocidad -= Acceleration;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // Giro izq
            Angle += RotationSpeed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            // Giro der
            Angle -= RotationSpeed;
        }
    }
    
    public void ProcessMouse(float elapsedTime)
    {
        var currentMouseState = Mouse.GetState();

        var mouseDelta = currentMouseState.Position.ToVector2() - pastMousePosition;
        mouseDelta *= MouseSensitivity * elapsedTime;
            
        var delta = Mouse.GetState().Position - _center;
        Mouse.SetPosition(_center.X, _center.Y);
        var rotationX = delta.X * _sensitivityX * elapsedTime;
        var rotationY = delta.Y * _sensitivityY * elapsedTime;

        // canion
        pitch = Math.Clamp(pitch - rotationY, -8f, 10f);

        // Torreta
        yaw = Math.Clamp(yaw - rotationX, -90.0f, 90.0f);

        UpdateRotations();

        pastMousePosition = Mouse.GetState().Position.ToVector2();
        
        // Disparo
        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !hasShot)
        {
            var bulletPosition = Position; //TODO por ahi es la position del cannon
            var yawRadians = MathHelper.ToRadians(yaw);
            var pitchRadians = MathHelper.ToRadians(pitch);
            var bulletDirection = Vector3.Transform(Vector3.Transform(cannonBone.Transform.Forward,Matrix.CreateFromYawPitchRoll(yawRadians,pitchRadians,0f)), Matrix.CreateRotationY(Angle));
            var bullet = new Bullet(BulletModel, BulletEffect, BulletReference,
                Matrix.CreateFromYawPitchRoll(yawRadians,-pitchRadians,0f), Matrix.CreateRotationY(Angle),
                bulletPosition, bulletDirection, 0.06f, 10000f);
            Bullets.Add(bullet);
            hasShot = true;
            shootTime = 0.25f;
        }
    }
}