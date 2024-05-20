using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Scenes;
using WarSteel.Utils;

namespace WarSteel.Entities.Components;

public class PlayerControls : IComponent
{
    DynamicBody rb;
    float Damage = 100;
    float BulletForce = 100;
    bool IsReloading = false;
    int ReloadingTimeInMs = 500;


    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            // model is reversed
            rb.ApplyForce(self.Transform.GetWorld().Backward * 100);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.ApplyForce(self.Transform.GetWorld().Forward * 100);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * 350f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * -350f);
        }
        if (isClickingLMB())
        {
            if (IsReloading) return;
            Vector3 CameraPos = scene.GetCamera().Transform.Pos;
            Vector3 Dir = new(-CameraPos.X, CameraPos.Y, -CameraPos.Z);
            Bullet bullet = new Bullet("player-bullet", Damage, self.Transform.Pos + new Vector3(Dir.X, 200, Dir.Z), Dir, BulletForce);
            bullet.Initialize(scene);
            scene.AddEntity(bullet);
            IsReloading = true;
            Timer.Timeout(ReloadingTimeInMs, () => IsReloading = false);
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