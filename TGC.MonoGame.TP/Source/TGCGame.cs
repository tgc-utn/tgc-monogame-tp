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
        private Camera Camera;
        Televisor tvPrincipal1, tvConferencias, tvPrincipal2; 
        private List<IHabitacion> Hogar = new List<IHabitacion>();

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

            tvPrincipal1 = new Televisor(new Vector3(-5000f,0f,5000f) + new Vector3(7500f,150f,300f), 0f);
            tvPrincipal2 = new Televisor(new Vector3(-4300f, 700f, 12300f), MathHelper.PiOver2);
            tvConferencias = new Televisor(new Vector3(-5000f,0f,-5000f) + new Vector3(3200f,150f,9200f), MathHelper.Pi);

            Hogar.Add( new HabitacionConferencias (new Vector3(-5000f,0f,-5000f)));
            Hogar.Add( new HabitacionCocina       (new Vector3(-11000f,0f,-1000f)));
            Hogar.Add( new HabitacionPrincipal    (new Vector3(-5000f,0f,5000f)));
            Hogar.Add( new HabitacionOficina      (new Vector3(0f,0f,-10000f)));
            Hogar.Add( new HabitacionCocina       (new Vector3(-11000f,0f,-1000f)));
            Hogar.Add( new HabitacionToilette     (new Vector3(-9000, 0f, 5000)));
            //HabitacionesPrueba.Add(Habitacion.Banio(4, 4, new Vector3(-9000, 0f, 5000)));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 
            
            Auto.Update(gameTime, keyboardState);

            foreach(IHabitacion habitacion in Hogar)
                habitacion.Update(gameTime, keyboardState);
            
            Camera.Mover(keyboardState);
            Camera.Update(gameTime, Auto.World);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
           
            GameContent.E_BasicShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_BasicShader.Parameters["Projection"].SetValue(Camera.Projection);
            
            GameContent.E_TextureShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_TextureShader.Parameters["Projection"].SetValue(Camera.Projection);

            GameContent.E_SpiralShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_SpiralShader.Parameters["Projection"].SetValue(Camera.Projection);
            GameContent.E_SpiralShader.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);

            foreach(IHabitacion habitacion in Hogar)
                habitacion.Draw(Camera.View, Camera.Projection);

            Auto.Draw(Camera.View, Camera.Projection);


            //Puesto acá solo para probar
            tvConferencias.Draw();            
            tvPrincipal1.Draw();            
            tvPrincipal2.Draw();            
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}