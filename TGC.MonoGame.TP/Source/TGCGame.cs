using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        internal static TGCGame Game;
        internal static Content GameContent;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Auto Auto;
        private List<Habitacion> HabitacionesPrueba;
        private Camera Camera;

        public TGCGame()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GameContent = new Content(Content, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Auto = new Auto(Vector3.Zero, Vector3.Zero);
            //Creo que cada habitación debería tener su medida ya establecida (10x10, 10x5, 4x2, etc.) no pasarla por parámetro
            HabitacionesPrueba = new List<Habitacion>();
            HabitacionesPrueba.Add(Habitacion.SalaConferencias(10, 10, new Vector3(-5000f,0f,-5000f)));
            HabitacionesPrueba.Add(Habitacion.Cocina(10, 10, new Vector3(-15000f,0f,-5000f)));
            HabitacionesPrueba.Add(Habitacion.Principal(10, 10, new Vector3(-5000f,0f,5000f)));
            HabitacionesPrueba.Add(Habitacion.Oficina(5, 5, new Vector3(-2500f,0f,-10000f)));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 
            
            Auto.Update(gameTime, keyboardState);

            foreach(Habitacion habitacion in HabitacionesPrueba)
                habitacion.Update(gameTime, keyboardState);
            
            Camera.Mover(keyboardState);
            Camera.Update(gameTime, Auto.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GameContent.E_BasicShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_BasicShader.Parameters["Projection"].SetValue(Camera.Projection);

            GraphicsDevice.Clear(Color.White);

            foreach(Habitacion habitacion in HabitacionesPrueba)
                habitacion.Draw(Camera.View, Camera.Projection);

            Auto.Draw(Camera.View, Camera.Projection);
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}