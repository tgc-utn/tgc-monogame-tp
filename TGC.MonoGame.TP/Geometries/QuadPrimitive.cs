using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Geometries
{
    internal class QuadPrimitive
    {
        internal QuadPrimitive(GraphicsDevice graphicsDevice)
        {
            CreateVertexBuffer(graphicsDevice);
            CreateIndexBuffer(graphicsDevice);
        }

        private VertexBuffer Vertices { get; set; }

        private IndexBuffer Indices { get; set; }


        private void CreateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            var textureCoordinateLowerLeft = Vector2.Zero;
            var textureCoordinateLowerRight = Vector2.UnitX;
            var textureCoordinateUpperLeft = Vector2.UnitY;
            var textureCoordinateUpperRight = Vector2.One;

            var vertices = new[]
            {
                // Possitive X, Possitive Z
                new VertexPositionNormalTexture(Vector3.UnitX + Vector3.UnitZ, Vector3.Up, textureCoordinateUpperRight),
                // Possitive X, Negative Z
                new VertexPositionNormalTexture(Vector3.UnitX - Vector3.UnitZ, Vector3.Up, textureCoordinateLowerRight),
                // Negative X, Possitive Z
                new VertexPositionNormalTexture(Vector3.UnitZ - Vector3.UnitX, Vector3.Up, textureCoordinateUpperLeft),
                // Negative X, Negative Z
                new VertexPositionNormalTexture(-Vector3.UnitX - Vector3.UnitZ, Vector3.Up, textureCoordinateLowerLeft)
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                3, 1, 0, 
                3, 0, 2,
            };

            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }

        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(Vertices);
            graphicsDevice.Indices = Indices;

            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.IndexCount / 3);
            }
        }
    }
}