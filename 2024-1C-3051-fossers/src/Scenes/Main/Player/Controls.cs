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
    float BulletForce = 5000;
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
            Shoot(self, scene);
        }
    }

    private bool isClickingLMB()
    {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            return true;

        return false;
    }

    public void Shoot(Entity self, Scene scene)
    {
        if (IsReloading) return;
        Camera camera = scene.GetCamera();
        MouseController mouse = camera.GetComponent<MouseController>();

        if (self is Tank tank)
        {
            // get cannon transform
            Matrix cannonTransform = tank.Renderable.cannonBone.Transform * tank.Transform.GetWorld();
            Vector3 cannonOffset = new(0, 300, 600);
            Bullet bullet = new Bullet("player-bullet", Damage, cannonTransform.Translation + cannonOffset, cannonTransform.Forward, BulletForce);

            // init and add to scene
            bullet.Initialize(scene);
            scene.AddEntity(bullet);

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