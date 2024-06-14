using Microsoft.Xna.Framework;
using System;
using System.Linq;
using TGC.MonoGame.Samples.Collisions;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.Utils
{


    public struct Interval
    {
        public float Min;
        public float Max;
    }

    public class Utils
    {
        public static NumericVector3 ToNumericVector3(Vector3 v)
        {
            return new NumericVector3(v.X, v.Y, v.Z);
        }

        public static float Max(float value1, float value2)
        {
            return value1 > value2 ? value1 : value2;
        }


        public static bool AABBOBB(BoundingBox aabb, OrientedBoundingBox obb)
        {
            Matrix orientationMatrix = obb.Orientation;
            Vector3[] obbAxes = new Vector3[3]
            {
        new Vector3(orientationMatrix.M11, orientationMatrix.M12, orientationMatrix.M13),
        new Vector3(orientationMatrix.M21, orientationMatrix.M22, orientationMatrix.M23),
        new Vector3(orientationMatrix.M31, orientationMatrix.M32, orientationMatrix.M33)
            };

            Vector3[] test = new Vector3[15];

            test[0] = new Vector3(1, 0, 0);
            test[1] = new Vector3(0, 1, 0);
            test[2] = new Vector3(0, 0, 1);

            // OBB axes
            test[3] = obbAxes[0];
            test[4] = obbAxes[1];
            test[5] = obbAxes[2];

            for (int i = 0; i < 3; ++i)
            {
                // Fill out rest of axis
                test[6 + i * 3 + 0] = Vector3.Cross(test[i], test[0]);
                test[6 + i * 3 + 1] = Vector3.Cross(test[i], test[1]);
                test[6 + i * 3 + 2] = Vector3.Cross(test[i], test[2]);
            }

            for (int i = 0; i < 15; ++i)
            {
                if (!OverlapOnAxis(aabb, obb, test[i]))
                {
                    return false; // Separating axis found
                }
            }

            return true; // Separating axis not found
        }

        private static bool OverlapOnAxis(BoundingBox aabb, OrientedBoundingBox obb, Vector3 axis)
        {
            Interval a = GetInterval(aabb, axis);
            Interval b = GetInterval(obb, axis);
            return ((b.Min <= a.Max) && (a.Min <= b.Max));
        }

        private static Interval GetInterval(BoundingBox aabb, Vector3 axis)
        {
            Vector3[] vertices = new Vector3[8]
            {
        new Vector3(aabb.Min.X, aabb.Min.Y, aabb.Min.Z),
        new Vector3(aabb.Max.X, aabb.Min.Y, aabb.Min.Z),
        new Vector3(aabb.Min.X, aabb.Max.Y, aabb.Min.Z),
        new Vector3(aabb.Max.X, aabb.Max.Y, aabb.Min.Z),
        new Vector3(aabb.Min.X, aabb.Min.Y, aabb.Max.Z),
        new Vector3(aabb.Max.X, aabb.Min.Y, aabb.Max.Z),
        new Vector3(aabb.Min.X, aabb.Max.Y, aabb.Max.Z),
        new Vector3(aabb.Max.X, aabb.Max.Y, aabb.Max.Z),
            };

            Interval result;
            result.Min = result.Max = Vector3.Dot(axis, vertices[0]);

            for (int i = 1; i < vertices.Length; i++)
            {
                float projection = Vector3.Dot(axis, vertices[i]);
                result.Min = Math.Min(result.Min, projection);
                result.Max = Math.Max(result.Max, projection);
            }

            return result;
        }

        private static Interval GetInterval(OrientedBoundingBox obb, Vector3 axis)
        {
            Vector3[] vertices = new Vector3[8];

            Vector3 center = obb.Center;
            Vector3[] axes = new Vector3[3]
            {
        new Vector3(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13),
        new Vector3(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23),
        new Vector3(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33)
            };
            Vector3 halfSize = obb.Extents;

            vertices[0] = center + axes[0] * halfSize.X + axes[1] * halfSize.Y + axes[2] * halfSize.Z;
            vertices[1] = center - axes[0] * halfSize.X + axes[1] * halfSize.Y + axes[2] * halfSize.Z;
            vertices[2] = center + axes[0] * halfSize.X - axes[1] * halfSize.Y + axes[2] * halfSize.Z;
            vertices[3] = center + axes[0] * halfSize.X + axes[1] * halfSize.Y - axes[2] * halfSize.Z;
            vertices[4] = center - axes[0] * halfSize.X - axes[1] * halfSize.Y + axes[2] * halfSize.Z;
            vertices[5] = center - axes[0] * halfSize.X + axes[1] * halfSize.Y - axes[2] * halfSize.Z;
            vertices[6] = center + axes[0] * halfSize.X - axes[1] * halfSize.Y - axes[2] * halfSize.Z;
            vertices[7] = center - axes[0] * halfSize.X - axes[1] * halfSize.Y - axes[2] * halfSize.Z;

            Interval result;
            result.Min = result.Max = Vector3.Dot(axis, vertices[0]);

            for (int i = 1; i < vertices.Length; i++)
            {
                float projection = Vector3.Dot(axis, vertices[i]);
                result.Min = Math.Min(result.Min, projection);
                result.Max = Math.Max(result.Max, projection);
            }

            return result;
        }




    }
}