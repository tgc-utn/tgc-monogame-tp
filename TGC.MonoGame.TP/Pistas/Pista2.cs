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
        private Model RushModel { get; set; }
        private Matrix[] RushPowerups { get; set; }

        //Signs

        private Model SignModel { get; set; }
        private Matrix[] Signs { get; set; }

        //Sonic

        private Model SonicModel { get; set; }  
        private Matrix Sonic { get; set; }

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
            //powerUps

            Matrix basicRush = Matrix.CreateScale(0.2f, 0.2f, 0.2f) * Matrix.CreateRotationX(1.5707f);
            RushPowerups = new Matrix[]
            {
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x, y + 30f, z + 210f),
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x, y + 30f, z + 720f),
                basicRush * Matrix.CreateTranslation(x + 1200f, y + 130f, z + 1350f),
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x + 1500f, y + 150f, z + 2000f)
            };


            //lista de plataformas
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
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 2150f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1900f, y + 140f, z + 2150f),

            };

            //Cajas movibles
            Boxes = new Matrix[]
            {
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x - 150f, y + 50f, z + 830f),
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x + 150f, y + 50f, z + 870f)
            };

            //logica inicial para calcular el movimiento
            box1InitialXPosition = Boxes[0].Translation.X;

            MovingRight = true;

            //Signs 

            Signs = new Matrix[]
            {
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 45f, y - 130f, z+210f),
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateTranslation(x + 1080f, y + 45f, z+1380f),
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 1430, y - 10f, z+2010f)
            };

            //Sonic
            Sonic = Matrix.CreateScale(2f, 2f, 2f) *
                Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 20f, y, z+100f);
        }

        private void LoadContent(ContentManager Content)
        {
            // Cargar Texturas
            Texture2D CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");

            //Carga modelo Rush
            RushModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "arrowpush/tinker") ;
            foreach (var mesh in RushModel.Meshes)
               ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

            //Carga modelo sign
            SignModel = Content.Load<Model>("Models/signs/warningSign/untitled");
            foreach (var mesh in SignModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

            //Carga modelo Sonic
            SonicModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3D"] + "/sonic/source/sonic");
            foreach (var mesh in SonicModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

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
            Array.ForEach(RushPowerups, PowerUp => RushModel.Draw(PowerUp, view, projection));
            Array.ForEach(Signs, Sign => SignModel.Draw(Sign, view, projection));
            SonicModel.Draw(Sonic, view, projection);
        }

    }
}
