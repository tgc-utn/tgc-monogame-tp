
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WarSteel.Common;

public class Transform
{
    public Vector3 Dim { get; set; }
    public Vector3 Pos { get; set; }
    public Quaternion Orientation { get; set; }
    public Matrix World { get; set; }

    public Transform()
    {
        Dim = Vector3.One;
        Pos = Vector3.Zero;
        Orientation = Quaternion.Identity;
    }

    public void Scale(Vector3 scale) => Dim += scale;
    public void Scale(float x, float y, float z) => Dim += new Vector3(x, y, z);

    public void Translate(Vector3 position) => Pos += position;
    public void Translate(float x, float y, float z) => Dim += new Vector3(x, y, z);

    // x,y,z angles in degrees
    public void Rotate(Vector3 eulerAngles) => Orientation += Quaternion.CreateFromYawPitchRoll(
        MathHelper.ToRadians(eulerAngles.Y),
        MathHelper.ToRadians(eulerAngles.X),
        MathHelper.ToRadians(eulerAngles.Z)
    );
    public void Rotate(float angleX, float angleY, float angleZ) => Dim += new Vector3(angleX, angleY, angleZ);

    public void Rotate(Quaternion quaternion) => Orientation += quaternion;

    public void UpdateWorldMatrix()
    {
        World = Matrix.CreateScale(Dim) * Matrix.CreateFromQuaternion(Orientation) * Matrix.CreateTranslation(Pos);
    }
}