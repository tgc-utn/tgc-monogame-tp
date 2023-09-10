using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP
{    
    public class Roca
    {
        private VertexBuffer vertexBuffer {get;set;}
        private IndexBuffer indexBuffer {get;set;}

        public Effect Effect {get; set;}


        /// <summary>
        /// Crea un cuadrado
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="centro"></param>
        /// <param name="tamañoLado"></param>
        public Roca(GraphicsDevice GraphicsDevice,Vector3 centro, float lado)
        {
            float tamaño = lado/2;

            var vertices = new[]
            {
                //new VertexPositionColor(new Vector3(-5f, -5f, 0f) + centro,Color.Red), //abajo izquierda adelante
                //new VertexPositionColor(new Vector3(-5f, 5f, 0f)  + centro,Color.Red), //abajo izquierda atras
                //new VertexPositionColor(new Vector3(-5f, -5f, 10f)+ centro,Color.Red), //arriba izquierda adelante
                //new VertexPositionColor(new Vector3(-5f, 5f, 10f) + centro,Color.Red), //arriba izquieda atras
                //new VertexPositionColor(new Vector3(5f, -5f, 0f)  + centro,Color.Red), //abajo derecha adelante
                //new VertexPositionColor(new Vector3(5f,  5f, 0f)   + centro,Color.Red), //abajo derecha atras
                //new VertexPositionColor(new Vector3(5f, -5f, 10f) + centro,Color.Red), //arriba izquierda adelante
                //new VertexPositionColor(new Vector3(5f,  5f, 10f)  + centro,Color.Red) //arriba izquierda atras
                new VertexPositionColor(new Vector3(-tamaño, -tamaño, -tamaño) + centro,Color.Red), //abajo izquierda adelante
                new VertexPositionColor(new Vector3(-tamaño, tamaño, -tamaño)  + centro,Color.Red), //abajo izquierda atras
                new VertexPositionColor(new Vector3(-tamaño, -tamaño, tamaño)+ centro,Color.Red), //arriba izquierda adelante
                new VertexPositionColor(new Vector3(-tamaño, tamaño, tamaño) + centro,Color.Red), //arriba izquieda atras
                new VertexPositionColor(new Vector3(tamaño, -tamaño, -tamaño)  + centro,Color.Red), //abajo derecha adelante
                new VertexPositionColor(new Vector3(tamaño,  tamaño, -tamaño)   + centro,Color.Red), //abajo derecha atras
                new VertexPositionColor(new Vector3(tamaño, -tamaño, tamaño) + centro,Color.Red), //arriba izquierda adelante
                new VertexPositionColor(new Vector3(tamaño,  tamaño, tamaño)  + centro,Color.Red) //arriba izquierda atras
            };

            var indices = new ushort[]
            {
                0, 1, 2, //izquierda
                1, 2, 3, 
                2, 3, 6, //arriba
                3, 6, 7,
                0, 4, 2, //adelante
                2, 4, 6,
                0, 1, 4, //abajo
                1, 4, 5,
                4, 5, 6, //derecha
                5, 6, 7,
                1, 3, 5, //atras
                3, 5, 7
                
            };
        
            vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
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
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }

            //graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        }
    }
        


}