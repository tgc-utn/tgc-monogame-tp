using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace ThunderingTanks
{
    public class Projectile : GameObject
{
    public Vector3 Direction { get; set; }
    public float Speed { get; set; }
    public new Vector3 Position { get; set; }

    public Projectile(Vector3 position, Vector3 direction, float speed)
    {
        this.Position = position;
        this.Direction = direction;
        this.Speed = speed;
    }

    public void Update(GameTime gameTime)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Direction * Speed * time;
    }
}
}