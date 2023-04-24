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
        private int IndiceHabAuto = 0;
        private Camera Camera; 
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

            //Seteando las variables de los efectos que *No van a cambiar*
            GameContent.E_BasicShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_BasicShader.Parameters["Projection"].SetValue(Camera.Projection);
            GameContent.E_SpiralShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_SpiralShader.Parameters["Projection"].SetValue(Camera.Projection);
            GameContent.E_TextureShader.Parameters["View"].SetValue(Camera.View);
            GameContent.E_TextureShader.Parameters["Projection"].SetValue(Camera.Projection);

            //Hogar.Add( new HabitacionTipo ( Vector3 ubicacionEsquinaSuperiorDerecha ));
            Hogar.Add( new HabitacionCocina       (new Vector3(-11000f,0f,7000f)));
            Hogar.Add( new HabitacionPrincipal    (new Vector3(-5000f,0f,5000f)));
            Hogar.Add( new HabitacionOficina      (new Vector3(-1000f,0f,0f)));
            Hogar.Add( new HabitacionToilette     (new Vector3(-9000, 0f, -1000)));
            Hogar.Add( new HabitacionPasillo1     (new Vector3(-5000f, 0f, 1000f)));
            Hogar.Add( new HabitacionPasillo2     (new Vector3(-5000f, 0f, -3000f)));
            Hogar.Add( new HabitacionDormitorio1  (new Vector3(-1000f, 0f, -5000f)));
            Hogar.Add( new HabitacionDormitorio2  (new Vector3(-10000f, 0f, -6000f)));

            foreach(var hab in Hogar)
                Console.WriteLine("Habitacion cargada con {0:D}"+ " elementos.", hab.cantidadElementos());
            //Auto ( posicionInicial )
            Auto = new Auto(Hogar[7].getCenter());
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 
            
            if (Keyboard.GetState().IsKeyDown(Keys.T)){
                IndiceHabAuto++;
                if(IndiceHabAuto >= Hogar.Count) IndiceHabAuto = 0;
                
                Auto.Teleport(Hogar[IndiceHabAuto].getCenter());
            }else
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
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}