using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ThunderingTanks.Collisions
{
    public static class BoundingVolumesExtensions
    {
        public static Vector3 GetExtents(BoundingBox box)
        {
            var max = box.Max;
            var min = box.Min;

            return (max - min) * 0.5f;            
        }
        public static float GetVolume(BoundingBox box)
        {
            var difference = box.Max - box.Min;
            return difference.X * difference.Y * difference.Z;
        }
        public static Vector3 GetCenter(BoundingBox box)
        {
            return (box.Max + box.Min) * 0.5f;
        }
        public static BoundingBox Scale(BoundingBox box, float scale)
        {
            var center = GetCenter(box);
            var extents = GetExtents(box);
            var scaledExtents = extents * scale;

            return new BoundingBox(center - scaledExtents, center + scaledExtents);
        }
        public static BoundingBox Scale(BoundingBox box, Vector3 scale)
        {
            var center = GetCenter(box);
            var extents = GetExtents(box);
            var scaledExtents = extents * scale;

            return new BoundingBox(center - scaledExtents, center + scaledExtents);
        }
        public static Vector3 ClosestPoint(BoundingBox box, Vector3 point)
        {
            var min = box.Min;
            var max = box.Max;
            point.X = MathHelper.Clamp(point.X, min.X, max.X);
            point.Y = MathHelper.Clamp(point.Y, min.Y, max.Y);
            point.Z = MathHelper.Clamp(point.Z, min.Z, max.Z);
            return point;
        }
        public static Vector3 GetNormalFromPoint(BoundingBox box, Vector3 point)
        {
            var normal = Vector3.Zero;
            var min = float.MaxValue;

            point -= GetCenter(box);
            var extents = GetExtents(box);

            var distance = MathF.Abs(extents.X - Math.Abs(point.X));
            if (distance < min)
            {
                min = distance;
                normal = Math.Sign(point.X) * Vector3.UnitX;   
            }
            distance = Math.Abs(extents.Y - Math.Abs(point.Y));
            if (distance < min)
            {
                min = distance;
                normal = Math.Sign(point.Y) * Vector3.UnitY;      
            }
            distance = Math.Abs(extents.Z - Math.Abs(point.Z));
            if (distance < min)
            {
                normal = Math.Sign(point.Z) * Vector3.UnitZ;         
            }
            return normal;
        }
        public static BoundingBox FromMatrix(Matrix matrix)
        {
            return new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, matrix), Vector3.Transform(Vector3.One * 0.5f, matrix));
        }
        public static BoundingBox CreateAABBFrom(Model model)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            var meshes = model.Meshes;
            for (int index = 0; index < meshes.Count; index++)
            {
                var meshParts = meshes[index].MeshParts;
                for (int subIndex = 0; subIndex < meshParts.Count; subIndex++)
                {
                    var vertexBuffer = meshParts[subIndex].VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[meshes[index].ParentBone.Index];
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }
            return new BoundingBox(minPoint, maxPoint);
        }
        public static BoundingSphere CreateSphereFrom(Model model)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            var meshes = model.Meshes;
            for (var index = 0; index < meshes.Count; index++)
            {
                var meshParts = meshes[index].MeshParts;
                for (var subIndex = 0; subIndex < meshParts.Count; subIndex++)
                {
                    var vertexBuffer = meshParts[subIndex].VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    int vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[meshes[index].ParentBone.Index];
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }
            var difference = (maxPoint - minPoint) * 0.5f;
            return new BoundingSphere(difference, difference.Length());
        }

    }
}
