using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Utils;

namespace WarSteel.Scenes.Main;

public class PlayerControls : IComponent
{
    DynamicBody rb;
    float Damage = 100;
    float BulletForce = 36000;
    bool IsReloading = false;
    int ReloadingTimeInMs = 100;

    Transform _tankCannon;

    public PlayerControls(Transform tankCannon){
        _tankCannon = tankCannon;
    }


    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // model is reversed
            rb.ApplyForce(self.Transform.Backward * 2000);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.ApplyForce(self.Transform.Forward * 2000);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            rb.ApplyTorque(self.Transform.Up * 8050f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            rb.ApplyTorque(self.Transform.Down * 8050f);
        }
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Shoot(self, scene);
        }
    }

    public void Shoot(Entity self, Scene scene)
    {
        if (IsReloading) return;

        if (self is Tank tank)
        {
            // get cannon transform

            Bullet bullet = new("player-bullet", Damage, _tankCannon.AbsolutePosition - _tankCannon.Forward * 500 + _tankCannon.Up * 200, -_tankCannon.Forward , BulletForce);

            scene.AddEntityDynamically(bullet);

            // mark as reloading and 
            IsReloading = true;
            Timer.Timeout(ReloadingTimeInMs, () => IsReloading = false);
        }
    }

    public void Initialize(Entity self, Scene scene)
    {
        rb = self.GetComponent<DynamicBody>();
    }

    public void Destroy(Entity self, Scene scene) { }
}