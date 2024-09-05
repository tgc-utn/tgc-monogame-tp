using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.Tp {
public class Elevator : IDisposable {
  private Tp.CubePrimitive Model;
  private Matrix World;
  private float Size;

  public static Vector3 Position;
  private float Speed;

  private float Max_height;
  private float Min_height;
  private Color Color;

  public Elevator(GraphicsDevice graphicsDevice, Vector3 position, float speed,
                  float size, Color color, float max_height) {
    Position = position;
    Speed = speed;
    Size = size;
    Color = color;
    Max_height = max_height;
    Min_height = position.Y;
    Model = new Tp.CubePrimitive(graphicsDevice, 1, color);
  }

  public void Update(float dt) {
    if (Speed > 0 && Position.Y >= Max_height ||
        Speed < 0 && Position.Y <= Min_height)
      Speed *= -1;

    Position.Y += Speed * dt;

    World = Matrix.CreateScale(Size) * Matrix.CreateTranslation(Position);
  }

  public void Draw(Effect Effect) {
    Effect.Parameters["DiffuseColor"].SetValue(Color.ToVector3());
    Effect.Parameters["World"].SetValue(World);
    Model.Draw(Effect);
  }

  public void Dispose() { Model.Dispose(); }
}
}
