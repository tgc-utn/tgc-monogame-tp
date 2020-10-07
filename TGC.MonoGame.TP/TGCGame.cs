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
        public const string ContentFolderEffect = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        
        private float time;
        private Vector3 boatPosition;
        private Matrix WaterMatrix;
        private Vector3 boatOriginForward;
        private Vector3 boatActualForward;
        private Vector3 boatTargetForward;
        private float fowardProgress;
        private Vector3 waterNormal;

        private float timeMultiplier = 1f;

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
        private Model Model { get; set; }
        private Model Model2 { get; set; }
        private Model Model3 { get; set; }
        private Model Model4 { get; set; }

        private Camera Camera;

        private Effect WaterEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();
            
            // Configuramos nuestras matrices de la escena.
            World = Matrix.CreateRotationY(MathHelper.Pi);
           // View = Matrix.CreateLookAt(Vector3.UnitZ * 500 + Vector3.Up * 150, Vector3.Zero, Vector3.Up);
           // Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 1000);

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-350, 50, 400), screenSize);
            
            boatPosition = new Vector3(120,25,0);
            WaterMatrix = Matrix.Identity;
            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargo el modelo del logo.
           Model = Content.Load<Model>(ContentFolder3D + "t-22/T-22");
           //Model = Content.Load<Model>(ContentFolder3D + "axis");
            Model2 = Content.Load<Model>(ContentFolder3D + "nagato/Nagato");
            Model3 = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            Model4 = Content.Load<Model>(ContentFolder3D + "water");
            // Obtengo su efecto para cambiarle el color y activar la luz predeterminada que tiene MonoGame.
            var modelEffect = (BasicEffect) Model.Meshes[0].Effects[0];
            modelEffect.DiffuseColor = Color.DarkBlue.ToVector3();
            modelEffect.EnableDefaultLighting();

            WaterEffect = Content.Load<Effect>(ContentFolderEffect + "WaterShader");
            
            WaterEffect.Parameters["KAmbient"]?.SetValue(0.15f);
            WaterEffect.Parameters["KDiffuse"]?.SetValue(0.75f);
            WaterEffect.Parameters["KSpecular"]?.SetValue(1f);
            WaterEffect.Parameters["Shininess"]?.SetValue(10f);
            
            WaterEffect.Parameters["AmbientColor"]?.SetValue(new Vector3(1f, 0.98f, 0.98f));
            WaterEffect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.0f, 0.5f, 0.7f));
            WaterEffect.Parameters["SpecularColor"]?.SetValue(new Vector3(1f, 1f, 1f));

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            
            // Capturar Input teclado
            Camera.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 0.5f * timeMultiplier;

            //   boatPosition.X -= Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 50f; 

            float waveFrequency = 2;
            float waveAmplitude = 25;
            float waveAmplitude2 = 25;
            
           // boatPosition.Z -= Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 50f;
            
            //var newY = (MathF.Sin(boatPosition.X*waveFrequency + time) + MathF.Sin(boatPosition.Z*waveFrequency + time))*waveAmplitude + MathF.Sin(boatPosition.X + boatPosition.Z + time)*waveAmplitude2;

            var tangent1 = Vector3.Normalize(new Vector3(0, 
                (MathF.Cos(boatPosition.X*waveFrequency+time)*waveFrequency*waveAmplitude + MathF.Cos(boatPosition.X + boatPosition.Z + time)*waveAmplitude2) * 0.005f
                ,1));
            var tangent2 = Vector3.Normalize(new Vector3(1, 
                (MathF.Cos(boatPosition.Z*waveFrequency+time)*waveFrequency*waveAmplitude + MathF.Cos(boatPosition.X + boatPosition.Z + time)*waveAmplitude2) * 0.005f
                ,0));
            
           // boatPosition.Y = newY - 10;

            var waterNormal = Vector3.Normalize(Vector3.Cross(tangent1, tangent2));

           // WaterMatrix = Matrix.CreateLookAt(Vector3.Zero,tangent1 , waterNormal);
            
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);
            //Finalmente invocamos al draw del modelo.
            //Model.Draw(World * Matrix.CreateRotationY(Rotation), View, Projection);
             Model.Draw(World * WaterMatrix * Matrix.CreateTranslation(boatPosition), Camera.View, Camera.Projection);
            //Model.Draw(World * WaterMatrix, Camera.View, Camera.Projection);
           // Model2.Draw(World * Matrix.CreateTranslation(-120, 20, 0), Camera.View, Camera.Projection);
            //Model3.Draw(World * Matrix.CreateTranslation(0, 0, 0), Camera.View, Camera.Projection);
            //Model4.Draw(World * Matrix.CreateTranslation(0, 45, 0), Camera.View, Camera.Projection);
            var waterMesh = Model4.Meshes[0];
            
            if (waterMesh != null)
           {
                   var part = waterMesh.MeshParts[0];
                   part.Effect = WaterEffect;
                   WaterEffect.Parameters["World"].SetValue(waterMesh.ParentBone.Transform);
                   WaterEffect.Parameters["View"].SetValue(Camera.View);
                   WaterEffect.Parameters["Projection"].SetValue(Camera.Projection);
                   WaterEffect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(World)));
                   WaterEffect.Parameters["Time"]?.SetValue(time);
                   WaterEffect.Parameters["CameraPosition"]?.SetValue(Camera.Position);
                   //Effect.Parameters["WorldViewProjection"].SetValue(Camera.WorldMatrix * Camera.View * Camera.Projection);
                   //Effect.Parameters["ModelTexture"].SetValue(Texture);
                 //  Effect.Parameters["Time"]?.SetValue(time);
                   waterMesh.Draw();
           }

            base.Draw(gameTime);
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