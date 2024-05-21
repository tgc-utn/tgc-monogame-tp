using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using ThunderingTanks.Cameras;
using ThunderingTanks.Content.Models;
using ThunderingTanks.Objects;

namespace ThunderingTanks
{
    public class TankGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        private GraphicsDeviceManager Graphics { get; }
        private GraphicsDevice _graphicsDevice;
        public KeyboardState keyboardState;

        private MapScene City { get; set; }

        private SkyBox _skyBox;
        private Tank Panzer { get; set; }
        private Effect Effect { get; set; }

        private readonly Vector3 _cameraInitialPosition = new(0f, 200f, 300f);

        private TargetCamera _targetCamera;

        private FreeCamera _freeCamera;

        private Roca roca;

        private AntiTanque antitanque;

        private Arbol arbol;

        private CasaAbandonada casa;

        private Projectile projectile;

        private Vector3 lastPosition;




        private Matrix World { get; set; }

        private List<Projectile> projectiles = new();


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


            _freeCamera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition); //creo que no se está usando

            Panzer = new Tank(GraphicsDevice)
            {
                TankVelocity = 300f,
                TankRotation = 20f
            };

            _targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, Panzer.PanzerMatrix.Forward);

            Panzer.PanzerCamera = _targetCamera;

            roca = new Roca();

            antitanque = new AntiTanque();

            arbol = new Arbol();

            casa = new CasaAbandonada();

            World = Matrix.Identity;
            /*NumerosX = new List<int>();
            NumerosZ = new List<int>();

            var random = new Random();

            N_Of_Rocks = 50;

            for (int i = 0; i < N_Of_Rocks; i++)
            {
                NumerosX.Add(random.Next(-20000, 20000)); // Rango más amplio
                NumerosZ.Add(random.Next(-20000, 20000)); // Rango más amplio
            }*/

            base.Initialize();
        }

        protected override void LoadContent()
        {

            City = new MapScene(Content);

            Panzer.LoadContent(Content);

            roca.LoadContent(Content);

            AgregarRocas(40);

            antitanque.LoadContent(Content);

            AgregarAntitanques();

            arbol.LoadContent(Content);

            AgregarArboles(15);

            casa.LoadContent(Content);

            casa.AgregarCasa(new Vector3(-3300f, -690f, 7000f));


            Panzer.TankBox = Collisions.CreateAABBFrom(Panzer.Tanque);

            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");


            _skyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 25000);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastPosition = Panzer.Position;

            CheckCollisions(lastPosition);

            if (Panzer.IsMoving)
            {
                Panzer.Update(gameTime, keyboardState);

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    Projectile projectile = Panzer.Shoot(Panzer.PanzerMatrix);

                    if (projectile != null)
                        projectiles.Add(projectile);
                }
            }

            UpdateProjectiles(gameTime);

            _freeCamera.Update(gameTime);
            _targetCamera.Update(Panzer.Position, Panzer.GunRotationFinal + MathHelper.ToRadians(180));

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            Camera camara = _targetCamera;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Panzer.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);

            City.Draw(gameTime, camara.View, camara.Projection);

            DrawProjectiles(camara.View, camara.Projection);

            roca.Draw(gameTime, camara.View, camara.Projection);

            antitanque.Draw(gameTime, camara.View, camara.Projection);

            arbol.Draw(gameTime, camara.View, camara.Projection);

            casa.Draw(gameTime, camara.View, camara.Projection);

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
        private void AgregarAntitanques()
        {
            float DistanceToEdge = 40f;

            for (float i = 0; i < 80f; i += 1f)
            {
                Vector3 Corner1 = new Vector3(DistanceToEdge, 0, DistanceToEdge - i);
                Vector3 Corner2 = new Vector3((-1 * DistanceToEdge) + i, 0, DistanceToEdge);
                Vector3 Corner3 = new Vector3(DistanceToEdge - i, 0, (-1 * DistanceToEdge));
                Vector3 Corner4 = new Vector3((-1 * DistanceToEdge), 0, (-1 * DistanceToEdge) + i);

                antitanque.AgregarAntitanque(Corner1);
                antitanque.AgregarAntitanque(Corner2);
                antitanque.AgregarAntitanque(Corner3);
                antitanque.AgregarAntitanque(Corner4);
            }
        }
        private void AgregarRocas(int cantidad)
        {
            Random random = new Random(42); // Aquí 42 es la semilla fija
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 randomPosition = new Vector3(
                    (float)(random.NextDouble() * 40000 - 20000), // X entre -100 y 100
                    0f,                                       // Y
                    (float)(random.NextDouble() * 40000 - 20000)  // Z entre -100 y 100
                );
                roca.AgregarRoca(randomPosition);
            }
        }
        private void AgregarArboles(int cantidad)
        {
            Random random = new Random(42); // Aquí 42 es la semilla fija
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 randomPosition = new Vector3(
                    (float)(random.NextDouble() * 40000 - 20000), // X entre -100 y 100
                    0f,                                       // Y
                    (float)(random.NextDouble() * 40000 - 20000)  // Z entre -100 y 100
                );
                arbol.AgregarArbol(randomPosition);
            }
        }
        public void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }
        }
        public void DrawProjectiles(Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.LoadContent(Content);
                projectile.Draw(view, projection);
            }
        }

        private void CheckCollisions(Vector3 previousPosition)
        {
            var tankBox = Panzer.TankBox;
            var isColliding = false;

            for (int i = 0; i < roca.BoundingBoxes.Count; i++)
            {
                if (tankBox.Intersects(roca.BoundingBoxes[i]))
                {
                    // Colisión detectada, manejarlo aquí
                    Console.WriteLine($"Colisión detectada con roca en índice {i}");
                    roca.BoundingBoxes.RemoveAt(i);
                    roca.RocaWorlds = roca.RocaWorlds.Where((val, idx) => idx != i).ToArray();
                    i--;
                    Panzer.IsMoving = false;
                    isColliding = true;
                }
            }

            if (isColliding)
            {
                Panzer.Position = previousPosition;
                Panzer.IsMoving = false;
                Console.WriteLine("Tanque detenido debido a colisión.");
            }

        }

    }
}
