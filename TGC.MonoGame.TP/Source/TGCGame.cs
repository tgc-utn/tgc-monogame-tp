using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Timers;
using System.Threading;
using System.Diagnostics;


namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        

        public static Mutex MutexDeltas = new Mutex();

        public Gizmos Gizmos;
        public bool ShowGizmos = false;
        public Xwing Xwing;
        
        
        
        public float Trench2Scale = 0.07f;

        
        public static TGCGame Instance;
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            Instance = this;
            Xwing = new Xwing();
            Gizmos = new Gizmos();
        }
        public GmState GameState { get; set; }
        public GraphicsDeviceManager Graphics { get; }

        public float PausedCameraRotation;
        public Trench[,] Map { get; set; }
        public const int MapSize = 21; //21x21
        public float MapLimit;
        

        public int FPS;
        
        public MyCamera Camera { get; set; }
        public MyCamera LookBack { get; set; }
        public MyCamera SelectedCamera { get; set; }
        public LightCamera LightCamera { get; set; }

        public Input Input;
        public HUD HUD;

       
        public Drawer Drawer;

        BackgroundCombat BackgroundCombat;
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            //var rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;
            Input = new Input(this);
            HUD = new HUD(this);
            Drawer = new Drawer();
            BackgroundCombat = new BackgroundCombat();
            GameState = GmState.StartScreen;
            
            Xwing.World = Matrix.Identity;

            // Hace que el mouse sea visible.
            IsMouseVisible = true;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60); //60);

            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            // Creo una camara libre con parametros de pitch, yaw que se puede mover con WASD, y rotar con mouse o flechas
            Camera = new MyCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.Zero, size);

            LookBack = new MyCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.Zero, size);

            SelectedCamera = Camera;

            //int mapSize = 9; //9x9
            //Algoritmo de generacion de mapa recursivo (ver debug output)
            Map = Trench.GenerateMap(MapSize);
            System.Diagnostics.Debug.WriteLine(Trench.ShowMapInConsole(Map, MapSize));

           
            
            base.Initialize();
        }

       
        

        public float kd = 0.8f;
        public float ks = 0.4f;

        protected override void LoadContent()
        {
            Drawer.Init();
            
            HUD.LoadContent();
            
            SoundManager.LoadContent();
            
            Gizmos.LoadContent(GraphicsDevice);

            
            Trench.UpdateTrenches();

            var blockSize = MapLimit / MapSize;
            Camera.MapLimit = MapLimit;
            Camera.MapSize = MapSize;
            Camera.BlockSize = blockSize;
            Camera.Position = new Vector3(MapLimit/2 - blockSize/2, 0, blockSize /2);
            Xwing.MapLimit = MapLimit;
            Xwing.MapSize = MapSize;
            Xwing.Update(0f, Camera);

            LightCamera = new LightCamera(Camera.AspectRatio, Xwing.Position - Vector3.Backward * 30 + Vector3.Up * 30);
            //Debug.WriteLine(LightCamera.Projection.ToString());

            LightCamera.BuildProjection(LightCamera.AspectRatio, 5f, 3000f, LightCamera.DefaultFieldOfViewDegrees);
            //LightCamera.BuildProjection(1f, 5f, 3000f, LightCamera.DefaultFieldOfViewDegrees);

            //Debug.WriteLine(LightCamera.Projection.ToString());

            Laser.MapLimit = MapLimit;
            Laser.MapSize = MapSize;
            Laser.BlockSize = blockSize;

           
            base.LoadContent();
        }

        public BoundingFrustum BoundingFrustum = new BoundingFrustum(Matrix.Identity);

        public int elementsDrawn, totalElements;
        
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Input.ProcessInput();

            BoundingFrustum.Matrix = SelectedCamera.View * SelectedCamera.Projection;
            Vector4 zone = Vector4.One;

            LightCamera.Update(gameTime);
            switch (GameState)
            {
                case GmState.StartScreen:
                    #region startscreen
                    Camera.Yaw += 10f * elapsedTime;
                    Camera.Yaw %= 360;
                    Camera.Pitch += 10f * elapsedTime;
                    Camera.Yaw %= 90;
                    Camera.UpdateVectorView();
                    #endregion
                    break;
                case GmState.Running:
                    #region running
                    Drawer.trenchesToDraw.Clear();
                    
                    Drawer.tiesToDraw.Clear();
                    Drawer.showXwing = true;
                    Drawer.lasersToDraw.Clear();
                    Drawer.shipsToDraw.Clear();

                    elementsDrawn = 0;
                    totalElements = 0;

                    //Update camara
                    Camera.Update(gameTime);
                    //Generacion de enemigos, de ser necesario
                    TieFighter.GenerateEnemies(Xwing);
                    TieFighter.AddAllRequiredToDraw(ref Drawer.tiesToDraw, BoundingFrustum);
                    elementsDrawn += Drawer.tiesToDraw.Count;
                    totalElements += TieFighter.Enemies.Count;
                    //Update Xwing
                    Xwing.Update(elapsedTime, Camera);
                    elementsDrawn += 1;
                    totalElements += 1;
                    
                    BackgroundCombat.UpdateAll(elapsedTime);
                    BackgroundCombat.AddAllRequiredToDraw(ref Drawer.shipsToDraw);


                    Trench.UpdateCurrent();
                    zone = Xwing.GetZone();

                    //enemyLasers.Clear();

                    for (int x = (int)zone.X; x < zone.Y; x++)
                        for (int z = (int)zone.Z; z < zone.W; z++)
                        {
                            var block = Map[x, z];
                            block.Turrets.ForEach(turret => turret.Update(Xwing, elapsedTime));
                            block.Turrets.RemoveAll(turret => turret.needsRemoval);

                            if(BoundingFrustum.Intersects(block.BB))
                                Drawer.trenchesToDraw.Add(Map[x, z]);

                            totalElements++;
                        }
                    elementsDrawn += Drawer.trenchesToDraw.Count;

                    Laser.UpdateAll(elapsedTime, Xwing);
                    totalElements += Laser.AlliedLasers.Count;
                    totalElements += Laser.EnemyLasers.Count;

                    Laser.AddAllRequiredtoDraw(ref Drawer.lasersToDraw, ref BoundingFrustum);
                    elementsDrawn += Drawer.lasersToDraw.Count;
                    
                    //Colisiones
                    Xwing.VerifyCollisions(Laser.EnemyLasers, Map);
                    //Xwing.fired.RemoveAll(laser => laser.Age >= laser.MaxAge);

                    
                    TieFighter.UpdateEnemies(elapsedTime, Xwing);
                    
                    Drawer.EffectLight.Parameters["lightPosition"].SetValue(LightCamera.Position);
                    Drawer.EffectLight.Parameters["eyePosition"].SetValue(SelectedCamera.Position);

                    SoundManager.UpdateRandomDistantSounds(elapsedTime);
                    #endregion
                    break;
                case GmState.Paused:
                    #region paused


                    Camera.PausedUpdate(elapsedTime, Xwing);

                    Drawer.trenchesToDraw.Clear();
                    
                    Vector4 xzone = Xwing.GetZone();
                    for (int x = (int)xzone.X; x < xzone.Y; x++)
                        for (int z = (int)xzone.Z; z < xzone.W; z++)
                        {
                            var block = Map[x, z];
                           
                            if (BoundingFrustum.Intersects(block.BB))
                                Drawer.trenchesToDraw.Add(Map[x, z]);
                        }
                    #endregion
                    break;
                case GmState.Victory:
                    #region victory
                    Camera.PausedUpdate(elapsedTime, Xwing);
                    #endregion
                    break;
                case GmState.Defeat:
                    #region defeat
                    Camera.PausedUpdate(elapsedTime, Xwing);
                    #endregion
                    break;
            }

            Gizmos.UpdateViewProjection(SelectedCamera.View, SelectedCamera.Projection);
            
            base.Update(gameTime);
        }

        public bool ApplyBloom = false;
        public bool ApplyShadowMap = false;
        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            FPS = (int)Math.Round(1 / deltaTime);
            
            if (!ApplyBloom && !ApplyShadowMap)
                Drawer.DrawRegular();
            

            if (ApplyBloom)
                Drawer.DrawBloom();


            #region ShadowMap
            if (ApplyShadowMap)
            {
                Drawer.DrawShadowed();
            }
            #endregion
            if (ShowGizmos)
                Gizmos.Draw();
            HUD.Draw();

        }
        
        



        public static Model GetModelFromType(TrenchType type)
        {
            switch(type)
            {
                case TrenchType.Platform:        return Drawer.TrenchPlatform;
                case TrenchType.Straight:        return Drawer.TrenchStraight;
                case TrenchType.T:               return Drawer.TrenchT;
                case TrenchType.Intersection:    return Drawer.TrenchIntersection;
                case TrenchType.Elbow:           return Drawer.TrenchElbow;
                default:                         return Drawer.TrenchPlatform;
            }
        }
        public enum GmState
        {
            StartScreen,
            Options,
            Running,
            Paused,
            Victory,
            Defeat
        }

        public void ChangeGameStateTo(GmState newState)
        {
            if (newState.Equals(GmState.StartScreen))
            {
                Xwing.Energy = 10;
                Xwing.HP = 100;
                Xwing.Score = 0;
            }
            bool switchLater = false;
            switch (GameState)
            {
                case GmState.StartScreen:
                    if(newState.Equals(GmState.Running))
                    {
                        Camera.Reset();
                        SoundManager.StopMusic();
                        IsMouseVisible = false;
                    }
                    break;
                case GmState.Running:
                    if (newState.Equals(GmState.Paused))
                    {
                        Camera.SaveCurrentState();
                        IsMouseVisible = true;
                    }
                    if (newState.Equals(GmState.Victory) ||
                        newState.Equals(GmState.Defeat))
                    {
                        IsMouseVisible = true;
                    }
                    break;
                case GmState.Paused:
                    if(newState.Equals(GmState.Running))
                    {
                        Camera.SoftReset();
                        switchLater = true;
                        IsMouseVisible = false;
                    }
                    break;
                

            }
            if(!switchLater)
                GameState = newState;
        }
        public String Vector3ToStr(Vector3 v)
        {
            return "(" + v.X + " " + v.Y + " " + v.Z + ")";
        }
        public String IntVector3ToStr(Vector3 v)
        {
            return "(" + (int)v.X + " " + (int)v.Y + " " + (int)v.Z + ")";
        }
        public String Vector2ToStr(Vector2 v)
        {
            return "(" + v.X + " " + v.Y + ")";
        }
        public String IntVector2ToStr(Vector2 v)
        {
            return "(" + (int)v.X + " " + (int)v.Y + ")";
        }
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            Drawer.Unload();

            base.UnloadContent();
        }
    }
    
}