using BEPUVector3 = System.Numerics.Vector3;
using BEPUQuaternion = System.Numerics.Quaternion;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Physics
{
    internal static class BEPUNumericsExtensions
    {
        internal static Vector3 ToVector3(this BEPUVector3 bepuVector3) => new Vector3(bepuVector3.X, bepuVector3.Y, bepuVector3.Z);
        internal static Quaternion ToQuaternion(this BEPUQuaternion bepuQuaternion) => new Quaternion(bepuQuaternion.X, bepuQuaternion.Y, bepuQuaternion.Z, bepuQuaternion.W);

        internal static BEPUVector3 ToBEPU(this Vector3 vector3) => new BEPUVector3(vector3.X, vector3.Y, vector3.Z);

        internal static BEPUQuaternion ToBEPU(this Quaternion quaternion) => new BEPUQuaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}