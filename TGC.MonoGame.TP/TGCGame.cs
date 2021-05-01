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
        public float xWingScale = 4f;
        public float tieScale = 0.02f;
        public float trenchScale = 0.07f;
        public float trench2Scale = 0.07f;

        public Vector3 trenchTranslation = new Vector3(0, -30, -130);
        public Vector3 trench2Translation = new Vector3(0, -80, -290);
        public Vector3 xWingTranslation = new Vector3(0,0,0);


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

        private Effect Effect { get; set; }
        private float Rotation { get; set; }
        private Matrix xWingWorld { get; set; }
        private Matrix tieWorld { get; set; }
        private Matrix tie2World { get; set; }
        private Matrix trenchWorld { get; set; }
        private Matrix trench2World { get; set; }

        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        private Camera Camera { get; set; }
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
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            xWingWorld = Matrix.Identity;
            tieWorld = Matrix.Identity;
            tie2World = Matrix.Identity;
            trenchWorld = Matrix.Identity;
            trench2World = Matrix.Identity;

            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 40, 200), size);

            base.Initialize();
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
            xWing = Content.Load<Model>(ContentFolder3D+"XWing/model");
            tie = Content.Load<Model>(ContentFolder3D + "TIE/TIE");
            tie2 = Content.Load<Model>(ContentFolder3D + "TIE2/TIE");
            trench = Content.Load<Model>(ContentFolder3D + "Trench/Trench");
            trench2 = Content.Load<Model>(ContentFolder3D + "Trench2/Trench");
            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            foreach (var mesh in xWing.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            foreach (var mesh in tie.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            
            foreach (var mesh in tie2.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            
            foreach (var mesh in trench.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            foreach (var mesh in trench2.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            SpriteFont = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            Camera.Update(gameTime);
            
            // Capturar Input teclado
            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape))
                Exit();

            float scaler = 0.01f;
            if(kState.IsKeyDown(Keys.OemPlus))
            {
                //xWingScale += scaler;
                trench2Scale += scaler;
            }
            if (kState.IsKeyDown(Keys.OemMinus))
            {
                //xWingScale -= scaler;
                trench2Scale -= scaler;
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
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);
            
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0f, 1f, 0f));
            foreach (var mesh in xWing.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                xWingWorld = mesh.ParentBone.Transform * Matrix.CreateScale(xWingScale) * Matrix.CreateTranslation(xWingTranslation) ;
                //xWingWorld = mesh.ParentBone.Transform * Matrix.CreateScale(3f)  * rotationMatrix;

                Effect.Parameters["World"].SetValue(xWingWorld);
                
                mesh.Draw();
            }
            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.5f, 0f, 0f));
            foreach (var mesh in tie.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                //tieWorld = mesh.ParentBone.Transform * Matrix.CreateScale(tieScale) * Matrix.CreateTranslation(xWingTranslation);
                tieWorld = mesh.ParentBone.Transform * Matrix.CreateScale(tieScale);

                Effect.Parameters["World"].SetValue(tieWorld);
                mesh.Draw();
            }

            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.5f, 0f, 0f));
            foreach (var mesh in tie2.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                //tieWorld = mesh.ParentBone.Transform * Matrix.CreateScale(tieScale) * Matrix.CreateTranslation(xWingTranslation);
                tieWorld = mesh.ParentBone.Transform * Matrix.CreateScale(tieScale);

                Effect.Parameters["World"].SetValue(tie2World);
                mesh.Draw();
            }
            Effect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.4f, 0.4f, 0.4f));

            foreach (var mesh in trench.Meshes)
            {
                //World = mesh.ParentBone.Transform * rotationMatrix;
                trenchWorld = mesh.ParentBone.Transform * 
                    Matrix.CreateScale(trenchScale) * 
                    Matrix.CreateRotationY(MathF.PI/2)* 
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
            SpriteBatch.DrawString(SpriteFont, "Escala: "+trenchScale, new Vector2(0, 0), Color.White);
            SpriteBatch.DrawString(SpriteFont, "X " + xWingTranslation.X + " Y " + xWingTranslation.Y + " Z " + xWingTranslation.Z, new Vector2(0, 30), Color.White);



            SpriteBatch.End();

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