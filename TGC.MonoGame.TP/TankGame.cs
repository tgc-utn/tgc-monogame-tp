using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ThunderingTanks.Cameras;
using ThunderingTanks.Content.Models;
using ThunderingTanks.Gizmos;
using ThunderingTanks.Objects;

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
        private EnemyTank enemyTank;
        private List<EnemyTank> EnemyTanks = new();
        private List<Projectile> Projectiles = new();
        private List<Roca> Rocas = new();
        private int CantidadRocas = 40;
        private List<Arbol> Arboles = new();
        private int CantidadArboles = 15;
        private int CantidadTanquesEnemigos = 3;

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

            Graphics.IsFullScreen = true;

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

            Rocas = new List<Roca>(CantidadRocas);
            AgregarRocas(CantidadRocas);

            antitanque = new AntiTanque();

            Arboles = new List<Arbol>(CantidadArboles);
            AgregarArboles(CantidadArboles);

            casa = new CasaAbandonada();
            casa.Position = new Vector3(-3300f, 200f, 7000f);

            for (int i = 0; i < CantidadTanquesEnemigos; i++)
            {
                EnemyTank enemyTank = new EnemyTank(GraphicsDevice)
                {
                    TankVelocity = 3000f,
                    TankRotation = 20f,
                    Position = new Vector3(3000 * i, 0, 9000)
                };
                EnemyTanks.Add(enemyTank);
            }

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            base.Initialize();
        }

        protected override void LoadContent()
        {

            City = new MapScene(Content);

            Panzer.LoadContent(Content);

            for (int i = 0; i < CantidadRocas; i++)
            {
                roca = Rocas[i];
                roca.LoadContent(Content);
                //Colliders[i] = roca.RocaBox;
                Console.WriteLine($"Roca {i} creada: Min={roca.RocaBox.Min}, Max={roca.RocaBox.Max}");
            }
            antitanque.LoadContent(Content);

            AgregarAntitanques();

            for (int i = 0; i < CantidadArboles; i++)
            {
                arbol = Arboles[i];
                arbol.LoadContent(Content);
                //Colliders[i] = arbol.AntiTanqueBox;
                Console.WriteLine($"Arbol {i} creada: Min={arbol.ArbolBox.Min}, Max={arbol.ArbolBox.Max}");
            }

            casa.LoadContent(Content);
            Console.WriteLine($"Casa creada: Min={casa.CasaBox.Min}, Max={casa.CasaBox.Max}");

            for (int i = 0; i < CantidadTanquesEnemigos; i++)
            {
                enemyTank = EnemyTanks[i];
                enemyTank.LoadContent(Content);
                //Colliders[i] = arbol.AntiTanqueBox;
                Console.WriteLine($"Tanque enemigo {i} creado: Min={enemyTank.TankBox.Min}, Max={enemyTank.TankBox.Max}");
            }

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

            var lastMatrix = Panzer.PanzerMatrix;
            var turretPosition = Panzer.TurretMatrix;
            var cannonPosition = Panzer.CannonMatrix;
            var direction = Panzer.Direction;

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Projectile projectile = Panzer.Shoot();

                if (projectile != null)
                    Projectiles.Add(projectile);
            }

            Panzer.Update(gameTime, keyboardState);

            Panzer.isColliding = false;

            if (CheckCollisions())
            {
                Panzer.isColliding = true;

                Panzer.PanzerMatrix = lastMatrix;
                Panzer.TurretMatrix = turretPosition;
                Panzer.CannonMatrix = cannonPosition;
                Panzer.Direction = direction;

                Panzer.Update(gameTime, keyboardState);

            }

            CrossHairAux = viewport.Project(
                Panzer.Direction, 
                _targetCamera.Projection, 
                _targetCamera.View, 
                Matrix.CreateRotationX(Panzer.GunElevation) * Panzer.TurretMatrix
                );

            CrossHairPosition = new Vector2(screenWidth / 2 - 50 , CrossHairAux.Y);

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;
            Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

            foreach (var enemyTank in EnemyTanks)
            {
                enemyTank.Update(gameTime, Panzer.Direction);
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
            //roca.Draw(gameTime, camara.View, camara.Projection);

            foreach (var roca in Rocas)
            {
                roca.Draw(gameTime, camara.View, camara.Projection);
            }

            // foreach (var antitanque in Antitanques)
            // {
            //     antitanque.Draw(gameTime, camara.View, camara.Projection);
            // }

            antitanque.Draw(gameTime, camara.View, camara.Projection);

            foreach (var arbol in Arboles)
            {
                //arbol.Draw(gameTime, camara.View, camara.Projection);
            }

            casa.Draw(gameTime, camara.View, camara.Projection);

            DrawSkyBox(camara.View, camara.Projection, camara.Position);

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
            Random random = new Random(42);
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 randomPosition = new Vector3(
                    (float)(random.NextDouble() * 40000 - 20000), // X entre -100 y 100
                    200f,                                       // Y
                    (float)(random.NextDouble() * 40000 - 20000)  // Z entre -100 y 100
                );
                roca = new Roca();
                {
                    roca.Position = randomPosition;
                }
                Rocas.Add(roca);
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
                arbol = new Arbol();
                {
                    arbol.Position = randomPosition;
                }
                Arboles.Add(arbol);
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
        private bool CheckCollisions()
        {
            var tankBox = Panzer.TankBox;

            foreach (var roca in Rocas)
            {
                if (tankBox.Intersects(roca.RocaBox))
                {
                    Console.WriteLine("Colisión detectada con una roca.");
                    return true;
                }
            }

            foreach (var arbol in Arboles)
            {
                if (tankBox.Intersects(arbol.ArbolBox))
                {
                    Console.WriteLine("Colisión detectada con un árbol.");
                    return true;
                }
            }

            if (tankBox.Intersects(casa.CasaBox))
            {
                Console.WriteLine("Colisión detectada con la casa.");
                return true;
            }
            foreach (var EnemyTank in EnemyTanks)
            {
                if (tankBox.Intersects(EnemyTank.TankBox))
                {
                    Console.WriteLine("Colisión detectada con un tanque enemigo.");
                    return true;
                }
            }

            return false;
        }
    }
}
