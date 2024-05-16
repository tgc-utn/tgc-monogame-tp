using System;
using System.Collections.Generic;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using ThunderingTanks.Cameras;
using ThunderingTanks.Content.Models;

namespace ThunderingTanks
{
    public class TankGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private GraphicsDeviceManager Graphics { get; }
        public KeyboardState keyboardState;

        private MapScene City { get; set; }
        private SkyBox _skyBox;

        private Tank Panzer { get; set; }
        private Model rock { get; set; }
        private Model tree { get; set; }
        private Model casa { get; set; }
        private Model antitank { get; set; }

        private FreeCamera _freeCamera;
        private readonly Vector3 _cameraInitialPosition = new(0f, 200f, 300f);

        private TargetCamera _targetCamera;

        private Matrix World { get; set; }
        private Matrix PanzerMatrix { get; set; }

        private List<Projectile> projectiles = new();

        private int N_Of_Rocks { get; set; }
        private List<int> NumerosX { get; set; }
        private List<int> NumerosZ { get; set; }

        public TankGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.GraphicsProfile = GraphicsProfile.Reach;
            Graphics.ApplyChanges();

            keyboardState = new KeyboardState();

            Panzer = new Tank
            {
                TankVelocity = 300f,
                TankRotation = 20f
            };

            _freeCamera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition);
            _targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, PanzerMatrix.Forward);

            World = Matrix.Identity;
            PanzerMatrix = Matrix.Identity;

            NumerosX = new List<int>();
            NumerosZ = new List<int>();

            var random = new Random();

            N_Of_Rocks = 50;

            for (int i = 0; i < N_Of_Rocks; i++)
            {
                NumerosX.Add(random.Next(-20000, 20000)); // Rango más amplio
                NumerosZ.Add(random.Next(-20000, 20000)); // Rango más amplio
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            City = new MapScene(Content);

            Panzer.GameModel = Content.Load<Model>("Models/Panzer/Panzer");
            rock = Content.Load<Model>("Models/nature/rock/Rock_1");
            antitank = Content.Load<Model>("Models/assets militares/rsg_military_antitank_hedgehog_01");
            tree = Content.Load<Model>("Models/nature/tree/Southern Magnolia-CORONA");
            casa = Content.Load<Model>("Models/casa/house");

            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");

            Panzer.TankBox = Collisions.CreateAABBFrom(Panzer.GameModel);

            _skyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 25000);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PanzerMatrix = Panzer.Update(gameTime, keyboardState, PanzerMatrix);

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Projectile projectile = Panzer.Shoot(PanzerMatrix);

                if (projectile != null)
                    projectiles.Add(projectile);
            }

            UpdateProjectiles(gameTime);

            _freeCamera.Update(gameTime);
            _targetCamera.Update(Panzer.Position, MathHelper.ToRadians(Panzer.Rotation) + MathHelper.ToRadians(180));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Camera camara = _targetCamera;

            tree.Draw(Matrix.CreateScale(3f) * Matrix.CreateTranslation(new Vector3(100, -10, 9000)), camara.View, camara.Projection);

            tree.Draw(Matrix.CreateScale(3f) * Matrix.CreateTranslation(new Vector3(100, -10, -9000)), camara.View, camara.Projection);

            tree.Draw(Matrix.CreateScale(3f) * Matrix.CreateTranslation(new Vector3(9000, -10, 100)), camara.View, camara.Projection);

            tree.Draw(Matrix.CreateScale(3f) * Matrix.CreateTranslation(new Vector3(-9000, -10, 100)), camara.View, camara.Projection);

            casa.Draw(Matrix.CreateScale(500f) * Matrix.CreateTranslation(new Vector3(-9000, 0, 7000)), camara.View, camara.Projection);

            // Dibuja Las Piedras Aleatoriamente en el Mapa
            DrawRocks(camara);

            // Dibuja el Borde del Mapa con el Objeto Antitanque
            DrawBorder(camara);

            // Dibuja La Ciudad
            City.Draw(gameTime, camara.View, camara.Projection);

            // Dibuja el Tanque (Panzer)
            Panzer.Draw(PanzerMatrix, camara.View, camara.Projection);

            // Dibujar los proyectiles
            DrawProjectiles(rock, camara.View, camara.Projection);

            // Dibuja El Cielo
            DrawSkyBox(camara.View, camara.Projection, camara.Position);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        // ------------ FUNCTIONS ------------ //

        private void DrawSkyBox(Matrix view, Matrix projection, Vector3 position)
        {
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();

            rasterizerState.CullMode = CullMode.None;

            GraphicsDevice.RasterizerState = rasterizerState;

            _skyBox.Draw(view, projection, position);

            GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        private void DrawRocks(Camera camera)
        {
            List<Matrix> rockTransforms = new List<Matrix>(); // Crear la lista de transformaciones de las rocas

            for (int i = 0; i < N_Of_Rocks; i++)
            {
                Vector3 vector = new Vector3(NumerosX[i], 2, NumerosZ[i]);

                Matrix translateMatrixAntitanque = Matrix.CreateTranslation(vector);
                Matrix worldMatrixAntitanque = Matrix.CreateScale(3f) * translateMatrixAntitanque;

                rockTransforms.Add(worldMatrixAntitanque);
            }

            foreach (var transform in rockTransforms)
            {
                rock.Draw(transform, camera.View, camera.Projection);
            }
        }

        private void DrawBorder(Camera camera)
        {
            float DistanceToEdge = 40f; //trate de usar este código -> MapScene.DistanceBetweenParcels * MapScene.NumInstances/4 <- para no hardcodear pero no funciona;

            for (float i = 0; i < 80f; i += 1f)
            {
                Vector3 Corner1 = new Vector3(DistanceToEdge, 0, DistanceToEdge - i);
                Vector3 Corner2 = new Vector3((-1 * DistanceToEdge) + i, 0, DistanceToEdge);
                Vector3 Corner3 = new Vector3(DistanceToEdge - i, 0, (-1 * DistanceToEdge));
                Vector3 Corner4 = new Vector3((-1 * DistanceToEdge), 0, (-1 * DistanceToEdge) + i);

                Matrix MC1 = Matrix.CreateTranslation(Corner1) * Matrix.Identity;
                MC1 *= Matrix.CreateScale(500);
                antitank.Draw(MC1, camera.View, camera.Projection);

                Matrix MC2 = Matrix.CreateTranslation(Corner2) * Matrix.Identity;
                MC2 *= Matrix.CreateScale(500);
                antitank.Draw(MC2, camera.View, camera.Projection);

                Matrix MC3 = Matrix.CreateTranslation(Corner3) * Matrix.Identity;
                MC3 *= Matrix.CreateScale(500);
                antitank.Draw(MC3, camera.View, camera.Projection);

                Matrix MC4 = Matrix.CreateTranslation(Corner4) * Matrix.Identity;
                MC4 *= Matrix.CreateScale(500);
                antitank.Draw(MC4, camera.View, camera.Projection);
            }
        }

        public void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }
        }

        public void DrawProjectiles(Model projectileModel, Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(projectileModel, view, projection);
            }
        }
    }
}
