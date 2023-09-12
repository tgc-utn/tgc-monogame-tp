using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGamers.Gemotries.Textures;

namespace MonoGamers.Pistas;

public class Pista4
{

    // ======== DECALRACION DE VARIABES ========

    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";

    // ____ Colliders ____

    // Bounding Boxes (for all our models)
    private BoundingBox[] Colliders { get; set; }

    // _____ Geometries _______
    private BoxPrimitive BoxPrimitive { get; set; }

    // ____ Textures ____
    private Texture2D CobbleTexture { get; set; }

    // ____ World matrices ____
    // Plataformas principales
    private Matrix Platform1World { get; set; }
    private Matrix Platform2World { get; set; }

    // Floating Platforms
    private Matrix[] FloatingPlatformsWorld { get; set; }

    // FloatingMovingPlatform (Vertical Movement)
    private Matrix FloatingMovingPlatformWorld { get; set; }

    // AnnoyingWalls
    private Matrix[] AnnoyingWallsWorld { get; set; }

    // AnnoyingMovingWalls
    private Matrix[] AnnoyingMovingWallsWorld { get; set; }

    // BoxWall
    private Matrix[] BoxesWorld { get; set; }

    // GraphicsDevice
    private GraphicsDevice GraphicsDevice { get; set; }


    // ======== Constructor de Pista 4 ========
    public Pista4(ContentManager Content, GraphicsDevice graphicsDevice, float x, float y, float z)
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

        // Plataforma 1
        Platform1World = Matrix.CreateScale(150f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY, lastZ);

        FloatingPlatformsWorld = new Matrix[]
        {
            
            Matrix.CreateScale(125f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY += 7f, lastZ += 60f),
            Matrix.CreateScale(100f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY += 7f, lastZ += 60f),
            Matrix.CreateScale(75f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY += 7f, lastZ += 60f),
            Matrix.CreateScale(50f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY += 7f, lastZ += 60f),
            Matrix.CreateScale(25f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY += 7f, lastZ += 60f),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX, lastY += 5f, lastZ += 40f),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX += 30f, lastY += 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX, lastY += 5f, lastZ -= 30f),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX, lastY += 5f, lastZ -= 30f),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX -= 30f, lastY += 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX -= 30f, lastY -= 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX -= 40f, lastY -= 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX -= 30f, lastY -= 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 25f) * Matrix.CreateTranslation(lastX -= 30f, lastY -= 5f, lastZ),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX, lastY -= 5f, lastZ += 20f),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX, lastY -= 5f, lastZ += 20f),
            Matrix.CreateScale(25f, 2f, 10f) * Matrix.CreateTranslation(lastX, lastY -= 5f, lastZ += 20f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX += 12f, lastY, lastZ += 17f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX += 12f, lastY, lastZ += 12f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX -= 12f, lastY, lastZ += 12f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX -= 12f, lastY, lastZ += 12f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX -= 12f, lastY, lastZ += 12f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX, lastY += 3f, lastZ += 12f),
            Matrix.CreateScale(8f, 2f, 8f) * Matrix.CreateTranslation(lastX, lastY += 3f, lastZ += 12f),
            Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(lastX, lastY += 3f, lastZ += 12f),
            Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(lastX, lastY, lastZ += 12f),
            Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(lastX, lastY, lastZ += 12f),
            Matrix.CreateScale(50f, 2f, 50f) * Matrix.CreateTranslation(lastX, lastY, lastZ += 35f),
            
        };

    }

    private void LoadContent(ContentManager Content)
    {
        // Cargar Texturas
        CobbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin");

        // Cargar Primitiva de caja con textura
        BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);/*  */
    }

    public void Draw(Matrix view, Matrix projection)
    {
        // Draw Platform1
        BoxPrimitive.Draw(Platform1World, view, projection);

        // Draw Floating Platforms
        for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
        {
            var matrix = FloatingPlatformsWorld[index];
            BoxPrimitive.Draw(matrix, view, projection);

        }

        // Draw Platform1
        BoxPrimitive.Draw(FloatingMovingPlatformWorld, view, projection);
    }
}

