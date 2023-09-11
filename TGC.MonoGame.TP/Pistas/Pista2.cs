using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Gemotries.Textures;
using System;

namespace TGC.MonoGame.TP.Pistas
{
    public class Pista2
    {
        // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set; }

        // ____ World matrices ____
        // Plataformas principales
        private Matrix[] Platforms { get; set; }

        //Obstaculos movibles
        private Matrix[] Boxes { get; set; }
        private bool MovingRight { get; set; }
        private float box1InitialXPosition;

        //Rush powerup
        private Matrix RushPowerup { get; set; }

        // GraphicsDevice
        private GraphicsDevice GraphicsDevice { get; set; }

        public Pista2(ContentManager Content, GraphicsDevice graphicsDevice, float x, float y, float z)
        {
            GraphicsDevice = graphicsDevice;
            Initialize(x, y, z);

            LoadContent(Content);

        }

        private void Initialize(float x, float y, float z)
        {
            //lista de boxes
            Platforms = new Matrix[]
            {
                Matrix.CreateScale(300f, 5f, 500f) * Matrix.CreateTranslation(x, y, z),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 600f),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 1100f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x, y, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 100f, y + 45f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 200f, y + 90f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 300f, y + 135f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 400f, y + 170f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 550f, y + 170f, z + 1350f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 850f, y + 170f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1200f, y + 100f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 1450f, y + 100f, z + 1350f),
                Matrix.CreateScale(500f, 5f, 500f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 1800f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 2250f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1900f, y + 140f, z + 2250f),

            };

            Boxes = new Matrix[]
            {
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x - 150f, y + 50f, z + 830f),
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x + 150f, y + 50f, z + 870f)
            };

            box1InitialXPosition = Boxes[0].Translation.X;

            MovingRight = true;
        }

        private void LoadContent(ContentManager Content)
        {
            // Cargar Texturas
            Texture2D CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");

            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
        }

        public void Update(float elapsedSeconds)
        {
            float box1XPosition = Boxes[0].Translation.X;
            float box1YPosition = Boxes[0].Translation.Y;
            float box1ZPosition = Boxes[0].Translation.Z;
            float box2XPosition = Boxes[1].Translation.X;
            float box2YPosition = Boxes[1].Translation.Y;
            float box2ZPosition = Boxes[1].Translation.Z;
            if (MovingRight)
            {
                box1XPosition += 70f * elapsedSeconds;
                box2XPosition -= 70f * elapsedSeconds;
                Boxes[0] = Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(box1XPosition, box1YPosition, box1ZPosition);
                Boxes[1] = Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(box2XPosition, box2YPosition, box2ZPosition);
                if (box1XPosition >= box1InitialXPosition + 300f) MovingRight = false;
            }
            else
            {
                box1XPosition -= 70f * elapsedSeconds;
                box2XPosition += 70f * elapsedSeconds;
                Boxes[0] = Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(box1XPosition, box1YPosition, box1ZPosition);
                Boxes[1] = Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(box2XPosition, box2YPosition, box2ZPosition);
                if (box1XPosition <= box1InitialXPosition) MovingRight = true;
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Array.ForEach(Platforms, Platform => BoxPrimitive.Draw(Platform, view, projection));
            Array.ForEach(Boxes, Box => BoxPrimitive.Draw(Box, view, projection));

        }

    }
}
