
using Microsoft.Xna.Framework;

namespace WarSteel.Common;

public class Transform
{
    public Vector3 Dim { get; private set; }
    public Vector3 Pos { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Matrix World { get; private set; }

    public Transform()
    {
        Dim = Vector3.One;
        Pos = Vector3.Zero;
        Rotation = Quaternion.Identity;
    }

    public void Scale(Vector3 scale) => this.Dim += scale;

    public void Translate(Vector3 position) => this.Pos += position;

    // x,y,z angles in degrees
    public void Rotate(Vector3 eulerAngles) => Rotation += Quaternion.CreateFromYawPitchRoll(
        MathHelper.ToRadians(eulerAngles.Y),
        MathHelper.ToRadians(eulerAngles.X),
        MathHelper.ToRadians(eulerAngles.Z)
    );

    public void Rotate(Quaternion quaternion) => Rotation += quaternion;

    public void UpdateWorldMatrix()
    {
        World = Matrix.CreateScale(Dim) * Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Pos);
    }
}