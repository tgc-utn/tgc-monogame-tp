using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Gemotries.Textures;

namespace TGC.MonoGame.TP.Pistas
{
    public class Pista2
    {
        // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set; }

        // ____ World matrices ____
        // Plataformas principales
        private Matrix Platform1World { get; set; }
        private Matrix Platform2World { get; set; }
        private Matrix Platform3World { get; set; }

        //Plataformas flotantes
        private Matrix[] FloatingPlatformsWorld { get; set; }

        //Obstaculos movibles
        private Matrix[] MovingBox { get; set; }

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
            Platform1World = Matrix.CreateScale(300f, 5f, 500f) * Matrix.CreateTranslation(x, y, z);
            //lista de boxes

        }

        private void LoadContent(ContentManager Content)
        {
            // Cargar Texturas
            Texture2D CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");

            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Draw Platform1
            BoxPrimitive.Draw(Platform1World, view, projection);

        }

    }
}
