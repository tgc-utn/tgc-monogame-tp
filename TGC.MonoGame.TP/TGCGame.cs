using System;
using System.Collections.Generic;
using BepuPhysics.Collidables;
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

        private CubePrimitive Cube1;

        // ---------------------------------------

        // primera plataforma
        private CubePrimitive Cube2;
        private CubePrimitive Cube3;
        private CubePrimitive Cube4;
        private CubePrimitive Cube5;
        private CubePrimitive Cube6;
        private CubePrimitive Cube7;
        private CubePrimitive Cube8;
        private CubePrimitive Cube9;
        private CubePrimitive Cube10;
        private CubePrimitive Cube11;
        private CubePrimitive Cube12;
        private CubePrimitive Cube13;
        private CubePrimitive Cube14;
        private CubePrimitive Cube15;
        private CubePrimitive Cube16;
        private CubePrimitive Cube17;
        private CubePrimitive Cube18;

        // segunda plataforma
        private CubePrimitive Cube19;
        private CubePrimitive Cube20;
        private CubePrimitive Cube21;
        private CubePrimitive Cube22;
        private CubePrimitive Cube23;
        private CubePrimitive Cube24;
        private CubePrimitive Cube25;
        private CubePrimitive Cube26;
        private CubePrimitive Cube27;

        // escalera
        private CubePrimitive Cube28;
        private CubePrimitive Cube29;
        private CubePrimitive Cube30;
        private CubePrimitive Cube31;
        private CubePrimitive Cube32;
        private CubePrimitive Cube33;
        private CubePrimitive Cube34;
        private CubePrimitive Cube35;
        private CubePrimitive Cube36;

        // tercera plataforma
        private CubePrimitive Cube37;
        private CubePrimitive Cube38;
        private CubePrimitive Cube39;
        private CubePrimitive Cube40;
        private CubePrimitive Cube41;
        private CubePrimitive Cube42;
        private CubePrimitive Cube43;
        private CubePrimitive Cube44;
        private CubePrimitive Cube45;

        // cuarta plataforma
        private CubePrimitive Cube46;
        private CubePrimitive Cube47;
        private CubePrimitive Cube48;
        private CubePrimitive Cube49;
        private CubePrimitive Cube50;
        private CubePrimitive Cube51;

        // quinta plataforma
        private CubePrimitive Cube52;
        private CubePrimitive Cube53;
        private CubePrimitive Cube54;
        private CubePrimitive Cube55;
        private CubePrimitive Cube56;
        private CubePrimitive Cube57;
        private CubePrimitive Cube58;
        private CubePrimitive Cube59;
        private CubePrimitive Cube60;

        // BOLITA

        private SpherePrimitive Bola;


        // ---------------------------------------

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

            // PRIMERA PLATAFORMA
            // primera linea
            Cube1 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube1.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));

            Cube2 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube2.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 0));

            Cube3 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube3.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 0));

            // segunda linea
            Cube4 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube4.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 25));

            Cube5 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube5.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 25));

            Cube6 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube6.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 25));

            // tercera linea
            Cube7 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube7.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 50));

            Cube8 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube8.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 50));

            Cube9 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube9.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 50));

            // cuarte linea
            Cube10 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube10.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 75));

            Cube11 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube11.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 75));

            Cube12 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube12.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 75));

            // quinta linea
            Cube13 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube13.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 100));

            Cube14 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube14.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 100));

            Cube15 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube15.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 100));

            // sexta linea 
            Cube16 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube16.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 125));

            Cube17 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube17.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 125));

            Cube18 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube18.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 125));

            // SEGUNDA PLATAFORMA
            // primera linea
            Cube19 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube19.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 175));

            Cube20 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube20.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 175));

            Cube21 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube21.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 175));

            // segunda linda
            Cube22 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube22.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 200));

            Cube23 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube23.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 200));

            Cube24 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube24.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 200));

            // tercera linea
            Cube25 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube25.Effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 225));

            Cube26 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube26.Effect.World = Matrix.CreateTranslation(new Vector3(25, 0, 225));

            Cube27 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube27.Effect.World = Matrix.CreateTranslation(new Vector3(50, 0, 225));

            // ESCALERA 
            // primer escalon 
            Cube28 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube28.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 175));

            Cube29 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube29.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 200));

            Cube30 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube30.Effect.World = Matrix.CreateTranslation(new Vector3(75, 25, 225));

            // segundo escalon
            Cube31 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube31.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 175));

            Cube32 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube32.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 200));

            Cube33 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube33.Effect.World = Matrix.CreateTranslation(new Vector3(100, 50, 225));

            // tercer escalon
            Cube34 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube34.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 175));

            Cube35 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube35.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 200));

            Cube36 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube36.Effect.World = Matrix.CreateTranslation(new Vector3(125, 75, 225));

            // TERCERA PLATAFORMA
            // primera linea
            Cube37 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube37.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 225));

            Cube38 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube38.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 225));

            Cube39 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube39.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 225));

            // segunda linea
            Cube40 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube40.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 200));

            Cube41 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube41.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 200));

            Cube42 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube42.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 200));

            // tercera linea
            Cube43 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube43.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, 175));

            Cube44 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube44.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 175));

            Cube45 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube45.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, 175));

            //CUARTA PLATAFORMA
            Cube46 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube46.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 125));

            Cube47 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube47.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 100));

            Cube48 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube48.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 75));

            Cube49 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube49.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 50));

            Cube50 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube50.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 25));

            Cube51 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube51.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, 0));

            // QUINTA PLATAFORMA
            // primera linea
            Cube52 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube52.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -50));

            Cube53 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube53.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -50));

            Cube54 = new CubePrimitive(GraphicsDevice, 25f, Color.BlueViolet);
            Cube54.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -50));

            // segunda linea
            Cube55 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube55.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -75));

            Cube56 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube56.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -75));

            Cube57 = new CubePrimitive(GraphicsDevice, 25f, Color.Aquamarine);
            Cube57.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -75));

            // tercera linea
            Cube58 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube58.Effect.World = Matrix.CreateTranslation(new Vector3(150, 100, -100));

            Cube59 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube59.Effect.World = Matrix.CreateTranslation(new Vector3(175, 100, -100));

            Cube60 = new CubePrimitive(GraphicsDevice, 25f, Color.LightPink);
            Cube60.Effect.World = Matrix.CreateTranslation(new Vector3(200, 100, -100));

            // BOLITA
            Bola = new SpherePrimitive(GraphicsDevice, 25, 50, Color.White);
            Bola.Effect.World = Matrix.CreateTranslation(new Vector3(25, 25, 0));


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
            GraphicsDevice.Clear(Color.LightSkyBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());

            //Terrain.Draw(Camera.View, Camera.Projection);
            //Terrain2.Draw(Camera.View, Camera.Projection);


            Cube1.Draw(Camera.View, Camera.Projection);
            Cube2.Draw(Camera.View, Camera.Projection);
            Cube3.Draw(Camera.View, Camera.Projection);
            Cube4.Draw(Camera.View, Camera.Projection);
            Cube5.Draw(Camera.View, Camera.Projection);
            Cube6.Draw(Camera.View, Camera.Projection);
            Cube7.Draw(Camera.View, Camera.Projection);
            Cube8.Draw(Camera.View, Camera.Projection);
            Cube9.Draw(Camera.View, Camera.Projection);
            Cube10.Draw(Camera.View, Camera.Projection);
            Cube11.Draw(Camera.View, Camera.Projection);
            Cube12.Draw(Camera.View, Camera.Projection);
            Cube13.Draw(Camera.View, Camera.Projection);
            Cube14.Draw(Camera.View, Camera.Projection);
            Cube15.Draw(Camera.View, Camera.Projection);
            Cube16.Draw(Camera.View, Camera.Projection);
            Cube17.Draw(Camera.View, Camera.Projection);
            Cube18.Draw(Camera.View, Camera.Projection);

            Cube19.Draw(Camera.View, Camera.Projection);
            Cube20.Draw(Camera.View, Camera.Projection);
            Cube21.Draw(Camera.View, Camera.Projection);
            Cube22.Draw(Camera.View, Camera.Projection);
            Cube23.Draw(Camera.View, Camera.Projection);
            Cube24.Draw(Camera.View, Camera.Projection);
            Cube25.Draw(Camera.View, Camera.Projection);
            Cube26.Draw(Camera.View, Camera.Projection);
            Cube27.Draw(Camera.View, Camera.Projection);

            Cube28.Draw(Camera.View, Camera.Projection);
            Cube29.Draw(Camera.View, Camera.Projection);
            Cube30.Draw(Camera.View, Camera.Projection);
            Cube31.Draw(Camera.View, Camera.Projection);
            Cube32.Draw(Camera.View, Camera.Projection);
            Cube33.Draw(Camera.View, Camera.Projection);
            Cube34.Draw(Camera.View, Camera.Projection);
            Cube35.Draw(Camera.View, Camera.Projection);
            Cube36.Draw(Camera.View, Camera.Projection);

            Cube37.Draw(Camera.View, Camera.Projection);
            Cube38.Draw(Camera.View, Camera.Projection);
            Cube39.Draw(Camera.View, Camera.Projection);
            Cube40.Draw(Camera.View, Camera.Projection);
            Cube41.Draw(Camera.View, Camera.Projection);
            Cube42.Draw(Camera.View, Camera.Projection);
            Cube43.Draw(Camera.View, Camera.Projection);
            Cube44.Draw(Camera.View, Camera.Projection);
            Cube45.Draw(Camera.View, Camera.Projection);

            Cube46.Draw(Camera.View, Camera.Projection);
            Cube47.Draw(Camera.View, Camera.Projection);
            Cube48.Draw(Camera.View, Camera.Projection);
            Cube49.Draw(Camera.View, Camera.Projection);
            Cube50.Draw(Camera.View, Camera.Projection);
            Cube51.Draw(Camera.View, Camera.Projection);

            Cube52.Draw(Camera.View, Camera.Projection);
            Cube53.Draw(Camera.View, Camera.Projection);
            Cube54.Draw(Camera.View, Camera.Projection);
            Cube55.Draw(Camera.View, Camera.Projection);
            Cube56.Draw(Camera.View, Camera.Projection);
            Cube57.Draw(Camera.View, Camera.Projection);
            Cube58.Draw(Camera.View, Camera.Projection);
            Cube59.Draw(Camera.View, Camera.Projection);
            Cube60.Draw(Camera.View, Camera.Projection);

            Bola.Draw(Camera.View, Camera.Projection);

            

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