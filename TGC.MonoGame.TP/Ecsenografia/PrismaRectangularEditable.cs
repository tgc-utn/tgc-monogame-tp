using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Escenografia;

public class PrismaRectangularEditable : Escenografia3D
{
    private GraphicsDevice _graphicsDevice;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;
    private Vector3 _dimensiones;

    public PrismaRectangularEditable(GraphicsDevice graphicsDevice, Vector3 dimensiones)
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
        var vertices = new VertexPositionNormalTexture[8];

        float halfX = dimensiones.X / 2;
        float halfY = dimensiones.Y / 2;
        float halfZ = dimensiones.Z / 2;

        // Vértices del prisma
        vertices[0] = new VertexPositionNormalTexture(new Vector3(-halfX, -halfY, -halfZ), Vector3.Up, Vector2.Zero);  // Frente inferior izquierdo
        vertices[1] = new VertexPositionNormalTexture(new Vector3(halfX, -halfY, -halfZ), Vector3.Up, Vector2.UnitX);   // Frente inferior derecho
        vertices[2] = new VertexPositionNormalTexture(new Vector3(-halfX, halfY, -halfZ), Vector3.Up, Vector2.UnitY);   // Frente superior izquierdo
        vertices[3] = new VertexPositionNormalTexture(new Vector3(halfX, halfY, -halfZ), Vector3.Up, Vector2.One);      // Frente superior derecho
        vertices[4] = new VertexPositionNormalTexture(new Vector3(-halfX, -halfY, halfZ), Vector3.Up, Vector2.Zero);    // Atrás inferior izquierdo
        vertices[5] = new VertexPositionNormalTexture(new Vector3(halfX, -halfY, halfZ), Vector3.Up, Vector2.UnitX);    // Atrás inferior derecho
        vertices[6] = new VertexPositionNormalTexture(new Vector3(-halfX, halfY, halfZ), Vector3.Up, Vector2.UnitY);    // Atrás superior izquierdo
        vertices[7] = new VertexPositionNormalTexture(new Vector3(halfX, halfY, halfZ), Vector3.Up, Vector2.One);       // Atrás superior derecho

        // Definir índices para los triángulos
        int[] indices = new int[]
        {
            // Frente
            0, 2, 1, 1, 2, 3,
            // Atrás
            5, 7, 4, 4, 7, 6,
            // Izquierda
            4, 6, 0, 0, 6, 2,
            // Derecha
            1, 3, 5, 5, 3, 7,
            // Arriba
            2, 6, 3, 3, 6, 7,
            // Abajo
            0, 1, 4, 4, 1, 5
        };

        // Crear y configurar los buffers
        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
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
        efecto.Parameters["View"].SetValue(view);
        efecto.Parameters["Projection"].SetValue(projection);
        efecto.Parameters["World"].SetValue(world);
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