using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.PhysicsAPI;
using TGC.MonoGame.TP.Sources.Entities;

namespace TGC.MonoGame.TP
{
    public class TGCGame : Game
    {
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";

        private readonly GraphicsDeviceManager graphics;
        private Effect effect;
        private SpriteBatch SpriteBatch { get; set; }

        private readonly Camera camera = new Camera();
        private readonly List<Entity> entities = new List<Entity>();

        private readonly BufferPool bufferPool = new BufferPool();
        private readonly SimpleThreadDispatcher threadDispatcher;
        private Simulation simulation;

        public TGCGame()
        {
            graphics = new GraphicsDeviceManager(this);
            // Graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            int targetThreadCount = Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            threadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            ModelManager.LoadModels(Content, effect);
            TextureManager.LoadTextures(Content);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            camera.Initialize(GraphicsDevice);
            base.Initialize();
            InitializePhysicSimulation();
            InitializeWorld();
        }

        private void InitializePhysicSimulation() => simulation = 
            Simulation.Create(bufferPool, new NarrowPhaseCallbacks(),
            new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -100, 0)), new PositionFirstTimestepper());

        private void InitializeWorld()
        {
            entities.Add(new XWing(new Vector3(50f, 0f, 0f), Quaternion.Identity));
            entities.Add(new TIE(new Vector3(100f, 0f, 0f), Quaternion.Identity));
            entities.Add(new Trench(new Vector3(150f, 0f, 0f), Quaternion.Identity));
            entities.Add(new Trench2(new Vector3(200f, 0f, 0f), Quaternion.Identity));
        }

        protected override void Update(GameTime gameTime)
        {
            simulation.Timestep(1 / 60f, threadDispatcher);
            camera.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            entities.ForEach(entity => entity.Draw(effect));
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}