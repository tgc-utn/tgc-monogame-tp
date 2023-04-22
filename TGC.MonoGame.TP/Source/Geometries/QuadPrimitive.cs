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
            Vector2 textureCoordinateLowerLeft = Vector2.Zero;
            Vector2 textureCoordinateLowerRight = Vector2.UnitX;
            Vector2 textureCoordinateUpperLeft = Vector2.UnitY;
            Vector2 textureCoordinateUpperRight = Vector2.One;

            var vertices = new[]
            {
                // (0,0,0)
                new VertexPositionNormalTexture(Vector3.Zero, Vector3.Up, textureCoordinateLowerLeft),
                // (0,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ, Vector3.Up, textureCoordinateUpperLeft),
                // (1,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ + Vector3.UnitX, Vector3.Up, textureCoordinateUpperRight),
                // (1,0,0)
                new VertexPositionNormalTexture(Vector3.UnitX, Vector3.Up, textureCoordinateLowerRight)
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                0, 1, 2, 
                0, 3, 2,
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