using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP
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
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }
    
        // Graphics
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        
        // Camera
        private Camera Camera { get; set; }
        private TargetCamera TargetCamera { get; set; }
        
        // Scene
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        
        // Geometries
        private SpherePrimitive Sphere { get; set; }
        private QuadPrimitive Quad { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        
        // Sphere position & rotation
        private Vector3 SpherePosition { get; set; }
        private float Yaw { get; set; }
        private float Pitch { get; set; }
        private float Roll { get; set; }
        
        // World matrices
        private List<Matrix> _platformMatrices;
        
        // Effects
        private Effect Effect { get; set; }
        private Effect TextureEffect { get; set; }
        
        // Textures
        private Texture2D StonesTexture { get; set; }

        // Models
        private Model StarModel { get; set; }
        private Matrix StarWorld { get; set; }

        //private Player _player;

        private float Speed = 0f;
        private float PitchSpeed = 0f; 
        private const float MaxSpeed = 180f;
        private const float PitchMaxSpeed = 15f;
        private const float PitchAcceleration = 5f;
        private const float Acceleration = 60f;
        private const float AngularSpeed = 4.5f;
        
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            
            // Configuro las dimensiones de la pantalla.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            
            // Camera
            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            //Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 40, 200), size);
            TargetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f, Vector3.Zero);
            
            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            
            // Sphere
            SpherePosition = new Vector3(0f, 10f, 0f);
            World *= Matrix.CreateTranslation(SpherePosition);
            Sphere = new SpherePrimitive(GraphicsDevice, 10);

            StarWorld = Matrix.Identity;
            
            // Box/platforms
            _platformMatrices = new List<Matrix>();

            //_player = new Player(SpherePosition);
            
            /*
             ===================================================================================================
             Circuit 1
             ===================================================================================================    
            */
            
            // Platform
            // Side platforms
            CreatePlatform(new Vector3(50f, 6f, 200f), Vector3.Zero);
            CreatePlatform(new Vector3(50f, 6f, 200f), new Vector3(300f, 0f, 0f));
            CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(150f, 0f, -200f));
            CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(150f, 0f, 200f));
            
            // Corner platforms
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(0f, 9.5f, -185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(0f, 9.5f, 185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(300f, 9.5f, -185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(300f, 9.5f, 185f));
            
            // Center platform
            // La idea sería que se vaya moviendo 
            CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(150f, 0f, 0f));
            
            // Ramp
            // Side ramps
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, -125f), Matrix.CreateRotationX(0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, -125f), Matrix.CreateRotationX(0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, 125f), Matrix.CreateRotationX(-0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, 125f), Matrix.CreateRotationX(-0.2f));
            
            // Corner ramps
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, -200f), Matrix.CreateRotationZ(-0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, 200f), Matrix.CreateRotationZ(-0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, -200f), Matrix.CreateRotationZ(0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, 200f), Matrix.CreateRotationZ(0.3f));
            
            CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(45f, 5f, 0f), Matrix.CreateRotationZ(0.3f));
            CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(255f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));
            
            /*
             ===================================================================================================
             Circuit 2
             ===================================================================================================
            */
            
            // Platform
            // Side platforms
            CreatePlatform(new Vector3(50f, 6f, 200f), new Vector3(-600f, 0f, 0f));
            CreatePlatform(new Vector3(50f, 6f, 200f), new Vector3(-300f, 0f, 0f));
            CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(-450f, 0f, -200f));
            CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(-450f, 0f, 200f));
            
            // Corner platforms
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(-600f, 9.5f, -185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(-600f, 9.5f, 185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(-300f, 9.5f, -185f));
            CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(-300f, 9.5f, 185f));
            
            // Center platform
            // La idea sería que se vaya moviendo 
            CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(-450f, 0f, 0f));
            
            // Ramp
            // Side ramps
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-600f, 5f, -125f), Matrix.CreateRotationX(0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-300f, 5f, -125f), Matrix.CreateRotationX(0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-600f, 5f, 125f), Matrix.CreateRotationX(-0.2f));
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-300f, 5f, 125f), Matrix.CreateRotationX(-0.2f));
            
            // Corner ramps
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(-560f, 5f, -200f), Matrix.CreateRotationZ(-0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(-560f, 5f, 200f), Matrix.CreateRotationZ(-0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(-340f, 5f, -200f), Matrix.CreateRotationZ(0.3f));
            CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(-340f, 5f, 200f), Matrix.CreateRotationZ(0.3f));
            
            CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(-555f, 5f, 0f), Matrix.CreateRotationZ(0.3f));
            CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(-345f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));
            
            /*
             ===================================================================================================
             Bridge between Circuit 1 and Circuit 2
             ===================================================================================================
            */
            
            // Platform
            CreatePlatform(new Vector3(90f, 6f, 30f), new Vector3(-50f, 0f, 0f));
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-120f, 0f, 0f));
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-160f, 0f, 0f));
            
            // Ramp
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-190f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));

            base.Initialize();
        }

        /// <summary>
        ///     Creates a platform with the specified scale, position and rotation.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /// <param name="rotation">The rotation of the platform</param>
        private void CreatePlatform(Vector3 scale, Vector3 position, Matrix rotation)
        {
            var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
            _platformMatrices.Add(platformWorld);
        }
        
        /// <summary>
        ///     Creates a platform with the specified scale and position.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        private void CreatePlatform(Vector3 scale, Vector3 position)
        {
            var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            _platformMatrices.Add(platformWorld);
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            StonesTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            
            // Create our Quad (to draw the Floor)
            Quad = new QuadPrimitive(GraphicsDevice);
            
            // Create our box
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, StonesTexture);

            // Cargo el modelo del logo.
            //Model = Content.Load<Model>(ContentFolder3D + "tgc-logo/tgc-logo");
            StarModel = Content.Load<Model>(ContentFolder3D + "star/Gold_Star");

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            loadEffectOnMesh(StarModel, Effect);

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "BasicTextureShader");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            /*foreach (var mesh in Model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }*/

            base.LoadContent();
        }
        
        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
        
            var keyboardState = Keyboard.GetState();
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Yaw += time * AngularSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Yaw -= time * AngularSpeed;
            }

            var rotationY = Matrix.CreateRotationY(Yaw);
            var forward = rotationY.Forward;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                Speed += Acceleration * time;
                PitchSpeed += PitchAcceleration * time;
                Pitch -= time * PitchSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                Speed -= Acceleration * time;
                PitchSpeed += PitchAcceleration * time;
                Pitch += time * PitchSpeed;
            }
            else
            {
                var decelerationDirection = Math.Sign(Speed) * -1;
                Speed += Acceleration * time * decelerationDirection;
                PitchSpeed += PitchAcceleration * time * decelerationDirection;
                Pitch += PitchSpeed * time * decelerationDirection;
            }
            
            PitchSpeed = MathHelper.Clamp(PitchSpeed, -PitchMaxSpeed, PitchMaxSpeed);
            Speed = MathHelper.Clamp(Speed, -MaxSpeed, MaxSpeed);
            SpherePosition += forward * time * Speed;
            
            var rotationX = Matrix.CreateRotationX(Pitch);
            var translation = Matrix.CreateTranslation(SpherePosition);
            
            World =  rotationX * rotationY * translation;
            
            // Capturar Input teclado
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            UpdateCamera();

            base.Update(gameTime);
        }
        
        private void UpdateCamera()
        {
            // Create a position that orbits the Robot by its direction (Rotation)

            // Create a normalized vector that points to the back of the Robot
            var sphereBackDirection = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(Yaw));
            // Then scale the vector by a radius, to set an horizontal distance between the Camera and the Robot
            var orbitalPosition = sphereBackDirection * 60f;

            // We will move the Camera in the Y axis by a given distance, relative to the Robot
            var upDistance = Vector3.Up * 15f;

            // Calculate the new Camera Position by using the Robot Position, then adding the vector orbitalPosition that sends 
            // the camera further in the back of the Robot, and then we move it up by a given distance
            TargetCamera.Position = SpherePosition + orbitalPosition + upDistance;

            // Set the Target as the Robot, the Camera needs to be always pointing to it
            TargetCamera.TargetPosition = SpherePosition;

            // Build the View matrix from the Position and TargetPosition
            TargetCamera.BuildView();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            foreach (var platformWorld in _platformMatrices)
            {
                // Configura la matriz de mundo del efecto con la matriz del Floor actual
                Effect.Parameters["World"].SetValue(platformWorld);
                Effect.Parameters["View"].SetValue(TargetCamera.View);
                Effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                Effect.Parameters["DiffuseColor"].SetValue(Color.ForestGreen.ToVector3());
                
                BoxPrimitive.Draw(Effect);
            }  
            
            Sphere.Draw(World, TargetCamera.View, TargetCamera.Projection); // TODO: no usar

            //DrawGeometry(Sphere, World, TextureEffect);

            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-450f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(150f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
        }

        private void DrawModel(Matrix world, Model model, Effect effect){
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());

            foreach (var mesh in model.Meshes)
            {
                Matrix meshMatrix = mesh.ParentBone.Transform;
                effect.Parameters["World"].SetValue(meshMatrix * world);
                mesh.Draw();
            }
        }
        
        private void DrawGeometry(GeometricPrimitive geometry, Matrix worldMatrix, Effect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"]?.SetValue(Color.IndianRed.ToVector3());
            effect.Parameters["Texture"].SetValue(StonesTexture);
            geometry.Draw(effect);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }

        public static void loadEffectOnMesh(Model modelo,Effect efecto)
        {
            foreach (var mesh in modelo.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }
    }
}