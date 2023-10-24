using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Helpers.Gizmos.Geometries;

using System;
using Microsoft.Xna.Framework.Graphics;

public abstract class GeometryPrimitive{
    internal VertexBuffer Vertices { get; set; }
    internal IndexBuffer Indices { get; set; }

    internal GeometryPrimitive(GraphicsDeviceManager graphicsDevice){   
        CreateVertexBuffer(graphicsDevice);
        CreateIndexBuffer(graphicsDevice);
    }
    internal abstract void CreateVertexBuffer(GraphicsDeviceManager graphicsDevice);
    internal abstract void CreateIndexBuffer(GraphicsDeviceManager graphicsDevice);
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