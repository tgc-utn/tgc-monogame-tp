
using System.Configuration;
using System.Data;
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
    // ____ Colliders ____

    // Bounding Boxes (for all our models)
        private BoundingBox[] Colliders { get; set; }    
    
    // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set;}

        // ____ Textures ____
        private Texture2D CobbleTexture { get; set; }
        private Texture2D WoodenTexture { get; set; }
        
    // Effects

        // Tiling Effect
        private Effect TilingEffect { get; set; }
        
    
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
            public BodyHandle FloatingMovingPlatformBodyHandle {get;set;}

            private float InitialYPositionFloatingMovingPlatform;

            private float HeightDistanceDifference = 40f;
            private Vector3 FloatingMovingPlatformScale { get; set; }
            
            
        // AnnoyingWalls
            private Matrix[] AnnoyingWallsWorld { get; set; }
            
        // AnnoyingMovingWalls
            private Matrix[] AnnoyingMovingWallsWorld { get; set; }
            private float InitialLeftXPositionAnnoyingMovingWalls;
            private float InitialRigthXPositionAnnoyingMovingWalls;
            private float LengthDistanceDifference = 250f;
            private BodyHandle[] AnnoyingMovingWallsBodyHandle { get; set; }
            private Vector3 AnnoyingMovingWallsScale { get; set; }
            
    
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
                                      Matrix.CreateTranslation(lastX, (lastY += 50f), (lastZ += 200f));

            InitialYPositionFloatingMovingPlatform = lastY;
            
            FloatingMovingPlatformWorld.Decompose(out scale, out rot, out translation);
            
            System.Numerics.Quaternion rotation = new System.Numerics.Quaternion(rot.X, rot.Y, rot.Z, rot.W);
            
            var initialPos = new RigidPose(Utils.ToNumericVector3(translation), rotation);
            
            FloatingMovingPlatformScale = scale;
            
            var floatingMovingPlatformShape = new Box(scale.X, scale.Y, scale.Z);
            
            FloatingMovingPlatformBodyHandle = Simulation.Bodies.Add(BodyDescription.CreateKinematic(initialPos,
                new CollidableDescription(Simulation.Shapes.Add(floatingMovingPlatformShape), 0.1f, ContinuousDetection.Passive), 
                new BodyActivityDescription(-0.1f)));
            
        
        // Create World matrix for Platform2World
            Platform2World = Matrix.CreateScale(200f, 5f, 1400f) * Matrix.CreateTranslation(lastX, (lastY = y ), (lastZ += 800f));
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
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), (lastZ += 150f)),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), (lastZ += 150f)),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), lastZ + 75f),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), lastZ + 75f),
                
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX + 250f), (lastY + 50f), (lastZ += 150f)),
                Matrix.CreateScale(250f, 100f, 50f) * Matrix.CreateTranslation((lastX - 250f), (lastY + 50f), lastZ + 75f),
            };

            AnnoyingMovingWallsBodyHandle = new BodyHandle[AnnoyingMovingWallsWorld.Length];
            AnnoyingMovingWallsScale = new Vector3(250f, 100f, 50f);
            InitialLeftXPositionAnnoyingMovingWalls = lastX + 250f;
            InitialRigthXPositionAnnoyingMovingWalls = lastX - 250f;


            
            for (int index = 0; index < AnnoyingMovingWallsWorld.Length; index++)
            {
                var matrix = AnnoyingMovingWallsWorld[index];
                
                matrix.Decompose(out scale, out rot, out translation);
                
                System.Numerics.Quaternion rotationMW = new System.Numerics.Quaternion(rot.X, rot.Y, rot.Z, rot.W);
                
                var initialPosMw = new RigidPose(Utils.ToNumericVector3(translation), rotationMW);
                
                var annoyingMovingWallsShape = new Box(AnnoyingMovingWallsScale.X, AnnoyingMovingWallsScale.Y, AnnoyingMovingWallsScale.Z);
                AnnoyingMovingWallsBodyHandle[index] = (
                    Simulation.Bodies.Add(BodyDescription.CreateKinematic(initialPosMw,
                        new CollidableDescription(Simulation.Shapes.Add(annoyingMovingWallsShape), 0.1f,
                            ContinuousDetection.Passive),
                        new BodyActivityDescription(-0.1f))));



            }
            
            

            
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
        // Load our Tiling Effect
            TilingEffect = Content.Load<Effect>(ConfigurationManager.AppSettings["ContentFolderEffects"] + "TextureTiling");
            
        // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");
            WoodenTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "wood/caja-madera-1");
            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
    }



    public void Update()
    {
        // FloatingMovingPlatform
            var floatingMovingPlatformBodyRef = Simulation.Bodies.GetBodyReference(FloatingMovingPlatformBodyHandle);
            if (floatingMovingPlatformBodyRef.Pose.Position.Y >= InitialYPositionFloatingMovingPlatform)
                floatingMovingPlatformBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Down * 15f ));
            if (floatingMovingPlatformBodyRef.Pose.Position.Y <= InitialYPositionFloatingMovingPlatform - HeightDistanceDifference)
                floatingMovingPlatformBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Up * 15f ));
        
        // AnnoyingMovingWalls
            for (int index = 0; index < AnnoyingMovingWallsBodyHandle.Length; index++)
            {
                var annoyingMovingWallsBodyHandleBodyRef = Simulation.Bodies.GetBodyReference(AnnoyingMovingWallsBodyHandle[index]);
                if (index%2 == 0){
                    if (annoyingMovingWallsBodyHandleBodyRef.Pose.Position.X >= InitialLeftXPositionAnnoyingMovingWalls)
                        annoyingMovingWallsBodyHandleBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 50f));
                    if (annoyingMovingWallsBodyHandleBodyRef.Pose.Position.X <= InitialLeftXPositionAnnoyingMovingWalls - LengthDistanceDifference)
                        annoyingMovingWallsBodyHandleBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 50f));
                }
                else
                {
                    if (annoyingMovingWallsBodyHandleBodyRef.Pose.Position.X <= InitialRigthXPositionAnnoyingMovingWalls)
                        annoyingMovingWallsBodyHandleBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 50f ));
                    if (annoyingMovingWallsBodyHandleBodyRef.Pose.Position.X >= InitialRigthXPositionAnnoyingMovingWalls + LengthDistanceDifference)
                        annoyingMovingWallsBodyHandleBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 50f));
                }

            }

    }
    
// ======== Dibujar Modelos ========  
    public void Draw(Matrix view, Matrix projection)
    {
        var viewProjection = view * projection;
        // Set the Technique inside the TilingEffect to "BaseTiling"
        TilingEffect.CurrentTechnique = TilingEffect.Techniques["BaseTiling"]; // Using its original Texture Coordinates
        TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 1f));
        TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);    
        
        
        // Draw Platform1
            TilingEffect.Parameters["WorldViewProjection"].SetValue(Platform1World * viewProjection);
            BoxPrimitive.Draw(TilingEffect);
        
        // Draw Floating Platforms
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 3f));
            for (int index = 0; index < FloatingPlatformsWorld.Length; index++)
            {
                var matrix = FloatingPlatformsWorld[index];
                TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(TilingEffect);

            }
        
        // Draw FloatingMovingPlatformWorld
            var pose = Simulation.Bodies.GetBodyReference(FloatingMovingPlatformBodyHandle).Pose;
            FloatingMovingPlatformWorld = Matrix.CreateScale(FloatingMovingPlatformScale) * Matrix.CreateTranslation(pose.Position.X, pose.Position.Y, pose.Position.Z);
            TilingEffect.Parameters["WorldViewProjection"].SetValue(FloatingMovingPlatformWorld * viewProjection);
            BoxPrimitive.Draw(TilingEffect);
            
        // Draw Platform2
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 1f));
            TilingEffect.Parameters["WorldViewProjection"].SetValue(Platform2World * viewProjection);
            BoxPrimitive.Draw(TilingEffect);   
        
        // Draw AnnoyingWallsWorld
        TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 3f));
            for (int index = 0; index < AnnoyingWallsWorld.Length; index++)
            {
                var matrix = AnnoyingWallsWorld[index];
                TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(TilingEffect);

            }
            
        // Draw AnnoyingMovingWallsWorld
            for (int index = 0; index < AnnoyingMovingWallsWorld.Length; index++)
            {
                

                var poseMw = Simulation.Bodies.GetBodyReference(AnnoyingMovingWallsBodyHandle[index]).Pose;
                AnnoyingMovingWallsWorld[index] = Matrix.CreateScale(AnnoyingMovingWallsScale) * Matrix.CreateTranslation(poseMw.Position.X, poseMw.Position.Y, poseMw.Position.Z);
                TilingEffect.Parameters["WorldViewProjection"].SetValue(AnnoyingMovingWallsWorld[index] * viewProjection);
                BoxPrimitive.Draw(TilingEffect);

            }
            
        // Draw Floating Platforms 2
            for (int index = 0; index < FloatingPlatforms2World.Length; index++)
            {
                var matrix = FloatingPlatforms2World[index];
                TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(TilingEffect);

            }    
            
        // Draw Platform3
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(3f, 1f));
            TilingEffect.Parameters["WorldViewProjection"].SetValue(Platform3World * viewProjection);
            BoxPrimitive.Draw(TilingEffect);     

        // Draw BoxesWorld
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(1f, 1f));
            TilingEffect.Parameters["Texture"].SetValue(WoodenTexture); 
            for (int index = 0; index < BoxesWorld.Length; index++)
            {
                var matrix = BoxesWorld[index];
                TilingEffect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(TilingEffect);

            }   
        
            
            
            
    }
}