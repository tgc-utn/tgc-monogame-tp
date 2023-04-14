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
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        internal static TGCGame Game;
        internal static GeometriesManager GeometriesManager;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Car Car;
        private Effect Effect;
        private Matrix View;
        private Matrix Projection;
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
            Car = new Car();

            // Necesarias del juego
            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            //Acá inicializamos los objetos que queremos cargar... Después carga la hace automáticamente
            /*AutoPrincipal = new Car("RacingCarA/RacingCar", new Vector3(-1000f,0,1000f));
            AutoSecundario = new IACar("", new Vector3(-800f,0,-800f));

            Elementos = new List<IElemento>();
            Elementos.Add(AutoPrincipal);
            Elementos.Add(AutoSecundario);
            
            */
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GeometriesManager = new GeometriesManager(GraphicsDevice);
            Car.Load(Content);
            
            
            //foreach (var elemento in Elementos){
            //        elemento.Load(Content);
            //}
            
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Car.Update(keyboardState, dTime);
            Camera.Update(gameTime, Car.World);

            // Controlables
            //AutoPrincipal.Update(gameTime, Keyboard.GetState());
            //AutoSecundario.Update(gameTime, Keyboard.GetState());
            Camera.Update(gameTime, Car.World);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            //AutoPrincipal.Draw(Camera.View, Camera.Projection);
            
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);

            Car.Model.Draw(Car.World, Camera.View, Camera.Projection);
            new Floor().Draw(Effect);

        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}