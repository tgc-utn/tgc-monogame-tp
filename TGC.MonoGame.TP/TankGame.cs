﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Framework;
using ThunderingTanks.Cameras;
using ThunderingTanks.Collisions;
using ThunderingTanks.Geometries;
using ThunderingTanks.Gizmos;
using ThunderingTanks.Objects;
using ThunderingTanks.Objects.Props;
using ThunderingTanks.Objects.Tanks;
using System.DirectoryServices;

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
        private Point screenCenter;

        Viewport viewport;

        public Effect BasicShader { get; private set; }

        public Effect TextureMerge { get; private set; }

        public Effect Shadows { get; private set; }

        public FullScreenQuad FSQ { get; private set; }

        public RenderTarget2D SceneRenderTarget { get; private set; }

        public RenderTarget2D ShadowRenderTarget { get; private set; }

        private const int ShadowMapSize = 2048;

        private readonly float LightCameraFarPlaneDistance = 3000f;
        private readonly float TargetCameraFarPlaneDistance = 50000f;

        private readonly float LightCameraNearPlaneDistance = 5f;
        private readonly float TargetCameraClosePlaneDistance = 0.1f;

        public SpriteBatch spriteBatch { get; set; }

        ParticleSystem particleSystem;

        public FrameCounter FrameCounter { get; set; }
        #endregion

        #region State
        public KeyboardState keyboardState;
        public MouseState MouseState;
        public MouseState PreviousMouseState;
        #endregion

        #region Camera
        private readonly Vector3 _cameraInitialPosition = new(0, 6000, 0);

        private TargetCamera _targetCamera;
        private StaticCamera _lightCamera;
        private Vector3 lightPosition;
        private StaticCamera _staticCamera;

        private FreeCamera _freeCamera;

        private BoundingFrustum _cameraFrustum;

        public bool freeCameraIsActivated = false;
        private bool GTrigger = true;

        #endregion

        #region Objects

        private Roca roca;

        private AntiTanque antitanque;

        private Trees arbol;

        private DestroyedHouse casa;

        private Molino molino;

        private Grass Grass { get; set; }
        private List<Vector3> GrassPosition { get; set; }

        public int GrassCant = 300;


        private GermanSoldier GermanSoldier { get; set; }

        private Shack LittleShack { get; set; }


        public List<Projectile> Projectiles = new();

        private List<Roca> Rocas = new();
        private readonly int CantidadRocas = 40;

        private List<Trees> Arboles = new();
        private readonly int CantidadArboles = 80;
        private List<AntiTanque> AntiTanques = new List<AntiTanque>();
        private List<Object> gameObjects = new();

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
        private SoundEffectInstance _shootSoundPlayer { get; set; }
        #endregion

        #region Tanks
        private Tank Panzer { get; set; }

        private EnemyTank enemyTank;
        private List<EnemyTank> EnemyTanks = new();
        private List<EnemyTank> EliminatedEnemyTanks = new();

        public float TanksEliminados;
        public float Oleada;
        public float OleadaAnterior;
        public float Puntos;
        private float tiempoTranscurrido = 0f;
        private bool mostrandoMensaje = false;

        #endregion

        #region MapScene
        private MapScene Map { get; set; }
        public Vector2 MapLimit { get; set; }
        private SkyBox SkyBox { get; set; }
        private Matrix LightBoxWorld { get; set; } = Matrix.Identity;
        private CubePrimitive lightBox;

        public Random randomSeed = new Random(47);
        #endregion

        public bool StartGame { get; set; } = false;

        Vector3 ambientColorValue = new(0.7f, 0.7f, 0.7f);         // Color ambiental (generalmente menos afectado por la dirección de la luz)
        Vector3 diffuseColorValue = new(0.6f, 0.6f, 0.6f);         // Color difuso (más brillante en la dirección de la luz)
        Vector3 specularColorValue = new(0.5f, 0.5f, 0.5f);        // Color especular (más brillante en la dirección de la luz)
        readonly float KAmbientValue = 0.8f;                       // Factor de ambiental
        readonly float KDiffuseValue = 0.8f;                       // Factor difuso
        readonly float KSpecularValue = 0.5f;                      // Factor especular
        readonly float shininessValue = 20.0f;                     // Brillo especular (puede ajustarse según sea necesario)

        private readonly int CantidadTanquesEnemigos = 0;


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
            #region Graphics
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            rasterizerState.MultiSampleAntiAlias = true;

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            viewport = GraphicsDevice.Viewport;
            Graphics.IsFullScreen = true;
            IsMouseVisible = false;

            Graphics.ApplyChanges();
            #endregion

            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            screenCenter = new Point((int)Math.Round(screenWidth / 2.0), (int)Math.Round(screenHeight / 2));
            keyboardState = new KeyboardState();
            MouseState = new MouseState();
            PreviousMouseState = new MouseState();
            MapLimit = new Vector2(20000f, 20000f);
            FrameCounter = new FrameCounter();
            Rocas = new List<Roca>(CantidadRocas);
            Arboles = new List<Trees>(CantidadArboles);
            antitanque = new AntiTanque();
            molino = new Molino(Matrix.CreateTranslation(new(randomSeed.Next((int)-MapLimit.X, (int)MapLimit.X), 0, randomSeed.Next((int)-MapLimit.Y, (int)MapLimit.Y))));
            casa = new DestroyedHouse();
            LittleShack = new Shack();
            Grass = new Grass();
            GermanSoldier = new GermanSoldier();
            Panzer = new Tank()
            {
                TankVelocity = 1000f,
                TankRotation = 20f,
                FireRate = 5f,
                NumberOfProyectiles = 3
            };
            lightBox = new CubePrimitive(GraphicsDevice, 500, Color.Transparent);
            _menu = new Menu(Content);
            _hud = new HUD(screenWidth, screenHeight);
            SkyBox = new SkyBox(25000);
            Map = new MapScene(Content, GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, Panzer.PanzerMatrix.Forward, TargetCameraClosePlaneDistance, TargetCameraFarPlaneDistance);
            _staticCamera = new StaticCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(400, 200, 1300), Vector3.Forward, Vector3.Up);
            _freeCamera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, _cameraInitialPosition, screenCenter);
            _lightCamera = new StaticCamera(1f, lightPosition, Vector3.Normalize(Vector3.Zero - lightPosition), Vector3.Up);
            _cameraFrustum = new BoundingFrustum(_targetCamera.View * _targetCamera.Projection); 
            SceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
            ShadowRenderTarget = new RenderTarget2D(GraphicsDevice, ShadowMapSize, ShadowMapSize, false, SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            FSQ = new FullScreenQuad(GraphicsDevice);
            particleSystem = new ParticleSystem(GraphicsDevice);

            _lightCamera.BuildProjection(1f, LightCameraNearPlaneDistance, LightCameraFarPlaneDistance, MathHelper.PiOver2);

            AgregarRocas(CantidadRocas);
            AgregarArboles(CantidadArboles);
            AgregarTanquesEnemigos(CantidadTanquesEnemigos);
            AgregarAntitanques();

            GermanSoldier.Initialize();
            Panzer.SensitivityFactor = 0.45f;
            casa.Position = new Vector3(-3300f, -700f, 7000f);
            LittleShack.SpawnPosition(new Vector3(randomSeed.Next((int)-MapLimit.X, (int)MapLimit.X), 0f, randomSeed.Next((int)-MapLimit.Y, (int)MapLimit.Y)));

            TanksEliminados = 0;
            Oleada = 1;
            OleadaAnterior = 1;
            Puntos = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {

            BasicShader = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            BasicShader.CurrentTechnique = BasicShader.Techniques["Impact"];
            TextureMerge = Content.Load<Effect>(ContentFolderEffects + "TextureMerge");
            Shadows = Content.Load<Effect>(ContentFolderEffects + "Shadows");

            shootSoundEffect = Content.Load<SoundEffect>(ContentFolderMusic + "shootSound");
            movingTankSoundEffect = Content.Load<SoundEffect>(ContentFolderMusic + "movingTank");
            movingTankSound = movingTankSoundEffect.CreateInstance();
            _shootSound = shootSoundEffect.CreateInstance();
            _shootSoundPlayer = shootSoundEffect.CreateInstance();

            Panzer.LoadContent(Content, BasicShader);
            molino.LoadContent(Content, BasicShader);
            antitanque.LoadContent(Content, BasicShader);
            casa.LoadContent(Content, BasicShader);
            Grass.LoadContent(Content, BasicShader);
            GermanSoldier.LoadContent(Content, BasicShader);
            LittleShack.LoadContent(Content, BasicShader);
            _menu.LoadContent(Content);
            _hud.LoadContent(Content);
            SkyBox.LoadContent(Content);
            Gizmos.LoadContent(GraphicsDevice, Content);
            for (int i = 0; i < CantidadRocas; i++)
            {
                roca = Rocas[i];
                roca.LoadContent(Content, BasicShader, Map.terrain);
            }
            for (int i = 0; i < CantidadArboles; i++)
            {
                arbol = Arboles[i];
                arbol.LoadList(Content, BasicShader, Map.terrain);
            }
            for (int i = 0; i < CantidadTanquesEnemigos; i++)
            {
                enemyTank = EnemyTanks[i];
                enemyTank.LoadContent(Content, GraphicsDevice);
            }

            GrassPosition = LoadGrassPositions(GrassCant);
            GermanSoldier.SpawnPosition(new Vector3(0, 0, 300));

            Panzer.MovingTankSound = movingTankSound;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            var timeForParticles = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var elapsedTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);
            _hud.elapsedTime = elapsedTime;
            _hud.Oleada = Oleada;

            UpdateOleada(time);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            BasicShader.Parameters["diffuseColor"].SetValue(diffuseColorValue);
            BasicShader.Parameters["ambientColor"].SetValue(ambientColorValue);
            BasicShader.Parameters["specularColor"].SetValue(specularColorValue);
            BasicShader.Parameters["KAmbient"].SetValue(KAmbientValue);
            BasicShader.Parameters["KDiffuse"].SetValue(KDiffuseValue);
            BasicShader.Parameters["KSpecular"].SetValue(KSpecularValue);
            BasicShader.Parameters["shininess"].SetValue(shininessValue);
            BasicShader.Parameters["lightPosition"].SetValue(lightPosition);
            BasicShader.Parameters["eyePosition"].SetValue(_targetCamera.Position);

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
            movingTankSound.Volume = MasterSound;

            #endregion

            if (!_juegoIniciado || Panzer.isDestroyed)
            {
                lightPosition = new Vector3(150, 500, 150);
                LightBoxWorld = Matrix.CreateTranslation(lightPosition);

                _lightCamera.Position = lightPosition;
                _lightCamera.BuildView();

                Panzer.Position = new Vector3(10, 0, 0);
                Panzer.isDestroyed = false;

                Panzer.PanzerMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(-10));
                Panzer.TurretMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(6));
                Panzer.CannonMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(6));

                _menu.Update(ref _juegoIniciado, gameTime);

                keyboardState = Keyboard.GetState();
            }

            else
            {
                lightPosition = new Vector3(20000, 5000, 0);
                LightBoxWorld = Matrix.CreateTranslation(lightPosition);
                _lightCamera.Position = lightPosition;
                _lightCamera.BuildView();

                if (!StartGame)
                {
                    Panzer.Position = new Vector3(-5000, 0, -11000);
                    StartGame = true;
                }

                keyboardState = Keyboard.GetState();

                _hud.Update(Panzer, ref viewport, Puntos);

                #region GodMode&DrawGizmos

                if (keyboardState.IsKeyDown(Keys.G) && GTrigger)
                {
                    GTrigger = false;
                    freeCameraIsActivated = !freeCameraIsActivated;
                }

                if (keyboardState.IsKeyUp(Keys.G))
                    GTrigger = true;

                if (keyboardState.IsKeyDown(Keys.C) && CTrigger)
                {
                    CTrigger = false;
                    DrawGizmos = !DrawGizmos;
                }

                if (keyboardState.IsKeyUp(Keys.C))
                    CTrigger = true;

                #endregion

                var lastMatrix = Panzer.PanzerMatrix;
                var turretPosition = Panzer.TurretMatrix;
                var cannonPosition = Panzer.CannonMatrix;
                var position = Panzer.Position;
                var lastRotation = Panzer.Rotation;

                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    Projectile projectile = Panzer.Shoot();

                    if (projectile != null)
                    {
                        projectile.LoadContent(Content, BasicShader);
                        Projectiles.Add(projectile);
                        _shootSoundPlayer.Play();
                    }
                }

                _hud.TankPosition = Panzer.Position;

                Panzer.Update(gameTime, keyboardState, Map.terrain);

                Panzer.isColliding = false;
                _hud.TankIsColliding = false;

                if (CheckCollisions())
                {
                    _hud.TankIsColliding = true;
                    Panzer.isColliding = true;

                    Panzer.PanzerMatrix = lastMatrix;
                    Panzer.TurretMatrix = turretPosition;
                    Panzer.CannonMatrix = cannonPosition;
                    Panzer.Position = position;
                    Panzer.Rotation = lastRotation;
                    Panzer.Update(gameTime, keyboardState, Map.terrain);
                }

                screenHeight = GraphicsDevice.Viewport.Height;
                screenWidth = GraphicsDevice.Viewport.Width;
                Mouse.SetPosition((int)screenWidth / 2, (int)screenHeight / 2);

                foreach (var enemyTank in EnemyTanks)
                {
                    enemyTank.Update(gameTime, Panzer.Position, Map.terrain, GraphicsDevice, _targetCamera);
                    enemyTank.lifeSpan += time;

                    if (enemyTank.lifeSpan >= enemyTank.shootInterval)
                    {
                        Projectile projectile = enemyTank.Shoot();
                        if (projectile != null)
                        {
                            Projectiles.Add(projectile);
                            projectile.LoadContent(Content, BasicShader);
                            _shootSound.Play();
                            _shootSound.Volume = 1.0f;
                        }
                        enemyTank.lifeSpan = 0f;
                    }
                }

                foreach (var enemyTank in EliminatedEnemyTanks)
                {
                    enemyTank.Update(gameTime, Panzer.Position, Map.terrain, GraphicsDevice, _targetCamera);
                    enemyTank.StopEnemy();
                }

                UpdateProjectiles(gameTime);

                _hud.TimeSinceLastShot = Panzer.TimeSinceLastShot;

                _targetCamera.Update(Panzer.CameraPosition, Panzer.GunRotation + MathHelper.ToRadians(180));
                _freeCamera.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            float time = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            FrameCounter.Update(time);
            _hud.FPS = FrameCounter.AverageFramesPerSecond;

            if (!_juegoIniciado || Panzer.isDestroyed)
            {
                Panzer.isDestroyed = false;

                #region MENU

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;

                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                Camera camara = _staticCamera;
                Panzer.Draw(camara.View, camara.Projection, GraphicsDevice);


                spriteBatch.Begin();
                _menu.Draw(spriteBatch);
                spriteBatch.End();
                Panzer.CurrentLife = Panzer.MaxLife;
                #endregion

            }
            else
            {
                #region Pass 1

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                GraphicsDevice.SetRenderTarget(SceneRenderTarget);

                TextureMerge.CurrentTechnique = TextureMerge.Techniques["Merge"];

                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

                Camera camera;

                if (freeCameraIsActivated)
                    camera = _freeCamera;
                else
                    camera = _targetCamera;

                DrawProjectiles(camera.View, camera.Projection);
                DrawSkyBox(camera.View, camera.Projection, camera.Position);

                FrustumDraw(gameTime, camera);

                lightBox.Draw(LightBoxWorld, _targetCamera.View, _targetCamera.Projection);
                Map.Draw(gameTime, camera.View, camera.Projection, GraphicsDevice);
                //antitanque.Draw(gameTime, camera.View, camera.Projection, Map.terrain);
                Panzer.Draw(camera.View, camera.Projection, GraphicsDevice);
                Grass.Draw(GrassPosition, camera.View, camera.Projection, Map.terrain);

                Gizmos.DrawCube((casa.BoundingBox.Max + casa.BoundingBox.Min) / 2f, casa.BoundingBox.Max - casa.BoundingBox.Min, Color.Red);
                Gizmos.DrawOrientedCube(Panzer.TankBox.Center, Panzer.TankBox.Orientation, Panzer.TankBox.Extents * 2, Color.Green);
                Gizmos.DrawFrustum((_targetCamera.View * _targetCamera.Projection), Color.White);

                var RayOrigin = Panzer.Position + new Vector3(0f, 210f, 0f);
                Ray ray = new(RayOrigin, Panzer.CannonMatrix.Backward);
                Gizmos.DrawLine(RayOrigin, RayOrigin + Panzer.TurretMatrix.Backward * 10000f, Color.Blue);

                /*
                foreach(var enemyTank in EnemyTanks)
                {
                    if(ray.Intersects(enemyTank.TankBox) != null)
                        Gizmos.DrawLine(RayOrigin, RayOrigin + Panzer.CannonMatrix.Backward * 10000f, Color.Red);
                    else
                        Gizmos.DrawLine(RayOrigin, RayOrigin + Panzer.CannonMatrix.Backward * 10000f, Color.Blue);

                }   
                */ // Rayo de colision con los tanques enemigos

                _hud.RayDirection = RayOrigin + Panzer.CannonMatrix.Backward;
                _hud.RayPosition = RayOrigin;

                if (DrawGizmos)
                {
                    _hud.DrawDebug = true;
                    Gizmos.UpdateViewProjection(camera.View, camera.Projection);
                    Gizmos.Draw();
                }
                else
                {
                    _hud.DrawDebug = false;
                }

                #endregion

                #region Pass 2

                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                GraphicsDevice.SetRenderTarget(null);

                TextureMerge.Parameters["baseTexture"].SetValue(SceneRenderTarget);
                FSQ.Draw(TextureMerge);

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
        ///     Dibuja el SkyBox en el juego
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
        ///     Carga la posicion de los los objetos Antitanque en el borde del mapa
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
        ///     Carga la posicion de de los objetos Rocas en posiciones aleatorias pregenerdas
        /// </summary>
        /// <param name="cantidad">Cantidad de Rocas</param>
        private void AgregarRocas(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 randomPosition = new Vector3(
                    (float)(randomSeed.NextDouble() * 40000 - 20000), // X entre -100 y 100
                    150f,                                             // Y
                    (float)(randomSeed.NextDouble() * 40000 - 20000)  // Z entre -100 y 100
                );
                roca = new Roca();
                {
                    roca.Position = randomPosition;
                }


                Rocas.Add(roca);
            }
        }

        /// <summary>
        ///     Carga la posicion de de los objetos Arboles en posiciones aleatorias pregenerdas
        /// </summary>
        /// <param name="cantidad">Cantidad de Arboles</param>
        private void AgregarArboles(int cantidad)
        {

            for (int i = 0; i < cantidad; i++)
            {
                Vector3 randomPosition = new Vector3(
                    (float)(randomSeed.NextDouble() * 40000 - 20000), // X entre -100 y 100
                    0f,                                       // Y
                    (float)(randomSeed.NextDouble() * 40000 - 20000)  // Z entre -100 y 100
                );

                arbol = new Trees();
                {
                    arbol.SpawnPosition(randomPosition);
                }
                Arboles.Add(arbol);

            }
        }

        /// <summary>
        ///     Carga la posicion de de los objetos TanquesEnemigos en posiciones pregenerdas
        /// </summary>
        /// <param name="cantidad"></param>
        private void AgregarTanquesEnemigos(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                EnemyTank enemyTank = new EnemyTank(GraphicsDevice)
                {
                    TankVelocity = 180f,
                    TankRotation = 20f,
                    FireRate = 5f,
                    Position = new Vector3(3000 * i, 0, 9000),
                    shootInterval = 5f + ((float)Math.Pow(2, i)),
                    lifeSpan = 0
                };
                EnemyTanks.Add(enemyTank);
            }
        }

        /// <summary>
        ///     Actualiza la posicion y verifica la colision de los projectiles disparados
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateProjectiles(GameTime gameTime)
        {
            for (int j = 0; j < Projectiles.Count; ++j)
            {
                _hud.BulletCount = Projectiles.Count;

                if (OutOfMap(Projectiles[j].PositionVector) || HitFlor(Projectiles[j].PositionVector))
                {
                    Projectiles.Remove(Projectiles[j]);
                    break;
                }

                if (Panzer.TankBox.Intersects(Projectiles[j].ProjectileBox))
                {
                    Panzer.ReceiveDamage(ref _juegoIniciado);

                    Projectiles.Remove(Projectiles[j]);
                    break;
                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                for (int i = 0; i < Rocas.Count; ++i)
                {
                    if (Projectiles[j].ProjectileBox.Intersects(Rocas[i].RocaBox))
                    {
                        Projectiles.Remove(Projectiles[j]);
                        Rocas.Remove(Rocas[i]);
                        break;
                    }
                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                for (int i = EnemyTanks.Count - 1; i >= 0; i--)
                {
                    if (Projectiles[j].ProjectileBox.Intersects(EnemyTanks[i].TankBox))
                    {
                        Projectiles.Remove(Projectiles[j]);

                        EnemyTanks[i].life -= 5;

                        if (EnemyTanks[i].life <= 0)
                        {
                            EliminatedEnemyTanks.Add(EnemyTanks[i]);
                            TanksEliminados++;

                            EnemyTanks.RemoveAt(i);

                            Puntos += (i + 1) * Oleada;
                        }

                        if (TanksEliminados == CantidadTanquesEnemigos)
                        {
                            AgregarTanquesEnemigos(CantidadTanquesEnemigos);

                            for (int k = 0; k < CantidadTanquesEnemigos; k++)
                            {
                                enemyTank = EnemyTanks[k];
                                enemyTank.LoadContent(Content, GraphicsDevice);
                            }

                            TanksEliminados = 0;
                            Oleada++;

                            if (Oleada == 10)
                            {
                                Panzer.CurrentLife = Panzer.MaxLife;
                                Exit();
                            }

                        }
                        break;
                    }
                }

                if (Projectiles.Count <= j || Projectiles.Count == 0)
                    break;

                Projectiles[j].Update(gameTime);

            }
        }

        /// <summary>
        ///     Verifica si la posicion pasada por parametro se encuentra fuera del mapa
        /// </summary>
        /// <param name="position">Posicion del obejto a evaluar</param>
        /// <returns>True si esta fuera del mapa</returns>
        public bool OutOfMap(Vector3 position)
        {
            if (
                position.X > MapLimit.X ||
                position.X < -MapLimit.X ||

                position.Z > MapLimit.Y ||
                position.Z < -MapLimit.Y
               )
                return true;
            else
                return false;
        }

        /// <summary>
        ///     Verifica que el proyectil no colicione con el terreno
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool HitFlor(Vector3 position)
        {
            var Height = Map.terrain.Height(position.X, position.Z) - 300; // Se le resta 300 porque el terreno tiene un offset de 300

            if (position.Y < Height)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     Dibuja los proyectiles que fueron disparados y estan presentes en el juego
        /// </summary>
        /// <param name="view">Matriz de Vista</param>
        /// <param name="projection">Matriz de Proyeccion</param>
        public void DrawProjectiles(Matrix view, Matrix projection)
        {
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Draw(view, projection);
                Gizmos.DrawCube(CollisionsClass.GetCenter(projectile.ProjectileBox), CollisionsClass.GetExtents(projectile.ProjectileBox) * 2, Color.Red);
            }
        }

        /// <summary>
        ///     Chequea si hay alguna colision entre los tanques y entre un tanques y el entorno
        /// </summary>
        /// <returns>True si hay colision</returns>
        private bool CheckCollisions()
        {
            OrientedBoundingBox tankBox = Panzer.TankBox;

            Vector3 deltaY = new(2500, 4800, 200);

            foreach (var roca in Rocas)
            {
                if (tankBox.Intersects(roca.RocaBox))
                {
                    return true;
                }
            }

            foreach (var arbol in Arboles)
            {
                if (tankBox.Intersects(arbol.BoundingBox))
                {
                    return true;
                }
            }

            if (tankBox.Intersects(casa.BoundingBox))
            {
                return true;
            }

            foreach (var EnemyTank in EnemyTanks)
            {
                if (tankBox.Intersects(EnemyTank.TankBox))
                {
                    return true;
                }
            }

            foreach (var EnemyTank in EliminatedEnemyTanks)
            {
                if (tankBox.Intersects(EnemyTank.TankBox))
                {
                    return true;
                }
            }

            if (OutOfMap(Panzer.Position))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Crea posiciones aleatorias para la hierva
        /// </summary>
        /// <param name="Cantidad"> Cantidad de elementos que se crearan</param>
        /// <returns>Una lista de posiciones</returns>
        public List<Vector3> LoadGrassPositions(int Cantidad)
        {
            List<Vector3> grassPositions = new List<Vector3>();

            Random random = new Random();
            for (int i = 0; i < Cantidad; i++)
            {
                grassPositions.Add(new Vector3(random.Next(-(int)MapLimit.X, (int)MapLimit.X), 0, random.Next(-(int)MapLimit.Y, (int)MapLimit.Y)));
            }

            return grassPositions;
        }

        /// <summary>
        ///     Actualiza los elementos cuando se cambia de oleada
        /// </summary>
        /// <param name="time"></param>
        private void UpdateOleada(float time)
        {
            if (Oleada != OleadaAnterior)
            {
                mostrandoMensaje = true;
                tiempoTranscurrido = 0f;
                OleadaAnterior = Oleada;
            }

            if (mostrandoMensaje)
            {
                tiempoTranscurrido += time;
                if (tiempoTranscurrido >= 3f)
                {
                    _hud.siguienteOleada = false;
                    mostrandoMensaje = false;
                    tiempoTranscurrido = 0f;
                }
                else
                {
                    _hud.siguienteOleada = true;

                    for (int i = 0; i < EnemyTanks.Count; i++)
                    {
                        EnemyTanks[i].Position = new Vector3(3000 * i, 0, 9000);
                    }
                }
            }
            else
            {
                _hud.siguienteOleada = false;
            }
        }

        /// <summary>
        ///     Dibuja los objetos que se encuentran dentro del frustum de la camara
        /// </summary>
        /// <param name="gameTime"></param>
        public void FrustumDraw(GameTime gameTime, Camera camera)
        {
            Camera camara = camera;

            foreach (var roca in Rocas)
            {
                if (_cameraFrustum.Intersects(roca.RocaBox))
                {
                    roca.Draw(gameTime, camara.View, camara.Projection);
                    Gizmos.DrawCube((roca.RocaBox.Max + roca.RocaBox.Min) / 2f, roca.RocaBox.Max - roca.RocaBox.Min, Color.Blue);
                }
            }

            foreach (var arbol in Arboles)
            {
                if (_cameraFrustum.Intersects(arbol.BoundingBox))
                {
                    arbol.Draw(camara.View, camara.Projection, GraphicsDevice, Map.terrain);
                    Gizmos.DrawCube((arbol.MaxBox + arbol.MinBox) / 2f, arbol.MaxBox - arbol.MinBox, Color.Red);
                }
            }

            foreach (var enemyTank in EnemyTanks)
            {
                if (_cameraFrustum.Intersects(enemyTank.TankBox))
                {
                    enemyTank.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);
                    Gizmos.DrawCube(CollisionsClass.GetCenter(enemyTank.TankBox), CollisionsClass.GetExtents(enemyTank.TankBox) * 2f, Color.Red);
                }
            }

            foreach (var enemyTank in EliminatedEnemyTanks)
            {
                if (_cameraFrustum.Intersects(enemyTank.TankBox))
                {
                    enemyTank.Draw(Panzer.PanzerMatrix, camara.View, camara.Projection, GraphicsDevice);
                    Gizmos.DrawCube(CollisionsClass.GetCenter(enemyTank.TankBox), CollisionsClass.GetExtents(enemyTank.TankBox) * 2f, Color.Red);
                }
            }

            if (_cameraFrustum.Intersects(LittleShack.ShackBox))
            {
                LittleShack.Draw(camara.View, camara.Projection);
            }

            if (_cameraFrustum.Intersects(molino.MolinoBox))
            {
                molino.Draw(gameTime, camara.View, camara.Projection);
            }

            if (_cameraFrustum.Intersects(casa.BoundingBox))
            {
                casa.Draw(camara.View, camara.Projection);
                Gizmos.DrawCube((casa.BoundingBox.Max + casa.BoundingBox.Min) / 2f, casa.BoundingBox.Max - casa.BoundingBox.Min, Color.Red);
            }
        }
    }
}
