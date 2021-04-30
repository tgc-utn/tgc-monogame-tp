using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;


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
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model ModelIsland { get; set; }
        private Effect IslandEffect { get; set; }
        private Model ModelBoatSM { get; set; }
        private Effect BoatSMEffect { get; set; }
        private Model ModelPatrol { get; set; }
        private Effect PatrolEffect { get; set; }
        private Model ModelCruiser { get; set; }
        private Effect CruiserEffect { get; set; }
        private Model ModelBarquito { get; set; }
        private Effect BarquitoEffect { get; set; }
        private Model ModelWater { get; set; }
        private Effect WaterEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private FreeCamera Camera { get; set; }

        public Texture2D IslandTexture;
        public Texture2D BoatSMTexture;
        public Texture2D PatrolTexture;
        public Texture2D CruiserTexture;
        public Texture2D BarquitoTexture;
        public Texture2D WaterTexture;

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
            //var rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 50, 500), screenSize);

            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

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

            // Cargo el modelos
            ModelIsland = Content.Load<Model>(ContentFolder3D + "Island/IslaGeo");
            IslandEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland02Diffuse");

            ModelBoatSM = Content.Load<Model>(ContentFolder3D + "Botes/SMGeo");
            BoatSMEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            BoatSMTexture = Content.Load<Texture2D>(ContentFolderTextures + "Botes/SM_T_Boat_M_Boat_BaseColor");

            ModelPatrol = Content.Load<Model>(ContentFolder3D + "Botes/PatrolGeo");
            PatrolEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            PatrolTexture = Content.Load<Texture2D>(ContentFolderTextures + "Botes/T_Patrol_Ship_1K_BaseColor");

            ModelCruiser = Content.Load<Model>(ContentFolder3D + "Botes/CruiserGeo");
            CruiserEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            CruiserTexture = Content.Load<Texture2D>(ContentFolderTextures + "Botes/T_Cruiser_M_Cruiser_BaseColor");

            ModelBarquito = Content.Load<Model>(ContentFolder3D + "Botes/BarquitoGeo");
            BarquitoEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            BarquitoTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland01Diffuse");

            ModelWater = Content.Load<Model>(ContentFolder3D + "Island/AguaGeo");
            WaterEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            WaterTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/Water01Diffuse");

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
                //Salgo del juego.
                Exit();

            // Basado en el tiempo que paso se va generando una rotacion.
            //Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.White);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            IslandEffect.Parameters["ModelTexture"].SetValue(IslandTexture);
            DrawModel(ModelIsland, Matrix.CreateScale(0.1f), IslandEffect);
            DrawModel(ModelIsland, Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(1.54f) * Matrix.CreateTranslation(600, 0, 300), IslandEffect);
            
            BoatSMEffect.Parameters["ModelTexture"].SetValue(BoatSMTexture);
            DrawModel(ModelBoatSM, Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(-100, 0, 300), BoatSMEffect);

            PatrolEffect.Parameters["ModelTexture"].SetValue(PatrolTexture);
            DrawModel(ModelPatrol, Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(-100, 0, 500), PatrolEffect);

            CruiserEffect.Parameters["ModelTexture"].SetValue(CruiserTexture);
            DrawModel(ModelCruiser, Matrix.CreateScale(0.05f) * Matrix.CreateRotationY(3.14f) * Matrix.CreateTranslation(550, 0, 700), CruiserEffect);

            BarquitoEffect.Parameters["ModelTexture"].SetValue(BarquitoTexture);
            DrawModel(ModelBarquito, Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(-100, 0, 700), PatrolEffect);

            WaterEffect.Parameters["ModelTexture"].SetValue(WaterTexture);
            DrawModel(ModelWater, Matrix.CreateScale(2f,0.01f,2f), WaterEffect);

        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect)
        {
            foreach (var mesh in geometry.Meshes)
            {
                effect.Parameters["World"].SetValue(transform);
                effect.Parameters["View"].SetValue(Camera.View);
                effect.Parameters["Projection"].SetValue(Camera.Projection);
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
                mesh.Draw();
            }
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