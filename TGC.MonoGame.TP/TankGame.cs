using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using ThunderingTanks.Cameras;
using ThunderingTanks.Content.Models;
using ThunderingTanks.Objects;
using ThunderingTanks.Collisions;
using ThunderingTanks.Gizmos;
using ThunderingTanks.Gizmos.Geometries;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework.Content;
using System.Reflection.PortableExecutable;

namespace ThunderingTanks
{
    public class TankGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        public float screenHeight;
        public float screenWidth;

        Viewport viewport;

        private GraphicsDeviceManager Graphics { get; }
        public KeyboardState keyboardState;
        public MouseState mouseState;
        private BoundingBox[] Colliders { get; set; }
        private Gizmoss Gizmos { get; set; }

        private MapScene City { get; set; }
        private Tank Panzer { get; set; }
        private SkyBox SkyBox { get; set; }

        private readonly Vector3 _cameraInitialPosition = new(0f, 200f, 300f);
        private TargetCamera _targetCamera;
        private FreeCamera _freeCamera;

        private Roca roca;
        private AntiTanque antitanque;
        private Arbol arbol;
        private CasaAbandonada casa;
        private List<EnemyTank> EnemyTanks = new();
        private List<Projectile> Projectiles = new();

        private Vector3 lastPosition;

        public Texture2D CrossHairTexture { get; set; }
        private Vector2 CrossHairPosition { get; set; }

        private const float ConstConver = 100f;
        public SpriteBatch spriteBatch { get; set; }

        private Vector3 CrossHairAux;

        public TankGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Gizmos = new Gizmoss();

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

            viewport = GraphicsDevice.Viewport;

            keyboardState = new KeyboardState();
            mouseState = new MouseState();

            IsMouseVisible = false;

            _freeCamera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition); //creo que no se está usando

            Panzer = new Tank(GraphicsDevice)
            {
                TankVelocity = 3000f,
                TankRotation = 20f,
                FireRate = 1f
            };

            _targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, Panzer.PanzerMatrix.Forward);

            Panzer.PanzerCamera = _targetCamera;

            roca = new Roca();

            antitanque = new AntiTanque();

            arbol = new Arbol();

            casa = new CasaAbandonada();
            casa.Position = new Vector3(-3300f, 200f, 7000f);

            for (int i = 0; i < 3; i++)
            {
                EnemyTank enemyTank = new EnemyTank(GraphicsDevice)
                {
                    TankVelocity = 3000f,
                    TankRotation = 20f,
                    FireRate = 1f
                };
                enemyTank.LoadContent(Content);
                enemyTank.Position = new Vector3(3000 * i, 0, 9000);
                EnemyTanks.Add(enemyTank);
            }

            Colliders = new BoundingBox[40];

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

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

            //casa.AgregarCasa(new Vector3(-3300f, -690f, 7000f));

            //Panzer.TankBox = new BoundingCylinder(Panzer.Position, 10f, 20f);

            CrossHairTexture = Content.Load<Texture2D>(ContentFolderTextures + "/punto-de-mira");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");

            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 25000);

            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "content"));

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
                    Projectile projectile = Panzer.Shoot();

                    if (projectile != null)
                        Projectiles.Add(projectile);
                }
            }
            CrossHairAux = viewport.Project(new Vector3(Panzer.GunElevation, Panzer.GunRotationFinal, ConstConver), _targetCamera.Projection, _targetCamera.View, Matrix.Identity);

            CrossHairPosition = new Vector2(CrossHairAux.X, CrossHairAux.Y);

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;
            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);



            foreach (var enemyTank in EnemyTanks)
            {
                enemyTank.Update(gameTime, Panzer.Position);
            }

            UpdateProjectiles(gameTime);

            _freeCamera.Update(gameTime);
            _targetCamera.Update(Panzer.Position, Panzer.GunRotationFinal + MathHelper.ToRadians(180));

            Gizmos.UpdateViewProjection(_targetCamera.View, _targetCamera.Projection);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region Pass 1

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            Camera camara = _targetCamera;

            Panzer.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);

            foreach (var enemyTank in EnemyTanks)
            {
                enemyTank.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);
            }

            City.Draw(gameTime, camara.View, camara.Projection);

            DrawProjectiles(camara.View, camara.Projection);

            roca.Draw(gameTime, camara.View, camara.Projection);

            antitanque.Draw(gameTime, camara.View, camara.Projection);

            //arbol.Draw(gameTime, camara.View, camara.Projection);

            casa.Draw(gameTime, camara.View, camara.Projection);

            DrawSkyBox(camara.View, camara.Projection, camara.Position);

            Gizmos.DrawCylinder(Matrix.CreateScale(500f) * Panzer.TankBox.Transform, Color.Orange);

            #endregion

            #region Pass 2

            spriteBatch.Begin();

            spriteBatch.Draw(
                CrossHairTexture,
                CrossHairPosition,
                null, Color.Black, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0.8f
             );

            spriteBatch.End();

            #endregion

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

            SkyBox.Draw(view, projection, position);

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
                Colliders[i] = roca.RocaBox;
                Console.WriteLine(roca.RocaBox);
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
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Update(gameTime);
            }
        }
        public void DrawProjectiles(Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in Projectiles)
            {
                projectile.LoadContent(Content);
                projectile.Draw(view, projection);
            }
        }
        private void CheckCollisions(Vector3 previousPosition)
        {
            var tankBox = Panzer.TankBox;
            var isColliding = false;

            for (int i = 0; i < 40; i++)
            {
                Console.WriteLine(tankBox.Intersects(Colliders[i]));
                //Console.WriteLine(tankBox.Center);
                if (!tankBox.Intersects(Colliders[i]).Equals(BoxCylinderIntersection.None))
                {
                    // Colisión detectada, manejarlo aquí
                    Console.WriteLine($"Colisión detectada con roca en índice {i}");
                    //roca.BoundingBoxes.RemoveAt(i);
                    //roca.RocaWorlds = roca.RocaWorlds.Where((val, idx) => idx != i).ToArray();
                    //i--;
                    //Panzer.IsMoving = false;
                    //isColliding = true;
                }
            }

            if(!tankBox.Intersects(casa.CasaBox).Equals(BoxCylinderIntersection.None))
            {
                Console.WriteLine($"Colisión detectada con casa");
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
