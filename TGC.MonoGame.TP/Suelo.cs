using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{    
    public class Suelo
    {
        private VertexBuffer vertexBuffer {get;set;}
        private IndexBuffer indexBuffer {get;set;}

        public Effect Effect {get; set;}

        public Suelo(GraphicsDevice GraphicsDevice)
        {
            var vertices = new[]
            {
                new VertexPositionColor(new Vector3(-20f, -20f, 0f),Color.Red),
                new VertexPositionColor(new Vector3(-20f, 20f, 0f),Color.Red),
                new VertexPositionColor(new Vector3(20f, 20f, 0f),Color.Red),
                new VertexPositionColor(new Vector3(20f, -20f, 0f),Color.Red)
            };

            var indices = new ushort[]
            {
                2, 1, 0,
                0, 2, 3
            };
        
            vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.None);
            indexBuffer.SetData(indices);

        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix View, Matrix Projection)
        {

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

//            Effect.Parameters["World"].SetValue(Matrix.Identity);
//            Effect.Parameters["View"].SetValue(View);
//            Effect.Parameters["Projection"].SetValue(Projection);
//            Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());


            foreach(var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }

            //graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        }
    }
        


}