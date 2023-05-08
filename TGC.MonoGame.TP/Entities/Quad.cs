using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace TGC.MonoGame.TP.Entities
{
    public class Quad
    {
        private VertexBuffer Vertices { get; set; }
        private Texture2D Texture { get; set; }
        private IndexBuffer Indices { get; set; }
        private Effect Effect { get; set; }

        public Quad(GraphicsDevice graphicsDevice, int rows)
        {
            SetVertexBuffer(graphicsDevice, rows);
            CreateIndexBuffer(graphicsDevice, rows);
        }

        public void LoadContent(Effect effect, Texture2D texture)
        {
            Effect = effect;
            Texture = texture;
        }

        public void SetVertexBuffer(GraphicsDevice graphicsDevice, int rows)
        {
            float subdivisionPosition = 2f / rows;
            // Si queremos que todo el quad tenga la misma textura
            // float subdivisionTexture = 1f / rows;
            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();

            /*
             * IMPORTANTE:
             * La textura se mueve de 0 a 1
             * La posición se mueve de -1 a 1
            */
            for (float i = 0; i <= rows; i++)
            {
                for (float j = 0; j <= rows; j++)
                {
                    vertices.Add(new (
                        new Vector3(Convert.ToSingle(i * subdivisionPosition - 1), 0, Convert.ToSingle(j * subdivisionPosition - 1)),
                        Vector3.UnitY,
                        // new Vector2(Convert.ToSingle(subdivisionTexture * i), Convert.ToSingle(subdivisionTexture * j)))
                        new Vector2(Convert.ToSingle(i % 2 == 0 ? 0 : 1), Convert.ToSingle(j % 2 == 0 ? 0 : 1)))
                    );
                }
            }
            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Count,
                BufferUsage.None);
            Vertices.SetData(vertices.ToArray());
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice, int rows)
        {
            List<ushort> indices = new List<ushort>();

            /*
             * 0 ---- 1  0 = left upper vertex
             * |   /  |  1 = right upper vertex
             * | /    |  2 = left bottom vertex
             * 2 ---- 3  3 = right bottom vertex
            */
            for (ushort i = 0; i <= rows - 1; i++)
            {
                for (ushort j = 0; j <= rows - 1; j++)
                {
                    // Es el salto en cantidad que hace el primer vertice de una row a otra row
                    var jump = rows + 1;
                    var leftUpperVertex = (ushort) (j + jump * i);
                    var rightUpperVertex = (ushort) (j + jump * i + 1);
                    var leftBottomVertex = (ushort) (jump * (i + 1) + j);
                    var rightBottomVertex = (ushort) (jump * (i + 1) + j + 1);
                    // Triangulo superior
                    indices.Add(leftUpperVertex);
                    indices.Add(rightUpperVertex);
                    indices.Add(leftBottomVertex);
                    // Triangulo inferior
                    indices.Add(rightUpperVertex);
                    indices.Add(rightBottomVertex);
                    indices.Add(leftBottomVertex);
                }
            }
            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Count,
                BufferUsage.None);
            Indices.SetData(indices.ToArray());
        }

        /// <summary>
        ///     Draw the Quad.
        /// </summary>
        /// <param name="world">The world matrix for this box.</param>
        /// <param name="view">The view matrix, normally from the camera.</param>
        /// <param name="projection">The projection matrix, normally from the application.</param>
        public void Draw(Matrix world, Matrix view, Matrix projection, float time)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["Time"].SetValue(time);
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
            graphicsDevice.BlendState = BlendState.Additive;
            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.IndexCount / 3);
            }
            graphicsDevice.BlendState = BlendState.Opaque;
        }
    }
}
