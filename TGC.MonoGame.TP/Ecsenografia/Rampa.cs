using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Escenografia;

public class Rampa : Escenografia3D
{
    private GraphicsDevice _graphicsDevice;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;
    private Vector3 _dimensiones;

    public Rampa(GraphicsDevice graphicsDevice, Vector3 dimensiones)
    {
        _graphicsDevice = graphicsDevice;
        _dimensiones = dimensiones;
        CrearPrismaRectangular(dimensiones);
    }

    public void SetEffect (Effect effect){
        this.efecto = effect;
    }

    private void CrearPrismaRectangular(Vector3 dimensiones)
    {
        // Definir vértices del prisma
        var vertices = new VertexPosition[6];

        float X = dimensiones.X / 2;
        float Y = dimensiones.Y;
        float Z = dimensiones.Z / 2;

        // Vértices del prisma
        vertices[0] = new VertexPosition(new Vector3(X, 0, Z));  // Frente inferior izquierdo
        vertices[1] = new VertexPosition(new Vector3(X, 0, -Z)) ;  // Frente inferior derecho
        vertices[2] = new VertexPosition(new Vector3(-X, 0, Z)) ;  // Frente superior izquierdo
        vertices[3] = new VertexPosition(new Vector3(-X, 0, -Z))  ; // Frente superior derecho
        vertices[4] = new VertexPosition(new Vector3(X, Y, Z)) ;  // Atrás inferior izquierdo
        vertices[5] = new VertexPosition(new Vector3(X, Y, -Z))  ; // Atrás inferior derecho

        // Definir índices para los triángulos
        int[] indices = new int[]
        {
            //Abajo
            0,1,2,1,2,3,
            
            // Cara Izquierda
            0,4,2,
            
            // Cara Derecha
            1,3,5,
            
            //Cara Superior
            0,1,4,1,4,5,
            
            //Cara Atras
            2,3,4,3,4,5
        };

        // Crear y configurar los buffers
        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPosition), vertices.Length, BufferUsage.WriteOnly);
        _vertexBuffer.SetData(vertices);

        _indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
        _indexBuffer.SetData(indices);
    }

    public override Matrix getWorldMatrix()
    {
        return world;
    }

    public override void dibujar(Matrix view, Matrix projection, Color color)
    {
        efecto.Parameters["World"].SetValue(world);
        efecto.Parameters["View"].SetValue(view);
        efecto.Parameters["Projection"].SetValue(projection);
        efecto.Parameters["DiffuseColor"].SetValue(color.ToVector3());

        _graphicsDevice.SetVertexBuffer(_vertexBuffer);
        _graphicsDevice.Indices = _indexBuffer;

        foreach (var pass in efecto.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount / 3);
        }
    }
}