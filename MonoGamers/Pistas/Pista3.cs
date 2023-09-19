using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGamers.Gemotries.Textures;
using System.Configuration;

//"Perdoname, plataformas de salto!"

namespace MonoGamers.Pistas;
public class Pista3
{ 
    
// ======== DECALRACION DE VARIABES ========
    
    // ____ Colliders ____

    // Bounding Boxes (for all our models)
        private BoundingBox[] Colliders { get; set; }    
    
    // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set;}
    
    // ____ Textures ____
        private Texture2D CobbleTexture { get; set; }
    
    // ____ World matrices ____
        // Plataformas principales
            private Matrix Platform1World { get; set;}  
            private Matrix Platform2World { get; set;}
            
        // Floating Platforms
            private Matrix[] FloatingPlatformsWorld { get; set; }
            
        // FloatingMovingPlatform (Vertical Movement)
            private Matrix FloatingMovingPlatformWorld { get; set;}
            
        // AnnoyingWalls
            private Matrix[] AnnoyingWallsWorld { get; set; }
            
        // AnnoyingMovingWalls
            private Matrix[] AnnoyingMovingWallsWorld { get; set; }
    
        // BoxWall
            private Matrix[] BoxesWorld { get; set; }
    
        // GraphicsDevice
            private GraphicsDevice GraphicsDevice { get; set; }


// ======== Constructor de Pista 3 ========            
    public Pista3(ContentManager Content,GraphicsDevice graphicsDevice, float x, float y, float z )
    {
        GraphicsDevice = graphicsDevice;
        Initialize(x, y, z);
        
        LoadContent(Content);

    }
    
// ======== Inicializar matrices ========       
    private void Initialize(float x, float y, float z)
    {

        float lastX = x;
        float lastY = y;
        float lastZ = z;
        
        Platform1World = Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation(lastX, lastY, lastZ);
        lastZ += 200f;
        
        // Create World matrices for FloatingPlatformsWorld
        FloatingPlatformsWorld = new Matrix[]
        {
            Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation((lastX += 100f), lastY, (lastZ -= 200f)),
            Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation((lastX += 100f), lastY, lastZ),
            Matrix.CreateScale(400f, 5f, 400f) * Matrix.CreateTranslation((lastX += 275f),lastY, lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 100f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ -= 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ -= 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX -= 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX -= 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX -= 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX -= 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ += 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ += 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ += 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ += 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 75f), (lastY += 10f), lastZ),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ -= 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation(lastX, (lastY += 10f), (lastZ -= 75f)),
            Matrix.CreateScale(50f, 5f, 50f) * Matrix.CreateTranslation((lastX += 100f), (lastY += 10f), (lastZ)),
            Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation((lastX += 100f), lastY, lastZ),
            Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation((lastX += 100f), lastY, lastZ),
            Matrix.CreateScale(200f, 5f, 200f) * Matrix.CreateTranslation((lastX += 150f),lastY, lastZ)
        };

        //FloatingMovingPlatformWorld = Matrix.CreateScale(100f, 5f, 100f) *
                                   //   Matrix.CreateTranslation(lastX, (lastY -= 50f), (lastZ += 200f));
    }
    
// ======== Cargar Contenidos ========
    private void LoadContent(ContentManager Content)
    {
        // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");
        
        // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
    }
    
    
// ======== Dibujar Modelos ========  
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