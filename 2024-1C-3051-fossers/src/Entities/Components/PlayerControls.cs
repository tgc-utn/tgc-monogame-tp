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
            rb.AddForce(new OVector3(Vector3.Zero, self.Transform.GetWorld().Forward * 20000));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            rb.AddForce(new OVector3(Vector3.Zero, self.Transform.GetWorld().Backward * 20000));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            self.Transform.Rotate(new Vector3(0, 5, 0));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            self.Transform.Rotate(new Vector3(0, -5, 0));
        }
    }

    public void Initialize(Entity self, Scene scene)
    {
        rb = self.GetComponent<RigidBody>();
    }

    public void Destroy(Entity self, Scene scene) { }
}