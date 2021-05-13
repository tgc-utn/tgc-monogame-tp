using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Physics
{
    internal class PhysicUtils
    {
        internal static Vector3 Forward(Quaternion rotation) => new Vector3(
            2 * (rotation.X * rotation.Z + rotation.W * rotation.Y),
            2 * (rotation.Y * rotation.Z - rotation.W * rotation.X),
            1 - 2 * (rotation.X * rotation.X + rotation.Y * rotation.Y)
        );

        internal static Vector3 Up(Quaternion rotation) => new Vector3(
            2 * (rotation.X * rotation.Y - rotation.W * rotation.Z),
            1 - 2 * (rotation.X * rotation.X + rotation.Z * rotation.Z),
            2 * (rotation.Y * rotation.Z + rotation.W * rotation.X)
        );

        internal static Vector3 Left(Quaternion rotation) => new Vector3(
            1 - 2 * (rotation.Y * rotation.Y + rotation.Z * rotation.Z),
            2 * (rotation.X * rotation.Y + rotation.W * rotation.Z),
            2 * (rotation.X * rotation.Z - rotation.W * rotation.Y)
        );
    }
}