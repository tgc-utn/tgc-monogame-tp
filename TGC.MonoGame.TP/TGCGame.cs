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
        private float time;

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
        private Model ModelIsland2 { get; set; }
        private Model ModelIsland3 { get; set; }
        private Model ModelCasa { get; set; }
        private Effect IslandEffect { get; set; }
        private Model ModelWater { get; set; }
        private Effect WaterEffect { get; set; }
        private Model ModelPalm1 { get; set; }
        private Model ModelPalm2 { get; set; }
        private Model ModelPalm3 { get; set; }
        private Model ModelPalm4 { get; set; }
        private Model ModelPalm5 { get; set; }
        private Model ModelRock1 { get; set; }
        private Model ModelRock2 { get; set; }
        private Model ModelRock3 { get; set; }
        //private Model ModelRock4 { get; set; }
        private Model ModelRock5 { get; set; }
        private Model ModelBoatSM { get; set; }
        private Effect BoatSMEffect { get; set; }
        private Model ModelPatrol { get; set; }
        private Effect PatrolEffect { get; set; }
        private Model ModelCruiser { get; set; }
        private Effect CruiserEffect { get; set; }
        private Model ModelBarquito { get; set; }
        private Effect IslandMiscEffect { get; set; }

        private Model PlayerBoatModel { get; set; }
        private Effect PlayerBoatEffect { get; set; }
        private Matrix PlayerBoatMatrix { get; set; }
        public Texture2D PlayerBoatTexture;
        public float PlayerSpeed = 0.5f;
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private BoatCamera Camera { get; set; }

        public Texture2D IslandTexture;
        public Texture2D BoatSMTexture;
        public Texture2D PatrolTexture;
        public Texture2D CruiserTexture;
        public Texture2D IslandMiscTexture;
        public Texture2D WaterTexture;

        private Vector3 FrontDirection;
        private float CameraArm;
        private double PlayerRotation;

        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }
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

            CameraArm = 60.0f;
            Camera = new BoatCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, CameraArm, 600), screenSize);

            MovementSpeed = 100.0f;
            RotationSpeed = 0.5f;
            FrontDirection = Vector3.Forward;
            PlayerRotation = 0;
            PlayerBoatMatrix = Matrix.Identity * Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(0, 0, 600);
            //PlayerBoatMatrix = Matrix.Identity * Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateTranslation(0,0,600);

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

            // Cargo el modelos /// ISLA ///
            ModelIsland = Content.Load<Model>(ContentFolder3D + "Island/IslaGeo");
            ModelIsland2 = Content.Load<Model>(ContentFolder3D + "Island/Isla2Geo");
            ModelIsland3 = Content.Load<Model>(ContentFolder3D + "Island/Isla3Geo");
            IslandEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland02Diffuse");

            ModelWater = Content.Load<Model>(ContentFolder3D + "Island/AguaGeo");
            WaterEffect = Content.Load<Effect>(ContentFolderEffects + "WaterShader");
            WaterTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/Water01Diffuse");

            ModelCasa = Content.Load<Model>(ContentFolder3D + "Island/CasaGeo");

            ModelPalm1 = Content.Load<Model>(ContentFolder3D + "Island/Palmera1Geo");
            ModelPalm2 = Content.Load<Model>(ContentFolder3D + "Island/Palmera2Geo");
            ModelPalm3 = Content.Load<Model>(ContentFolder3D + "Island/Palmera3Geo");
            ModelPalm4 = Content.Load<Model>(ContentFolder3D + "Island/Palmera4Geo");
            ModelPalm5 = Content.Load<Model>(ContentFolder3D + "Island/Palmera5Geo");

            ModelRock1 = Content.Load<Model>(ContentFolder3D + "Island/Roca1Geo");
            ModelRock2 = Content.Load<Model>(ContentFolder3D + "Island/Roca2Geo");
            ModelRock3 = Content.Load<Model>(ContentFolder3D + "Island/Roca3Geo");
            //ModelRock4 = Content.Load<Model>(ContentFolder3D + "Island/Roca4Geo");
            ModelRock5 = Content.Load<Model>(ContentFolder3D + "Island/Roca5Geo");

            //// BOTES ////

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
            IslandMiscEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandMiscTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland01Diffuse");

            PlayerBoatModel = Content.Load<Model>(ContentFolder3D + "ShipB/Source/Ship");
            PlayerBoatEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            PlayerBoatTexture = Content.Load<Texture2D>(ContentFolder3D + "ShipB/textures/Battleship_lambert1_AlbedoTransparency.tga");


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
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ProcessKeyboard(elapsedTime);

            // Basado en el tiempo que paso se va generando una rotacion.
            //Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Position = PlayerBoatMatrix.Translation + new Vector3(0, CameraArm, 0);
            FrontDirection = - new Vector3((float)Math.Sin(PlayerRotation), 0.0f, (float)Math.Cos(PlayerRotation));
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

            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            IslandEffect.Parameters["ModelTexture"].SetValue(IslandTexture);
            DrawModel(ModelIsland, Matrix.CreateScale(0.2f), IslandEffect);
            DrawModel(ModelIsland, Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(1.54f) * Matrix.CreateTranslation(800, 0, -300), IslandEffect);
            DrawModel(ModelCasa, Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(780, 56, 620), IslandEffect);

            DrawModel(ModelRock1, Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(350, -10, 350), IslandEffect);
            DrawModel(ModelRock2, Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-350, -10, 350), IslandEffect);
            DrawModel(ModelRock2, Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(0.8f) * Matrix.CreateTranslation(-350, -10, -680), IslandEffect);
            DrawModel(ModelRock3, Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(3f) * Matrix.CreateTranslation(850, -10, 50), IslandEffect);
            DrawModel(ModelRock5, Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(100, -10, -780), IslandEffect);
            DrawModel(ModelRock2, Matrix.CreateScale(0.18f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(530, -10, 780), IslandEffect);
            DrawModel(ModelRock1, Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(1050, -10, 300), IslandEffect);


            IslandMiscEffect.Parameters["ModelTexture"].SetValue(IslandMiscTexture);
            DrawModel(ModelIsland2, Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(800, -2, 600), IslandMiscEffect);
            DrawModel(ModelIsland3, Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-650, -2, -100), IslandMiscEffect);

            DrawModel(ModelPalm1, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(60, 10, 280), IslandMiscEffect);
            DrawModel(ModelPalm2, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(110, 0, 300), IslandMiscEffect);
            DrawModel(ModelPalm3, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(-50, 48, 150), IslandMiscEffect);
            DrawModel(ModelPalm4, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(750, 0, -60), IslandMiscEffect);
            DrawModel(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(580, 0, -150), IslandMiscEffect);
            DrawModel(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(-650, 30, -100), IslandMiscEffect);

            DrawModel(ModelWater, Matrix.CreateScale(2f, 0.01f, 2f), WaterEffect);
            WaterEffect.Parameters["ModelTexture"]?.SetValue(WaterTexture);
            WaterEffect.Parameters["Time"]?.SetValue(time);

            /// Dibujo Botes

            BoatSMEffect.Parameters["ModelTexture"].SetValue(BoatSMTexture);
            DrawModel(ModelBoatSM, Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(-100, 0, 300), BoatSMEffect);

            PatrolEffect.Parameters["ModelTexture"].SetValue(PatrolTexture);
            DrawModel(ModelPatrol, Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(-300, 0, 500), PatrolEffect);

            CruiserEffect.Parameters["ModelTexture"].SetValue(CruiserTexture);
            DrawModel(ModelCruiser, Matrix.CreateScale(0.03f) * Matrix.CreateTranslation(-100, 0, 900), CruiserEffect);

            IslandMiscEffect.Parameters["ModelTexture"].SetValue(IslandMiscTexture);
            DrawModel(ModelBarquito, Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(-200, 0, 700), IslandMiscEffect);

            DrawModel(PlayerBoatModel, Matrix.CreateRotationY((float)PlayerRotation)* PlayerBoatMatrix  , PlayerBoatEffect);
            base.Draw(gameTime);
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

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //var currentMovementSpeed = MovementSpeed;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                MoveForward(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                MoveBackwards(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                RotateRight(RotationSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                RotateLeft(RotationSpeed * elapsedTime);
            }


        }

        private void MoveForward(float amount)
        {
            PlayerBoatMatrix *= Matrix.CreateTranslation(FrontDirection * amount);
        }
        private void MoveBackwards(float amount)
        {
            MoveForward(-amount);
        }
        private void RotateRight(float amount)
        {
               PlayerRotation += amount;
        }
        private void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
    }
}