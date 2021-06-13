using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.IO;

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

            Gizmos = new Gizmos();
        }
        public GmState GameState { get; set; }
        public GraphicsDeviceManager Graphics { get; }

        private Model Tie { get; set; }
        private static Model TrenchPlatform { get; set; }
        private static Model TrenchStraight { get; set; }
        private static Model TrenchT { get; set; }
        private static Model TrenchIntersection { get; set; }
        private static Model TrenchElbow { get; set; }
        public static Model TrenchTurret { get; set; }
        public Trench[,] Map { get; set; }
        public const int MapSize = 21; //21x21
        public float MapLimit;
        private Model Trench2 { get; set; }
        Model skyboxModel;
        public Model LaserModel { get; set; }
        private Effect EffectTexture { get; set; }
        private Effect Effect { get; set; }
        public Effect EffectLight { get; set; }
        public Effect EffectBloom { get; set; }
        public Effect EffectBlur { get; set; }

        public float PausedCameraRotation { get; set; }

        private Texture TieTexture;
        private Texture TrenchTexture;
        

        public int FPS;
        
        public MyCamera Camera { get; set; }
        public MyCamera LookBack { get; set; }
        public MyCamera SelectedCamera { get; set; }
        public Vector2 MouseXY;
        public Input Input;
        public HUD HUD;

        public bool ApplyBloom = false;
        private RenderTarget2D FirstPassBloomRenderTarget;
        private FullScreenQuad FullScreenQuad;
        private RenderTarget2D MainSceneRenderTarget;
        private RenderTarget2D SecondPassBloomRenderTarget;
        int BloomPassCount = 2;

        EffectParameter EPlightWorldViewProjection;
        EffectParameter EPlightView;
        EffectParameter EPlightProjection;
        EffectParameter EPlightWorld;
        EffectParameter EPlightInverseTransposeWorld;
        EffectParameter EPlightLightPosition;
        EffectParameter EPlightEyePosition;
        EffectParameter EPlightTexture;

        EffectParameter EPbloomWorldViewProjection;
        EffectParameter EPbloomTexture;
        EffectParameter EPblurTexture;

        EffectParameter EPbasicView;
        EffectParameter EPbasicProjection;
        EffectParameter EPbasicWorld;
        EffectParameter EPbasicColor;

        EffectParameter EPtextureView;
        EffectParameter EPtextureProjection;
        EffectParameter EPtextureWorld;
        EffectParameter EPtextureColor;

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

            LookBack = new MyCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.Zero, size);

            SelectedCamera = Camera;
            //int mapSize = 9; //9x9
            //Algoritmo de generacion de mapa recursivo (ver debug output)
            Map = Trench.GenerateMap(MapSize);
            System.Diagnostics.Debug.WriteLine(Trench.ShowMapInConsole(Map, MapSize));

           
            
            base.Initialize();
        }

        void assignEffectToModel(Model model, Effect effect)
        {
            foreach (var mesh in model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
        }
        void assignEffectToModels(Model[] models, Effect effect)
        {
            foreach (Model model in models)
                assignEffectToModel(model, effect);
        }
        

        public float kd = 0.8f;
        public float ks = 0.4f;

        protected override void LoadContent()
        {
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
            EffectBloom = Content.Load<Effect>(ContentFolderEffects + "Bloom");
            EffectBlur = Content.Load<Effect>(ContentFolderEffects + "GaussianBlur");

            Xwing.Textures = new Texture[] { Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                            Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };

            TieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");

            TrenchTexture = Content.Load<Texture2D>(ContentFolderTextures + "Trench/MetalSurface");

            HUD.LoadContent();
            
            SoundManager.LoadContent();
            
            Gizmos.LoadContent(GraphicsDevice);

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

            manageEffectParameters();
            
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

            FullScreenQuad = new FullScreenQuad(GraphicsDevice);
            MainSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
               GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
               RenderTargetUsage.DiscardContents);
            FirstPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            SecondPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);
            //MainSceneRenderTarget       = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //FirstPassBloomRenderTarget  = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //SecondPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            base.LoadContent();
        }

        BoundingFrustum BoundingFrustum = new BoundingFrustum(Matrix.Identity);

        public int TrenchesDrawn, TrenchesInZone;
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MouseXY.X = Mouse.GetState().X;
            MouseXY.Y = Mouse.GetState().Y;

            Input.ProcessInput();

            BoundingFrustum.Matrix = SelectedCamera.View * SelectedCamera.Projection;
            
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

                    trenchesToDraw.Clear();
                    TrenchesInZone = 0;
                    
                    for (int x = (int)zone.X; x < zone.Y; x++)
                        for (int z = (int)zone.Z; z < zone.W; z++)
                        {
                            var block = Map[x, z];
                            block.Turrets.ForEach(turret => turret.Update(Xwing, elapsedTime));
                            block.Turrets.RemoveAll(turret => turret.needsRemoval);

                            if(BoundingFrustum.Intersects(block.BS))
                                trenchesToDraw.Add(Map[x, z]);

                            TrenchesInZone++;
                        }
                    TrenchesDrawn = trenchesToDraw.Count;

                    Laser.UpdateAll(elapsedTime, Xwing);

                    //Colisiones
                    Xwing.VerifyCollisions(Laser.EnemyLasers, Map);
                    //Xwing.fired.RemoveAll(laser => laser.Age >= laser.MaxAge);

                    ////Zona de vision del xwing, para mejorar performance y no tener que renderizar todo el mapa
                    //for (int x = (int)zone.X; x < zone.Y; x++)
                    //    for (int z = (int)zone.Z; z < zone.W; z++)
                    //        DrawTrench(Map[x, z], isBloomPass);


                    TieFighter.UpdateEnemies(elapsedTime, Xwing);

                    EffectLight.Parameters["lightPosition"].SetValue(Xwing.Position - Vector3.Left * 500 + Vector3.Up * 50);
                    EffectLight.Parameters["eyePosition"].SetValue(SelectedCamera.Position);

                    SoundManager.UpdateRandomDistantSounds(elapsedTime);
                    #endregion
                    break;
                case GmState.Paused:
                    #region paused

                
                    Camera.PausedUpdate(elapsedTime, Xwing);

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

        

        
        public bool saveToFile = true;
        public bool modelTechnique = false;

        
        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            FPS = (int)Math.Round(1 / deltaTime);
            
            Stream stream;

            if (!ApplyBloom)
            {
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                DrawScene(false);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(MainSceneRenderTarget);
                
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawScene(false);
            
                #region Pass 2

                // Set the render target as our bloomRenderTarget, we are drawing the bloom color into this texture
                GraphicsDevice.SetRenderTarget(FirstPassBloomRenderTarget);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                if (modelTechnique)
                {
                    EffectBloom.CurrentTechnique = EffectBloom.Techniques["BloomModelPass"];
                    EffectBloom.Parameters["Color"].SetValue(Vector3.Zero);
                    //Debug.WriteLine("drawing bloom filter");
                    DrawScene(true);
                }
                else
                {
                    EffectBloom.CurrentTechnique = EffectBloom.Techniques["BloomPixelPass"];

                    FullScreenQuad.Draw(EffectBloom);
                }

                if (saveToFile)
                {
                    stream = File.OpenWrite("bloomFilter.png");
                    FirstPassBloomRenderTarget.SaveAsPng(stream, 1280, 720);
                    stream.Dispose();
                }
                #endregion

                #region Multipass Bloom

                // Now we apply a blur effect to the bloom texture
                // Note that we apply this a number of times and we switch
                // the render target with the source texture
                // Basically, this applies the blur effect N times
                EffectBlur.CurrentTechnique = EffectBlur.Techniques["Blur"];
                EffectBlur.Parameters["screenSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                var bloomTexture = FirstPassBloomRenderTarget;
                var finalBloomRenderTarget = SecondPassBloomRenderTarget;
                //Debug.WriteLine("starting blur");

                for (var index = 0; index < BloomPassCount; index++)
                {
                    //Exchange(ref SecondaPassBloomRenderTarget, ref FirstPassBloomRenderTarget);

                    // Set the render target as null, we are drawing into the screen now!
                    GraphicsDevice.SetRenderTarget(finalBloomRenderTarget);
                    GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                    EffectBlur.Parameters["baseTexture"].SetValue(bloomTexture);
                    FullScreenQuad.Draw(EffectBlur);

                    if (index != BloomPassCount - 1)
                    {
                        var auxiliar = bloomTexture;
                        bloomTexture = finalBloomRenderTarget;
                        finalBloomRenderTarget = auxiliar;
                    }
                }
                if(saveToFile)
                {
                    stream = File.OpenWrite("blurred.png");
                    finalBloomRenderTarget.SaveAsPng(stream, 1280, 720);
                    stream.Dispose();
                }
                saveToFile = false;
                #endregion

                #region Final Pass

                // Set the depth configuration as none, as we don't use depth in this pass
                GraphicsDevice.DepthStencilState = DepthStencilState.None;

                // Set the render target as null, we are drawing into the screen now!
                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);
                //Debug.WriteLine("integrating");

                // Set the technique to our blur technique
                // Then draw a texture into a full-screen quad
                // using our rendertarget as texture
                EffectBloom.CurrentTechnique = EffectBloom.Techniques["Integrate"];
                EffectBloom.Parameters["baseTexture"].SetValue(MainSceneRenderTarget);
                EffectBloom.Parameters["bloomTexture"].SetValue(finalBloomRenderTarget);
                FullScreenQuad.Draw(EffectBloom);

                #endregion
            }

            if (ShowGizmos)
                Gizmos.Draw();
            HUD.Draw();

        }
        void DrawScene(bool isBloomPass)
        {
            var CameraView = SelectedCamera.View;
            var CameraProjection = SelectedCamera.Projection;
            var CameraPosition = SelectedCamera.Position;

            //Effect.Parameters["View"].SetValue(CameraView);
            //Effect.Parameters["Projection"].SetValue(CameraProjection);
            //EffectTexture.Parameters["View"].SetValue(CameraView);
            //EffectTexture.Parameters["Projection"].SetValue(CameraProjection);
            EPbasicView.SetValue(CameraView);
            EPtextureView.SetValue(CameraView);
            EPbasicProjection.SetValue(CameraProjection);
            EPtextureProjection.SetValue(CameraProjection);


            if (!isBloomPass)
                SkyBox.Draw(CameraView, CameraProjection, CameraPosition);

            switch (GameState)
            {
                case GmState.StartScreen:
                    #region startscreen

                    #endregion
                    break;
                case GmState.Running:
                    #region running
                    DrawMap(isBloomPass);
                    foreach (var enemy in TieFighter.Enemies)
                        DrawTie(enemy, isBloomPass);
                    foreach (var laser in Laser.AlliedLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);
                    foreach (var laser in Laser.EnemyLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);

                    if (ShowGizmos)
                    {
                        Matrix SRT;
                        foreach (var laser in Laser.AlliedLasers)
                        {
                            var OBB = laser.BoundingBox;
                            SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                            Gizmos.DrawCube(SRT, Color.White);
                        }
                        foreach (var laser in Laser.EnemyLasers)
                        {
                            var OBB = laser.BoundingBox;
                            SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                            Gizmos.DrawCube(SRT, Color.White);
                        }
                    }
                    DrawXWing(isBloomPass);

                    #endregion
                    break;
                case GmState.Paused:
                    #region paused
                    DrawMap(isBloomPass);
                    foreach (var enemy in TieFighter.Enemies)
                        DrawTie(enemy, isBloomPass);
                    foreach (var laser in Laser.AlliedLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);
                    foreach (var laser in Laser.EnemyLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);

                    if (ShowGizmos)
                    {
                        Matrix SRT;
                        foreach (var laser in Laser.AlliedLasers)
                        {
                            var OBB = laser.BoundingBox;
                            SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                            Gizmos.DrawCube(SRT, Color.White);
                        }
                        foreach (var laser in Laser.EnemyLasers)
                        {
                            var OBB = laser.BoundingBox;
                            SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                            Gizmos.DrawCube(SRT, Color.White);
                        }
                    }
                    DrawXWing(isBloomPass);

                    #endregion
                    break;
                case GmState.Victory:
                    #region victory
                    #endregion
                    break;
                case GmState.Defeat:
                    #region defeat
                    if (!isBloomPass)
                        DrawMap(isBloomPass);
                    foreach (var enemy in TieFighter.Enemies)
                        DrawTie(enemy, isBloomPass);
                    foreach (var laser in Laser.AlliedLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);
                    foreach (var laser in Laser.EnemyLasers)
                        DrawModel(LaserModel, laser.SRT, laser.Color, isBloomPass);
                    DrawXWing(isBloomPass);
                    #endregion
                    break;
            }

        }
        //float vDistance(Vector3 v, Vector3 w)
        //{
        //    return MathF.Sqrt(MathF.Pow(w.X - w.X, 2) + MathF.Pow(w.Y - w.Y, 2) + MathF.Pow(w.Z - w.Z, 2));
        //}
        List<Trench> trenchesToDraw = new List<Trench>();
        void DrawMap(bool isBloomPass)
        {
            foreach (var t in trenchesToDraw)
                DrawTrench(t, isBloomPass);
        }
        

        void DrawTrench(Trench t, bool isBloomPass)
        {
            if (isBloomPass) 
            {
                assignEffectToModel(t.Model, EffectBloom);
                assignEffectToModel(TrenchTurret, EffectBloom);
                EffectBloom.Parameters["ApplyBloom"].SetValue(0f);
                EPbloomTexture.SetValue(TrenchTexture);
                //EffectBloom.CurrentTechnique = EffectBloom.Techniques["BloomPass"];
            }
            EPlightTexture.SetValue(TrenchTexture);
            //EffectLight.Parameters["baseTexture"].SetValue(TrenchTexture);
            //EffectBloom.Parameters["baseTexture"].SetValue(TrenchTexture);

            Matrix world;
            var meshCount = 0;

            foreach (var mesh in t.Model.Meshes)
            {
                
                world = mesh.ParentBone.Transform * t.SRT;

                EPlightWorld.SetValue(world);
                EPlightInverseTransposeWorld.SetValue(Matrix.Transpose(Matrix.Invert(world)));

                //EffectLight.Parameters["World"].SetValue(world);
                //EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                
                var wvp = world * SelectedCamera.View * SelectedCamera.Projection;
                //EffectLight.Parameters["WorldViewProjection"].SetValue(wvp);
                //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp); 
                EPlightWorldViewProjection.SetValue(wvp);
                EPbloomWorldViewProjection.SetValue(wvp);

                mesh.Draw();

                meshCount++;
            }
            foreach(var turret in t.Turrets)
            {
                foreach(var mesh in TrenchTurret.Meshes)
                {
                    world = mesh.ParentBone.Transform * turret.SRT;

                    //EffectLight.Parameters["World"].SetValue(world);
                    //EffectLight.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(world)));
                    EPlightWorld.SetValue(world);
                    EPlightWorldViewProjection.SetValue(Matrix.Transpose(Matrix.Invert(world)));

                    var wvp = world * SelectedCamera.View * SelectedCamera.Projection;
                    //EffectLight.Parameters["WorldViewProjection"].SetValue(wvp);
                    //EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp);
                    EPlightWorldViewProjection.SetValue(wvp);
                    EPbloomWorldViewProjection.SetValue(wvp);
                    mesh.Draw();
                }
                if (ShowGizmos && !isBloomPass)
                {
                    var BB = turret.BoundingBox;
                    Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), BoundingVolumesExtensions.GetExtents(BB) * 2f, Color.Magenta);

                }
            }
            if (ShowGizmos && !isBloomPass)
            {
                var index = 0;
                Color[] colors = { Color.White, Color.Yellow, Color.Blue, Color.Magenta };
                //foreach (var BB in t.boundingBoxes)
                //{
                //    var color = colors[index];

                //    Gizmos.DrawCube(BoundingVolumesExtensions.GetCenter(BB), BoundingVolumesExtensions.GetExtents(BB) * 2f, Xwing.OBB.Intersects(BB) ? Color.Red : color);
                //    index++;
                //}
                Matrix SRT;
                foreach (var OBB in t.boundingBoxes)
                {
                    var color = colors[index];
                    SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                    Gizmos.DrawCube(SRT, Xwing.OBB.Intersects(OBB) ? Color.Red : color);
                    index++;
                }


            }
            if (isBloomPass)
            {
                assignEffectToModel(t.Model, EffectLight);
                assignEffectToModel(TrenchTurret, EffectLight);
            }
        }

        void DrawXWing(bool isBloomPass)
        {
            int meshCount = 0; //Como el xwing tiene 2 texturas, tengo que dibujarlo de esta manera
            
            //efecto para verificar colisiones, se pone rojo
            if (Xwing.hit)
                EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Right * 0.5f);

            DrawModel(Xwing.EnginesModel, Xwing.EnginesSRT, Xwing.EnginesColor, isBloomPass);
            
            if(isBloomPass)
            {
                assignEffectToModel(Xwing.Model, EffectBloom);
                EffectBloom.Parameters["ApplyBloom"].SetValue(0f);
            }
            
            foreach (var mesh in Xwing.Model.Meshes)
            {
                Xwing.World = mesh.ParentBone.Transform * Xwing.SRT;

                EffectTexture.Parameters["World"].SetValue(Xwing.World);
                EffectTexture.Parameters["ModelTexture"].SetValue(Xwing.Textures[meshCount]);
                meshCount++;

                mesh.Draw();
            }

            var sphere = Xwing.boundingSphere;
            //Gizmos.DrawSphere(sphere.Center, new Vector3(sphere.Radius), Color.White);

            //Gizmos.DrawCube(Xwing.Position, new Vector3(20f, 20f, 20f), Xwing.hit? Color.Red : Color.Cyan);
            if (ShowGizmos)
            {
                Gizmos.DrawSphere(Xwing.boundingSphere.Center, Xwing.boundingSphere.Radius * Vector3.One, Color.White);

                var OBB = Xwing.OBB;
                Matrix SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.White);

                OBB = Xwing.OBBL;
                SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Yellow);

                OBB = Xwing.OBBR;
                SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Cyan);

                OBB = Xwing.OBBU;
                SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Magenta);

                OBB = Xwing.OBBD;
                SRT = Matrix.CreateScale(OBB.Extents * 2f) * OBB.Orientation * Matrix.CreateTranslation(OBB.Center);
                Gizmos.DrawCube(SRT, Xwing.hit ? Color.Red : Color.Green);
            }
            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);

            if (isBloomPass)
            {
                assignEffectToModel(Xwing.Model, EffectTexture);
            }
        }

        void DrawTie(TieFighter tie, bool isBloomPass)
        {
            if(isBloomPass)
            {
                assignEffectToModel(Tie, EffectBloom);
                EffectBloom.Parameters["ApplyBloom"].SetValue(0f);
            }
            Matrix world;
            foreach (var mesh in Tie.Meshes)
            {
                world = mesh.ParentBone.Transform * tie.SRT;

                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(TieTexture);
                //EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.One);
                mesh.Draw();
            }
            if(ShowGizmos)
            {
                Gizmos.DrawSphere(tie.boundingSphere.Center, tie.boundingSphere.Radius * Vector3.One, Color.White);
            }
            //EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
            if (isBloomPass)
            {
                assignEffectToModel(Tie, EffectTexture);
            }
        }
        void DrawModel(Model model, Matrix SRT, Vector3 color, bool isBloomPass)
        {
            if (isBloomPass)
            {
                assignEffectToModel(model, EffectBloom);
                EffectBloom.Parameters["ApplyBloom"].SetValue(1f);
                EffectBloom.Parameters["Color"].SetValue(color);
            }
            Effect.Parameters["DiffuseColor"]?.SetValue(color);

            foreach (var mesh in model.Meshes)
            {
                var world = mesh.ParentBone.Transform * SRT;
                var wvp = world * SelectedCamera.View * SelectedCamera.Projection;
                Effect.Parameters["World"].SetValue(world);
                EffectBloom.Parameters["WorldViewProjection"].SetValue(wvp);
                mesh.Draw();
            }

            if (isBloomPass)
                assignEffectToModel(model, Effect);

        }
        void manageEffectParameters()
        {
            EffectTexture.Parameters["ModifierColor"].SetValue(Vector3.Zero);
            EffectTexture.Parameters["TextureMultiplier"].SetValue(1f);


            EffectLight.Parameters["ambientColor"].SetValue(new Vector3(1f, 1f, 1f));
            EffectLight.Parameters["diffuseColor"].SetValue(new Vector3(1f, 1f, 1f));
            EffectLight.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));

            EffectLight.Parameters["KAmbient"].SetValue(0.3f);
            EffectLight.Parameters["KDiffuse"].SetValue(kd);
            EffectLight.Parameters["KSpecular"].SetValue(ks);
            EffectLight.Parameters["shininess"].SetValue(3f);

            EffectBloom.Parameters["enginesColor1"].SetValue(new Vector3(0f, 0.6f, 0.8f));
            EffectBloom.Parameters["enginesColor2"].SetValue(new Vector3(0.7f, 0.15f, 0f));
            EffectBloom.Parameters["laserColor1"].SetValue(new Vector3(0.8f, 0f, 0f));
            EffectBloom.Parameters["laserColor2"].SetValue(new Vector3(0f, 0.8f, 0f));
            EffectBloom.Parameters["laserColor3"].SetValue(new Vector3(0.8f, 0f, 0.8f));


            EPlightWorldViewProjection =    EffectLight.Parameters["WorldViewProjection"];
            EPlightWorld =                  EffectLight.Parameters["World"];
            EPlightInverseTransposeWorld =  EffectLight.Parameters["InverseTransposeWorld"];
            EPlightLightPosition =          EffectLight.Parameters["lightPosition"];
            EPlightEyePosition =            EffectLight.Parameters["eyePosition"];
            EPlightTexture =                EffectLight.Parameters["baseTexture"];

            EPbloomWorldViewProjection =    EffectBloom.Parameters["WorldViewProjection"];
            EPbloomTexture =                EffectBloom.Parameters["baseTexture"];

            EPblurTexture =                 EffectBlur.Parameters["baseTexture"];

            EPbasicView =                   Effect.Parameters["View"];
            EPbasicProjection =             Effect.Parameters["Projection"];
            EPbasicWorld =                  Effect.Parameters["World"];
            EPbasicColor =                  Effect.Parameters["DiffuseColor"];

            EPtextureView =                 EffectTexture.Parameters["View"];
            EPtextureProjection =           EffectTexture.Parameters["Projection"];
            EPtextureWorld =                EffectTexture.Parameters["World"];
            EPtextureColor =                EffectTexture.Parameters["ModifierColor"];
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
            FullScreenQuad.Dispose();
            FirstPassBloomRenderTarget.Dispose();
            MainSceneRenderTarget.Dispose();
            SecondPassBloomRenderTarget.Dispose();
            base.UnloadContent();
        }
    }
    
}