using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const float S_METRO = 500f; // Prueben con 250 y con 1000
        internal static TGCGame Game;
        internal static Content GameContent;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Auto Auto;
        private Auto2 Auto2;
        private int IndiceHabAuto = 0;
        private Camera Camera; 
        private Casa Casa;
        private Song Soundtrack;
        private BoundingBox Cajita1;
        private BoundingBox Cajita2;


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
            GraphicsDevice.BlendState = BlendState.Opaque;

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            Casa = new Casa();

            Game.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Game.Graphics.PreferredBackBufferWidth  = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Game.Graphics.ApplyChanges();
        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            GameContent = new Content(Content, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;         
            // Culling
            // GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            // Soundtrack = GameContent.S_SynthWars;
            // MediaPlayer.IsRepeating = true;
            // MediaPlayer.Volume = 0.5f;

            foreach (var e in GameContent.Efectos){
                e.Parameters["Projection"].SetValue(Camera.Projection);
            }

            Vector3 origen = Vector3.Zero;
            Vector3 fin = new Vector3(1f,1f,1f);
            
            Vector3 desplazamiento = new Vector3(5f,0f,5f);

            Cajita1 = new BoundingBox(origen, fin);
            Cajita2 = new BoundingBox(origen+desplazamiento, fin+desplazamiento);


            Console.WriteLine("> > > > > EN LOAD CONTENT");
            Console.WriteLine("Acá no se tendrían que intersectar");
            Console.WriteLine(Cajita1.Intersects(Cajita2));

            // Defaults
            GameContent.E_BasicShader.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            GameContent.E_BlacksFilter.Parameters["Filter"].SetValue(TGCGame.GameContent.T_MeshFilter); 

            Casa.LoadContent();
            
            Auto  = new Auto (Casa.GetCenter(0));
            Auto2 = new Auto2(Casa.GetCenter(1));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState =Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds); 

            Auto.Update(gameTime, keyboardState);
            Auto2.Update(gameTime, keyboardState);
                                    
            // Control de la música
            // if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Stopped)
            //     MediaPlayer.Play(Soundtrack);
            // else if (keyboardState.IsKeyUp(Keys.W) && MediaPlayer.State == MediaState.Playing)
            //     MediaPlayer.Pause();
            // else if (keyboardState.IsKeyDown(Keys.W) && MediaPlayer.State == MediaState.Paused)
            //     MediaPlayer.Resume();
            // else if (keyboardState.IsKeyDown(Keys.P) && MediaPlayer.State == MediaState.Playing)
            //     MediaPlayer.Stop();

            Casa.Update(gameTime, keyboardState);
            
            Camera.Mover(keyboardState);
            Camera.Update(Auto.World);

            Console.WriteLine("> > > > > EN UPDATE");
            // Acá no se intersectan
            Cajita2.Min = new Vector3(0.5f,0.5f,0.5f);
            Cajita2.Max = new Vector3(1.5f,1.5f,1.5f);

            Console.WriteLine("Acá se van a intersectar");
            Console.WriteLine(Cajita1.Intersects(Cajita2));


            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            foreach (var e in GameContent.Efectos){
                e.Parameters["View"].SetValue(Camera.View);
                e.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }


            Auto.Draw();          
            Auto2.Draw();          
            Casa.Draw();
        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}