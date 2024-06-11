﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;
using ThunderingTanks.Gizmos;
using ThunderingTanks.Objects;
using ThunderingTanks.Objects.Props;
using ThunderingTanks.Objects.Tanks;

namespace ThunderingTanks
{
    public class TankGame : Game
    {
        #region Content
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderFonts = "Fonts/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderAudio = "Audio/";
        #endregion

        #region Graphics
        private GraphicsDeviceManager Graphics { get; }

        public float screenHeight;
        public float screenWidth;

        Viewport viewport;
        #endregion

        #region State
        public KeyboardState keyboardState;
        public MouseState MouseState;
        public MouseState PreviousMouseState;
        #endregion

        #region Camera
        private readonly Vector3 _cameraInitialPosition = new(0, 0, 0);

        private TargetCamera _targetCamera;
        private StaticCamera _staticCamera;
        #endregion

        #region Objects

        private Roca roca;

        private AntiTanque antitanque;

        private Arbol arbol;

        private CasaAbandonada casa;

        private Molino molino;

        private readonly int CantidadTanquesEnemigos = 3;
        private EnemyTank enemyTank;
        private List<EnemyTank> EnemyTanks = new();

        public List<Projectile> Projectiles = new();

        private List<Roca> Rocas = new();
        private readonly int CantidadRocas = 40;

        private List<Arbol> Arboles = new();
        private readonly int CantidadArboles = 15;

        #endregion

        #region Menu
        private Menu _menu;
        private HUD _hud;
        private bool _juegoIniciado = false;
        #endregion

        #region Gizmos

        private Gizmoss Gizmos { get; set; }

        private bool DrawGizmos = false;
        private bool CTrigger = true;

        #endregion

        #region Sounds
        public float MasterSound { get; set; } = 1f;
        public AudioEngine AudioEngine { get; set; }
        public AudioEmitter AudioEmitter { get; set; }
        private SoundEffect movingTankSoundEffect { get; set; }
        private SoundEffect shootSoundEffect { get; set; }
        private SoundEffectInstance movingTankSound { get; set; }
        private SoundEffectInstance _shootSound { get; set; }
        #endregion

        #region Tanks
        private Tank Panzer { get; set; }
        private float elapsedTime = 0f;
        private const float shootInterval = 5f;
        #endregion

        #region MapScene
        private MapScene City { get; set; }
        public Vector2 MapLimit { get; set; }
        #endregion

        private SkyBox SkyBox { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        public bool StartGame { get; set; } = false;

        // ------------ GAME ------------ //

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
            MouseState = new MouseState();
            PreviousMouseState = new MouseState();

            IsMouseVisible = false;

            MapLimit = new Vector2(20000f, 20000f);

            Panzer = new Tank(GraphicsDevice)
            {
                TankVelocity = 200f,
                TankRotation = 20f,
                FireRate = 8f
            };

            _targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, Panzer.PanzerMatrix.Forward);
            _staticCamera = new StaticCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(400, 200, 1300), Vector3.Forward, Vector3.Up);

            Panzer.PanzerCamera = _targetCamera;

            Rocas = new List<Roca>(CantidadRocas);
            AgregarRocas(CantidadRocas);

            antitanque = new AntiTanque();

            molino = new Molino(Matrix.CreateTranslation(new(0, 0, 0)));

            Arboles = new List<Arbol>(CantidadArboles);
            AgregarArboles(CantidadArboles);

            casa = new CasaAbandonada();
            casa.Position = new Vector3(-3300f, -700f, 7000f);

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

            _menu = new Menu(Content);
            _hud = new HUD(screenWidth, screenHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            City = new MapScene(Content);

            Panzer.LoadContent(Content);
            molino.LoadContent(Content);
            roca.LoadContent(Content);
            antitanque.LoadContent(Content);
            casa.LoadContent(Content);

            shootSoundEffect = Content.Load<SoundEffect>(ContentFolderMusic + "shootSound");
            movingTankSoundEffect = Content.Load<SoundEffect>(ContentFolderMusic + "movingTank");

            movingTankSound = movingTankSoundEffect.CreateInstance();
            _shootSound = shootSoundEffect.CreateInstance();

            movingTankSound.Volume = 0.5f;

            Panzer.MovingTankSound = movingTankSound;

            AgregarAntitanques();

            for (int i = 0; i < CantidadRocas; i++)
            {
                roca = Rocas[i];
                roca.LoadContent(Content);
            }

            for (int i = 0; i < CantidadArboles; i++)
            {
                arbol = Arboles[i];
                arbol.LoadContent(Content);
            }

            for (int i = 0; i < CantidadTanquesEnemigos; i++)
            {
                enemyTank = EnemyTanks[i];
                enemyTank.LoadContent(Content);
                Console.WriteLine($"Tanque enemigo {i} creado: Min={enemyTank.TankBox.Min}, Max={enemyTank.TankBox.Max}");
            }

            spriteBatch = new SpriteBatch(GraphicsDevice);

            _menu.LoadContent(Content);
            _hud.LoadContent(Content);

            var skyBox = Content.Load<Model>(ContentFolder3D + "cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/mountain_skybox_hd");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");

            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 25000);

            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "content"));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            _hud.FPS = (1 / time);

            elapsedTime += time;

            #region Volume

            PreviousMouseState = MouseState;

            MouseState = Mouse.GetState();

            int currentScrollValue = MouseState.ScrollWheelValue;
            int previousScrollValue = PreviousMouseState.ScrollWheelValue;

            int scrollDifference = currentScrollValue - previousScrollValue;

            if (scrollDifference > 0)
            {
                MasterSound = Math.Min(1, MasterSound + 0.05f);
            }
            else if (scrollDifference < 0)
            {
                MasterSound = Math.Max(0, MasterSound - 0.05f);
            }

            _menu.MasterSound = MasterSound;
            _shootSound.Volume = MasterSound;
            movingTankSound.Volume = Math.Max(0, MasterSound - 0.3f);

            #endregion

            if (!_juegoIniciado || Panzer.isDestroyed)
            {

                Panzer.isDestroyed = false;

                Panzer.PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(-10));
                Panzer.TurretMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(6));
                Panzer.CannonMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(6));

                _menu.Update(ref _juegoIniciado, gameTime);

            }
            else
            {

                if (!StartGame)
                {
                    Panzer.Direction = new Vector3(-5000, 0, -11000);
                    StartGame = true;
                }

                keyboardState = Keyboard.GetState();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                _hud.Update(Panzer, ref viewport);

                if (keyboardState.IsKeyDown(Keys.C) && CTrigger)
                {

                    CTrigger = false;

                    if (DrawGizmos)
                        DrawGizmos = false;
                    else
                        DrawGizmos = true;
                }

                if (keyboardState.IsKeyUp(Keys.C))
                    CTrigger = true;

                var lastMatrix = Panzer.PanzerMatrix;
                var turretPosition = Panzer.TurretMatrix;
                var cannonPosition = Panzer.CannonMatrix;
                var direction = Panzer.Direction;

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    Projectile projectile = Panzer.Shoot();

                    if (projectile != null)
                    {
                        projectile.LoadContent(Content);
                        Projectiles.Add(projectile);

                        _shootSound.Play();
                    }

                }

                _hud.TankPosition = Panzer.Position;

                Panzer.Update(gameTime, keyboardState);

                Panzer.isColliding = false;
                _hud.TankIsColliding = false;

                if (CheckCollisions())
                {
                    _hud.TankIsColliding = true;

                    Panzer.isColliding = true;

                    Panzer.PanzerMatrix = lastMatrix;
                    Panzer.TurretMatrix = turretPosition;
                    Panzer.CannonMatrix = cannonPosition;
                    Panzer.Direction = direction;

                    Panzer.Update(gameTime, keyboardState);
                }

                screenHeight = GraphicsDevice.Viewport.Height;
                screenWidth = GraphicsDevice.Viewport.Width;
                Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

                foreach (var enemyTank in EnemyTanks)
                {
                    enemyTank.Update(gameTime, Panzer.Direction);

                    if (elapsedTime >= shootInterval)
                    {
                        Projectile projectile = enemyTank.Shoot();

                        Console.WriteLine("Tanque enemigo quiere disparar");

                        if (projectile != null)
                        {
                            Projectiles.Add(projectile);
                            projectile.LoadContent(Content);

                            _shootSound.Play();
                            _shootSound.Volume = 1.0f;
                            Console.WriteLine("Disparo Tanque enemigo");
                        }
                        elapsedTime = 0f;
                    }
                }

                UpdateProjectiles(gameTime);

                _targetCamera.Update(Panzer.Position, Panzer.GunRotationFinal + MathHelper.ToRadians(180));

                _hud.TimeSinceLastShot = Panzer.TimeSinceLastShot;

            }

            Gizmos.UpdateViewProjection(_targetCamera.View, _targetCamera.Projection);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!_juegoIniciado || Panzer.isDestroyed)
            {
                Panzer.isDestroyed = false;

                #region MENU

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;

                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                Camera camara = _staticCamera;
                Panzer.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);

                spriteBatch.Begin();
                _menu.Draw(spriteBatch);
                spriteBatch.End();

                #endregion

            }
            else
            {

                #region Pass 1

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;

                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

                Camera camara = _targetCamera;

                Panzer.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);

                Gizmos.DrawCube(Panzer.Center, Panzer.Extents * 2f, Color.Green);

                molino.Draw(gameTime, camara.View, camara.Projection);

                foreach (var enemyTank in EnemyTanks)
                {
                    enemyTank.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);
                    Gizmos.DrawCube(CollisionsClass.GetCenter(enemyTank.TankBox), CollisionsClass.GetExtents(enemyTank.TankBox) * 2f, Color.Red);
                }

                City.Draw(gameTime, camara.View, camara.Projection);

                DrawProjectiles(camara.View, camara.Projection);

                foreach (var roca in Rocas)
                {
                    roca.Draw(gameTime, camara.View, camara.Projection);
                    Gizmos.DrawCube((roca.RocaBox.Max + roca.RocaBox.Min) / 2f, roca.RocaBox.Max - roca.RocaBox.Min, Color.Blue);
                }

                antitanque.Draw(gameTime, camara.View, camara.Projection);

                foreach (var arbol in Arboles)
                {
                    arbol.Draw(gameTime, camara.View, camara.Projection);
                    Gizmos.DrawCube((arbol.ArbolBox.Max + arbol.ArbolBox.Min) / 2f, arbol.ArbolBox.Max - arbol.ArbolBox.Min, Color.Red);
                }

                casa.Draw(gameTime, camara.View, camara.Projection);
                Gizmos.DrawCube((casa.CasaBox.Max + casa.CasaBox.Min) / 2f, casa.CasaBox.Max - casa.CasaBox.Min, Color.Red);

                DrawSkyBox(camara.View, camara.Projection, camara.Position);

                if (DrawGizmos)
                    Gizmos.Draw();

                #endregion

                #region Pass 2

                spriteBatch.Begin();

                _hud.Draw(spriteBatch);

                spriteBatch.End();

                #endregion

            }
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        // ------------ FUNCTIONS ------------ //

        /// <summary>
        /// Dibuja el SkyBox en el juego
        /// </summary>
        /// <param name="view">Matriz de Vista</param>
        /// <param name="projection">Matriz de Proyeccion</param>
        /// <param name="position">Posicion del Jugador</param>
        private void DrawSkyBox(Matrix view, Matrix projection, Vector3 position)
        {
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();

            rasterizerState.CullMode = CullMode.None;

            GraphicsDevice.RasterizerState = rasterizerState;

            SkyBox.Draw(view, projection, position);

            GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        /// <summary>
        /// Carga la posicion de los los objetos Antitanque en el borde del mapa
        /// </summary>
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

        /// <summary>
        /// Carga la posicion de de los objetos Rocas en posiciones aleatorias pregenerdas
        /// </summary>
        /// <param name="cantidad">Cantidad de Rocas</param>
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

        /// <summary>
        /// Carga la posicion de de los objetos Arboles en posiciones aleatorias pregenerdas
        /// </summary>
        /// <param name="cantidad">Cantidad de Arboles</param>
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

        /// <summary>
        /// Actualiza la posicion y verifica la colision de los projectiles disparados
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateProjectiles(GameTime gameTime)
        {
            for (int j = 0; j < Projectiles.Count; ++j)
            {

                _hud.BulletCount = Projectiles.Count;

                if (OutOfMap(Projectiles[j].PositionVector))
                {
                    Projectiles.Remove(Projectiles[j]);
                    break;
                }

                if (Panzer.TankBox.Intersects(Projectiles[j].ProjectileBox))
                {
                    Panzer.ReceiveDamage(ref _juegoIniciado);
                    Projectiles.Remove(Projectiles[j]);
                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                for (int i = 0; i < Rocas.Count; ++i)
                {
                    if (Projectiles[j].ProjectileBox.Intersects(Rocas[i].RocaBox))
                    {
                        Console.WriteLine("Colisión detectada de proyectil con una roca.");

                        Projectiles.Remove(Projectiles[j]);
                        Rocas.Remove(Rocas[i]);
                        break;

                    }

                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                for (int i = 0; i < EnemyTanks.Count; ++i)
                {
                    if (Projectiles[j].ProjectileBox.Intersects(EnemyTanks[i].TankBox))
                    {
                        Console.WriteLine("Colisión detectada de proyectil con un tanque enemigo.");

                        Projectiles.Remove(Projectiles[j]);
                        EnemyTanks.Remove(EnemyTanks[i]);
                        break;

                    }

                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                Projectiles[j].Update(gameTime);

            }
        }

        /// <summary>
        /// Verifica si la posicion pasada por parametro se encuentra fuera del mapa
        /// </summary>
        /// <param name="position">Posicion del obejto a evaluar</param>
        /// <returns>True si esta fuera del mapa</returns>
        public bool OutOfMap(Vector3 position)
        {
            if (
                position.X > MapLimit.X ||
                position.X < -MapLimit.X ||

                position.Y > MapLimit.Y ||
                position.Y < -MapLimit.Y
               )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Dibuja los proyectiles que fueron disparados y estan presentes en el juego
        /// </summary>
        /// <param name="view">Matriz de Vista</param>
        /// <param name="projection">Matriz de Proyeccion</param>
        public void DrawProjectiles(Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Draw(view, projection);
                Gizmos.DrawCube(CollisionsClass.GetCenter(projectile.BoundingBox), CollisionsClass.GetExtents(projectile.BoundingBox), Color.Red);
            }
        }

        /// <summary>
        /// Chequea si hay alguna colision entre los tanques y entre un tanques y el entorno
        /// </summary>
        /// <returns>True si hay colision</returns>
        private bool CheckCollisions()
        {
            BoundingBox tankBox = Panzer.TankBox;

            foreach (var roca in Rocas)
            {
                if (tankBox.Intersects(roca.RocaBox))
                {
                    Panzer.ReceiveDamage(ref _juegoIniciado);
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
                    Panzer.ReceiveDamage(ref _juegoIniciado);
                    Console.WriteLine("Colisión detectada con un tanque enemigo.");
                    return true;
                }
            }
            /*for (int j = 0; j < Projectiles.Count; ++j)
            {
                if (Panzer.TankBox.Intersects(Projectiles[j].ProjectileBox))
                    Panzer.ReceiveDamage(ref _juegoIniciado);
            }*/
            return false;
        }
    }
}
