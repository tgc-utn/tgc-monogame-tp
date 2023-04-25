using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace TGC.MonoGame.TP.Entities
{
    public class SimpleQuad
    {
        /// <summary>
        ///     Create a textured quad.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        public SimpleQuad(GraphicsDevice graphicsDevice, Effect effect)
        {
            Effect = effect;
            Vertices = new VertexBuffer(graphicsDevice, VertexPositionColorNormal.VertexDeclaration, 4,
                BufferUsage.WriteOnly);
            CreateIndexBuffer(graphicsDevice);
        }

        /// <summary>
        ///     Represents a list of 3D vertices to be streamed to the graphics device.
        /// </summary>
        private VertexBuffer Vertices { get; set; }

        /// <summary>
        ///     Describes the rendering order of the vertices in a vertex buffer, using counter-clockwise winding.
        /// </summary>
        private IndexBuffer Indices { get; set; }


        /// <summary>
        ///     Built-in effect that supports optional texturing, vertex coloring, fog, and lighting.
        /// </summary>
        public Effect Effect { get; private set; }

        /// <summary>
        ///     Create a vertex buffer for the figure with the given information.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        public void ModifyVertexBuffer(float[,] heights)
        {
            // Set the position and texture coordinate for each vertex
            // Normals point Up as the Quad is originally XZ aligned
            var vertices = new[]
            {
                // Positive X, Positive Z
                new VertexPositionColorNormal(Vector3.UnitX + Vector3.UnitZ + Vector3.UnitY * heights[1,0],Color.Aquamarine ,Vector3.UnitY),
                // Positive X, Negative Z
                new VertexPositionColorNormal(Vector3.UnitX - Vector3.UnitZ + Vector3.UnitY * heights[1,1], Color.Blue, Vector3.UnitY),
                // Negative X, Positive Z
                new VertexPositionColorNormal(-Vector3.UnitX - Vector3.UnitZ + Vector3.UnitY * heights[0,1], Color.Aqua, Vector3.UnitY),
                // Negative X, Negative Z
                new VertexPositionColorNormal(Vector3.UnitZ - Vector3.UnitX + Vector3.UnitY * heights[0,0], Color.Blue, Vector3.UnitY)
            };
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            // Set the index buffer for each vertex, using clockwise winding
            var indices = new ushort[]
            {
                1, 2, 0,
                0, 3, 2,
            };

            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }

        /// <summary>
        ///     Draw the Quad.
        /// </summary>
        /// <param name="world">The world matrix for this box.</param>
        /// <param name="view">The view matrix, normally from the camera.</param>
        /// <param name="projection">The projection matrix, normally from the application.</param>
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.Blue.ToVector3());
            Draw(Effect);
        }

        /// <summary>
        ///     Draws the primitive model, using the specified effect. Unlike the other Draw overload where you just specify the
        ///     world/view/projection matrices and color, this method does not set any render states, so you must make sure all
        ///     states are set to sensible values before you call it.
        /// </summary>
        /// <param name="effect">Used to set and query effects, and to choose techniques.</param>
        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
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
