using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Environment;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
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
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Effect Effect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        


        // Camera to draw the scene
        private FreeCamera Camera { get; set; }

        private Terrain Terrain { get; set; }
        private Terrain Terrain2 { get; set; }

        private List<GeometricPrimitive> Track;

        private float SquareSize = 50f;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.UnitZ * 150f, size);

            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.
            
            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

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
            //Model = Content.Load<Model>(ContentFolder3D + "wooden-floor/Floor");
            

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            //foreach (var mesh in Model.Meshes)
            //{
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            //  foreach (var meshPart in mesh.MeshParts)
            //{
            //  meshPart.Effect = Effect;
            //}
            //}

            //var floorTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/tiling-base");

            // Terrain = new Terrain(GraphicsDevice, SquareSize, Color.BlueViolet, 4, 4);
            // Terrain2 = new Terrain(GraphicsDevice, SquareSize, Color.Coral, 2, 2, new Vector3(50, 20, 3));

            RampPrimitive Ramp;
            CubePrimitive Cube;
            Track = new List<GeometricPrimitive>();

            // PRIMERA PLATAFORMA
            // primera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 0));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 0));
            Track.Add(Cube);

            // segunda linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 25));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 25));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 25));
            Track.Add(Cube);

            // tercera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 50));
            Track.Add(Cube);

            // cuarte linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 75));
            Track.Add(Cube);

            // quinta linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 100));
            Track.Add(Cube);

            // sexta linea 
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 125));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 125));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 125));
            Track.Add(Cube);

            // SEGUNDA PLATAFORMA
            // primera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 175));
            Track.Add(Cube);

            // segunda linda
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 200));
            Track.Add(Cube);

            // tercera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 225));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 225));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 225));
            Track.Add(Cube);

            // ESCALERA 
            // primer escalon 
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 225));
            Track.Add(Cube);

            // segundo escalon
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 225));
            Track.Add(Cube);

            // tercer escalon
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 225));
            Track.Add(Cube);

            // TERCERA PLATAFORMA
            // primera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 225));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 225));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 225));
            Track.Add(Cube);

            // segunda linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 200));
            Track.Add(Cube);

            // tercera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 175));
            Track.Add(Cube);

            //CUARTA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 125));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 25));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 0));
            Track.Add(Cube);

            // QUINTA PLATAFORMA
            // primera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -50));
            Track.Add(Cube);

            // segunda linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -75));
            Track.Add(Cube);

            // tercera linea
            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -100));
            Track.Add(Cube);

            // rampa
            Ramp = new RampPrimitive(GraphicsDevice, 25f, Color.Red);
            Ramp.Effect.World = Matrix.CreateTranslation(new Vector3(150, 150, -100));
            Track.Add(Ramp);


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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            
            // Basado en el tiempo que paso se va generando una rotacion.
            //Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            //World = Matrix.CreateRotationY(Rotation);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.DarkBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());

            //Terrain.Draw(Camera.View, Camera.Projection);
            //Terrain2.Draw(Camera.View, Camera.Projection);


            foreach (GeometricPrimitive primitive in Track)
            {
                primitive.Draw(primitive.Effect.World, Camera.View, Camera.Projection);
            }

            /* foreach (var mesh in Model.Meshes)
             {
                 Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
                 mesh.Draw();
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