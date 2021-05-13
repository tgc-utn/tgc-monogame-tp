using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP
{
    internal class TGCGame : Game
    {
        private SpriteBatch SpriteBatch { get; set; }

        internal static Content content;
        internal static readonly PhysicSimulation physicSimulation = new PhysicSimulation();
        internal static readonly World world = new World();
        private readonly Camera camera = new Camera();

        internal TGCGame()
        {
            new GraphicsDeviceManager(this);
            // Graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void LoadContent()
        {
            content = new Content(Content);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            base.Initialize();
            world.Initialize();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            Sphere sphere = new Sphere(5f);
            Vector3 position = new Vector3(0f, 0f, 150f);
            BodyHandle bodyHandle = physicSimulation.CreateDynamic(position, Quaternion.Identity, sphere, 100f);
            camera.Initialize(GraphicsDevice, bodyHandle);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.Exit())
                Exit();

            world.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            physicSimulation.Update();
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            content.E_BasicShader.Parameters["View"].SetValue(camera.View);
            content.E_BasicShader.Parameters["Projection"].SetValue(camera.Projection);
            world.Draw();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            physicSimulation.Dispose();
            base.UnloadContent();
        }
    }
}