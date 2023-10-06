using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Geometries;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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
        
        // Skybox
        private SkyBox SkyBox { get; set; }
        
        // Camera
        private Camera Camera { get; set; }
        private TargetCamera TargetCamera { get; set; }
        
        // Scene
        private Matrix SphereWorld { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        
        // Geometries
        private SpherePrimitive Sphere { get; set; }
        private QuadPrimitive Quad { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        
        // Sphere position & rotation
        private Vector3 InitialSpherePosition { get; set; }
        private Matrix SphereScale { get; set; }
        
        // World matrices
        private List<Matrix> _platformMatrices;

        private List<Matrix> _rampMatrices;

        private List<Matrix> _platformMatricesLevel2;
        
        // Effects
        // Effect for the Platforms
        private Effect PlatformEffect { get; set; }

        // Effect for the ball
        private Effect Effect { get; set; }
        private Effect TextureEffect { get; set; }
        
        // private Effect SkyboxEffect { get; set; }
        
        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D MarbleTexture { get; set; }
        private Texture2D RubberTexture { get; set; }
        private Texture2D MetalTexture { get; set; }
        
        // private Texture2D Sk

        // Models
        private Model StarModel { get; set; }
        private Model SphereModel { get; set; }
        private Matrix StarWorld { get; set; }
        private Player _player;
        
        
        // Colliders
        public BoundingBox[] Colliders { get; set; }
        public static OrientedBoundingBox[] OrientedColliders { get; set; }
        public Gizmos.Gizmos Gizmos { get; set; }


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
            SphereWorld = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            
            // Sphere
            InitialSpherePosition = new Vector3(0f, 10f, 0f);
            SphereScale = Matrix.CreateScale(5f);
            
            // Player
            _player = new Player(SphereScale, InitialSpherePosition, new BoundingSphere(InitialSpherePosition, 5f));
            
            // Gizmos
            Gizmos = new Gizmos.Gizmos();
            Gizmos.Enabled = true;
            
            // Star
            StarWorld = Matrix.Identity;
            
            // Box/platforms
            _platformMatrices = new List<Matrix>();
            _rampMatrices = new List<Matrix>();

            _platformMatricesLevel2 = new List<Matrix>();

            
            Prefab.CreateSquareCircuit(Vector3.Zero);
            Prefab.CreateSquareCircuit(new Vector3(-600, 0f, 0f));
            _platformMatrices = Prefab.PlatformMatrices;
            _rampMatrices = Prefab.RampMatrices;
            
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

            CreateRamp(new Vector3(30f, 6f, 30f), new Vector3(-190f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));
            
            /*
             ===================================================================================================
             COLLIDERS
             ===================================================================================================
            */
            // Create bounding boxes for static geometries
            // Circuit 1 floor + Bridge's platforms
            Colliders = new BoundingBox[_platformMatrices.Count + 4];
            OrientedColliders = new OrientedBoundingBox[_rampMatrices.Count];
            
            // Instantiate the circuits' platforms bounding boxes.
            int index = 0;
            for (; index < _platformMatrices.Count; index++)
            {
                Colliders[index] = BoundingVolumesExtensions.FromMatrix(_platformMatrices[index]);
            }
            
            // Instantiate the bridges boxes
            // platforms
            Colliders[index] = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(new Vector3(90f, 6f, 30f)) *
                                                                    Matrix.CreateTranslation(new Vector3(-50f, 0f,
                                                                        0f)));
            index++;
            Colliders[index] = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(new Vector3(30f, 6f, 30f)) *
                                                                    Matrix.CreateTranslation(new Vector3(-120f, 0f, 0f)));
            index++;
            Colliders[index] = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(new Vector3(30f, 6f, 30f)) *
                                                                    Matrix.CreateTranslation(new Vector3(-160f, 0f, 0f)));

            OrientedColliders = Prefab.RampOBB.ToArray();
            
            // ramp
            /*index++;
            Colliders[index] = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(new Vector3(30f, 6f, 30f)) * 
                                                                    Matrix.CreateRotationZ(-0.3f) * 
                                                                    Matrix.CreateTranslation(new Vector3(-190f, 5f, 0f)));*/

            CreateRamp(new Vector3(30f, 6f, 30f), new Vector3(-190f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));
            
            /*
             ===================================================================================================
             Circuit 3
             ===================================================================================================
            */
            float altura = -24;
            for (int pisos = 0; pisos < 6; pisos++) {
                altura += 29;
                // Ramp
                CreateRamp(new Vector3(200f, 6f, 50f), new Vector3(-800f, altura, 0f), Matrix.CreateRotationZ(-0.3f));
                altura += 29;
                // Platform
                CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(-920f, altura, 25f));
                altura += 29;
                // Ramp
                CreateRamp(new Vector3(200f, 6f, 50f), new Vector3(-800f, altura, 50f), Matrix.CreateRotationZ(0.3f));
                altura += 29;
                // Platform
                CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(-680f, altura, 25f));
            }
            
            /*
             ===================================================================================================
             Bridge between Circuit 3 and Maze
             ===================================================================================================
            */
            
            // Platform
            CreatePlatform(new Vector3(50f, 6f, 30f), new Vector3(-620f, altura, 0f));
            CreatePlatform(new Vector3(50f, 6f, 25f), new Vector3(-560f, altura, 0f));
            CreatePlatform(new Vector3(50f, 6f, 20f), new Vector3(-500f, altura, 0f));
            CreatePlatform(new Vector3(50f, 6f, 15f), new Vector3(-440f, altura, 0f));
            
            // Ramp
            CreateRamp(new Vector3(30f, 6f, 15f), new Vector3(-390f, altura, 0f), Matrix.CreateRotationZ(0.3f));
            
            /*
             ===================================================================================================
             Maze
             ===================================================================================================
            */
            
            // Entrance platform
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-300f, altura, 0f));
            
            // Maze platform
            CreatePlatform(new Vector3(750f, 6f, 750f), new Vector3(100f, altura, 0f));
            
            // Center platform to go next level, tendria que moverse hacia arriba hasta 900f
            CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(100f, altura, 0f));
            
            // Border Walls
            CreatePlatform(new Vector3(750f, 50f, 6f), new Vector3(100f, altura+25f, 375f));
            CreatePlatform(new Vector3(750f, 50f, 6f), new Vector3(100f, altura+25f, -375f));
            CreatePlatform(new Vector3(6f, 50f, 750f), new Vector3(475f, altura+25f, 0f));
            CreatePlatform(new Vector3(6f, 50f, 350f), new Vector3(-275f, altura+25f, 200f));
            CreatePlatform(new Vector3(6f, 50f, 350f), new Vector3(-275f, altura+25f, -200f));
            
            // Vertical Walls from largest to shortest
            CreatePlatform(new Vector3(6f, 50f, 250f), new Vector3(225f, altura+25f, -50f));
            CreatePlatform(new Vector3(6f, 50f, 250f), new Vector3(275f, altura+25f, -200f));
            CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-125f, altura+25f, 225f));
            CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-75f, altura+25f, -75f));
            CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-25f, altura+25f, 175f));
            CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-25f, altura+25f, -125f));
            CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(-225f, altura+25f, 250f));
            CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(-225f, altura+25f, -100f));
            CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(25f, altura+25f, 0f));
            CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(275f, altura+25f, 250f));
            CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(275f, altura+25f, 50f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-225f, altura+25f, 75f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-175f, altura+25f, -25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-175f, altura+25f, -225f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-125f, altura+25f, 25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-125f, altura+25f, -225f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, altura+25f, 325f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, altura+25f, 175f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, altura+25f, 25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, altura+25f, -225f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(125f, altura+25f, -25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(125f, altura+25f, -225f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(175f, altura+25f, 275f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(175f, altura+25f, 25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(225f, altura+25f, -325f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(325f, altura+25f, -25f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(375f, altura+25f, 125f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(375f, altura+25f, -275f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(425f, altura+25f, 75f));
            CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(425f, altura+25f, -125f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-225f, altura+25f, -300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, altura+25f, 250f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, altura+25f, 100f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, altura+25f, -350f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, altura+25f, 300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, altura+25f, 200f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, altura+25f, -300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-25f, altura+25f, 350f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-25f, altura+25f, -300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, altura+25f, 150f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, altura+25f, -150f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, altura+25f, -300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(75f, altura+25f, -350f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(125f, altura+25f, 100f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(175f, altura+25f, -150f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(175f, altura+25f, -300f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(225f, altura+25f, 150f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(325f, altura+25f, 250f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(325f, altura+25f, -150f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(375f, altura+25f, -100f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(425f, altura+25f, 200f));
            CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(425f, altura+25f, -250f));
            
            // Columns
            CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(125f, altura+25f, 225f));
            CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(125f, altura+25f, -325f));
            CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(325f, altura+25f, -275f));
            
            // Horizontal walls from largest to shortest
            CreatePlatform(new Vector3(200f, 50f, 6f), new Vector3(-125f, altura+25f, 125f));
            CreatePlatform(new Vector3(200f, 50f, 6f), new Vector3(225f, altura+25f, 125f));
            CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(200f, altura+25f, -225f));
            CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(100f, altura+25f, -125f));
            CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(-50f, altura+25f, 75f));
            CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(150f, altura+25f, 175f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, altura+25f, -325f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-125f, altura+25f, -275f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-25f, altura+25f, -225f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-125f, altura+25f, -125f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, altura+25f, -125f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(125f, altura+25f, -75f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(375f, altura+25f, 25f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-225f, altura+25f, 175f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, altura+25f, 175f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(25f, altura+25f, 225f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(225f, altura+25f, 225f));
            CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-25f, altura+25f, 275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-100f, altura+25f, -325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, altura+25f, -325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-250f, altura+25f, -275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, altura+25f, -275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, altura+25f, -275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, altura+25f, -225f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, altura+25f, -225f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, altura+25f, -225f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, altura+25f, -175f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, altura+25f, -175f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, altura+25f, -175f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, altura+25f, -175f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-150f, altura+25f, -75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(0f, altura+25f, -75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, altura+25f, -75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-100f, altura+25f, -25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, altura+25f, -25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, altura+25f, -25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, altura+25f, -25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, altura+25f, 25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(0f, altura+25f, 25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(150f, altura+25f, 25f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, altura+25f, 75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, altura+25f, 75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, altura+25f, 75f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, altura+25f, 125f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-50f, altura+25f, 175f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, altura+25f, 225f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, altura+25f, 225f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, altura+25f, 275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(250f, altura+25f, 275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, altura+25f, 275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, altura+25f, 275f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-150f, altura+25f, 325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, altura+25f, 325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(150f, altura+25f, 325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(250f, altura+25f, 325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, altura+25f, 325f));
            CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, altura+25f, 325f));
            
            /*
             ===================================================================================================
             Circuit 4
             ===================================================================================================
            */
            
            //TODO cada 3 circuitos y un maze "subir de nivel" (alcanzar una altura mayor y cambiar texturas)

            altura = 900f;
            CreatePlatformLevel2(new Vector3(50f, 6f, 50f), new Vector3(150f, altura, 0f));
            
            base.Initialize();
        }

        /// <summary>
        ///     Creates a platform with the specified scale, position and rotation.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /// <param name="rotation">The rotation of the platform</param>
        private void CreateRamp(Vector3 scale, Vector3 position, Matrix rotation)
        {
            var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
            _rampMatrices.Add(platformWorld);
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
        ///     Creates a platform with the specified scale, position and rotation.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /// <param name="rotation">The rotation of the platform</param>
        
         private void CreatePlatformLevel2(Vector3 scale, Vector3 position, Matrix rotation)
        {
            var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
            _platformMatricesLevel2.Add(platformWorld);
        }
        
        /// <summary>
        ///     Creates a platform with the specified scale and position.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        
        private void CreatePlatformLevel2(Vector3 scale, Vector3 position)
        {
            var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            _platformMatricesLevel2.Add(platformWorld);
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
            MarbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble_black_01_c");
            RubberTexture = Content.Load<Texture2D>(ContentFolderTextures + "goma_diffuse");
            MetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal_diffuse");
            
            Quad = new QuadPrimitive(GraphicsDevice);
            
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, StonesTexture);
            
            StarModel = Content.Load<Model>(ContentFolder3D + "star/Gold_Star");

            SphereModel = Content.Load<Model>(ContentFolder3D + "geometries/sphere");
            
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            PlatformEffect = Content.Load<Effect>(ContentFolderEffects + "PlatformShader");
            loadEffectOnMesh(StarModel, Effect);

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "BasicTextureShader");
            loadEffectOnMesh(SphereModel, TextureEffect);

            SphereWorld = SphereScale * Matrix.CreateTranslation(InitialSpherePosition);
            
            var skyBox = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 1000f);

            Gizmos.LoadContent(GraphicsDevice, Content);

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

            // SphereWorld = _player.Update(time, keyboardState);
            SphereWorld = _player.Update(time, keyboardState, Colliders);
            
            // Capturar Input teclado
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }
            
            // Simple restart when falling off
            if (_player.SpherePosition.Y <= -150f)
            {
                _player.SpherePosition = InitialSpherePosition;
            }

            UpdateCamera(_player.SpherePosition, _player.Yaw);
            
            Gizmos.UpdateViewProjection(TargetCamera.View, TargetCamera.Projection);

            base.Update(gameTime);
        }
        
        private void UpdateCamera(Vector3 position, float yaw)
        {
            var sphereBackDirection = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(yaw));
            
            var orbitalPosition = sphereBackDirection * 60f;
            
            var upDistance = Vector3.Up * 15f;
            
            TargetCamera.Position = position + orbitalPosition + upDistance;

            TargetCamera.TargetPosition = position;
            
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
                PlatformEffect.Parameters["World"].SetValue(platformWorld);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                BoxPrimitive.Draw(PlatformEffect);
            }

            /*foreach (var boundingBox in Colliders)
            {
                var center = BoundingVolumesExtensions.GetCenter(boundingBox);
                var extents = BoundingVolumesExtensions.GetExtents(boundingBox);
                Gizmos.DrawCube(center, extents * 2f, Color.Red);
            }*/
            
            foreach (var orientedBoundingBox in Prefab.RampOBB)
            {
                var orientedBoundingBoxWorld = Matrix.CreateScale(orientedBoundingBox.Extents * 2f) 
                                               * orientedBoundingBox.Orientation * Matrix.CreateTranslation(orientedBoundingBox.Center);
                Gizmos.DrawCube(orientedBoundingBoxWorld, Color.Red);
            }

            foreach (var rampWorld in _rampMatrices)
            {
                PlatformEffect.Parameters["World"].SetValue(rampWorld);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                
                BoxPrimitive.Draw(PlatformEffect);
            } 
            
            foreach (var platformWorld in _platformMatricesLevel2)
            {
                PlatformEffect.Parameters["World"].SetValue(platformWorld);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture); // TODO agregar otra textura
                BoxPrimitive.Draw(PlatformEffect);
            } 

            DrawTexturedModel(SphereWorld, SphereModel, TextureEffect, RubberTexture);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-450f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(150f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            
            Gizmos.DrawSphere(_player.BoundingSphere.Center, _player.BoundingSphere.Radius * Vector3.One, Color.Yellow);
            Gizmos.Draw();
            
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Graphics.GraphicsDevice.RasterizerState = rasterizerState;
            
            SkyBox.Draw(TargetCamera.View, TargetCamera.Projection, new Vector3(0f,0f,0f));
            GraphicsDevice.RasterizerState = originalRasterizerState;
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
        
        private void DrawTexturedModel(Matrix worldMatrix, Model model, Effect effect, Texture2D texture){
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"]?.SetValue(Color.IndianRed.ToVector3());
            effect.Parameters["Texture"]?.SetValue(texture);

            chequearPropiedadesTextura(texture);

            foreach (var mesh in model.Meshes)
            {   
                mesh.Draw();
            }
        }
        
        private void DrawGeometry(GeometricPrimitive geometry, Matrix worldMatrix, Effect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"]?.SetValue(Color.IndianRed.ToVector3());
            effect.Parameters["Texture"]?.SetValue(StonesTexture);
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

        public void chequearPropiedadesTextura(Texture2D texture){
            //La bola de marmol acelera mas lento
            //La bola de goma salta mas alto
            //La bola de metal acelera mas rápido
            if(texture == MarbleTexture){
                _player.Acceleration = 30f;
            }else if(texture == RubberTexture){
                _player.MaxJumpHeight = 70f;
            }else if(texture == MetalTexture){
                _player.Acceleration = 100f;
                _player.MaxSpeed = 230f;
            }
        }
    }
}