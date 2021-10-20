using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
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
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = false;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        public Model Model { get; set; }
        public Model island { get; set; }
        
        public Model Rock { get; set; }
        private Model Barco { get; set; }
        private Vector3 BarcoPositionCenter = new Vector3(-200f, -10, 0);
        private Model Barco2 { get; set; }
        public Model Barco3 { get; set; }
        private Model Projektil { get; set; }
        
        private Model Projektil2 { get; set; }
        public Model Terreno2 { get; set; }
        public Model islandTwo { get; set; }
        public Model[] islands { get; set; }

        public Vector3[] posicionesIslas;

        public int cantIslas;
        public Water ocean { get; set; }
        public Matrix World { get; set; }
        public Camera Camera { get; set; }
        
        public MainShip MainShip;

        public float ElapsedTime = 0;
        private Song Song { get; set; }
        private string SongName { get; set; }
        
        public SpriteBatch spriteBatch ;

        public Texture2D Mira;
        private GameRun gameRun;
        private Menu menu;
        public string GameState = "START"; //posibles estados PLAY, RETRY, RESUME, END, PAUSE


        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.CreateRotationY(MathHelper.Pi);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            MainShip = new MainShip(BarcoPositionCenter, new Vector3(0,0,0), 10, this );
            Camera = new BuilderCamaras(GraphicsDevice.Viewport.AspectRatio , screenSize, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, MainShip, GameState == "START");
            gameRun = new GameRun(this);
            menu = new Menu(this);
            posicionesIslas = new[] { new Vector3(-3000f, -60f, 200f) ,new Vector3(2000f,-60f,400f),new Vector3(1500f,-60f,200f), new Vector3(-4500f,-60f,-600f),new Vector3(-2000f,-60f,-1500f),
                new Vector3(4000f,-60f,-1500f),new Vector3(500f,-60f,-3000f),new Vector3(0,-60f,-4000f), new Vector3 (-2000f,-60f,0)};

            cantIslas = posicionesIslas.Length;
            
            base.Initialize();
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
            MainShip.LoadContent();
            // Cargo el modelo del logo.
            Model = Content.Load<Model>(ContentFolder3D + "WarVessel/1124");
            island = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            Barco = Content.Load<Model>(ContentFolder3D + "Barco");
            Barco2 = Content.Load<Model>(ContentFolder3D + "Barco2");
            Barco3 = Content.Load<Model>(ContentFolder3D + "Barco3");
            Projektil = Content.Load<Model>(ContentFolder3D + "projektil FBX");
            Terreno2 = Content.Load<Model>(ContentFolder3D + "FBX");
            Projektil2 = Content.Load<Model>(ContentFolder3D + "9x18 pm");
            Rock = Content.Load<Model>(ContentFolder3D + "RockSet06-A");
            islandTwo = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            ocean = new Water(Content);
            islands = new Model[cantIslas];
            Mira = Content.Load<Texture2D>(ContentFolderTextures + "Mira0");
            for (int isla = 0; isla < cantIslas; isla++)
            {
                islands[isla] = Content.Load<Model>(ContentFolder3D + "islands/isla" + (isla + 1));
            }


            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            //Music
            SongName = "rhythm-of-war-main-7233";
            Song = Content.Load<Song>(ContentFolderMusic + SongName);
            MediaPlayer.IsRepeating = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            ElapsedTime += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (GameState == "START")
            {
                if (MediaPlayer.State != MediaState.Playing )
                {
                    MediaPlayer.Play(menu.Song, new TimeSpan(0,0,2));
                }
                IsMouseVisible = true;
                menu.Update(gameTime);
            }

            if (GameState == "PLAY" || GameState == "RESUME")
            {
                if (MediaPlayer.State != MediaState.Playing )
                {
                    MediaPlayer.Play(Song);
                }
                IsMouseVisible = false;
                gameRun.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && GameState == "END")
                //Salgo del juego.
                Exit();
            // Basado en el tiempo que paso se va generando una rotacion.
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            if (GameState == "START")
                menu.Draw(gameTime);
            if (GameState == "PLAY" || GameState == "RESUME")
                gameRun.Draw(gameTime);
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
    }
}