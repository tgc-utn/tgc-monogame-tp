using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Objects;

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

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        public float ElapsedTime;
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Model island { get; set; }

        private Model Rock { get; set; }
        private Model Barco { get; set; }
        private Vector3 BarcoPositionCenter = new Vector3(-200f, 50, 0);
        private Model Barco2 { get; set; }
        private Model Barco3 { get; set; }
        private float position { get; set; }
        private Model Projektil { get; set; }

        private Model Projektil2 { get; set; }
        private Model Terreno2 { get; set; }
        private Effect Effect { get; set; }
        private Effect WaterEffect { get; set; }
        private float Rotation { get; set; }
        private Model islandTwo { get; set; }
        private Model islandThree { get; set; }
        private Model[] islands { get; set; }

        private Vector3[] posicionesIslas;

        private int cantIslas;
        private Model WaterModel { get; set; }
        public Matrix World { get; set; }
        private TargetCamera targetCamera { get; set; }
        public Camera Camera { get; set; }
        private float time;
        private MainShip MainShip;
        private Model Ocean { get; set; }

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
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.CreateRotationY(MathHelper.Pi);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            MainShip = new MainShip(BarcoPositionCenter, new Vector3(0,0,0), 10, this );
            //targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(BarcoPositionCenter.X, BarcoPositionCenter.Y + 150, BarcoPositionCenter.Z - 250), BarcoPositionCenter, screenSize, (float)(GraphicsDevice.Viewport.Height), (float)(GraphicsDevice.Viewport.Width));
            Camera = new BuilderCamaras(GraphicsDevice.Viewport.AspectRatio , screenSize, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, MainShip);
            
            posicionesIslas = new[] { new Vector3(-3000f, -60f, 200f) ,new Vector3(2000f,-60f,400f),new Vector3(1500f,-60f,200f), new Vector3(-4500f,-60f,-600f),new Vector3(-2000f,-60f,-1500f),
                new Vector3(4000f,-60f,-1500f),new Vector3(500f,-60f,-3000f),new Vector3(0,-60f,-4000f), new Vector3 (-2000f,-60f,0)};

            cantIslas = posicionesIslas.Length;
            time = 0;
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
            MainShip.LoadContent();
            Model = Content.Load<Model>(ContentFolder3D + "WarVessel/1124");
            island = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            Barco = Content.Load<Model>(ContentFolder3D + "Barco");
            Barco2 = Content.Load<Model>(ContentFolder3D + "Barco2");
            Barco3 = Content.Load<Model>(ContentFolder3D + "Barco3");
            Projektil = Content.Load<Model>(ContentFolder3D + "projektil FBX");
            Terreno2 = Content.Load<Model>(ContentFolder3D + "FBX");
            Projektil2 = Content.Load<Model>(ContentFolder3D + "9x18 pm");
            Rock = Content.Load<Model>(ContentFolder3D + "RockSet06-A");
            islandTwo = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            Ocean = Content.Load<Model>(ContentFolder3D + "cube");
            islands = new Model[cantIslas];
            //WaterModel = Content.Load<Model>(ContentFolder3D + "water");
            for (int isla = 0; isla < cantIslas; isla++)
            {
                islands[isla] = Content.Load<Model>(ContentFolder3D + "islands/isla" + (isla + 1));
            }
            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            //var modelEffect = (BasicEffect)Model.Meshes[0].Effects[0];
            //WaterEffect = Content.Load<Effect>(ContentFolderEffects + "WaterShader");
            //Ocean =  Content.Load<Effect>(ContentFolderEffects + "WaterShader");
            /*WaterEffect.Parameters["KAmbient"]?.SetValue(0.15f);
            WaterEffect.Parameters["KDiffuse"]?.SetValue(0.75f);
            WaterEffect.Parameters["KSpecular"]?.SetValue(1f);
            WaterEffect.Parameters["Shininess"]?.SetValue(10f);

            WaterEffect.Parameters["AmbientColor"]?.SetValue(new Vector3(1f, 0.98f, 0.98f));
            WaterEffect.Parameters["DiffuseColor"]?.SetValue(new Vector3(0.0f, 0.5f, 0.7f));
            WaterEffect.Parameters["SpecularColor"]?.SetValue(new Vector3(1f, 1f, 1f));*/

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            ElapsedTime += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            
            MainShip.Update(gameTime);
            Camera.Update(gameTime);
            //targetCamera.UpdatePosition(gameTime,MainShip.Position);
            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            // Basado en el tiempo que paso se va generando una rotacion.
            //Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Model.Draw(World * Matrix.CreateTranslation(120, 25, 0), Camera.View, Camera.Projection);
            island.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(1000, 0, 0), Camera.View, Camera.Projection);
            Barco3.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-100f, 0, 0), Camera.View, Camera.Projection);
            Ocean.Draw(Matrix.CreateTranslation(BarcoPositionCenter.X, BarcoPositionCenter.Y+60,BarcoPositionCenter.Z),Camera.View, Camera.Projection);
            
            
            //BARCO PRINCIPAL---------------------------------------
            //Barco.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(BarcoPositionCenter), Camera.View, Camera.Projection);
            MainShip.Draw();
            //BARCO PRINCIPAL---------------------------------------
            Terreno2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(-550, 50, 0), Camera.View, Camera.Projection);
            Rock.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-800, 20, 0), Camera.View, Camera.Projection);

            island.Draw(World * Matrix.CreateTranslation(200f, -60f, -600), Camera.View, Camera.Projection);
            islandTwo.Draw(World * Matrix.CreateTranslation(-900f, -60f, -1000f), Camera.View, Camera.Projection);
            for (int isla = 0; isla < cantIslas; isla++)
            {
                islands[isla].Draw(World * Matrix.CreateScale(500f) * Matrix.CreateTranslation(posicionesIslas[isla]), Camera.View, Camera.Projection);
            }
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            /*var waterMesh = WaterModel.Meshes[0];
            if (waterMesh != null)
            {
                var part = waterMesh.MeshParts[0];
                part.Effect = WaterEffect;
                WaterEffect.Parameters["World"].SetValue(World);
                WaterEffect.Parameters["View"].SetValue(targetCamera.View);
                WaterEffect.Parameters["Projection"].SetValue(targetCamera.Projection);
                WaterEffect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(World)));
                WaterEffect.Parameters["Time"]?.SetValue(ElapsedTime);
                WaterEffect.Parameters["CameraPosition"]?.SetValue(targetCamera.Position);
                waterMesh.Draw();
            }*/

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