using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGamers.Camera;
using MonoGamers.Collisions;
using MonoGamers.Geometries.Textures;
using MonoGamers.Pistas;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using MonoGamers.Geometries;
using MonoGamers.Physics;
using NumericVector3 = System.Numerics.Vector3;
using TGC.MonoGame.Samples.Physics.Bepu;
using MonoGamers.Checkpoints;

namespace MonoGamers
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        
        private const float Gravity = 350f;
        private const float yMinimo = -450f;

        private bool godMode = false;

        //Esfera lógica
        MonoSphere MonoSphere;

        // Camera to draw the scene
        private TargetCamera Camera { get; set; }
        
        private GraphicsDeviceManager Graphics { get; }

        // Geometries
        private BoxPrimitive BoxPrimitive { get; set; }
        private QuadPrimitive Floor { get; set; }
        
        private Sphere sphereShape { get; set; }

        // Handlers
        private BodyHandle FloorHandle { get; set; }
        
        // Simulation
        private MonoSimulation MonoSimulation { get; set; }
        private Simulation Simulation { get; set; }


        /// <summary>
        ///     Gets the thread dispatcher available for use by the simulation.
        /// </summary>

        public float velocidadAngularYAnt;
        public float velocidadLinearYAnt;


        // World matrices
        private Matrix FloorWorld { get; set; }

        //Texturas
        private Texture2D StonesTexture { get; set; }

        // Effects

        // Tiling Effect for the floor
        private Effect TilingEffect { get; set; }
        
        
        // Pistas
        Pista1 Pista1 { get; set; }
        Pista2 Pista2 { get; set; }
        Pista3 Pista3 {get; set;}
        Pista4 Pista4 {get; set;}

        // Checkpoints
        private Checkpoint[] Checkpoints { get; set; }
        private int CurrentCheckpoint { get; set; }
        

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = false;
        }


        /// <inheritdoc />
        protected override void Initialize()
        {
            // Enciendo Back-Face culling.
            // Configuro Blend State a Opaco.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            // Configuro las dimensiones de la pantalla.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            

            // Creo una camara para seguir a la esfera.
            //FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            Camera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f, Vector3.Zero, GraphicsDevice.Viewport);
            //Camera.BuildProjection(GraphicsDevice.Viewport.AspectRatio, 0.1f, 100000f, MathF.PI / 3f);

            // Creo los checkpoints
            Checkpoints = new Checkpoint[]
            {
                new Checkpoint(new Vector3(100f, 10f, 160f)),
                new Checkpoint(new Vector3(100f, 10f, 4594f)),
                new Checkpoint(new Vector3(2100f, 150f, 6744f)),
                new Checkpoint(new Vector3(3300f, 343f, 6800f)),

            };
            CurrentCheckpoint = 0;

            // Sphere position and matrix initialization

            MonoSphere = new MonoSphere(Checkpoints[CurrentCheckpoint].Position, Gravity);
            
            // Empezar Simulacion
            MonoSimulation = new MonoSimulation();
            Simulation = MonoSimulation.Init();
            
            // Inicializar pistas
            Pista1 = new Pista1(Content, GraphicsDevice, 100f, -3f, 450f, Simulation);
            Pista2 = new Pista2(Content, GraphicsDevice, 100f, -3f, 4594f, Simulation);
            Pista3 = new Pista3(Content, GraphicsDevice, 2100f, 137f, 6744f, Simulation);
            Pista4 = new Pista4(Content, GraphicsDevice, 3300f, 330f, 6800f, Simulation);

            base.Initialize();
        }
        

        /// <inheritdoc />
        protected override void LoadContent()
        {
            MonoSphere.spherePrimitive = new SpherePrimitive(GraphicsDevice);
            MonoSphere.SphereWorld = new Matrix();
            MonoSphere.SphereHandle = new BodyHandle();
            
            
            /*
            // Enable default lighting for the Sphere
            foreach (var mesh in Sphere.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();
            */

            // Load our Tiling Effect
                TilingEffect = Content.Load<Effect>(ContentFolderEffects + "TextureTiling");
                TilingEffect.Parameters["Tiling"].SetValue(new Vector2(10f, 10f));
                
            // Load Textures
                StonesTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
                MonoSphere.SphereCommonTexture = Content.Load<Texture2D>(ContentFolderTextures + "common");
                MonoSphere.SphereStoneTexture = Content.Load<Texture2D>(ContentFolderTextures + "stone");
                MonoSphere.SphereMetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal");
                MonoSphere.SphereGumTexture = Content.Load<Texture2D>(ContentFolderTextures + "gum");

            // Create our Quad (to draw the Floor) and add it to Simulation
                Floor = new QuadPrimitive(GraphicsDevice);
                FloorWorld = Matrix.CreateScale(200f, 0.001f, 200f);
                Simulation.Statics.Add(new StaticDescription(new NumericVector3(0f, 0f, 0f),
                    Simulation.Shapes.Add(new Box(400f, 0.002f, 400f))));
            
            // Create our Sphere and add it to Simulation
                sphereShape = new Sphere(10f);
                var position = new NumericVector3(100f, 20f, 150f);
                var initialVelocity = new BodyVelocity(new NumericVector3((float)0f, 0f, 0f));
                var mass = sphereShape.Radius * sphereShape.Radius * sphereShape.Radius;
                var bodyDescription = BodyDescription.CreateConvexDynamic(position, initialVelocity, mass, Simulation.Shapes, sphereShape);
                MonoSphere.SphereHandle = Simulation.Bodies.Add(bodyDescription);
            
            base.LoadContent();
        }

        

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            // The time that passed between the last loop
                var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            // Update Simulation
                MonoSimulation.Update();
            
            var keyboardState = Keyboard.GetState();
            // Check for key presses and rotate accordingly
            // We can stack rotations in a given axis by multiplying our past matrix
            // By a new matrix containing a new rotation to apply
            // Also, recalculate the Front Directoin
            
            var sphereBody= Simulation.Bodies.GetBodyReference(MonoSphere.SphereHandle);
            sphereBody.Awake = true;
            MonoSphere.SphereRotation = Camera.CameraRotation;
            MonoSphere.SphereFrontDirection = Vector3.Transform(Vector3.Backward, MonoSphere.SphereRotation);
            MonoSphere.SphereLateralDirection = Vector3.Transform(Vector3.Right, MonoSphere.SphereRotation);
            
            if (keyboardState.IsKeyDown(Keys.D))
            {
                MonoSphere.MoveRight();
                sphereBody.ApplyLinearImpulse(new NumericVector3(MonoSphere.SphereVelocity.X,
                    MonoSphere.SphereVelocity.Y,MonoSphere.SphereVelocity.Z));
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                MonoSphere.MoveLeft();
                sphereBody.ApplyLinearImpulse(new NumericVector3(MonoSphere.SphereVelocity.X,
                    MonoSphere.SphereVelocity.Y,MonoSphere.SphereVelocity.Z));
            }

            // Check for key presses and add a velocity in the Sphere's Front Direction
            if (keyboardState.IsKeyDown(Keys.W))
            {
                MonoSphere.MoveFront();
                sphereBody.ApplyLinearImpulse(new NumericVector3(MonoSphere.SphereVelocity.X,
                    MonoSphere.SphereVelocity.Y,MonoSphere.SphereVelocity.Z));
            }
                
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                MonoSphere.MoveBack();
                sphereBody.ApplyLinearImpulse(new NumericVector3(MonoSphere.SphereVelocity.X,
                    MonoSphere.SphereVelocity.Y,MonoSphere.SphereVelocity.Z));
            }
            
            // Check for the Jump key press, and add velocity in Y only if the Sphere is on the ground
            if(MathHelper.Distance(sphereBody.Velocity.Linear.Y, velocidadLinearYAnt) < 0.1  
                && MathHelper.Distance(sphereBody.Velocity.Angular.Y, velocidadAngularYAnt) < 0.1) 
                    MonoSphere.OnGround = true; // Se revisa que la velocidad lineal como la angular de la esfera en Y, su distancia se menor a 0,1 con respecto a la velocidad anterior
            
            if (keyboardState.IsKeyDown(Keys.Space) && MonoSphere.OnGround){
                MonoSphere.Jump();
                sphereBody.ApplyLinearImpulse(new NumericVector3(MonoSphere.SphereVelocity.X,
                    MonoSphere.SphereVelocity.Y,MonoSphere.SphereVelocity.Z));
            }
            
            if (keyboardState.IsKeyDown(Keys.G)) {
                if (!godMode) godMode = true;
                else godMode = false;
            }
            
            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();
            /* if (keyboardState.IsKeyDown(Keys.U)) {
                sphereBody.Pose = new NumericVector3(100f, 20f, 4580f);
            }
            if (keyboardState.IsKeyDown(Keys.I)) {
                sphereBody.Pose = new NumericVector3(2090f, 150f, 6744f);
            }
            if (keyboardState.IsKeyDown(Keys.O)) {
                sphereBody.Pose = new NumericVector3(3300f, 343f, 6790f);
            } */


            velocidadAngularYAnt = sphereBody.Velocity.Angular.Y;
            velocidadLinearYAnt = sphereBody.Velocity.Linear.Y;


            CheckpointManager();

            
            // Actualizo la camara, enviandole la matriz de mundo de la esfera.
            //FollowCamera.Update(gameTime, SphereWorld);
            
            var pose = Simulation.Bodies.GetBodyReference(MonoSphere.SphereHandle).Pose;
            MonoSphere.SpherePosition = pose.Position;
            MonoSphere.SphereWorld = Matrix.CreateScale(sphereShape.Radius*2) * 
                Matrix.CreateFromQuaternion(pose.Orientation) * Matrix.CreateTranslation(MonoSphere.SpherePosition);
            Pista2.Update(deltaTime);
            Camera.UpdateCamera(gameTime, MonoSphere.SpherePosition);

            base.Update(gameTime);
        }

        private void CheckpointManager()
        {
            var bodyRef = Simulation.Bodies.GetBodyReference(MonoSphere.SphereHandle);

            if(MonoSphere.SphereFalling(yMinimo))
            {
                bodyRef.Pose.Position = MonoGamers.Utilities.Utils.ToNumericVector3(Checkpoints[CurrentCheckpoint].Position);
                bodyRef.Velocity.Linear = NumericVector3.Zero;
                bodyRef.Velocity.Angular = NumericVector3.Zero;
                return;
            }
            for(int i = CurrentCheckpoint+1; i < Checkpoints.Length; i++)
            {
                if(Checkpoints[i].IsWithinBounds(bodyRef.Pose.Position))
                {
                    CurrentCheckpoint = i;
                    return;
                }
            }
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            // Limpio la pantalla.
                GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Calculate the ViewProjection matrix
            //var viewProjection = FollowCamera.View * FollowCamera.Projection;
            var viewProjection = Camera.View * Camera.Projection;
            
            // Sphere drawing
                MonoSphere.spherePrimitive.Draw(MonoSphere.SphereWorld, Camera.View, Camera.Projection);

            // Floor drawing
                // Set the Technique inside the TilingEffect to "BaseTiling", we want to control the tiling on the floor
                TilingEffect.CurrentTechnique = TilingEffect.Techniques["BaseTiling"]; // Using its original Texture Coordinates
                TilingEffect.Parameters["Tiling"].SetValue(new Vector2(10f, 10f));
                TilingEffect.Parameters["WorldViewProjection"].SetValue(FloorWorld * viewProjection);
                TilingEffect.Parameters["Texture"].SetValue(StonesTexture);  // Set the Texture that the Floor will use
                Floor.Draw(TilingEffect);
            
            

            // Dibujamos las pistas
                Pista1.Draw(Camera.View,Camera.Projection);
                Pista2.Draw(Camera.View, Camera.Projection);
                Pista3.Draw(Camera.View, Camera.Projection);
                Pista4.Draw(Camera.View, Camera.Projection);
            



            base.Draw(gameTime);
        }
    }
}