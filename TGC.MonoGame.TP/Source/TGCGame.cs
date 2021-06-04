using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
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

        public SpriteFont SpriteFont;
        public Xwing Xwing = new Xwing();
        SkyBox SkyBox;
        
        
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
        }
        public GmState GameState { get; set; }
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch SpriteBatch { get; set; }

        private Model Tie { get; set; }
        private static Model TrenchPlatform { get; set; }
        private static Model TrenchStraight { get; set; }
        private static Model TrenchT { get; set; }
        private static Model TrenchIntersection { get; set; }
        private static Model TrenchElbow { get; set; }
        private static Model TrenchTurret { get; set; }
        public Trench[,] Map { get; set; }
        public const int MapSize = 21; //21x21
        public float MapLimit;
        private Model Trench2 { get; set; }
        Model skyboxModel;
        private Model LaserModel { get; set; }
        private Effect EffectTexture { get; set; }
        private Effect Effect { get; set; }
        private Effect EffectLight { get; set; }
        private float Rotation { get; set; }

        private Texture TieTexture;
        private Texture TrenchTexture;
        public Texture2D[] Crosshairs;
        public Texture2D[] HudEnergy;
        public Texture2D TopLeftBar;
        public Texture2D BtnPlay, BtnContinue, BtnMenu, BtnExit, BtnOptions;


        public int FPS;
        
        public MyCamera Camera { get; set; }
        public Vector2 MouseXY;
        public Input Input;
        public HUD HUD;
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

        
            
            //int mapSize = 9; //9x9
            //Algoritmo de generacion de mapa recursivo (ver debug output)
            Map = Trench.GenerateMap(MapSize);
            System.Diagnostics.Debug.WriteLine(Trench.ShowMapInConsole(Map, MapSize));
            

            base.Initialize();
        }

        void assignEffectToModels(Model[] models, Effect effect)
        {
            foreach (Model model in models)
                foreach (var mesh in model.Meshes)
                    foreach (var meshPart in mesh.MeshParts)
                        meshPart.Effect = effect;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Tie = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            Xwing.Model = Content.Load<Model>(ContentFolder3D + "XWing/model");
            //Xwing.EngineModel = Content.Load<Model>(ContentFolder3D + "XWing/xwing-engine");
            Xwing.EnginesModel = Content.Load<Model>(ContentFolder3D + "XWing/xwing-engines");

            TrenchPlatform = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Platform-Block");
            TrenchStraight = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Straight-Block");
            TrenchT = Content.Load<Model>(ContentFolder3D + "Trench/Trench-T-Block");
            TrenchElbow = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Elbow-Block");
            TrenchIntersection = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Intersection");
            TrenchTurret = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");
            Trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            LaserModel = Content.Load<Model>(ContentFolder3D + "Laser/Laser");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectTexture = Content.Load<Effect>(ContentFolderEffects + "BasicTexture");
            EffectLight = Content.Load<Effect>(ContentFolderEffects + "BlinnPhong");

            Xwing.Textures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };
            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");

            TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/Plates");
            Crosshairs = new Texture2D[] {  Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair-red")};
            TopLeftBar = Content.Load<Texture2D>(ContentFolderTextures + "HUD/TopLeftBar");
            HudEnergy = new Texture2D[]{
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-0"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-1"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-2"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-3"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-4"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-5"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-6"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-7"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-8"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-9"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "HUD/Energy-10")};
            BtnPlay = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Jugar");
            BtnContinue = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Continuar");
            BtnMenu = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Menu");
            BtnExit = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Salir");
            BtnOptions = Content.Load<Texture2D>(ContentFolderTextures + "HUD/Opciones");
            HUD.Init();
            //Para escribir en la pantalla
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Starjedi");
            System.Diagnostics.Debug.WriteLine("loading skybox.");
            //Skybox
            skyboxModel = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            //boxModel = Content.Load<Model>(ContentFolder3D + "Trench/Trench-Turret");

            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skybox/space_earth_small_skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyboxModel, skyBoxTexture, skyBoxEffect);


            //Asigno los efectos a los modelos correspondientes
            assignEffectToModels(new Model[] { Xwing.Model, Tie}, EffectTexture);
            assignEffectToModels(new Model[] { TrenchPlatform, TrenchStraight, TrenchElbow, TrenchT, TrenchIntersection, TrenchTurret }, EffectLight);
            assignEffectToModels(new Model[] { Trench2, LaserModel, Xwing.EnginesModel}, Effect);

            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
            EffectTexture.Parameters["TextureMultiplier"].SetValue(1f);

            EffectLight.Parameters["baseTexture"].SetValue(TrenchTexture);
            EffectLight.Parameters["ambientColor"].SetValue(new Vector3(0.25f, 0.25f, 0.25f));
            EffectLight.Parameters["diffuseColor"].SetValue(new Vector3(0.6f, 0.6f, 0.6f));
            EffectLight.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));

            EffectLight.Parameters["KAmbient"].SetValue(0.5f);
            EffectLight.Parameters["KDiffuse"].SetValue(5.0f);
            EffectLight.Parameters["KSpecular"].SetValue(20f);
            EffectLight.Parameters["shininess"].SetValue(40f);
            
            Trench.UpdateTrenches();
            
            var blockSize = MapLimit / MapSize;
            Camera.MapLimit = MapLimit;
            Camera.MapSize = MapSize;
            Camera.BlockSize = blockSize;
            Camera.Position = new Vector3(MapLimit/2 - blockSize/2, 0, blockSize /2);
            Xwing.MapLimit = MapLimit;
            Xwing.MapSize = MapSize;
            
            Laser.MapLimit = MapLimit;
            Laser.MapSize = MapSize;
            Laser.BlockSize = blockSize;
            TieFighter.GenerateEnemies(Xwing);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MouseXY.X = Mouse.GetState().X;
            MouseXY.Y = Mouse.GetState().Y;

            Input.ProcessInput();
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += elapsedTime * 0.5f;
            Rotation %= MathHelper.TwoPi;
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
                    //Update camara
                    Camera.Update(gameTime);
                    //Generacion de enemigos, de ser necesario
                    TieFighter.GenerateEnemies(Xwing);
                    //Update Xwing
                    Xwing.Update(elapsedTime, Camera);

                    Vector4 zone = Xwing.GetZone();

                    //enemyLasers.Clear();

                    for (int x = (int)zone.X; x < zone.Y; x++)
                        for (int z = (int)zone.Z; z < zone.W; z++)
                            foreach (var turret in Map[x, z].Turrets)
                                turret.Update(Xwing, elapsedTime);
                    Laser.UpdateAll(elapsedTime, Xwing);

                    //Colisiones
                    Xwing.VerifyCollisions(Laser.EnemyLasers, Map);
                    //Xwing.fired.RemoveAll(laser => laser.Age >= laser.MaxAge);

                    TieFighter.UpdateEnemies(elapsedTime, Xwing);

                    EffectLight.Parameters["lightPosition"].SetValue(Xwing.Position - Vector3.Left * 500 + Vector3.Up * 500);
                    EffectLight.Parameters["eyePosition"].SetValue(Camera.Position);
                    #endregion
                    break;
                case GmState.Paused:
                    #region paused

                    float y;

                    if (Xwing.Position.Y > 0)
                    {
                        y = Xwing.Position.Y;
                        Camera.Pitch = 0f;
                    }
                    else
                    {
                        y = 30f;
                        Camera.Pitch = -15f;
                    }
                    Camera.Position = new Vector3(
                        Xwing.Position.X + 100f * MathF.Cos(Rotation),
                        y,
                        Xwing.Position.Z + 100f * MathF.Sin(Rotation));
                    Vector3 frontDirection = Xwing.Position - Camera.Position;

                    Camera.Yaw = MathHelper.ToDegrees(MathF.Atan2(frontDirection.Z, frontDirection.X));
                    //Camera.Pitch = MathF.Asin(frontDirection.Y);
                    
                    Camera.UpdateVectorView();
                    #endregion
                    break;
                case GmState.Victory:
                    #region victory
                    #endregion
                    break;
                case GmState.Defeat:
                    #region defeat
                    #endregion
                    break;
            }
            
            
            base.Update(gameTime);
        }

        //funciones que pueden ser utiles
        Vector3 directionalAngles(Vector3 v)
        {
            return new Vector3(
                MathF.Acos(v.X / v.Length()),
                MathF.Acos(v.Y / v.Length()),
                MathF.Acos(v.Z / v.Length()));
        }
        String mensaje1 = "Movimiento: WASD, Camara: flechas, para habilitar mouse apretar M";
        String mensaje2 = "Movimiento: WASD, Camara: flechas + mouse, para deshabilitar flechas apretar M";
        String mensaje3 = "Movimiento: WASD, Camara: mouse, para solo flechas apretar M";
        String mensaje;
        public String Vector3ToStr(Vector3 v)
        {
            return "(" + v.X + " " + v.Y + " " + v.Z +")";
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
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            //Configuro efectos
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            EffectTexture.Parameters["View"].SetValue(Camera.View);
            EffectTexture.Parameters["Projection"].SetValue(Camera.Projection);

            var rotationMatrix = Matrix.CreateRotationY(Rotation);
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            FPS = (int)Math.Round(1 / deltaTime);

            SkyBox.Draw(Camera.View, Camera.Projection, Camera.Position);
            
            switch (GameState)
            {
                case GmState.StartScreen:
                    #region startscreen
                   
                    #endregion
                    break;
                case GmState.Running:
                    #region running

                    DrawMap();
                    foreach (var enemy in TieFighter.Enemies)
                        DrawTie(enemy);
                    //DrawModel(Tie, enemy.SRT, new Vector3(0.5f, 0f, 0.5f));
                    foreach (var laser in Laser.AlliedLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color);
                    foreach (var laser in Laser.EnemyLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color);
                    DrawXWing();
                   
                    #endregion
                    break;
                case GmState.Paused:
                    #region paused

                    DrawMap();
                    foreach (var enemy in TieFighter.Enemies)
                        DrawTie(enemy);
                    //DrawModel(Tie, enemy.SRT, new Vector3(0.5f, 0f, 0.5f));
                    foreach (var laser in Laser.AlliedLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color);
                    foreach (var laser in Laser.EnemyLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color);
                    DrawXWing();

                    #endregion
                    break;
                case GmState.Victory:
                    #region victory
                    #endregion
                    break;
                case GmState.Defeat:
                    #region defeat
                    #endregion
                    break;
            }
            HUD.Draw();
        }
        //float vDistance(Vector3 v, Vector3 w)
        //{
        //    return MathF.Sqrt(MathF.Pow(w.X - w.X, 2) + MathF.Pow(w.Y - w.Y, 2) + MathF.Pow(w.Z - w.Z, 2));
        //}
        void DrawMap()
        {
            Vector4 zone = Xwing.GetZone();
            //Zona de vision del xwing, para mejorar performance y no tener que renderizar todo el mapa
            for (int x = (int)zone.X; x < zone.Y; x++)
                for (int z = (int)zone.Z; z < zone.W; z++)
                    DrawTrench(Map[x, z]);
        }
        
        void DrawTrench(Trench t)
        {
            Matrix world;
            foreach (var mesh in t.Model.Meshes)
            {
                world = mesh.ParentBone.Transform * t.SRT;

                EffectLight.Parameters["World"].SetValue(world);
                EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                EffectLight.Parameters["WorldViewProjection"].SetValue(world * Camera.View * Camera.Projection);

                mesh.Draw();
            }
            foreach(var turret in t.Turrets)
            {
                foreach(var mesh in TrenchTurret.Meshes)
                {
                    world = mesh.ParentBone.Transform * turret.SRT;

                    EffectLight.Parameters["World"].SetValue(world);
                    EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    EffectLight.Parameters["WorldViewProjection"].SetValue(world * Camera.View * Camera.Projection);

                    mesh.Draw();
                }
            }
        }

        void DrawXWing()
        {
            int meshCount = 0; //Como el xwing tiene 2 texturas, tengo que dibujarlo de esta manera
            
            //efecto para verificar colisiones, se pone rojo
            if (Xwing.hit)
                EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Right * 0.5f);
            
            foreach (var mesh in Xwing.Model.Meshes)
            {
                Xwing.World = mesh.ParentBone.Transform * Xwing.SRT;
                
                EffectTexture.Parameters["World"].SetValue(Xwing.World);
                EffectTexture.Parameters["ModelTexture"].SetValue(Xwing.Textures[meshCount]);
                meshCount++;

                mesh.Draw();
            }
            DrawModel(Xwing.EnginesModel, Xwing.EnginesSRT, Xwing.EnginesColor);
            
            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
        }

        void DrawTie(TieFighter tie)
        {
            Matrix world;
            foreach (var mesh in Tie.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(TieTexture);
                //EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.One);
                mesh.Draw();
            }
            //EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
        }
        void DrawModel(Model model, Matrix SRT, Vector3 color)
        {
            Effect.Parameters["DiffuseColor"]?.SetValue(color);

            foreach (var mesh in model.Meshes)
            {
                var world = mesh.ParentBone.Transform * SRT;
                Effect.Parameters["World"].SetValue(world);
                mesh.Draw();
            }
        }

        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
        public static Model GetModelFromType(TrenchType type)
        {
            switch(type)
            {
                case TrenchType.Platform:        return TrenchPlatform;
                case TrenchType.Straight:        return TrenchStraight;
                case TrenchType.T:               return TrenchT;
                case TrenchType.Intersection:    return TrenchIntersection;
                case TrenchType.Elbow:           return TrenchElbow;
                default:                         return TrenchPlatform;
            }
        }
        public enum GmState
        {
            StartScreen,
            Running,
            Paused,
            Victory,
            Defeat
        }
    }
}