
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Gemotries.Textures;
using Microsoft.Xna.Framework.Graphics;


namespace TGC.MonoGame.TP.Pistas;

public class Pista1
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
        private BoxPrimitive BoxPrimitive { get; set;}
    
    // ____ Textures ____
        private Texture2D CobbleTexture { get; set; }
    
    // ____ World matrices ____
        // Plataformas principales
            private Matrix Platform1World { get; set;}  
            private Matrix Platform2World { get; set;} 
            private Matrix Platform3World { get; set;} 
            
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


// ======== Constructor de Pista 1 ========            
    public Pista1(ContentManager Content,GraphicsDevice graphicsDevice, float x, float y, float z )
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
        
        Platform1World = Matrix.CreateScale(70f, 5f, 500f) * Matrix.CreateTranslation(lastX, lastY, lastZ);
        lastZ += 200f;
        
        // Create World matrices for FloatingPlatformsWorld
        FloatingPlatformsWorld = new Matrix[]
        {
            Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY += 50f), (lastZ += 170f)),
            Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX,(lastY += 50f), (lastZ += 170f)),
            Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX,(lastY += 50f), (lastZ += 170f))
        };

        FloatingMovingPlatformWorld = Matrix.CreateScale(100f, 5f, 100f) *
                                      Matrix.CreateTranslation(lastX, (lastY -= 50f), (lastZ += 200f));
    }
    
// ======== Cargar Contenidos ========
    private void LoadContent(ContentManager Content)
    {
        // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin");
        
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