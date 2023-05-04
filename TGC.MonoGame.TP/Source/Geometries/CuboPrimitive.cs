using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Geometries
{
    internal class CuboPrimitive
    {
        internal CuboPrimitive(GraphicsDevice graphicsDevice)
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
                // [0] : (0,0,0)
                new VertexPositionNormalTexture(Vector3.Zero, Vector3.Up, textureCoordinateLowerLeft),
                // [1] : (0,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ, Vector3.Up, textureCoordinateUpperLeft),
                // [2] : (1,0,1)
                new VertexPositionNormalTexture(Vector3.UnitZ + Vector3.UnitX, Vector3.Up, textureCoordinateUpperRight),
                // [3] : (1,0,0)
                new VertexPositionNormalTexture(Vector3.UnitX, Vector3.Up, textureCoordinateLowerRight),
                // [4] : (0,1,0)
                new VertexPositionNormalTexture(Vector3.UnitY + Vector3.Zero, Vector3.Up, textureCoordinateLowerLeft),
                // [5] : (0,1,1)
                new VertexPositionNormalTexture(Vector3.UnitY + Vector3.UnitZ, Vector3.Up, textureCoordinateUpperLeft),
                // [6] : (1,1,1)
                new VertexPositionNormalTexture(Vector3.UnitY + Vector3.UnitZ + Vector3.UnitX, Vector3.Up, textureCoordinateUpperRight),
                // [7] : (1,1,0)
                new VertexPositionNormalTexture(Vector3.UnitY + Vector3.UnitX, Vector3.Up, textureCoordinateLowerRight)
            };

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            var indices = new ushort[]
            {
                // Triangulos base
                0, 2, 1,
                0, 3, 2,
                // Cara Lateral Derecha
                0, 3, 7,
                0, 7, 4,
                // Cara Lateral Izquierda
                1, 2, 6,
                1, 6, 5,
                // Cara Frontal
                2, 6, 7, 
                2, 7, 3,
                // Cara Posterior
                4, 0, 1,
                4, 1, 5,
                // Triangulos tapa
                4, 6, 5,
                4, 7, 6,
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