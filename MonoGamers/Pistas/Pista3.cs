using System.Linq;
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGamers.Geometries.Textures;
using NumericVector3 = System.Numerics.Vector3;
using BepuPhysics.Collidables;
using MonoGamers.Utilities;
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

            // Floating Platforms
            private Matrix[] FloatingPlatformsWorld { get; set; }
            
        // FloatingMovingPlatform (Vertical Movement)
            private Matrix FloatingMovingPlatformWorld { get; set;}
            

        // GraphicsDevice
            private GraphicsDevice GraphicsDevice { get; set; }
    
        // Simulation           
            private Simulation Simulation { get; set; }

        // Tiling Effect
            private Effect TilingEffect { get; set; }  
            
// ======== Constructor de Pista 3 ========            
    public Pista3(ContentManager Content,GraphicsDevice graphicsDevice, float x, float y, float z, Simulation simulation )
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
        
        Platform1World = Matrix.CreateScale(75f, 5f, 75f) * Matrix.CreateTranslation(lastX, lastY, lastZ);
        Platform1World.Decompose(out scale, out rot, out translation);
        Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
            Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));
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

        for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
        {
            var matrix = FloatingPlatformsWorld[index];
            matrix.Decompose(out scale, out rot, out translation);
            Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));

        }
    }
    
// ======== Cargar Contenidos ========
    private void LoadContent(ContentManager Content)
    {
        // Load our Tiling Effect
            TilingEffect = Content.Load<Effect>(ConfigurationManager.AppSettings["ContentFolderEffects"] + "TextureTiling");
        // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");
        
        // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
    }
    
    
// ======== Dibujar Modelos ========  
    public void Draw(Matrix view, Matrix projection)
    {
        var viewProjection = view * projection;
        // Set the Technique inside the TilingEffect to "BaseTiling"
        TilingEffect.CurrentTechnique = TilingEffect.Techniques["BaseTiling"]; // Using its original Texture Coordinates
        TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 3f));
        TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);    

        // Draw Platform1
            TilingEffect.Parameters["WorldViewProjection"].SetValue(Platform1World * viewProjection);
            BoxPrimitive.Draw(TilingEffect);
        
        // Draw Floating Platforms
        for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
        {
            var matrix = FloatingPlatformsWorld[index];
            TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
            BoxPrimitive.Draw(TilingEffect);


        }
        
        // Draw Platform1
            BoxPrimitive.Draw(FloatingMovingPlatformWorld, view, projection);
    }
}