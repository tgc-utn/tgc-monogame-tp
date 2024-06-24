using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Entity : IDisposable
{

    public Vector3 Position {get; set;}
    public Vector3 Velocity {get; set;}
    public Vector3 Acceleration {get; set;} = Vector3.Zero;
    public Quaternion Rotation {get; set;} = Quaternion.Identity;
    public Vector3 RotationAxis {get; set;} = Vector3.UnitY;
    public float RotationAngle {get; set;} = 0f;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}