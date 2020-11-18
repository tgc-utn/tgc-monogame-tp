using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chinchulines.Graphics;
using Chinchulines.Enemigo;
using System.Collections.Generic;
using Chinchulines.Entities;
using Microsoft.Xna.Framework.Media;

namespace Chinchulines
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class ChinchuGame : Game
    {
        public const string ContentFolderModels = "Models/";
        public const string ContentFolderEffect = "Effects/";
        public const string ContentFolderMusic = "Music/";
        // public const string ContentFolderSounds = "Sounds/";
        // public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        public const string ModelMK1 = "Models/Spaceships/SpaceShip-MK1";
        public const string ModelMK3 = "Models/Spaceships/SpaceShip-MK3";
        public const string TextureMK1 = "Textures/Spaceships/MK1/MK1-Texture";
        public const string TextureMK3 = "Textures/Spaceships/MK3/MK3-Albedo";

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public ChinchuGame()
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
        private Model SpaceShipModelMK1 { get; set; }
        private Model SpaceShipModelMK3 { get; set; }
        private Model VenusModel { get; set; }
        private float RotationY { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }


        private float VenusRotation { get; set; }

        private float movementSpeed;
        private float speedUp;

        private Vector3 position;

        float clock = 0f;

        private enemyManager EM;

        Skybox skybox;
        private Trench _trench;
        private Vector3 _lightDirection = new Vector3(3, -2, 5);

        private Vector3 _spaceshipPosition = new Vector3(0, 0, 0);
        private Quaternion _spaceshipRotation = Quaternion.Identity;
        private float _gameSpeed = 1.0f;

        private int barrelSide = 0; // -1 for left, 1 for rigth, 0 for nothing

        private bool turnBack = false;

        private Vector3 _cameraPosition;
        private Vector3 _cameraDirection;

        private Random ran = new Random();

        private Song background;


        private LaserManager _laserManager;

        public GameState State { get; private set; }

        private SpriteFont _spriteFont;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            View = Matrix.CreateLookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 1000f);


            //World = Matrix.Identity;
            //View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            //Projection =
            //    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

            position = new Vector3(0, 0, 0);

            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.ApplyChanges();

            EM = new enemyManager();
            for (int i = 0; i < 10; i++) EM.CrearEnemigo();

            _trench = new Trench();
            _laserManager = new LaserManager();

            State = GameState.Playing;

            base.Initialize();
        }
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SpaceShipModelMK1 = Content.Load<Model>(ModelMK1); // Se puede cambiar por MK2 y MK3

            var spaceShipEffect = (BasicEffect)SpaceShipModelMK1.Meshes[0].Effects[0];
            spaceShipEffect.TextureEnabled = true;
            spaceShipEffect.Texture = Content.Load<Texture2D>(TextureMK1);

            VenusModel = Content.Load<Model>(ContentFolderModels + "Venus/Venus");

            EM.LoadContent(Content);

            background = Content.Load<Song>(ContentFolderMusic + "Rising Tide (faster)");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(background);

            SpaceShipModelMK3 = Content.Load<Model>(ModelMK3);

            var spaceShipEffect3 = (BasicEffect)SpaceShipModelMK3.Meshes[0].Effects[0];
            spaceShipEffect3.TextureEnabled = true;
            spaceShipEffect3.Texture = Content.Load<Texture2D>(TextureMK3); // Se puede cambiar por MK2 y MK3

            var venusEffect = (BasicEffect)VenusModel.Meshes[0].Effects[0];
            venusEffect.TextureEnabled = true;
            venusEffect.Texture = Content.Load<Texture2D>(ContentFolderTextures + "Venus/Venus-Texture");


            skybox = new Skybox("Skyboxes/SunInSpace", Content);
            _trench.LoadContent(ContentFolderTextures + "Trench/TrenchTexture", ContentFolderEffect + "Trench", Content, Graphics);
            _laserManager.LoadContent(ContentFolderTextures + "Lasers/doble-laser-verde", ContentFolderEffect + "Trench", Content, Graphics);

            SetUpCamera();

            _spriteFont = Content.Load<SpriteFont>("Fonts/Font");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                State = GameState.GameOver;
                this.UnloadContent();
                Exit();
            }

            if (State == GameState.Playing)
            {

                UpdateCamera();
                MoveSpaceship(gameTime);
                InputController(gameTime);

                movementSpeed = gameTime.ElapsedGameTime.Milliseconds / 500.0f * _gameSpeed;
                MoveForward(ref _spaceshipPosition, _spaceshipRotation, movementSpeed);

                EM.Update(gameTime, position);

                RotationY += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
                VenusRotation += .005f;

                _laserManager.UpdateLaser(movementSpeed);
            }

            base.Update(gameTime);
        }

        private void InputController(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.Space))
            {
                _laserManager.ShootLaser(gameTime, _spaceshipPosition, _spaceshipRotation);
            }
        }

        private void SetUpCamera()
        {
            View = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.2f, 500.0f);
        }

        private void MoveSpaceship(GameTime gameTime)
        {
            float leftRightRotation = 0;
            float upDownRotation = 0;

            float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            turningSpeed *= 1.6f * _gameSpeed;

            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.D)) leftRightRotation += turningSpeed;
            if (keys.IsKeyDown(Keys.A)) leftRightRotation -= turningSpeed;
            if (keys.IsKeyDown(Keys.S)) upDownRotation += turningSpeed;
            if (keys.IsKeyDown(Keys.W)) upDownRotation -= turningSpeed;

            if (keys.IsKeyDown(Keys.LeftShift)) if(speedUp < 0.10f)speedUp += 0.01f;
            if (keys.IsKeyUp(Keys.LeftShift)) if(speedUp != 0) speedUp -= 0.01f;

            if (keys.IsKeyDown(Keys.E)) barrelSide= -1;
            if (keys.IsKeyDown(Keys.Q)) barrelSide = 1;
            if (keys.IsKeyDown(Keys.X)) turnBack = true;

            if(turnBack)
            {
                clock++;
                upDownRotation += turningSpeed;
                if(clock > 117f)
                {
                    clock = 0;
                    turnBack = !turnBack;
                    int turn = ran.Next(-1, 1);
                    while (turn == 0) turn = ran.Next(-1, 1);
                    barrelSide = 2 * turn;
                }
            }
            
            if(barrelSide != 0)
            {
                if(Math.Abs(barrelSide) == 1)BarrelRoll(59f, ref barrelSide);
                else
                {
                    if(Math.Abs(barrelSide) == 2)
                    {
                        BarrelRoll((59f / 2), ref barrelSide);// con esto tendria que manejar para que se ponga en horizontal
                    }                                         // pero no está acabado
                }
            }
            
            
            
                _spaceshipRotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), leftRightRotation)
                        * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRotation);
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            position += addVector * (speed + speedUp);
        }

        private void BarrelRoll(float time, ref int side)
        {
            clock++;

           if(Math.Sign(side) == -1) _spaceshipRotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), MathHelper.PiOver2 / 15);
           else if(Math.Sign(side) == 1) _spaceshipRotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), MathHelper.PiOver2 / 15);

            if (clock > time)
            {
                clock = 0;
                side = 0;
                _spaceshipRotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), 0);
            }
        }

        private void UpdateCamera()
        {

            Vector3 cameraPosition = new Vector3(0, 0.1f, 0.6f);
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateFromQuaternion(_spaceshipRotation));
            cameraPosition += _spaceshipPosition;
            Vector3 cameraUpDirection = new Vector3(0, 1, 0);
            cameraUpDirection = Vector3.Transform(cameraUpDirection, Matrix.CreateFromQuaternion(_spaceshipRotation));
            
            View = Matrix.CreateLookAt(cameraPosition, _spaceshipPosition, cameraUpDirection);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.2f, 500.0f);

            _cameraPosition = cameraPosition;
            _cameraDirection = cameraUpDirection;
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (State == GameState.Playing)
            {
                //Finalmente invocamos al draw del modelo.
                RasterizerState originalRasterizerState = Graphics.GraphicsDevice.RasterizerState;
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Graphics.GraphicsDevice.RasterizerState = rasterizerState;

                skybox.Draw(View, Projection, position);

                Graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

                VenusModel.Draw(World *
                                Matrix.CreateScale(.05f) *
                                Matrix.CreateRotationY(VenusRotation) *
                                Matrix.CreateTranslation(-5f, -2f, -10), View, Projection);

                SpaceShipModelMK1.Draw(
                                    Matrix.CreateScale(0.005f) *
                                    Matrix.CreateFromQuaternion(_spaceshipRotation) *
                                    Matrix.CreateTranslation(_spaceshipPosition)
                    , View, Projection);

                EM.Draw(View, Projection);

                SpaceShipModelMK3.Draw(
                                Matrix.CreateScale(.008f) *
                                Matrix.CreateRotationY(-VenusRotation) *
                                Matrix.CreateTranslation(3f, 2f, -10), View, Projection);

                _trench.Draw(View, Projection, _lightDirection, Graphics);

                _laserManager.DrawLasers(View, Projection, _cameraPosition, _cameraDirection, Graphics);
                
                DrawHUD();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            base.Draw(gameTime);
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

        // TODO: Agregar variables para el tiempo restante, checkpoints y vida
        private void DrawHUD()
        {
            SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);
            SpriteBatch.DrawString(_spriteFont, "TIEMPO RESTANTE: 04:32",
                new Vector2(50, 50), Color.Green);
            SpriteBatch.DrawString(_spriteFont, "CHECKPOINTS: 1 de 10", 
                new Vector2(50, 80), Color.Green);
            SpriteBatch.DrawString(_spriteFont, "VIDA: 100%",
                new Vector2(50, 110), Color.Green);
            SpriteBatch.End();
        }
    }
}
