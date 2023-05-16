using System;
using BepuPhysics;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.Samples.Physics.Bepu;
using TGC.MonoGame.Samples.Viewer.Gizmos;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const float GRAVITY = -150f;
        public const float S_METRO = 250f; // Prueben con 250 y con 1000
        internal static TGCGame Game;
        internal static Content GameContent;
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        internal static Simulation Simulation;
        internal static Gizmos Gizmos;
        public SimpleThreadDispatcher ThreadDispatcher { get; private set; }
        public BufferPool BufferPool { get; private set; }
        private Auto Auto;
        private Auto2 Auto2;
        private Camera Camera; 
        private Casa Casa;
        private Song Soundtrack;


        public TGCGame()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // > > > > Simulación
            BufferPool = new BufferPool(); // optimización
            var targetThreadCount = Math.Max(1,
                Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            ThreadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
            // > > > > Fin simulación

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            Casa = new Casa();

            Game.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height*3/4;
            Game.Graphics.PreferredBackBufferWidth  = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*3/4;
            Game.Graphics.ApplyChanges();
        
            Gizmos = new Gizmos();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // > > > > Simulación
            Simulation = Simulation.Create(
                                BufferPool, 
                                new NarrowPhaseCallbacks(new SpringSettings(30, 1)),
                                new PoseIntegratorCallbacks(new NumericVector3(0, GRAVITY, 0), 0.5f, 0.5f), 
                                new SolveDescription(8, 1));

            // > > > > Fin simulación


            base.LoadContent();
            GameContent = new Content(Content, GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;     

            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));    
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

            Casa.LoadContent();
            
            Auto  = new Auto (Casa.GetCenter(0));
            Auto2 = new Auto2(Casa.GetCenter(0));
        }

        protected override void Update(GameTime gameTime)
        {

            Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);

            Simulation.Timestep(1 / 60f, ThreadDispatcher);

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
            
            // Camera.Mover(keyboardState);
            Camera.Update(Auto.World);

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


            Gizmos.Draw();
        }
        
        protected override void UnloadContent()
        {
            // TODO check why Simulation.Dispose method sometimes fails
            Simulation.Dispose();
            BufferPool.Clear();
            ThreadDispatcher.Dispose();
            Content.Unload();

            base.UnloadContent();
        }
    }
}