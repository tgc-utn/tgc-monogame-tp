﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Graphics;

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
        public const string ModelMK1 = "Models/Spaceships/SpaceShip-MK1";
        public const string ModelMK2 = "Models/Spaceships/Motorcycle-MK2";
        public const string ModelMK3 = "Models/Spaceships/SpaceShip-MK3";
        public const string TextureMK1 = "Textures/Spaceships/MK1/MK1-Texture";
        public const string TextureMK2 = "Textures/Spaceships/MK2/MK2-BaseColor";
        public const string TextureMK3 = "Textures/Spaceships/MK3/MK3-Albedo";

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
        private Model SpaceShipModelMK1 { get; set; }
        private Model SpaceShipModelMK2 { get; set; }
        private Model SpaceShipModelMK3 { get; set; }
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

        Skybox skybox;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            View = Matrix.CreateLookAt(new Vector3(0, 0, 20), Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 1000f);


            //World = Matrix.Identity;
            //View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            //Projection =
            //    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

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

            SpaceShipModelMK1 = Content.Load<Model>(ModelMK1); // Se puede cambiar por MK2 y MK3
            VenusModel = Content.Load<Model>(ContentFolderModels + "Venus/Venus");
            
            var spaceShipEffect = (BasicEffect)SpaceShipModelMK1.Meshes[0].Effects[0];
            spaceShipEffect.TextureEnabled = true;
            spaceShipEffect.Texture = Content.Load<Texture2D>(TextureMK1); // Se puede cambiar por MK2 y MK3

            SpaceShipModelMK2 = Content.Load<Model>(ModelMK2);

            var spaceShipEffect2 = (BasicEffect)SpaceShipModelMK2.Meshes[0].Effects[0];
            spaceShipEffect2.TextureEnabled = true;
            spaceShipEffect2.Texture = Content.Load<Texture2D>(TextureMK2);

            SpaceShipModelMK3 = Content.Load<Model>(ModelMK3);

            var spaceShipEffect3 = (BasicEffect)SpaceShipModelMK3.Meshes[0].Effects[0];
            spaceShipEffect3.TextureEnabled = true;
            spaceShipEffect3.Texture = Content.Load<Texture2D>(TextureMK3); // Se puede cambiar por MK2 y MK3

            var venusEffect = (BasicEffect) VenusModel.Meshes[0].Effects[0];
            venusEffect.TextureEnabled = true;
            venusEffect.Texture = Content.Load<Texture2D>(ContentFolderTextures + "Venus/Venus-Texture");

            skybox = new Skybox("Skyboxes/SunInSpace", Content);

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {


            // Con Numpad 1 -> Movimientos simples de nave (a,s,d,w)
            // Con Numpad 2 -> Movimientos posicion y rotacion (a,s,d,w,Up,Down,y,u,i)
            var state = Keyboard.GetState();
            var rotationSpeed = .02f;

            InputController(state, rotationSpeed);

            RotationY += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            VenusRotation += .005f;



            base.Update(gameTime);
        }

        private void InputController(KeyboardState state, float rotationSpeed)
        {
            if (state.IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            if (state.IsKeyDown(Keys.NumPad1))
            {
                TestRealControls = true;
                position = new Vector3(0, 0, 0);
                // Rotation = Matrix.Identity;
                Rotation = new Vector3(0, 0, 0);
            }
            if (state.IsKeyDown(Keys.NumPad2))
            {
                TestRealControls = false;
                position = new Vector3(0, 0, 0);
                // Rotation = Matrix.Identity;
                Rotation = new Vector3(0, 0, 0);
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
                if (pressedKeys.Length == 2 && pressedKeys.Contains(Keys.Space))
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
                    Rotation = new Vector3(0, 0, 0);
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
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
                            Matrix.CreateTranslation(-5f,-2f,-10), View, Projection);
            
            // SpaceShipModel.Draw(World * Matrix.CreateScale(.8f) * Matrix.CreateRotationY(RotationY), View, Projection);
            
            SpaceShipModelMK1.Draw(World * //Matrix.CreateTranslation(0,-15f,0) * 
                                Matrix.CreateScale(.15f) *
                                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                                // Rotation *
                                Matrix.CreateTranslation(position) 
                , View, Projection);

            SpaceShipModelMK2.Draw(World *
                            Matrix.CreateScale(.08f) *
                            Matrix.CreateRotationY(VenusRotation) *
                            Matrix.CreateTranslation(4f, -2f, -10), View, Projection);

            SpaceShipModelMK3.Draw(World *
                            Matrix.CreateScale(.08f) *
                            Matrix.CreateRotationY(-VenusRotation) *
                            Matrix.CreateTranslation(3f, 2f, -10), View, Projection);

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