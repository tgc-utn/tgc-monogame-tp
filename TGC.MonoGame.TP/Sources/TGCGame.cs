using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;
using TGC.MonoGame.TP.ConcreteEntities;
using TGC.MonoGame.TP.ResourceManagers;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP
{
    internal class TGCGame : Game
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

        internal static readonly ModelManager modelManager = new ModelManager();
        internal static readonly TextureManager textureManager = new TextureManager();
        internal static readonly PhysicSimulation physicSimulation = new PhysicSimulation();

        internal TGCGame()
        {
            graphics = new GraphicsDeviceManager(this);
            // Graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            modelManager.LoadModels(Content, effect);
            textureManager.LoadTextures(Content);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            base.Initialize();
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            entities.Add(new XWing(new Vector3(50f, 0f, 0f), Quaternion.Identity));
            entities.Add(new TIE(new Vector3(100f, 0f, 0f), Quaternion.Identity));
            entities.Add(new Trench(new Vector3(150f, 0f, 0f), Quaternion.Identity));
            entities.Add(new Trench2(new Vector3(200f, 0f, 0f), Quaternion.Identity));

            Box box = new Box(50, 50, 50);
            physicSimulation.CreateStatic(new Vector3(50f, 0f, 0f), Quaternion.Identity, box);

            Sphere sphere = new Sphere(5f);
            Vector3 position = new Vector3(0f, 0f, 150f);
            BodyHandle bodyHandle = physicSimulation.CreateDynamic(position, Quaternion.Identity, sphere, 100f);
            camera.Initialize(GraphicsDevice, bodyHandle);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.Exit())
                Exit();

            physicSimulation.Update();
            camera.Update(gameTime);
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
            physicSimulation.Dispose();
            base.UnloadContent();
        }
    }
}