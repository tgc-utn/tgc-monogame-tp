using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarSteel.Entities;
using WarSteel.Scenes;

public class PlayerControls : IComponent
{
    RigidBody rb;

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            rb.ApplyForce(self.Transform.GetWorld().Forward * 350);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.ApplyForce(self.Transform.GetWorld().Backward * 350);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * 6000);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            rb.ApplyTorque(self.Transform.GetWorld().Up * -6000);
        }
    }

    public void Initialize(Entity self, Scene scene)
    {
        rb = self.GetComponent<RigidBody>();
    }

    public void Destroy(Entity self, Scene scene) { }
}