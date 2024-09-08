using System;
using Escenografia;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }

        private Effect _basicShader;
        


        //Control.Camera camara;
        Control.Camarografo camarografo;
        Escenografia.AutoJugador auto;
        Control.AdministradorNPCs generadorPrueba;

        private Escenografia.Plane _plane { get; set; }
        private PrismaRectangularEditable _edificio {get; set;}
        private Model _plant { get; set; }

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

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
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            generadorPrueba = new Control.AdministradorNPCs();
            generadorPrueba.generarNPCsV1(new Vector3(-1000f,0f,-1000f), new Vector3(1000f,0f,1000f));
            auto = new Escenografia.AutoJugador(Vector3.Zero, Vector3.Backward, 1000f, Convert.ToSingle(Math.PI)/6f);
            camarografo = new Control.Camarografo(
                new Vector3(1f,1f,1f) * 1500, Vector3.Zero, GraphicsDevice.Viewport.AspectRatio, 1f, 6000f);
            //_plant = new Model(GraphicsDevice, );
            _plane = new Escenografia.Plane(GraphicsDevice);
            _edificio = new PrismaRectangularEditable(GraphicsDevice, new Vector3(100f, 400f, 100f));

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
            generadorPrueba.loadModelosAutos(ContentFolder3D + "Auto/RacingCar", ContentFolderEffects + "BasicShader", Content);
            auto.loadModel(ContentFolder3D + "Auto/RacingCar", ContentFolderEffects + "BasicShader", Content);
            
            _plant = Content.Load<Model>(ContentFolder3D + "Plant/indoor plant_02_fbx/plant");
            _basicShader = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            _plane.SetWorldMatrix(Matrix.CreateTranslation(new Vector3(-5f, 0f, -5f)));
            _plane.SetEffect(_basicShader);

            _edificio.SetWorldMatrix(Matrix.CreateTranslation(Vector3.Zero));
            _edificio.SetEffect(_basicShader);

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

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }
            auto.mover(Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds));
            //para que el camarografo nos siga siempre
            camarografo.setPuntoAtencion(auto.posicion);
            //camara.getInputs(Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds));
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        ///
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.LightBlue);

            
            _plane.dibujar(camarografo.getViewMatrix(), camarografo.getProjectionMatrix(), Color.LightGray);
            //_plant.Draw(_plane.getWorldMatrix(), camarografo.getViewMatrix(), camarografo.getProjectionMatrix());
            
            
            auto.dibujar(camarografo.getViewMatrix(), camarografo.getProjectionMatrix(), Color.White);
            generadorPrueba.drawAutos(camarografo.getViewMatrix(), camarografo.getProjectionMatrix(), Color.Crimson);

            _edificio.dibujar(camarografo.getViewMatrix(), camarografo.getProjectionMatrix(), Color.DarkGray);

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