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
        private Vector3 BarcoPositionCenter = new Vector3(-200f, -10, 0);
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
        private Water ocean { get; set; }
        public Matrix World { get; set; }
        public Camera Camera { get; set; }
        private float time;
        private MainShip MainShip;

        /* LO DE MASTER NO FUNCIONA LO DE SHIPS :( -------------
        private Ship ShipOne { get; set; }
        private Ship ShipTwo { get; set; }
        private Ship ShipThree { get; set; }
        private Ship ShipFour { get; set; }
        private Ship ShipFive { get; set; }*/



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
            //View = Matrix.CreateLookAt(Vector3.UnitZ * 500 + Vector3.Up * 150, Vector3.Zero, Vector3.Up);
            //Projection =
            //    Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            MainShip = new MainShip(BarcoPositionCenter, new Vector3(0,0,0), 10, this );
            //targetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(BarcoPositionCenter.X, BarcoPositionCenter.Y + 150, BarcoPositionCenter.Z - 250), BarcoPositionCenter, screenSize, (float)(GraphicsDevice.Viewport.Height), (float)(GraphicsDevice.Viewport.Width));
            Camera = new BuilderCamaras(GraphicsDevice.Viewport.AspectRatio , screenSize, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, MainShip);
            
            
            /*
            ShipOne = new Ship();
            ShipTwo = new Ship();
            ShipThree = new Ship();
            ShipFour = new Ship();
            ShipFive = new Ship();*/

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
            // Cargo el modelo del logo.
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
            //islandThree = Content.Load<Model>(ContentFolder3D + "islands/isla7");
            ocean = new Water(Content);
            islands = new Model[cantIslas];
            for (int isla = 0; isla < cantIslas; isla++)
            {
                islands[isla] = Content.Load<Model>(ContentFolder3D + "islands/isla" + (isla + 1));
            }
            // ModelShipOne = Content.Load<Model>(ContentFolder3D + "Antisubmarine1124/source/1124");
            // ModelShipTwo = Content.Load<Model>(ContentFolder3D + "Pensacola/source/full");

            // loading warships initial positions
            /*
            ShipOne.LoadContent(Content, ContentFolder3D + "Pensacola/source/full", 0f, 0.1f);
            ShipTwo.LoadContent(Content, ContentFolder3D + "WarVessel/1124", -400f, 2f);
            ShipThree.LoadContent(Content, ContentFolder3D + "Antisubmarine1124/source/1124", new Vector3(-350, 0, -400), 0.2f);
            ShipFour.LoadContent(Content, ContentFolder3D + "Bismark/source/full", -900f, 0.05f);
            ShipFive.LoadContent(Content, ContentFolder3D + "Battleship/source/BB", 500f, 0.015f);

            //set rotations
            ShipTwo.Rotate(-10f);
            ShipThree.Rotate(10f);
            ShipFour.Rotate(10f);
            */


            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            //var modelEffect = (BasicEffect)Model.Meshes[0].Effects[0];
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
            //Effect.Parameters["View"].SetValue(View);
            //Effect.Parameters["Projection"].SetValue(Projection);

            Model.Draw(World * Matrix.CreateTranslation(120, 25, 0), Camera.View, Camera.Projection);
            island.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(1000, 0, 0), Camera.View, Camera.Projection);
            //Barco2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(-10f, 0, 0), Camera.View, Camera.Projection);
            Barco3.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-100f, 0, 0), Camera.View, Camera.Projection);
            
            
            //BARCO PRINCIPAL---------------------------------------
            //Barco.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(BarcoPositionCenter), Camera.View, Camera.Projection);
            MainShip.Draw();
            //BARCO PRINCIPAL---------------------------------------
            
            
            
            //Projektil.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(5f) * Matrix.CreateTranslation(-400f, 100, 0), Camera.View, Camera.Projection);
            Terreno2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(-550, 50, 0), Camera.View, Camera.Projection);
            //Projektil2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2) * Matrix.CreateTranslation(-650, 100, 0), Camera.View, Camera.Projection);
            Rock.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-800, 20, 0), Camera.View, Camera.Projection);
            //Model.Draw(World * Matrix.CreateTranslation(120, 25, 0), Camera.View, Camera.Projection);
            // ModelShipOne.Draw(Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(250, 25, 0), Camera.View, Camera.Projection);
            
            /*LO DE MASTER NO FUNCIONA LO DE SHIPS :( -------------
            ShipOne.Draw(Camera);
            ShipTwo.Draw(Camera);
            ShipThree.Draw(Camera);
            ShipFour.Draw(Camera);
            ShipFive.Draw(Camera);*/

            island.Draw(World * Matrix.CreateTranslation(200f, -60f, -600), Camera.View, Camera.Projection);
            islandTwo.Draw(World * Matrix.CreateTranslation(-900f, -60f, -1000f), Camera.View, Camera.Projection);
            //islandThree.Draw(World * Matrix.CreateScale(500f) * Matrix.CreateTranslation(-3000f,-60f,200f), Camera.View, Camera.Projection);
            for (int isla = 0; isla < cantIslas; isla++)
            {
                islands[isla].Draw(World * Matrix.CreateScale(500f) * Matrix.CreateTranslation(posicionesIslas[isla]), Camera.View, Camera.Projection);
            }
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            //ocean.Draw(World * Matrix.CreateTranslation(0, -60f, 0), Camera.View, Camera.Projection);
            ocean.Draw(gameTime, Camera.View, Camera.Projection, this);
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