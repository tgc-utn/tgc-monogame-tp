using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolderModels = "Models/";
        // public const string ContentFolderEffect = "Effects/";
        // public const string ContentFolderMusic = "Music/";
        // public const string ContentFolderSounds = "Sounds/";
        // public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        public const string ModelMK1 = "Models/Spaceship/SpaceShip";
        public const string ModelMK2 = "Models/Spaceship/Motorcycle-MK2";
        public const string ModelMK3 = "Models/Spaceship/SpaceShip-MK-3";
        public const string TextureMK1 = "Textures/SpaceShip/MK-1/SpaceShip-Texture";
        public const string TextureMK2 = "Textures/SpaceShip/MK-2/Motorcycle-MK2-BaseColor";
        public const string TextureMK3 = "Textures/SpaceShip/MK-3/SpaceShip-MK3-Albedo";

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
        private Model SpaceShipModel { get; set; }
        private Model VenusModel { get; set; }
        private float RotationY { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private float VenusRotation { get; set; }

        private Boolean TestRealControls { get; set; } = true;

        private Vector3 Rotation = new Vector3(0,0,0);
        
        private float movementSpeed;
        private float speedUp;

        private Vector3 position;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

            position = new Vector3(0,0,0);
            
            movementSpeed = .5f;
            speedUp = 1;

            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.ApplyChanges();
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

            VenusModel = Content.Load<Model>(ContentFolderModels + "Venus/Venus");

            var venusEffect = (BasicEffect) VenusModel.Meshes[0].Effects[0];
            venusEffect.TextureEnabled = true;
            venusEffect.Texture = Content.Load<Texture2D>(ContentFolderTextures + "Venus/Venus-Texture");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var rotationSpeed = .02f;

            if (state.IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            if (state.IsKeyDown(Keys.NumPad1))
            {
                TestRealControls = true;
                position = new Vector3(0,0,0);
                // Rotation = Matrix.Identity;
                Rotation = new Vector3(0,0,0);
            }
            if (state.IsKeyDown(Keys.NumPad2))
            {
                TestRealControls = false;
                position = new Vector3(0,0,0);
                // Rotation = Matrix.Identity;
                Rotation = new Vector3(0,0,0);
            }

            if (TestRealControls)
            {
                var isMoving = false;
                if (state.IsKeyDown(Keys.W)) 
                {
                    position.Y += movementSpeed * speedUp;
                    // Rotation *= Matrix.CreateRotationY(.05f);
                    
                    if (Rotation.Y < MathHelper.PiOver4)
                    {
                        Rotation.Y += rotationSpeed * (speedUp / 2);
                    }
                    isMoving = true;

                }
                if (state.IsKeyDown(Keys.S))
                {
                    position.Y -= movementSpeed * speedUp;
                    // Rotation.Y = MathHelper.PiOver4 * (-1);
                    // Rotation *= Matrix.CreateRotationY(-.05f);
                    if (Rotation.Y > -MathHelper.PiOver4)
                    {
                        Rotation.Y += -rotationSpeed * (speedUp / 2);
                    }
                    isMoving = true;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    position.X -= movementSpeed * speedUp;
                    // Rotation.X = MathHelper.PiOver4;
                    // Rotation.Z = MathHelper.PiOver4;
                    // Rotation *= Matrix.CreateRotationZ(.05f);
                    if (Rotation.Z < MathHelper.PiOver2)
                    {
                        Rotation.Z += rotationSpeed * (speedUp / 2);
                    }
                    isMoving = true;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    position.X += movementSpeed * speedUp;
                    // Rotation.X = MathHelper.PiOver4  * (-1);
                    // Rotation.Z = MathHelper.PiOver4 * (-1);
                    // Rotation *= Matrix.CreateRotationZ(-.05f);
                    if (Rotation.Z > -MathHelper.PiOver2)
                    {
                        Rotation.Z += -rotationSpeed * (speedUp / 2);
                    }
                    isMoving = true;
                }

                var pressedKeys = state.GetPressedKeys();
                if ( pressedKeys.Length == 2 && pressedKeys.Contains(Keys.Space))
                {
                    if (speedUp < 5)
                    {
                        speedUp += 1;
                    }
                }

                if (pressedKeys.Length == 1)
                {
                    speedUp = 1;
                }

                if (!isMoving)
                {
                    // Rotation = Matrix.Identity;
                    Rotation = new Vector3(0,0,0);
                }
                
            }
            else
            {
                if (state.IsKeyDown(Keys.W)) 
                {
                    // Adelante eje Z
                    position.Z -= 1;

                }
                if (state.IsKeyDown(Keys.S))
                {
                    // Atras eje Z
                    position.Z += 1;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    // Izquierda eje X (negativo) posicion + giro de nave
                    position.X -= 1;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    // Derecha eje X (positivo) posicion + giro de nave
                    position.X += 1;
                }
                if (state.IsKeyDown(Keys.Up))
                {
                    // Subo eje Y (positivo) posicion + giro de nave
                    position.Y += 1;
                }
                if (state.IsKeyDown(Keys.Down))
                {
                    // Bajo eje Y (negativo) posicion + giro de nave
                    position.Y -= 1;
                }

                if (state.IsKeyDown(Keys.Y))
                {
                    Rotation.X += .1f;
                }
                if (state.IsKeyDown(Keys.U))
                {
                    Rotation.Y += .1f;
                }            
                if (state.IsKeyDown(Keys.I))
                {
                    Rotation.Z += .1f;
                }
            }
            

            RotationY += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            VenusRotation += .005f;
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            VenusModel.Draw(World * 
                            Matrix.CreateScale(.3f) * 
                            Matrix.CreateRotationY(VenusRotation) * 
                            Matrix.CreateTranslation(-50f,-25f,0), View, Projection);

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
    }
}