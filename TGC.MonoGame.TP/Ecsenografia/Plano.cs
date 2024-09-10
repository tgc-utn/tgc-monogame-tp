using Escenografia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Escenografia
{
    public class Plano : Escenografia3D, System.IDisposable{
        private GraphicsDevice _graphicsDevice;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        public Plano(GraphicsDevice graphicsDevice, Vector3 posicion)
        {
            this.posicion = posicion;
            _graphicsDevice = graphicsDevice;
            CreatePlaneMesh(16, 16);  // Crea un plano con 8x8 cuadrículas, es decir, 64 triángulos.
        }

        public void SetEffect (Effect effect){
            this.efecto = effect;
        }

        private void CreatePlaneMesh(int width, int height)
        {
            int numeroVertices = (width + 1) * (height + 1);
            VertexPosition[] vertices = new VertexPosition[numeroVertices];

            // Cada cuadrado tiene 2 triángulos, y cada triángulo tiene 3 índices.
            int numeroIndices = width * height * 6;
            int[] indices = new int[numeroIndices];

            // Crear los vértices.
            int indiceVertice = 0;
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    float posX = x;
                    float posY = 0;  // altura del plano
                    float posZ = y;

                    vertices[indiceVertice] = new VertexPosition(
                        new Vector3(posX, posY, posZ)  // Posición del vértice
                    );
                    indiceVertice++;
                }
            }

            // Crear los índices.
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int topLeft = (y * (width + 1)) + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + (width + 1);
                    int bottomRight = bottomLeft + 1;

                    // Primer triángulo
                    indices[index++] = topLeft;
                    indices[index++] = bottomRight;
                    indices[index++] = bottomLeft;

                    // Segundo triángulo
                    indices[index++] = topLeft;
                    indices[index++] = topRight;
                    indices[index++] = bottomRight;
                }
            }

            // Crear el VertexBuffer y el IndexBuffer.
            _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPosition), vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }

        public override void dibujar(Matrix view, Matrix projection, Color color)
        {
            // Establece los parámetros de transformación en el Effect.
            efecto.Parameters["World"].SetValue(getWorldMatrix());
            efecto.Parameters["View"].SetValue(view);
            efecto.Parameters["Projection"].SetValue(projection);

            // Configuración del color
            efecto.Parameters["DiffuseColor"].SetValue(color.ToVector3());

            // Establecer los buffers.
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _graphicsDevice.Indices = _indexBuffer;

            foreach (var pass in efecto.CurrentTechnique.Passes)
            {
                pass.Apply();  // Aplica la técnica del Effect.
                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount / 3);
            }
        }

        public override void loadModel(string direcionModelo, string direccionEfecto, ContentManager contManager)
        {
            base.loadModel(direcionModelo, direccionEfecto, contManager);
            foreach ( ModelMesh mesh in modelo.Meshes )
                {
                    foreach ( ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = efecto;
                    }
                }
        }
        public override Matrix getWorldMatrix()
        {   
            //posicion = new Vector3(0, -5, 0);

            return Matrix.CreateScale(1000f) * Matrix.CreateTranslation(posicion);

        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
