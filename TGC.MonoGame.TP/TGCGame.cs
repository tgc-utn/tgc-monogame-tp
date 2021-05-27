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

        private SpriteFont SpriteFont;
        Xwing Xwing = new Xwing();
        SkyBox SkyBox;
        public float TieScale = 0.02f;

        public float TrenchScale = 0.07f;
        public float Trench2Scale = 0.07f;

        public Vector3 TrenchTranslation = new Vector3(0, -30, -130);
        public Vector3 Trench2Translation = new Vector3(0, -80, -290);
        public Vector3 XwingTranslation = new Vector3(0, -5, -40);

        System.Timers.Timer camUpdateTimer;

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


        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }

        private Model Tie { get; set; }
        private Model Tie2 { get; set; }
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

        private BasicEffect BasicEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix XwingWorld { get; set; }
        private Matrix TieWorld { get; set; }
        private Matrix Tie2World { get; set; }
        private Matrix TrenchWorld { get; set; }
        private Matrix Trench2World { get; set; }

        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        private Texture TieTexture;
        private Texture TrenchTexture;
        private Texture2D[] Crosshairs;

        List<TieFighter> enemies = new List<TieFighter>();
        private MyCamera Camera { get; set; }
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            //var rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;


            // Configuramos nuestras matrices de la escena.
            TieWorld = Matrix.Identity;
            Tie2World = Matrix.Identity;
            TrenchWorld = Matrix.Identity;
            Trench2World = Matrix.Identity;

            Xwing.World = Matrix.Identity;
            Xwing.Scale = 2.5f;

            // Hace que el mouse sea visible.
            IsMouseVisible = false;

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
            Camera = new MyCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f, 0f, 0f), size);

            camUpdateTimer = new System.Timers.Timer(5);
            camUpdateTimer.Elapsed += CamUpdateTimerTick;
            camUpdateTimer.AutoReset = true;
            camUpdateTimer.Enabled = true;

            
            //int mapSize = 9; //9x9
            //Algoritmo de generacion de mapa recursivo (ver debug output)
            Map = Trench.GenerateMap(MapSize);
            System.Diagnostics.Debug.WriteLine(Trench.ShowMapInConsole(Map, MapSize));
            

            base.Initialize();
        }

        private void CamUpdateTimerTick(object sender, ElapsedEventArgs e)
        {
            if (Camera.MouseLookEnabled && IsActive)
                Camera.ProcessMouse(Xwing);
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

            Xwing.Model = Content.Load<Model>(ContentFolder3D + "XWing/model");
            Tie = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
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
            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.Identity,
                TextureEnabled = false,
            };

            Xwing.Textures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };
            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");

            TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/Plates");
            Crosshairs = new Texture2D[] {  Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "Crosshair/crosshair-red")};

            //Para escribir en la pantalla
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");
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
            assignEffectToModels(new Model[] { Trench2, LaserModel }, Effect);

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
            //Trench.UpdateModels(ref Map, MapSize);
            UpdateTrenches();

            var blockSize = MapLimit / MapSize;
            Camera.Position = new Vector3(MapLimit/2 - blockSize/2, 0, blockSize /2);
            generateEnemies();

            base.LoadContent();
        }

        List<Keys> ignoredKeys = new List<Keys>();
        List<Matrix> trenches = new List<Matrix>();
        void UpdateTrenches()
        {
            //Inicializo valores importantes del mapa
            float tx = 0;
            float tz = 0;
            Matrix S = Matrix.CreateScale(TrenchScale);
            Matrix R = Matrix.Identity;
            Matrix T = Matrix.CreateTranslation(new Vector3(0, -35, 0));
            float delta = 395.5f;

            Random rnd = new Random();
            for (int x = 0; x < MapSize; x++)
            {

                tz = 0;
                for (int z = 0; z < MapSize; z++)
                {
                    var r = rnd.Next(0, 100);

                    Trench block = Map[x, z];

                    
                    block.Model = GetModelFromType(Map[x, z].Type);
                    
                    block.Position = new Vector3(tx, 0, tz);

                    var boxWidth = 20;
                    
                    var VerticalFullBox = new BoundingBox(
                        block.Position - new Vector3(boxWidth * 0.5f, 50, delta), block.Position + new Vector3(boxWidth * 0.5f, 0, delta));
                    var HorizontalFullBox = new BoundingBox(
                        block.Position - new Vector3(delta, 50, boxWidth * 0.5f), block.Position + new Vector3(delta, 0, boxWidth * 0.5f));
                    
                    var VerticalHalfBox = new BoundingBox(
                        block.Position - new Vector3(boxWidth * 0.5f, 50, delta), block.Position + new Vector3(boxWidth * 0.5f, 0, delta * 0.5f));
                    var VerticalHalfBox2 = new BoundingBox(
                        block.Position - new Vector3(boxWidth * 0.5f, 50, delta * 0.5f), block.Position + new Vector3(boxWidth * 0.5f, 0, delta));

                    var HorizontalHalfBox = new BoundingBox(
                        block.Position - new Vector3(delta, 50, boxWidth * 0.5f), block.Position + new Vector3(delta * 0.5f, 0, boxWidth * 0.5f));
                    var HorizontalHalfBox2 = new BoundingBox(
                        block.Position - new Vector3(delta * 0.5f, 50, boxWidth * 0.5f), block.Position + new Vector3(delta, 0, boxWidth * 0.5f));

                    switch (block.Type)
                    {
                        case TrenchType.Platform: 
                            R = Matrix.Identity;  
                            break;
                        case TrenchType.Straight: 
                            R = Matrix.CreateRotationY(-MathHelper.PiOver2);
                            if (block.Rotation == 0f || block.Rotation == 180f)
                                block.boundingBoxes.Add(VerticalFullBox);
                            else
                                block.boundingBoxes.Add(HorizontalFullBox);
                            break;
                        case TrenchType.T: 
                            R = Matrix.CreateRotationY(MathHelper.PiOver2);
                            switch(block.Rotation)
                            {
                                case 0f:
                                    block.boundingBoxes.Add(VerticalHalfBox);
                                    block.boundingBoxes.Add(HorizontalFullBox);
                                    break;
                                case 90f:
                                    block.boundingBoxes.Add(HorizontalHalfBox);
                                    block.boundingBoxes.Add(VerticalFullBox);
                                    break;
                                case 180f:
                                    block.boundingBoxes.Add(VerticalHalfBox2);
                                    block.boundingBoxes.Add(HorizontalFullBox);
                                    break;
                                case 270f:
                                    block.boundingBoxes.Add(HorizontalHalfBox2);
                                    block.boundingBoxes.Add(VerticalFullBox);
                                    break;
                            }
                            break;
                        case TrenchType.Elbow: 
                            R = Matrix.Identity;
                            switch (block.Rotation)
                            {
                                case 0f:
                                    block.boundingBoxes.Add(VerticalHalfBox);
                                    block.boundingBoxes.Add(HorizontalHalfBox);
                                    break;
                                case 90f:
                                    block.boundingBoxes.Add(HorizontalHalfBox);
                                    block.boundingBoxes.Add(VerticalHalfBox2);
                                    break;
                                case 180f:
                                    block.boundingBoxes.Add(VerticalHalfBox2);
                                    block.boundingBoxes.Add(HorizontalHalfBox2);
                                    break;
                                case 270f:
                                    block.boundingBoxes.Add(HorizontalHalfBox2);
                                    block.boundingBoxes.Add(VerticalHalfBox);
                                    break;
                            }
                            break;
                        case TrenchType.Intersection: 
                            R = Matrix.Identity;
                            block.boundingBoxes.Add(HorizontalFullBox);
                            block.boundingBoxes.Add(VerticalFullBox);
                            break;

                    }

                    block.SRT =
                        S * R * Matrix.CreateRotationY(MathHelper.ToRadians(block.Rotation)) * 
                        Matrix.CreateTranslation(block.Position) * T;

                    //TODO: Corregir valores para cada trench distinto
                    var turretPos = block.Position - new Vector3(0,15,0);

                    if (r < 50) // %50 chance de tener una torre
                        block.Turrets.Add(new TrenchTurret());
                    if (r < 20) // %20 chance de tener dos
                        block.Turrets.Add(new TrenchTurret());

                    foreach (var turret in block.Turrets)
                    {
                        turret.S = S;
                        turret.SRT = S * R * Matrix.CreateTranslation(turretPos);
                        turret.Position = turretPos;
                        turretPos += new Vector3(0, 0, 20);
                    }
                    
                    tz += delta;
                }
                tx += delta;
            }
            MapLimit = tz;
            Xwing.MapLimit = MapLimit;
            Xwing.MapSize = MapSize;
            //}
        }
        void generateEnemies()
        {
            Random rnd = new Random();
            int maxEnemies = 2;
            int distance = 500;
            for (int i = 0; i < maxEnemies - enemies.Count; i++)
            {
                Vector3 random = new Vector3(rnd.Next(-distance, distance), 0f, rnd.Next(-distance, distance));
                Vector3 pos = Xwing.Position + random;
                Vector3 dir = Vector3.Normalize(Xwing.Position - pos);


                Matrix SRT = Matrix.CreateScale(TieScale) * Matrix.CreateTranslation(pos);
                enemies.Add(new TieFighter(pos, dir, Matrix.Identity, SRT, TieScale));
            }
        }
        void updateEnemies(float time)
        {
            foreach (var enemy in enemies)
            {
                enemy.Update(Xwing, time);
                enemy.fireLaser();
            }
            //TODO: ver calculos pitch y yaw, lasers ok
            //System.Diagnostics.Debug.WriteLine(
            //    "P " +
            //    enemies[0].Pitch +
            //    " Y " +
            //    enemies[0].Yaw);
        }
        public List<Laser> enemyLasers = new List<Laser>();


        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Update camara
            Camera.Update(gameTime);
            //Generacion de enemigos, de ser necesario
            generateEnemies();
            //Update Xwing
            Xwing.Update(elapsedTime, Camera);

            Vector4 zone = Xwing.GetZone();

            enemyLasers.Clear();

            //TODO: Corregir YAW torres
            for (int x = (int)zone.X; x < zone.Y; x++)
                for (int z = (int)zone.Z; z < zone.W; z++)
                    foreach(var turret in Map[x, z].Turrets)
                    {
                        turret.Update(Xwing, elapsedTime);
                        foreach(var laser in turret.fired)
                        {
                            laser.Update(elapsedTime);
                            enemyLasers.Add(laser);
                        }
                    }
                   

            //Movimiento de lasers, copia en una sola lista
            
            foreach (var enemy in enemies)
                foreach (var laser in enemy.fired)
                {
                    laser.Update(elapsedTime);
                    enemyLasers.Add(laser);
                }

            //Colisiones
            Xwing.VerifyCollisions(enemyLasers, Map);
            enemies.ForEach(enemy => enemy.VerifyCollisions(Xwing.fired));
            enemies.RemoveAll(enemy => enemy.HP <= 0);
            //TODO: Corregir YAW enemigos
            updateEnemies(elapsedTime);

            EffectLight.Parameters["lightPosition"].SetValue(Xwing.Position - Vector3.Left* 500 + Vector3.Up * 500);
            EffectLight.Parameters["eyePosition"].SetValue(Camera.Position);

            //Teclado
            #region Input
            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();
            if (Camera.MouseLookEnabled)
            {
                if (mState.LeftButton.Equals(ButtonState.Pressed))
                {
                    Xwing.fireLaser();
                }
            }
            if (kState.IsKeyDown(Keys.Escape))
                Exit();
            if (kState.IsKeyDown(Keys.P))
            {
                ignoredKeys.Add(Keys.P);
            }
            if (kState.IsKeyDown(Keys.H))
            {
                if (!ignoredKeys.Contains(Keys.H))
                {
                    ignoredKeys.Add(Keys.H);
                    Xwing.hit = !Xwing.hit;
                }
            }
            if (kState.IsKeyDown(Keys.F))
            {
                Xwing.fireLaser();
            }
            if (kState.IsKeyDown(Keys.V))
            {
                if (!ignoredKeys.Contains(Keys.V))
                {
                    ignoredKeys.Add(Keys.V);
                    IsFixedTimeStep = !IsFixedTimeStep;
                }
            }
            if (kState.IsKeyDown(Keys.F11))
            {
                //evito que se cambie constantemente manteniendo apretada la tecla
                if (!ignoredKeys.Contains(Keys.F11))
                {
                    ignoredKeys.Add(Keys.F11);
                    if (Graphics.IsFullScreen) //720 windowed
                    {
                        Graphics.IsFullScreen = false;
                        Graphics.PreferredBackBufferWidth = 1280;
                        Graphics.PreferredBackBufferHeight = 720;
                    }
                    else //1080 fullscreen
                    {
                        Graphics.IsFullScreen = true;
                        Graphics.PreferredBackBufferWidth = 1920;
                        Graphics.PreferredBackBufferHeight = 1080;
                    }
                    Graphics.ApplyChanges();
                }
            }

            if (kState.IsKeyDown(Keys.M))
            {
                //evito que se cambie constantemente manteniendo apretada la tecla
                if (!ignoredKeys.Contains(Keys.M))
                {
                    ignoredKeys.Add(Keys.M);
                    if (!Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                    {
                        Camera.MouseLookEnabled = true;
                        //correccion inicial para que no salte a un punto cualquiera
                        Camera.pastMousePosition = Mouse.GetState().Position.ToVector2();
                        IsMouseVisible = false;
                    }
                    else if (Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                    {
                        Camera.ArrowsLookEnabled = false;
                    }
                    else if (Camera.MouseLookEnabled && !Camera.ArrowsLookEnabled)
                    {
                        Camera.MouseLookEnabled = false;
                        Camera.ArrowsLookEnabled = true;
                        IsMouseVisible = true;
                    }
                }
            }
            if (kState.IsKeyDown(Keys.R))
            {
                if (!ignoredKeys.Contains(Keys.R))
                {
                    ignoredKeys.Add(Keys.R);
                    Xwing.barrelRolling = true;
                }
            }
            //remuevo de la lista aquellas teclas que solte
            ignoredKeys.RemoveAll(kState.IsKeyUp);
            #endregion Input

            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

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
        String Vector3ToStr(Vector3 v)
        {
            return "(" + v.X + " " + v.Y + " " + v.Z +")";
        }
        String IntVector3ToStr(Vector3 v)
        {
            return "(" + (int)v.X + " " + (int)v.Y + " " + (int)v.Z + ")";
        }
        String Vector2ToStr(Vector2 v)
        {
            return "(" + v.X + " " + v.Y + ")";
        }
        String IntVector2ToStr(Vector2 v)
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
            int fps = (int)Math.Round(1 / deltaTime);

            SkyBox.Draw(Camera.View, Camera.Projection, Camera.Position);

            DrawXWing();

            DrawMap();
            //debug de Tie, rotando 
            //Matrix SRT =
            //    Matrix.CreateScale(TieScale) *
            //    Matrix.CreateRotationY(MathF.PI) *
            //    Matrix.CreateTranslation(new Vector3(40, 0, 0)) *
            //    rotationMatrix;
            //DrawTie(TieWorld, SRT);

            foreach (var laser in Xwing.fired)
            {
                laser.Update(deltaTime);
                DrawModel(LaserModel, Xwing.World, laser.SRT, laser.Color);
            }
            
            foreach (var enemy in enemies)
            {
                DrawTie(enemy);
                foreach (var laser in enemy.fired)
                {
                    laser.Update(deltaTime);
                    DrawModel(LaserModel, Xwing.World, laser.SRT, laser.Color);
                }
            }

            if (!Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                mensaje = mensaje1;
            else if (Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                mensaje = mensaje2;
            else if (Camera.MouseLookEnabled && !Camera.ArrowsLookEnabled)
                mensaje = mensaje3;

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            SpriteBatch.DrawString(SpriteFont, "FPS " + fps + " HP " + Xwing.HP + " Invulnerable: " + Xwing.barrelRolling, Vector2.Zero, Color.White);
            SpriteBatch.End();

            var center = GraphicsDevice.Viewport.Bounds.Size;
            center.X /= 2;
            center.Y /= 2;

            var scale = 0.1f;
            var sz = 512 * scale;

            //Crosshair
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            SpriteBatch.Draw(Crosshairs[0], new Vector2(center.X - sz / 2, center.Y - sz / 2), null, Color.White, 0f, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
            SpriteBatch.End();



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
                foreach(var laser in turret.fired)
                {
                    DrawModel(LaserModel, Xwing.World, laser.SRT, laser.Color);
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
            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
        }

        void DrawTie(TieFighter tie)
        {
            tie.drawn = true;
            foreach (var mesh in Tie.Meshes)
            {
                var world = mesh.ParentBone.Transform * tie.SRT;

                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(TieTexture);
                mesh.Draw();
            }
        }
        void DrawModel(Model model, Matrix world, Matrix SRT, Vector3 color)
        {
            Effect.Parameters["DiffuseColor"]?.SetValue(color);

            foreach (var mesh in model.Meshes)
            {
                world = mesh.ParentBone.Transform * SRT;
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
    }
}