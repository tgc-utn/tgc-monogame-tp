using Microsoft.Xna.Framework;
using System;

namespace ThunderingTanks.Collisions
{

    public class OrientedBoundingBox
    {

        public Vector3 Center { get; set; }

        public Matrix Orientation { get; set; }

        public Vector3 Extents { get; set; }

        public OrientedBoundingBox() { }

        public OrientedBoundingBox(Vector3 center, Vector3 extents)
        {
            Center = center;
            Extents = extents;
            Orientation = Matrix.Identity;
        }

        public void Rotate(Matrix rotation)
        {
            Orientation = rotation;
        }

        public void Rotate(Quaternion rotation)
        {
            Rotate(Matrix.CreateFromQuaternion(rotation));
        }

        public static OrientedBoundingBox ComputeFromPoints(Vector3[] points)
        {
            return ComputeFromPointsRecursive(points, Vector3.Zero, new Vector3(360, 360, 360), 10f);
        }

        private static OrientedBoundingBox ComputeFromPointsRecursive(Vector3[] points, Vector3 initValues, Vector3 endValues,
            float step)
        {
            var minObb = new OrientedBoundingBox();
            var minimumVolume = float.MaxValue;
            var minInitValues = Vector3.Zero;
            var minEndValues = Vector3.Zero;
            var transformedPoints = new Vector3[points.Length];
            float y, z;

            var x = initValues.X;
            while (x <= endValues.X)
            {
                y = initValues.Y;
                var rotationX = MathHelper.ToRadians(x);
                while (y <= endValues.Y)
                {
                    z = initValues.Z;
                    var rotationY = MathHelper.ToRadians(y);
                    while (z <= endValues.Z)
                    {
                        var rotationZ = MathHelper.ToRadians(z);
                        var rotationMatrix = Matrix.CreateFromYawPitchRoll(rotationY, rotationX, rotationZ);

                        for (var index = 0; index < transformedPoints.Length; index++)
                            transformedPoints[index] = Vector3.Transform(points[index], rotationMatrix);

                        var aabb = BoundingBox.CreateFromPoints(transformedPoints);

                        var volume = BoundingVolumesExtensions.GetVolume(aabb);

                        if (volume < minimumVolume)
                        {
                            minimumVolume = volume;
                            minInitValues = new Vector3(x, y, z);
                            minEndValues = new Vector3(x + step, y + step, z + step);

                            var center = BoundingVolumesExtensions.GetCenter(aabb);
                            center = Vector3.Transform(center, rotationMatrix);

                            minObb = new OrientedBoundingBox(center, BoundingVolumesExtensions.GetExtents(aabb));
                            minObb.Orientation = rotationMatrix;
                        }

                        z += step;
                    }
                    y += step;
                }
                x += step;
            }

            if (step > 0.01f)
                minObb = ComputeFromPointsRecursive(points, minInitValues, minEndValues, step / 10f);

            return minObb;
        }
        public static OrientedBoundingBox FromAABB(BoundingBox box)
        {
            var center = BoundingVolumesExtensions.GetCenter(box);
            var extents = BoundingVolumesExtensions.GetExtents(box);
            return new OrientedBoundingBox(center, extents);
        }

        public Vector3 ToOBBSpace(Vector3 point)
        {
            var difference = point - Center;
            return Vector3.Transform(difference, Orientation);
        }

        private float[] ToArray(Vector3 vector)
        {
            return new[] { vector.X, vector.Y, vector.Z };
        }

        private float[] ToFloatArray(Matrix matrix)
        {
            return new[]
            {
                matrix.M11, matrix.M21, matrix.M31,
                matrix.M12, matrix.M22, matrix.M32,
                matrix.M13, matrix.M23, matrix.M33,
            };
        }

        public bool Intersects(OrientedBoundingBox box)
        {
            float ra;
            float rb;
            var R = new float[3, 3];
            var AbsR = new float[3, 3];
            var ae = ToArray(Extents);
            var be = ToArray(box.Extents);

            var result = ToFloatArray(Matrix.Multiply(Orientation, box.Orientation));

            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    R[i, j] = result[i * 3 + j];

            var tVec = box.Center - Center;

            var t = ToArray(Vector3.Transform(tVec, Orientation));

            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    AbsR[i, j] = MathF.Abs(R[i, j]) + float.Epsilon;

            for (var i = 0; i < 3; i++)
            {
                ra = ae[i];
                rb = be[0] * AbsR[i, 0] + be[1] * AbsR[i, 1] + be[2] * AbsR[i, 2];
                if (MathF.Abs(t[i]) > ra + rb) return false;
            }

            for (var i = 0; i < 3; i++)
            {
                ra = ae[0] * AbsR[0, i] + ae[1] * AbsR[1, i] + ae[2] * AbsR[2, i];
                rb = be[i];
                if (MathF.Abs(t[0] * R[0, i] + t[1] * R[1, i] + t[2] * R[2, i]) > ra + rb) return false;
            }

            ra = ae[1] * AbsR[2, 0] + ae[2] * AbsR[1, 0];
            rb = be[1] * AbsR[0, 2] + be[2] * AbsR[0, 1];
            if (MathF.Abs(t[2] * R[1, 0] - t[1] * R[2, 0]) > ra + rb) return false;

            ra = ae[1] * AbsR[2, 1] + ae[2] * AbsR[1, 1];
            rb = be[0] * AbsR[0, 2] + be[2] * AbsR[0, 0];
            if (MathF.Abs(t[2] * R[1, 1] - t[1] * R[2, 1]) > ra + rb) return false;

            ra = ae[1] * AbsR[2, 2] + ae[2] * AbsR[1, 2];
            rb = be[0] * AbsR[0, 1] + be[1] * AbsR[0, 0];
            if (MathF.Abs(t[2] * R[1, 2] - t[1] * R[2, 2]) > ra + rb) return false;

            ra = ae[0] * AbsR[2, 0] + ae[2] * AbsR[0, 0];
            rb = be[1] * AbsR[1, 2] + be[2] * AbsR[1, 1];
            if (MathF.Abs(t[0] * R[2, 0] - t[2] * R[0, 0]) > ra + rb) return false;

            ra = ae[0] * AbsR[2, 1] + ae[2] * AbsR[0, 1];
            rb = be[0] * AbsR[1, 2] + be[2] * AbsR[1, 0];
            if (MathF.Abs(t[0] * R[2, 1] - t[2] * R[0, 1]) > ra + rb) return false;

            ra = ae[0] * AbsR[2, 2] + ae[2] * AbsR[0, 2];
            rb = be[0] * AbsR[1, 1] + be[1] * AbsR[1, 0];
            if (MathF.Abs(t[0] * R[2, 2] - t[2] * R[0, 2]) > ra + rb) return false;

            ra = ae[0] * AbsR[1, 0] + ae[1] * AbsR[0, 0];
            rb = be[1] * AbsR[2, 2] + be[2] * AbsR[2, 1];
            if (MathF.Abs(t[1] * R[0, 0] - t[0] * R[1, 0]) > ra + rb) return false;

            ra = ae[0] * AbsR[1, 1] + ae[1] * AbsR[0, 1];
            rb = be[0] * AbsR[2, 2] + be[2] * AbsR[2, 0];
            if (MathF.Abs(t[1] * R[0, 1] - t[0] * R[1, 1]) > ra + rb) return false;

            ra = ae[0] * AbsR[1, 2] + ae[1] * AbsR[0, 2];
            rb = be[0] * AbsR[2, 1] + be[1] * AbsR[2, 0];
            if (MathF.Abs(t[1] * R[0, 2] - t[0] * R[1, 2]) > ra + rb) return false;

            return true;
        }

        public bool Intersects(BoundingBox box)
        {
            return Intersects(FromAABB(box));
        }

        public bool Intersects(Ray ray, out float? result)
        {
            var rayOrigin = ray.Position;
            var rayDestination = rayOrigin + ray.Direction;

            var rayOriginInOBBSpace = ToOBBSpace(rayOrigin);
            var rayDestinationInOBBSpace = ToOBBSpace(rayDestination);

            var rayInOBBSpace = new Ray(rayOriginInOBBSpace, Vector3.Normalize(rayDestinationInOBBSpace - rayOriginInOBBSpace));

            var enclosingBox = new BoundingBox(-Extents, Extents);

            var testResult = enclosingBox.Intersects(rayInOBBSpace);
            result = testResult;

            return testResult != null;
        }

        public bool Intersects(BoundingSphere sphere)
        {

            var obbSpaceSphere = new BoundingSphere(ToOBBSpace(sphere.Center), sphere.Radius);

            var aabb = new BoundingBox(-Extents, Extents);

            return aabb.Intersects(obbSpaceSphere);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {

            var normal = Vector3.Transform(plane.Normal, Orientation);

            var r = MathF.Abs(Extents.X * normal.X)
                + MathF.Abs(Extents.Y * normal.Y)
                + MathF.Abs(Extents.Z * normal.Z);

            var d = Vector3.Dot(plane.Normal, Center) + plane.D;

            if (MathF.Abs(d) < r)
                return PlaneIntersectionType.Intersecting;
            else if (d < 0.0f)
                return PlaneIntersectionType.Front;
            else
                return PlaneIntersectionType.Back;
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            var planes = new[]
            {
                frustum.Left,
                frustum.Right,
                frustum.Far,
                frustum.Near,
                frustum.Bottom,
                frustum.Top
            };

            for (var faceIndex = 0; faceIndex < 6; ++faceIndex)
            {
                var side = Intersects(planes[faceIndex]);
                if (side == PlaneIntersectionType.Back)
                    return false;
            }
            return true;
        }

        public Vector3 ToWorldSpace(Vector3 point)
        {
            return Center + Vector3.Transform(point, Orientation);
        }

    }

}
