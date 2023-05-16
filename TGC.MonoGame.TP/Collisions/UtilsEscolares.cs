
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BepuVector3    = System.Numerics.Vector3;
using BepuQuaternion = System.Numerics.Quaternion;
using System.Linq;

namespace TGC.MonoGame.TP.Collisions {

    public static class Utils {
        internal static Vector3 ToVector3(this BepuVector3 bepuVector3) => new Vector3(bepuVector3.X, bepuVector3.Y, bepuVector3.Z);
        internal static Quaternion ToQuaternion(this BepuQuaternion bepuQuaternion) => new Quaternion(bepuQuaternion.X, bepuQuaternion.Y, bepuQuaternion.Z, bepuQuaternion.W);
        internal static BepuVector3 ToBepu(this Vector3 vector3) => new BepuVector3(vector3.X, vector3.Y, vector3.Z);
        internal static BepuQuaternion ToBepu(this Quaternion quaternion) => new BepuQuaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        internal static Vector3[] Generate(Model model, float scale)
        {
            int vertexCount = GetVertexCount(model);
            // TGCGame.Simulation.BufferPool.Take<Vector3>(vertexCount, out var points);
            Vector3[] points = new Vector3[vertexCount];

            int pointIndex = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Vector3[] vertices = GetVertexElement(meshPart, VertexElementUsage.Position);
                    foreach (Vector3 vertex in vertices)
                    {
                        points[pointIndex] = vertex * scale;
                        pointIndex++;
                    }
                }
            }
            
            return points;
        }
        private static int GetVertexCount(Model model)
        {
            int vertexCount = 0;
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    vertexCount += meshPart.NumVertices;
            return vertexCount;
        }
        private static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
        {
            VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] elements = vd.GetVertexElements();

            bool elementPredicate(VertexElement ve) => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
            if (!elements.Any(elementPredicate))
                return null;

            VertexElement element = elements.First(elementPredicate);

            Vector3[] vertexData = new Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                vertexData, 0, vertexData.Length, vd.VertexStride);

            return vertexData;
        }

        public static Vector3 FowardFromQuaternion(Quaternion q){
            return new Vector3(
                                2 * (q.X*q.Z + q.W*q.Y),
                                2 * (q.Y*q.Z - q.W*q.X),
                                1 - 2 * (q.X*q.X + q.Y*q.Y));
        }

        /// <summarize> Devuelve el tamaño de la caja que envuelve tu corazon </summarize> 
        public static Vector3 ModelSize(Model model)
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
                return new Vector3(
                    Math.Abs(minPoint.X - maxPoint.X),
                    Math.Abs(minPoint.Y - maxPoint.Y),
                    Math.Abs(minPoint.Z - maxPoint.Z)
                    );
            }
    }
} 