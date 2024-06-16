using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TGC.MonoGame.Samples.Collisions;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP
{

    public static class Utils
    {
        private static int ArenaWidth = 200;
        private static int ArenaHeight = 200;
        private static Random _random = new Random(0);

        public static NumericVector3 ToNumericVector3(Vector3 v)
        {
            return new NumericVector3(v.X, v.Y, v.Z);
        }
        public static float Max(float value1, float value2)
        {
            return value1 > value2 ? value1 : value2;
        }
        public static List<Vector3> GenerateRandomListPositions(int count)
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(-ArenaWidth, ArenaWidth);
                int z = _random.Next(-ArenaHeight, ArenaHeight);
                positions.Add(new Vector3(x, 0, z));
            }

            return positions;
        }
        public static Vector3 GenerateRandomPositions()
        {
            int x = _random.Next(-ArenaWidth, ArenaWidth);
            int z = _random.Next(-ArenaHeight, ArenaHeight);
            return new Vector3(x, 0, z);
        }
        public static Vector3 GenerateRandomPositionsWithY( float y)
        {
            int x = _random.Next(-ArenaWidth, ArenaWidth);
            int z = _random.Next(-ArenaHeight, ArenaHeight);
            return new Vector3(x, y, z);
        }
        public static List<Vector3> GenerateRandomListPositions(int count, float y)
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(-ArenaWidth, ArenaWidth);
                int z = _random.Next(-ArenaHeight, ArenaHeight);
                positions.Add(new Vector3(x, y, z));
            }

            return positions;
        }
        private static NumericVector3[] GetVerticesFromModel(Model model)
        {
            List<NumericVector3> vertices = new List<NumericVector3>();


            foreach (ModelMeshPart part in model.Meshes[0].MeshParts)
            {
                VertexBuffer vertexBuffer = part.VertexBuffer;
                int vertexStride = part.VertexBuffer.VertexDeclaration.VertexStride;
                int vertexBufferSize = vertexBuffer.VertexCount * vertexStride;

                // Get the vertices from the vertex buffer
                NumericVector3[] vertexData = new NumericVector3[vertexBuffer.VertexCount];
                vertexBuffer.GetData(vertexData);

                // Add the vertices to the list
                vertices.AddRange(vertexData);
            }


            return vertices.ToArray();
        }
        public static  void AddModelRandomPosition(Model model, Effect effect, float scale, Simulation simulation, int lenght, List<GameModel> gameModels)
        {

            for (int i = 0; i < lenght; i++)
            {
                gameModels.Add(new GameModel(
                    model,
                    effect,
                    scale,
                    GenerateRandomPositions(),
                    simulation
                ));

            }
        }
        public static void AddModelRandomPositionWithY(Model model, Effect effect, float scale, Simulation simulation, int lenght, List<GameModel> gameModels , float y)
        {

            for (int i = 0; i < lenght; i++)
            {
                gameModels.Add(new GameModel(
                    model,
                    effect,
                    scale,
                    GenerateRandomPositionsWithY(y),
                    simulation
                ));

            }
        }
        public static void ToAxisAngle(Quaternion q, out Vector3 axis, out float angle)
        {
            if (q.W > 1) q.Normalize(); // Si el cuaternión no está normalizado, normalízalo
            angle = 2 * (float)Math.Acos(q.W); // Calcular el ángulo
            float s = (float)Math.Sqrt(1 - q.W * q.W); // Suponiendo que q.W está normalizado

            if (s < 0.001f)
            {
                // Si s es cero, el eje se puede elegir arbitrariamente
                axis = new Vector3(q.X, q.Y, q.Z);
            }
            else
            {
                axis = new Vector3(q.X / s, q.Y / s, q.Z / s); // Normalizar el eje
            }
        }
    }
}