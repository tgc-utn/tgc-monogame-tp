using System;
using System.Collections.Generic;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private MapScene City { get; set; }
        private Model Modelo { get; set; }
        private Model rock { get; set; }
        private Model antitank { get; set; }
        private List<int> NumerosX { get; set; }
        private List<int> NumerosZ { get; set; }

        private FreeCamera _freeCamera;

        private int N_Of_Rocks {  get; set; }
        private Matrix World { get; set; }

        private SkyBox _skyBox;
        private readonly Vector3 _cameraInitialPosition = new(0f, 200f, 300f);
        private readonly Vector3 _lightPosition = new(1000f, 500f, 300f);

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
            Graphics.ApplyChanges();
            _freeCamera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition);
            World = Matrix.Identity;

            NumerosX = new List<int>();
            NumerosZ = new List<int>();
            var random = new Random();

            N_Of_Rocks = 20;

            for (int i = 0; i < N_Of_Rocks; i++)
            {
                NumerosX.Add(random.Next(-10000, 10000)); // Rango más amplio
                NumerosZ.Add(random.Next(-10000, 10000)); // Rango más amplio
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            City = new MapScene(Content);
            Modelo = Content.Load<Model>("Models/Panzer/Panzer");
            rock = Content.Load<Model>("Models/nature/rock/Rock_1");
            antitank = Content.Load<Model>("Models/assets militares/rsg_military_antitank_hedgehog_01");

            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            _skyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 5000);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _freeCamera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            City.Draw(gameTime, _freeCamera.View, _freeCamera.Projection);
            Modelo.Draw(World, _freeCamera.View, _freeCamera.Projection);
            DrawSkyBox(_freeCamera.View, _freeCamera.Projection, _freeCamera.Position);

            List<Matrix> rockTransforms = new List<Matrix>(); // Crear la lista de transformaciones de las rocas

            for (int i = 0; i < N_Of_Rocks; i++)
            {
                Vector3 vector = new Vector3(NumerosX[i], 2, NumerosZ[i]);
                Matrix translateMatrixAntitanque = Matrix.CreateTranslation(vector);
                Matrix worldMatrixAntitanque = Matrix.CreateScale(5f) * translateMatrixAntitanque;
                rockTransforms.Add(worldMatrixAntitanque);
            }

            foreach (var transform in rockTransforms)
            {
                rock.Draw(transform, _freeCamera.View, _freeCamera.Projection);
            }

            float DistanceToEdge = 40f; //trate de usar este código -> MapScene.DistanceBetweenParcels * MapScene.NumInstances/4 <- para no hardcodear pero no funciona;
            

            
            for (float i = 0; i < 80f; i += 1f)
            {
                Vector3 Corner1 = new Vector3(DistanceToEdge, 0, DistanceToEdge - i);
                Vector3 Corner2 = new Vector3((-1*DistanceToEdge) + i, 0, DistanceToEdge);
                Vector3 Corner3 = new Vector3(DistanceToEdge - i, 0, (-1 * DistanceToEdge));
                Vector3 Corner4 = new Vector3((-1 * DistanceToEdge), 0, (-1 * DistanceToEdge) + i);
                Matrix MC1 = Matrix.CreateTranslation(Corner1) * Matrix.Identity;
                MC1 *= Matrix.CreateScale(500);
                antitank.Draw(MC1, _freeCamera.View, _freeCamera.Projection);
                Matrix MC2 = Matrix.CreateTranslation(Corner2) * Matrix.Identity;
                MC2 *= Matrix.CreateScale(500);
                antitank.Draw(MC2, _freeCamera.View, _freeCamera.Projection);
                Matrix MC3 = Matrix.CreateTranslation(Corner3) * Matrix.Identity;
                MC3 *= Matrix.CreateScale(500);
                antitank.Draw(MC3, _freeCamera.View, _freeCamera.Projection);
                Matrix MC4 = Matrix.CreateTranslation(Corner4) * Matrix.Identity;
                MC4 *= Matrix.CreateScale(500);
                antitank.Draw(MC4, _freeCamera.View, _freeCamera.Projection);
            }
            //funciona, pero mirá este choclo, horrible
            base.Draw(gameTime);
        }

        private void DrawSkyBox(Matrix view, Matrix projection, Vector3 position)
        {
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            _skyBox.Draw(view, projection, position);
            GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }
    }
}
