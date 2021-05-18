using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TGC.MonoGame.Samples.Cameras;
using System.Timers;

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

        private SpriteFont SpriteFont;
        Xwing Xwing = new Xwing();
        
        public float TieScale = 0.02f;

        public float TrenchScale = 0.07f;
        public float Trench2Scale = 0.07f;

        public Vector3 TrenchTranslation = new Vector3(0, -30, -130);
        public Vector3 Trench2Translation = new Vector3(0, -80, -290);
        public Vector3 XwingTranslation = new Vector3(0, -5, -40);

        Timer camUpdateTimer;

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
        private Model Trench { get; set; }
        private Model Trench2 { get; set; }
        private Model LaserModel { get; set; }
        private Effect EffectTexture { get; set; }
        private Effect Effect { get; set; }
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
            Camera = new MyCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f, 0f, 0f), size);
            camUpdateTimer = new Timer(5);
            camUpdateTimer.Elapsed += CamUpdateTimerTick;
            camUpdateTimer.AutoReset = true;
            camUpdateTimer.Enabled = true;

            generateEnemies();

            base.Initialize();
        }

        private void CamUpdateTimerTick(object sender, ElapsedEventArgs e)
        {
            if(Camera.MouseLookEnabled)
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
            Trench = Content.Load<Model>(ContentFolder3D + "Trench/Trench");
            Trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            LaserModel = Content.Load<Model>(ContentFolder3D + "Laser/Laser");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectTexture = Content.Load<Effect>(ContentFolderEffects + "BasicTexture");

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
            //Asigno los efectos a los modelos correspondientes
            assignEffectToModels(new Model[] { Xwing.Model, Tie, Trench }, EffectTexture);
            assignEffectToModels(new Model[] { Trench2, LaserModel }, Effect);

            //Para escribir en la pantalla
            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");

                
            for (var i = 0; i < 5; i++)
            { 
                Matrix SRT = Matrix.CreateScale(TrenchScale) *
                    Matrix.CreateRotationY(MathF.PI / 2) *
                    Matrix.CreateTranslation(TrenchTranslation = new Vector3(0, 0, -i * 170));

                trenches.Add(SRT);
            }
            List<Matrix> rotation = new List<Matrix>() ;
            foreach (var t in trenches)
                rotation.Add(t);
            
            foreach(var t in rotation)
            {
                trenches.Add(t * Matrix.CreateTranslation(new Vector3(170, 0, 0)));
            }
                    
            base.LoadContent();
        }
        
        List<Keys> ignoredKeys = new List<Keys>();
        List<Matrix> trenches = new List<Matrix>();
        void generateEnemies()
        {
            Random rnd = new Random();
            if (enemies.Count < 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 random = new Vector3(rnd.Next(-1000, 1000), 0f, rnd.Next(-1000, 1000));
                    Vector3 pos = Xwing.Position + random;
                    Vector3 dir = Vector3.Normalize(Xwing.Position - pos);


                    Matrix SRT = Matrix.CreateScale(TieScale) * Matrix.CreateTranslation(pos);
                    enemies.Add(new TieFighter(pos, dir, Matrix.Identity, SRT, TieScale));
                }
      
            }
        }
        void updateEnemies(float time)
        {
            foreach(var enemy in enemies)
            {
                enemy.Update(Xwing, time);
                enemy.fireLaser();
            }

            //System.Diagnostics.Debug.WriteLine(
            //    "P " +
            //    enemies[0].Pitch+
            //    " Y " +
            //    enemies[0].Yaw);
            
        }


        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Camera.Update(gameTime);

            // Calculo la posicion del xwing en base a la posicion de la camara, y a donde esta mirando la camara
            // para que siempre quede en frente de la camara

            //obtengo la posicion de la camara, y la muevo para adelante 40 unidades
            

            Xwing.Update(elapsedTime, Camera);
            
            updateEnemies(elapsedTime);
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
            if(kState.IsKeyDown(Keys.F))
            {
                Xwing.fireLaser();
            }
            if(kState.IsKeyDown(Keys.V))
            {
                if(!ignoredKeys.Contains(Keys.V))
                {
                    ignoredKeys.Add(Keys.V);
                    IsFixedTimeStep = !IsFixedTimeStep;
                }
            }
            if (kState.IsKeyDown(Keys.F11))
            {
                //evito que se cambie constantemente manteniendo apretada la tecla
                if(!ignoredKeys.Contains(Keys.F11))
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
                if(!ignoredKeys.Contains(Keys.M))
                {
                    ignoredKeys.Add(Keys.M);
                    if (!Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                    {
                        Camera.MouseLookEnabled = true;
                        //correccion inicial para que no salte a un punto cualquiera
                        Camera.pastMousePosition = Mouse.GetState().Position.ToVector2(); 
                        IsMouseVisible = false;
                    }
                    else if(Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
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
            if(kState.IsKeyDown(Keys.R))
            {
                if(!ignoredKeys.Contains(Keys.R))
                {
                    ignoredKeys.Add(Keys.R);
                    Xwing.barrelRolling = true;
                }
            }
            //remuevo de la lista aquellas teclas que solte
            ignoredKeys.RemoveAll(kState.IsKeyUp);

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            //Configuro efectos
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            EffectTexture.Parameters["View"].SetValue(Camera.View);
            EffectTexture.Parameters["Projection"].SetValue(Camera.Projection);

            var rotationMatrix = Matrix.CreateRotationY(Rotation);
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            int fps = (int)Math.Round(1 / deltaTime);
            
            //SRT contiene la matriz final que queremos aplicarle al modelo al dibujarlo
            DrawXWing(Xwing.SRT);

            //debug de Tie, rotando 
            Matrix SRT =
                Matrix.CreateScale(TieScale) *
                Matrix.CreateRotationY(MathF.PI) *
                Matrix.CreateTranslation(new Vector3(40, 0, 0)) *
                rotationMatrix;
            //DrawTie(TieWorld, SRT);


            foreach (var srt in trenches)
            {
                DrawTrench(Matrix.Identity, srt);
            }


            foreach (var laser in Xwing.fired)
            {
                laser.Update(deltaTime);
                DrawModel(LaserModel, Xwing.World, laser.SRT, laser.Color);
            }
            foreach(var enemy in enemies)
                foreach (var laser in enemy.fired)
                {
                    laser.Update(deltaTime);
                    DrawModel(LaserModel, Xwing.World, laser.SRT, laser.Color);
                }
            foreach (var enemy in enemies)
            {
                DrawTie(enemy);
            }

            SRT =
                    Matrix.CreateScale(Trench2Scale) *
                    //Matrix.CreateRotationY(MathF.PI / 2) *
                    Matrix.CreateTranslation(Trench2Translation);
            //Este si se muestra bien
            //DrawModel(Trench2, Trench2World, SRT, new Vector3(0.4f, 0.4f, 0.4f));

            if (!Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                mensaje = mensaje1;
            else if (Camera.MouseLookEnabled && Camera.ArrowsLookEnabled)
                mensaje = mensaje2;
            else if (Camera.MouseLookEnabled && !Camera.ArrowsLookEnabled)
                mensaje = mensaje3;

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            //SpriteBatch.DrawString(SpriteFont, Xwing.barrelRolling + " " + Xwing.barrelRollStep, Vector2.Zero, Color.White);
            //SpriteBatch.DrawString(SpriteFont,
            //    "C " + Math.Round(Camera.FrontDirection.X, 2) +
            //    "|" + Math.Round(Camera.FrontDirection.Y, 2) +
            //    "|" + Math.Round(Camera.FrontDirection.Z, 2) +
            //    "  up " + Math.Round(Camera.UpDirection.X, 2) +
            //    "|" + Math.Round(Camera.UpDirection.Y, 2) +
            //    "|" + Math.Round(Camera.UpDirection.Z, 2) +

            //    "   P " + Math.Round(pos.X, 2) +
            //    "|" + Math.Round(pos.Y, 2) +
            //    "|" + Math.Round(pos.Z, 2) +
            //    "   UPX " + Math.Round(mxQuat.Up.X, 2) +
            //    "|" + Math.Round(mxQuat.Up.Y, 2) +
            //    "|" + Math.Round(mxQuat.Up.Z, 2) +
            //    " Roll " + Math.Round(Xwing.Roll, 2)
            //    , Vector2.Zero, Color.White); 
            SpriteBatch.DrawString(SpriteFont, "FPS "+fps, Vector2.Zero, Color.White);
            SpriteBatch.End();

            var center= GraphicsDevice.Viewport.Bounds.Size;
            center.X /= 2;
            center.Y /= 2;

            var scale = 0.1f;
            var sz = 512 * scale;
            
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            SpriteBatch.Draw(Crosshairs[0], new Vector2(center.X - sz / 2, center.Y - sz / 2), null, Color.White, 0f, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
            SpriteBatch.End();
            //debug
            //SpriteBatch.DrawString(SpriteFont, 
            //                                    " yaw " + Math.Round(Camera.Yaw, 2) +
            //                                    " pitch " + Math.Round(Camera.Pitch, 2)
            //                                    , new Vector2(0, 0), Color.White);
            //SpriteBatch.End();


        }
        void DrawXWing(Matrix SRT)
        {
            int meshCount = 0; //Como el xwing tiene 2 texturas, tengo que dibujarlo de esta manera
            foreach (var mesh in Xwing.Model.Meshes)
            {
                Xwing.World = mesh.ParentBone.Transform * SRT;
                EffectTexture.Parameters["World"].SetValue(Xwing.World);
                EffectTexture.Parameters["ModelTexture"].SetValue(Xwing.Textures[meshCount]);
                meshCount++;

                mesh.Draw();
            }
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
        void DrawTrench(Matrix world, Matrix SRT)
        {
            foreach (var mesh in Trench.Meshes)
            {
                world = mesh.ParentBone.Transform * SRT;

                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(TrenchTexture);
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
    }
}