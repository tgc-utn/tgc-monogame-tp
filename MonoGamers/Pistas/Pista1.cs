
using System.Linq;
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Geometries.Textures;
using NumericVector3 = System.Numerics.Vector3;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using MonoGamers.Utilities;


namespace MonoGamers.Pistas;

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
        private BoxPrimitive BoxPrimitiveCobble { get; set;}
        private BoxPrimitive BoxPrimitiveWooden { get; set;}
    
    // ____ Textures ____
        private Texture2D CobbleTexture { get; set; }
        private Texture2D WoodenTexture { get; set; }
        
    
    // ____ World matrices ____
        // Plataformas principales
            private Matrix Platform1World { get; set;}  
            private Matrix Platform2World { get; set;} 
            private Matrix Platform3World { get; set;} 
            
        // Floating Platforms
            private Matrix[] FloatingPlatformsWorld { get; set; }
            private Matrix[] FloatingPlatforms2World { get; set; }
            
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
            
         // Simulation           
            private Simulation Simulation { get; set; }
            

// ======== Constructor de Pista 1 ========            
    public Pista1(ContentManager Content,GraphicsDevice graphicsDevice, float x, float y, float z, Simulation simulation )
    {
        GraphicsDevice = graphicsDevice;
        Simulation = simulation;

        Initialize(x, y, z);
        
        
        LoadContent(Content);

    }
    
// ======== Inicializar matrices ========       
    private void Initialize(float x, float y, float z)
    {

        float lastX = x;
        float lastY = y;
        float lastZ = z;
        Vector3 scale;
        Quaternion rot;
        Vector3 translation;
        
        // Create World matrix for Platform1World
            Platform1World = Matrix.CreateScale(70f, 5f, 500f) * Matrix.CreateTranslation(lastX, lastY, lastZ);
            Platform1World.Decompose(out scale, out rot, out translation);
            Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));
            lastZ += 200f;

        
        // Create World matrices for FloatingPlatformsWorld AND add to Simulation
            FloatingPlatformsWorld = new Matrix[]
            {
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY += 50f), (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX,(lastY += 50f), (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX,(lastY += 50f), (lastZ += 170f))
            };
            
            for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
            {
                var matrix = FloatingPlatformsWorld[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                    Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));

            }
    
        // Create World matrix for FloatingMovingPlatformWorld
            FloatingMovingPlatformWorld = Matrix.CreateScale(100f, 5f, 100f) *
                                      Matrix.CreateTranslation(lastX, (lastY -= 50f), (lastZ += 200f));
            
            FloatingMovingPlatformWorld.Decompose(out scale, out rot, out translation);
            Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));
        
        // Create World matrix for Platform2World
            Platform2World = Matrix.CreateScale(200f, 5f, 1400f) * Matrix.CreateTranslation(lastX, (lastY = y), (lastZ += 800f));
            lastZ -= 700f;
            lastY += 2.5f;
            
            Platform2World.Decompose(out scale, out rot, out translation);
            Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));
            
        // Create World matrices for AnnoyingWallsWorld
            AnnoyingWallsWorld = new Matrix[]
                {
                    Matrix.CreateScale(100f, 250f, 25f) * Matrix.CreateTranslation((lastX - 50f), (lastY + 125f), (lastZ += 12.5f)),
                    Matrix.CreateScale(55f, 100f, 25f) * Matrix.CreateTranslation((lastX - 70f), (lastY + 50f), (lastZ += 150f)),
                    Matrix.CreateScale(55f, 100f, 25f) * Matrix.CreateTranslation((lastX + 70f), (lastY + 50f), lastZ),
                    Matrix.CreateScale(80f, 100f, 25f) * Matrix.CreateTranslation(lastX, (lastY + 50f), (lastZ += 150f)),
                    Matrix.CreateScale(55f, 100f, 25f) * Matrix.CreateTranslation((lastX - 70f), (lastY + 50f), (lastZ += 150f)),
                    Matrix.CreateScale(55f, 100f, 25f) * Matrix.CreateTranslation((lastX + 70f), (lastY + 50f), lastZ),
                    Matrix.CreateScale(80f, 100f, 25f) * Matrix.CreateTranslation(lastX, (lastY + 50f), (lastZ += 150f))
                };

            for (int index = 0; index < AnnoyingWallsWorld.Length; index++)
            {
                var matrix = AnnoyingWallsWorld[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation ),
                    Simulation.Shapes.Add(new Box(scale.X,scale.Y, scale.Z))));

            }
        
        // Create World matrices for AnnoyingMovingWalls
            AnnoyingMovingWallsWorld = new Matrix[]
            {
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), (lastZ += 150f)),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), (lastZ += 150f)),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), lastZ),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), lastZ),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), lastZ),
            };
            
            
        // Create World matrices for FloatingPlatforms2World
            FloatingPlatforms2World = new Matrix[]
            {
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY - 100f), (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY - 50f), (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, lastY, (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY + 50f), (lastZ += 170f)),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(lastX, (lastY + 100f), (lastZ += 170f)),
            };
            
            for (int index = 0; index < FloatingPlatforms2World.Length; index++)
            {
                var matrix = FloatingPlatforms2World[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation ),
                    Simulation.Shapes.Add(new Box(scale.X,scale.Y, scale.Z))));

            }
            lastZ += 170f;
            
        // Create World matrix for Platform2World
            Platform3World = Matrix.CreateScale(200f, 5f, 500f) * Matrix.CreateTranslation(lastX, (lastY = y), (lastZ += 250f));
            Platform3World.Decompose(out scale, out rot, out translation);
            Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));


            lastZ -= 250f;
            lastY += 2.5f;

        // Create World matrices for FloatingPlatformsWorld
            lastZ += 25f; 
            
            BoxesWorld = new Matrix[]
            {
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 75f), (lastY += 25f), lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 75f), lastY, lastZ),
                
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 25f), lastY, lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 25f), lastY, lastZ),
                
                
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 75f), (lastY += 50f), lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 75f), lastY, lastZ),
                
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 75f), (lastY += 50f), lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 75f), lastY, lastZ),
                
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 75f), (lastY += 50f), lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 75f), lastY, lastZ),


                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX + 25f), lastY, lastZ),
                Matrix.CreateScale(50f, 50f, 50f) * Matrix.CreateTranslation((lastX - 25f), lastY, lastZ),
                
            };       
            for (int index = 0; index < BoxesWorld.Length; index++)
            {
                var matrix = BoxesWorld[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation ),
                    Simulation.Shapes.Add(new Box(scale.X,scale.Y, scale.Z))));

            }
    }
    
// ======== Cargar Contenidos ========
    private void LoadContent(ContentManager Content)
    {
        // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin");
            WoodenTexture = Content.Load<Texture2D>(ContentFolderTextures + "wood/caja-madera-1");
        
        // Cargar Primitiva de caja con textura
            BoxPrimitiveCobble = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
            BoxPrimitiveWooden = new BoxPrimitive(GraphicsDevice, Vector3.One, WoodenTexture);
    }
    
    
// ======== Dibujar Modelos ========  
    public void Draw(Matrix view, Matrix projection)
    {
        // Draw Platform1
            BoxPrimitiveCobble.Draw(Platform1World, view, projection);
        
        // Draw Floating Platforms
            for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
            {
                var matrix = FloatingPlatformsWorld[index];
                BoxPrimitiveCobble.Draw(matrix, view, projection);

            }
        
        // Draw Platform1
            BoxPrimitiveCobble.Draw(FloatingMovingPlatformWorld, view, projection);
            
        // Draw Platform2
            BoxPrimitiveCobble.Draw(Platform2World, view, projection);    
        
        // Draw AnnoyingWallsWorld
            for (int index = 0; index < AnnoyingWallsWorld.Length; index++)
            {
                var matrix = AnnoyingWallsWorld[index];
                BoxPrimitiveCobble.Draw(matrix, view, projection);

            }
            
        // Draw AnnoyingMovingWallsWorld
            for (int index = 0; index < AnnoyingMovingWallsWorld.Length; index++)
            {
                var matrix = AnnoyingMovingWallsWorld[index];
                BoxPrimitiveCobble.Draw(matrix, view, projection);

            }
            
        // Draw Floating Platforms 2
            for (int index = 0; index < FloatingPlatforms2World.Length; index++)
            {
                var matrix = FloatingPlatforms2World[index];
                BoxPrimitiveCobble.Draw(matrix, view, projection);

            }    
            
        // Draw Platform3
            BoxPrimitiveCobble.Draw(Platform3World, view, projection);    

        // Draw BoxesWorld
            for (int index = 0; index < BoxesWorld.Length; index++)
            {
                var matrix = BoxesWorld[index];
                BoxPrimitiveWooden.Draw(matrix, view, projection);

            }   
        
            
            
            
    }
}