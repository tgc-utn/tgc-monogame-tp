using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;

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
        public float xWingScale = 2.5f;

        public float tieScale = 0.02f;

        public float trenchScale = 0.07f;
        public float trench2Scale = 0.07f;

        public Vector3 trenchTranslation = new Vector3(0, -30, -130);
        public Vector3 trench2Translation = new Vector3(0, -80, -290);
        public Vector3 xWingTranslation = new Vector3(0, -5, -40);


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
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model xWing { get; set; }
        private Model tie { get; set; }
        private Model tie2 { get; set; }
        private Model trench { get; set; }
        private Model trench2 { get; set; }

        private Effect EffectTexture { get; set; }
        private Effect Effect { get; set; }
        private BasicEffect BasicEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix xWingWorld { get; set; }
        private Matrix tieWorld { get; set; }
        private Matrix tie2World { get; set; }
        private Matrix trenchWorld { get; set; }
        private Matrix trench2World { get; set; }

        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        private Texture[] xWingTextures;

        private Texture tieTexture;

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


            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            // Configuramos nuestras matrices de la escena.
            xWingWorld = Matrix.Identity;
            tieWorld = Matrix.Identity;
            tie2World = Matrix.Identity;
            trenchWorld = Matrix.Identity;
            trench2World = Matrix.Identity;

            //View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            //Projection =
            //    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 50000);


            //Camera = new SimpleCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 40, 200), size);

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            Camera = new MyCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f, 0f, 0f), size);

            //Camera = new MyCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f, 0f, 0f), 100, 1.0f, 1,
            //    6000);

            base.Initialize();
        }
        void assignEffectToModels(Model[] models, Effect effect)
        {
            foreach (Model model in models)
                foreach (var mesh in model.Meshes)
                    foreach (var meshPart in mesh.MeshParts)
                        meshPart.Effect = effect;

        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargo el modelo del logo.
            xWing = Content.Load<Model>(ContentFolder3D + "XWing/model");
            tie = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            tie2 = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            trench = Content.Load<Model>(ContentFolder3D + "Trench/Trench");
            trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            EffectTexture = Content.Load<Effect>(ContentFolderEffects + "BasicTexture");

            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.Identity,
                TextureEnabled = false,
            };

            // Cargo Texturas a usar en cada modelo
            xWingTextures = new Texture[] {   Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert6_Base_Color"),
                                             Content.Load<Texture2D>(ContentFolderTextures + "xWing/lambert5_Base_Color") };
            tieTexture = Content.Load<Texture2D>(ContentFolderTextures + "TIE/TIE_IN_Diff");
            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.

            //Model[] models = new Model[] { xWing, tie };//...

            assignEffectToModels(new Model[] { xWing, tie }, EffectTexture);
            assignEffectToModels(new Model[] { trench, trench2 }, Effect);



            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        bool ignoreF11 = false;
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            Camera.Update(gameTime);

            // Capturar Input teclado
            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape))
                Exit();

            if (kState.IsKeyDown(Keys.F11))
            {
                if (!ignoreF11) //evito que se cambie constantemente manteniendo apretada la tecla
                {
                    ignoreF11 = true;

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
            if (kState.IsKeyUp(Keys.F11))
                ignoreF11 = false;
            float scaler = 0.01f;
            if (kState.IsKeyDown(Keys.OemPlus))
            {
                //xWingScale += scaler;
                xWingScale += scaler;
            }
            if (kState.IsKeyDown(Keys.OemMinus))
            {
                //xWingScale -= scaler;
                xWingScale -= scaler;
            }
            if (kState.IsKeyDown(Keys.L))
            {
                xWingTranslation.X += 2;
            }
            if (kState.IsKeyDown(Keys.J))
            {
                xWingTranslation.X -= 2;
            }
            if (kState.IsKeyDown(Keys.I))
            {
                xWingTranslation.Y += 2;
            }
            if (kState.IsKeyDown(Keys.K))
            {
                xWingTranslation.Y -= 2;
            }
            if (kState.IsKeyDown(Keys.Y))
            {
                xWingTranslation.Z += 2;
            }
            if (kState.IsKeyDown(Keys.H))
            {
                xWingTranslation.Z -= 2;
            }


            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>

        float angleBetweenVectors(Vector3 a, Vector3 b)
        {
            return MathF.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
        }
        Vector3 directionalAngles(Vector3 v)
        {
            return new Vector3(
                MathF.Acos(v.X / v.Length()),
                MathF.Acos(v.Y / v.Length()),
                MathF.Acos(v.Z / v.Length()));
        }
        //Vector3 calculateRotation()
        //{
        //    Vector3 rot = new Vector3(0,0,0);

        //    Vector3 angles = directionalAngles(Camera.FrontDirection);

        //    rot.X = angles.X;
        //    rot.Y = angles.Y;
        //    rot.Z = angles.Z;

        //    return rot;
        //}
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            EffectTexture.Parameters["View"].SetValue(Camera.View);
            EffectTexture.Parameters["Projection"].SetValue(Camera.Projection);

            Effect.Parameters["DiffuseColor"]?.SetValue(Color.DarkBlue.ToVector3());
            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            // Calculo la posicion del xwing en base a la posicion de la camara, y a donde esta mirando la camara
            Vector3 pos = Camera.Position + (Camera.FrontDirection*40);

            //debug
            //Matrix SRT = Matrix.CreateScale(3f) * rotationMatrix;
            Matrix SRT = 
                Matrix.CreateScale(xWingScale) *
                //correccion de angulo inicial, correccion por camara
                Matrix.CreateRotationY(-MathF.PI / 2) *
                Matrix.CreateRotationY(-Camera.yaw * MathF.PI / 180) *
                //correccion por camara
                Matrix.CreateRotationX(Camera.pitch * MathF.PI / 180) *
                //correccion de posicion
                Matrix.CreateTranslation(pos);

            DrawXWing(xWingWorld, SRT);

            SRT =
                Matrix.CreateScale(tieScale) *
                Matrix.CreateRotationY(MathF.PI) *
                Matrix.CreateTranslation(new Vector3(40, 0, 0)) *
                rotationMatrix;
            DrawTie(tieWorld, SRT);

           
            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.4f, 0.4f, 0.4f));
            foreach (var mesh in trench.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                trenchWorld = mesh.ParentBone.Transform *
                    Matrix.CreateScale(trenchScale) *
                    Matrix.CreateRotationY(MathF.PI / 2) *
                    Matrix.CreateTranslation(trenchTranslation);

                Effect.Parameters["World"].SetValue(trenchWorld);
                mesh.Draw();
            }
            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.3f, 0.3f, 0.3f));
            foreach (var mesh in trench2.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                trench2World = mesh.ParentBone.Transform *
                    Matrix.CreateScale(trench2Scale) *
                    //Matrix.CreateRotationY(MathF.PI / 2) *
                    Matrix.CreateTranslation(trench2Translation);

                Effect.Parameters["World"].SetValue(trench2World);
                mesh.Draw();
            }

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            //SpriteBatch.DrawString(SpriteFont, "Escala: "+trenchScale, new Vector2(0, 0), Color.White);
            SpriteBatch.DrawString(SpriteFont, "Pos " + Math.Floor(Camera.Position.X) +
                                                "," + Math.Floor(Camera.Position.Y) +
                                                "," + Math.Floor(Camera.Position.Z) +
                                                //" FD " + Camera.FrontDirection.X +
                                                //"," + Camera.FrontDirection.Y +
                                                //"," + Camera.FrontDirection.Z +
                                                //" newP " + Math.Floor(pos.X) +
                                                //"," + Math.Floor(pos.Y) +
                                                //"," + Math.Floor(pos.Z) +
                                                " yaw " + Camera.yaw+
                                                " pitch " + Camera.pitch
                                                
                                                , new Vector2(0, 0), Color.White);
            //SpriteBatch.DrawString(SpriteFont, "X " + xWingTranslation.X + " Y " + xWingTranslation.Y + " Z " + xWingTranslation.Z, new Vector2(0, 30), Color.White);



            SpriteBatch.End();

        }
        void DrawXWing(Matrix world, Matrix SRT)
        {
            int meshCount = 0;
            foreach (var mesh in xWing.Meshes)
            {
                world = mesh.ParentBone.Transform * SRT;
                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(xWingTextures[meshCount]);
                meshCount++;

                mesh.Draw();
            }
        }

        void DrawTie(Matrix world, Matrix SRT)
        {
            foreach (var mesh in tie.Meshes)
            {
                world = mesh.ParentBone.Transform * SRT;

                EffectTexture.Parameters["World"].SetValue(world);
                EffectTexture.Parameters["ModelTexture"].SetValue(tieTexture);
                mesh.Draw();
            }
        }
        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
    }
}