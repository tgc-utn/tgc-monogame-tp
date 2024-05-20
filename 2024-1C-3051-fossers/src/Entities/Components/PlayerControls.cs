using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Entities;
using WarSteel.Scenes;
using WarSteel.Utils;

public class PlayerControls : IComponent
{
    DynamicBody rb;
    float Damage = 100;
    bool IsReloading = false;
    int ReloadingTimeInMs = 2000;


    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            rb.ApplyForce(self.Transform.GetWorld().Forward * 100);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.ApplyForce(self.Transform.GetWorld().Backward * 100);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * 350f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * -6000);
        }
        if (isClickingLMB())
        {
            if (IsReloading) return;
            scene.AddEntity(new Bullet("player-bullet", Damage, new Vector3(50, 50, 50), new Vector3(1, 1, 1), 100));
            IsReloading = true;
            Timer.Timeout(ReloadingTimeInMs, () => IsReloading = false);
            Console.WriteLine("Realoding");
        }
    }

    private bool isClickingLMB()
    {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            return true;

        return false;
    }

    public void Initialize(Entity self, Scene scene)
    {
        rb = self.GetComponent<DynamicBody>();
    }

    public void Destroy(Entity self, Scene scene) { }
}