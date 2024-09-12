using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Escenografia;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP;
using Microsoft.Xna.Framework.Content;

public class Cilindro : Escenografia3D
{
    private GraphicsDevice _graphicsDevice;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;

    private const int Segmentos = 36;
    float scale;
    public Cilindro(GraphicsDevice graphicsDevice, float unRadio, float unAlto)
    {
        _graphicsDevice = graphicsDevice;
        CrearCilindro(unRadio, unAlto);
    }

    public void SetEffect(Effect effect)
    {
        this.efecto = effect;
    }

        public void SetScale(float scale)
        {
            this.scale = scale;
        }

    private void CrearCilindro(float unRadio, float unAlto)
    {
        var vertices = new List<VertexPosition>();
        var indices = new List<int>();
        float radio = unRadio;
        float alto = unAlto;

        //Crear vértices para la base superior e inferior.
        for (int i = 0; i < Segmentos; i++)
        {
            float angle = MathHelper.ToRadians(i * 360f / Segmentos);
            float x = radio * (float)Math.Cos(angle);
            float z = radio * (float)Math.Sin(angle);

            // Vértices de la base inferior
            vertices.Add(new VertexPosition(new Vector3(x, 0, z)));

            // Vértices de la base superior
            vertices.Add(new VertexPosition(new Vector3(x, alto, z)));
        }

        //Crear índices
        for (int i = 0; i < Segmentos; i++)
        {
            int next = (i + 1) % Segmentos;

            // Lateral
            indices.Add(i * 2);
            indices.Add(next * 2);
            indices.Add(i * 2 + 1);

            indices.Add(next * 2);
            indices.Add(next * 2 + 1);
            indices.Add(i * 2 + 1);

            // Base inferior
            indices.Add(0);
            indices.Add(i * 2);
            indices.Add(next * 2);

            // Base superior
            indices.Add(1);
            indices.Add(next * 2 + 1);
            indices.Add(i * 2 + 1);
        }

        // Crear Vertex Buffer
        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPosition), vertices.Count, BufferUsage.WriteOnly);
        _vertexBuffer.SetData(vertices.ToArray());

        // Crear Index Buffer
        _indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
        _indexBuffer.SetData(indices.ToArray());
    }



    public override void dibujar(Matrix view, Matrix projection, Color color)
    {
        // Configurar el shader y matrices
        efecto.Parameters["World"].SetValue(getWorldMatrix());
        efecto.Parameters["View"].SetValue(view);
        efecto.Parameters["Projection"].SetValue(projection);
        efecto.Parameters["DiffuseColor"].SetValue(color.ToVector3());

        //Configurar el buffer de vértices
        _graphicsDevice.SetVertexBuffer(_vertexBuffer);

        // Configurar el buffer de índices
        _graphicsDevice.Indices = _indexBuffer;

        // Dibujar el cilindro
        foreach (var pass in efecto.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount / 3);
        }
    }

    public void SetPosition(Vector3 unaPosicion){
        posicion = unaPosicion;
    }

    public void SetRotacion(float rotacionX, float rotacionY, float rotacionZ)
    {
        this.rotacionX = rotacionX;
        this.rotacionY = rotacionY;
        this.rotacionZ = rotacionZ;
    }


    public override Matrix getWorldMatrix()
    {
        return Matrix.CreateRotationX(rotacionX) *
        Matrix.CreateRotationZ(rotacionZ)*
        Matrix.CreateRotationY(rotacionY)*
         Matrix.CreateScale(scale) * 
         Matrix.CreateTranslation(posicion);
    }



    public override void loadModel(string direccionModelo, string direccionEfecto, ContentManager contManager)
    {
        base.loadModel(direccionModelo, direccionEfecto, contManager);
        foreach (ModelMesh mesh in modelo.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                meshPart.Effect = efecto;
            }
        }
    }

}
