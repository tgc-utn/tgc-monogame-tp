using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarSteel.Utils;

public class ModelUtils
{
    public static float GetHeight(Model model)
    {
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                Vector3[] vertices = new Vector3[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData(0, vertices, 0, meshPart.NumVertices);

                foreach (Vector3 vertex in vertices)
                {
                    minZ = Math.Min(minZ, vertex.Z);
                    maxZ = Math.Max(maxZ, vertex.Z);
                }
            }
        }

        return Math.Abs(maxZ - minZ);

    }

    public static float GetWidth(Model model)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                Vector3[] vertices = new Vector3[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData(0, vertices, 0, meshPart.NumVertices);

                foreach (Vector3 vertex in vertices)
                {
                    minX = Math.Min(minX, vertex.X);
                    maxX = Math.Max(maxX, vertex.X);
                }
            }
        }

        return Math.Abs(maxX - minX);
    }
}