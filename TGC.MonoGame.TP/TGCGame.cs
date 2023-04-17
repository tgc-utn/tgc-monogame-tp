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

        private List<IElementoDinamico> AutosEnemigos;
        private List<IElemento> Muebles;
        private FollowCamera Camera;

        private Habitacion HabitacionPrueba;

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
            HabitacionPrueba = new Habitacion(10, 10, Vector3.Zero);

            #region CargaElementosDinámicos
            AutosEnemigos = new List<IElementoDinamico>();
            
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;

                var unAuto = new EnemyCar("Models/CombatVehicle/Vehicle", escala, posicionesAutosIA, Vector3.Zero);
                AutosEnemigos.Add(unAuto);
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion
            
            #region CargaElementosEstáticos
            Muebles = new List<IElemento>();
            for(int i = 0 ; i<100*20 ; i+=100){
                Muebles.Add(new Mueble("Chair", 10f, new Vector3(   i , 40f , -30f ), Vector3.Zero));
            }
            Muebles.Add( new Mueble("Mesa", 12f, new Vector3(415f,0f,415f), new Vector3(0, MathHelper.PiOver2, 0) ));
            Muebles.Add( new Mueble("Mesa", 12f, new Vector3(30f,0f,415f), Vector3.Zero ));
            Muebles.Add( new Mueble("Inodoro", 15f, new Vector3(30f,0f,215f), new Vector3(0, MathHelper.PiOver2, 0) ));
            Muebles.Add(new Mueble("Sillon", 10f, new Vector3(250f,30f,250f), Vector3.Zero));
            #endregion

            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GeometriesManager = new GeometriesManager(GraphicsDevice);
            
            Car.Load(Content);
            HabitacionPrueba.Load(Content);

            foreach(var a in AutosEnemigos){
                a.Load(Content);
            }
            foreach(var m in Muebles){
                m.Load(Content);
            }
          
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Car.Update(gameTime, keyboardState);
            
            foreach(var a in AutosEnemigos){
                a.Update(gameTime, keyboardState);
            }

            Camera.Update(gameTime, Car.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            HabitacionPrueba.Draw(Camera.View, Camera.Projection);
/* 
            var pared = Pared.Derecha(10,10,Vector3.Zero);
            pared.Load(Content);
            pared.Draw(Camera.View, Camera.Projection);
             */
            Car.Draw(Camera.View, Camera.Projection);

            foreach(var a in AutosEnemigos){
                a.Draw(Camera.View, Camera.Projection);
            }
            foreach(var m in Muebles){
                m.Draw(Camera.View, Camera.Projection);
            }
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}