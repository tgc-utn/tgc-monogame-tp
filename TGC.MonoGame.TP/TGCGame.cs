using System;
using System.Collections.Generic;
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
        //private List<Matrix> _platformMatricesLevel2;
        
        // Effects
        // Effect for the Platforms
        private Effect PlatformEffect { get; set; }

        // Effect for the ball
        private Effect Effect { get; set; }
        
        // Textures
        private Texture2D StonesTexture { get; set; }

        // Models
        private Model StarModel { get; set; }
        private Matrix StarWorld { get; set; }
        
        
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
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 40, 200), size);
            
            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            
            // Sphere
            SpherePosition = new Vector3(0f, 10f, 0f);
            Sphere = new SpherePrimitive(GraphicsDevice, 10);

            StarWorld = Matrix.Identity;
            
            // Box/platforms
            _platformMatrices = new List<Matrix>();
            //_platformMatricesLevel2 = new List<Matrix>();
            
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
            
            /*
             ===================================================================================================
             Circuit 3
             ===================================================================================================
            */
            float altura = -24;
            for (int pisos = 0; pisos < 6; pisos++) {
                altura += 29;
                // Ramp
                CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(-800f, altura, 0f), Matrix.CreateRotationZ(-0.3f));
                altura += 29;
                // Platform
                CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(-920f, altura, 25f));
                altura += 29;
                // Ramp
                CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(-800f, altura, 50f), Matrix.CreateRotationZ(0.3f));
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
            CreatePlatform(new Vector3(30f, 6f, 15f), new Vector3(-390f, altura, 0f), Matrix.CreateRotationZ(0.3f));
            
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
            /*
             TODO cada 3 circuitos y un maze "subir de nivel" (alcanzar una altura mayor y cambiar texturas)

            altura = 900f;
            CreatePlatformLevel2(new Vector3(50f, 6f, 50f), new Vector3(150f, altura, 0f));
            */
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
        ///     Creates a platform with the specified scale, position and rotation.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /// <param name="rotation">The rotation of the platform</param>
        /*
         private void CreatePlatformLevel2(Vector3 scale, Vector3 position, Matrix rotation)
        {
            var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
            _platformMatricesLevel2.Add(platformWorld);
        }
        */
        
        /// <summary>
        ///     Creates a platform with the specified scale and position.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /*
        private void CreatePlatformLevel2(Vector3 scale, Vector3 position)
        {
            var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            _platformMatricesLevel2.Add(platformWorld);
        }
        */

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
            PlatformEffect = Content.Load<Effect>(ContentFolderEffects + "PlatformShader");
            loadEffectOnMesh(StarModel, Effect);

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
            
            Camera.Update(gameTime);

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }
            
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Yaw += time * 0.4f;
            Pitch += time * 0.8f;
            Roll += time * 0.9f;

            base.Update(gameTime);
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
                PlatformEffect.Parameters["View"].SetValue(Camera.View);
                PlatformEffect.Parameters["Projection"].SetValue(Camera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                BoxPrimitive.Draw(PlatformEffect);
            }  
            
            /*
            foreach (var platformWorld in _platformMatricesLevel2)
            {
                // Configura la matriz de mundo del efecto con la matriz del Floor actual
                PlatformEffect.Parameters["World"].SetValue(platformWorld);
                PlatformEffect.Parameters["View"].SetValue(Camera.View);
                PlatformEffect.Parameters["Projection"].SetValue(Camera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture); // TODO agregar otra textura
                BoxPrimitive.Draw(PlatformEffect);
            } 
            */
            

            DrawGeometry(Sphere, SpherePosition, -Yaw, Pitch, Roll, Effect);

            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-450f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(150f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
        }

        private void DrawModel(Matrix world, Model model, Effect effect){
            effect.Parameters["View"].SetValue(Camera.View);
            effect.Parameters["Projection"].SetValue(Camera.Projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());

            foreach (var mesh in model.Meshes)
            {
                Matrix meshMatrix = mesh.ParentBone.Transform;
                effect.Parameters["World"].SetValue(meshMatrix * world);
                mesh.Draw();
            }
        }

        /// <summary>
        ///     Draw the geometry applying a rotation and translation.
        /// </summary>
        /// <param name="geometry">The geometry to draw.</param>
        /// <param name="position">The position of the geometry.</param>
        /// <param name="yaw">Vertical axis (yaw).</param>
        /// <param name="pitch">Transverse axis (pitch).</param>
        /// <param name="roll">Longitudinal axis (roll).</param>
        /// <param name="effect">Used to set and query effects.</param>;
        private void DrawGeometry(GeometricPrimitive geometry, Vector3 position, float yaw, float pitch, float roll, Effect effect)
        {
            Effect.Parameters["World"].SetValue(Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position));
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.IndianRed.ToVector3());
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