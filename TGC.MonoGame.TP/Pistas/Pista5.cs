using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.Pistas
{
    internal class Pista5
    {

        // GraphicsDevice
        private GraphicsDevice GraphicsDevice { get; set; }

        public Pista5(ContentManager Content, GraphicsDevice graphicsDevice, float x, float y, float z)
        {
            GraphicsDevice = graphicsDevice;
            Initialize(x, y, z);

            LoadContent(Content);

        }

        private void Initialize(float x, float y, float z)
        {

            float lastX = x;
            float lastY = y;
            float lastZ = z;
        }

        private void LoadContent(ContentManager Content)
        {
        }
        public void Draw(Matrix view, Matrix projection)
        {
        }
    }
}
