using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries; 

namespace TGC.MonoGame.TP.Obstacles {

public class Elevator : IDisposable {

  private CubePrimitive Model;
  private Matrix World;
  private float Size;

  private Vector3 Position;
  public float Speed;

  private float MaxHeight;
  private float MinHeight;
  private Color Color;
  private float scale_y = 0.1f;

  public Elevator(CubePrimitive model, Vector3 position, float speed,
                  float size, Color color, float offset) {
    Model = model;
    Position = position;
    Speed = speed;
    Size = size;
    Color = color;
    MinHeight = position.Y - scale_y * Size / 2;
    MaxHeight = MinHeight + offset;
  }

  public void Update(float dt) {
    if (Speed > 0 && Position.Y >= MaxHeight ||
        Speed < 0 && Position.Y <= MinHeight)
      Speed *= -1;

    Position.Y += Speed * dt;

    World = Matrix.CreateScale(new Vector3(Size, scale_y * Size, Size)) *
            Matrix.CreateTranslation(Position);
  }

  public void Draw(Effect Effect) {
    Effect.Parameters["DiffuseColor"].SetValue(Color.ToVector3());
    Effect.Parameters["World"].SetValue(World);
    Model.Draw(Effect);
  }

  public void Dispose() { Model.Dispose(); }
}
}
