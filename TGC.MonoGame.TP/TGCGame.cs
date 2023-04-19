using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        #region constantes
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        #endregion
        internal static TGCGame Game;
        internal static GeometriesManager GeometriesManager;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Car Car;
        private List<Habitacion> HabitacionesPrueba;
        private FollowCamera Camera;

        public TGCGame()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            Car = new Car("Models/RacingCar/RacingCar", Vector3.Zero, Vector3.Zero);


            //Creo que cada habitación debería tener su medida ya establecida (10x10, 10x5, 4x2, etc.) no pasarla por parámetro
            HabitacionesPrueba = new List<Habitacion>();
            HabitacionesPrueba.Add(
                Habitacion.Principal(10, 10, new Vector3(-5000f,0f,-5000f))         
            );
            
            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GeometriesManager = new GeometriesManager(GraphicsDevice);
            
            foreach(var h in HabitacionesPrueba){
                h.Load(Content);
            }
            Car.Load(Content);
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Car.Update(gameTime, keyboardState);

            foreach(var h in HabitacionesPrueba){
                h.Update(gameTime, keyboardState);
            }
            Camera.Mover(keyboardState);

            Camera.Update(gameTime, Car.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            foreach(var h in HabitacionesPrueba){
                h.Draw(Camera.View, Camera.Projection);
            }
            Car.Draw(Camera.View, Camera.Projection);
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}