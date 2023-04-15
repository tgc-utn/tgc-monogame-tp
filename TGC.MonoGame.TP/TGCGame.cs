﻿using System;
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
        private Mapa Mapa;
        private List<IElementoDinamico> AutosEnemigos;
        private List<IElemento> Muebles;
        private Effect Effect;
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

            Car = new Car("Models/RacingCarA/RacingCar", Vector3.Zero);
            Mapa = new Mapa();

            #region CargaElementosDinámicos
            AutosEnemigos = new List<IElementoDinamico>();
            var posicionesAutosIA = new Vector3(0f,0f,300f);           
            for(int i=0; i<20; i++){
                var escala = 0.04f * Random.Shared.NextSingle() + 0.04f;

                var unAuto = new EnemyCar("Models/CombatVehicle/Vehicle", escala, posicionesAutosIA);
                AutosEnemigos.Add(unAuto);
                posicionesAutosIA += new Vector3(500f,0f,500f);
            }
            #endregion
            
            #region CargaElementosEstáticos
            Muebles = new List<IElemento>();
            for(int i = 0 ; i<100*20 ; i+=100){
                Muebles.Add(new Mueble("chair", 10f, new Vector3(   i , 40f , -30f )));
            }
            Muebles.Add( new Mueble("mesa", 12f, new Vector3(415f,0f,415f) ));
            Muebles.Add( new Mueble("mesa", 12f, new Vector3(30f,0f,415f) ));
            #endregion

            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GeometriesManager = new GeometriesManager(GraphicsDevice);
            
            Car.Load(Content);
            Mapa.Load(Content);

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

            Mapa.Draw(Camera.View, Camera.Projection);
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