
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Geometries
{
    internal class GeometriesManager
    {
        //private readonly GraphicsDevice GraphicsDevice;
        internal readonly QuadPrimitive Quad;

        internal GeometriesManager(GraphicsDevice GraphicsDevice)
        {
            //this.GraphicsDevice = GraphicsDevice;

            Quad = new QuadPrimitive(GraphicsDevice);
        }
    }
}