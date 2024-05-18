using System;
using System.Collections.Generic;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
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

        // BOLITA
        private SpherePrimitive Bola;
        private List<GeometricPrimitive> Track;

        // Comentada porque no está en uso
        //private float SquareSize = 50f;
        

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            var cameraPosition = new Vector3(25f, 100f, -1100f);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, cameraPosition, size);

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

            RampPrimitive Ramp;
            CubePrimitive Cube;
            Track = new List<GeometricPrimitive>();

            // PRIMERA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 0));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 25));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 100));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 125));
            Track.Add(Cube);

            // SEGUNDA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 175));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(25, 0, 225));
            Track.Add(Cube);

            // ESCALERA

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(1f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(75, 25, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(1f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(100, 50, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(1f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(125, 75, 200));
            Track.Add(Cube);

            // TERCERA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 225));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 200));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, 175));
            Track.Add(Cube);

            //CUARTA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(1f, 1f, 6f) * Matrix.CreateTranslation(new Vector3(175, 100, 62));
            Track.Add(Cube);


            // QUINTA PLATAFORMA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -50));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -75));
            Track.Add(Cube);

            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 100, -100));
            Track.Add(Cube);

            // RAMPA
            Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Ramp.World = Matrix.CreateScale(3f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(175, 125, -125));
            Track.Add(Ramp);

            // CANALETA
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(175, 125, -250));
            Track.Add(Cube);
            Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Ramp.World = Matrix.CreateScale(9f, 1f, 1f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0) * Matrix.CreateTranslation(new Vector3(150, 150, -250)); // * ;
            Track.Add(Ramp);
            Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Ramp.World = Matrix.CreateScale(9f, 1f, 1f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / (-2), 0, 0) * Matrix.CreateTranslation(new Vector3(200, 150, -250)); // * ;
            Track.Add(Ramp);

            // BOLITA
            // Propuesta de punto de inicio del escenario
            Bola = new SpherePrimitive(GraphicsDevice, Content, 25, 50, Color.Red, Matrix.CreateTranslation(new Vector3(25, 25, -800)));

            // PLANOS INCLINADOS (ROLL)
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6)) * Matrix.CreateTranslation(new Vector3(25, 0, -75));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -175));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6) * Matrix.CreateTranslation(new Vector3(25, 0, -275));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -375));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / (-6)) * Matrix.CreateTranslation(new Vector3(25, 0, -475));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.BlueViolet);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -575));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI / 6) * Matrix.CreateTranslation(new Vector3(25, 0, -675));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(3f, 1f, 4f) * Matrix.CreateTranslation(new Vector3(25, 0, -775));
            Track.Add(Cube);

            // RAMPA GRANDE
            Ramp = new RampPrimitive(GraphicsDevice, Content, 25f, Color.LightPink);
            Ramp.World = Matrix.CreateScale(7f, 7f, 7f) * Matrix.CreateFromYawPitchRoll((float)Math.PI / 2, 0, 0) * Matrix.CreateTranslation(new Vector3(175, 125, -450));
            Track.Add(Ramp);
            
            // CUADRADOS GRANDES 
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Red);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(275, 25, -425));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Orange);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(300, 0, -400));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Yellow);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(325, -25, -375));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Green);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(350, -50, -350));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Blue);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(375, -75, -325));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Indigo);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(400, -100, -300));
            Track.Add(Cube);
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Purple);
            Cube.World = Matrix.CreateScale(9f, 1f, 9f) * Matrix.CreateTranslation(new Vector3(425, -125, -275));
            Track.Add(Cube);

            // PLATAFORMA FINAL
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Aquamarine);
            Cube.World = Matrix.CreateScale(7f, 1f, 6f) * Matrix.CreateTranslation(new Vector3(425 , -125, -75));
            Track.Add(Cube);

            // aca iria la banderita del final
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.White);
            Cube.World = Matrix.CreateTranslation(new Vector3(425, -100, -75));
            Track.Add(Cube);

            // aca irian cartelitos
            //jump
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
            Cube.World = Matrix.CreateTranslation(new Vector3(-25, 50, 150));
            Track.Add(Cube);

            //up (flechita?)
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
            Cube.World = Matrix.CreateTranslation(new Vector3(50, 75, 250));
            Track.Add(Cube);

            //jump
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
            Cube.World = Matrix.CreateTranslation(new Vector3(200, 150, 150));
            Track.Add(Cube);

            //jump
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
            Cube.World = Matrix.CreateTranslation(new Vector3(200, 150, -25));
            Track.Add(Cube);

            //down (flechita?)
            Cube = new CubePrimitive(GraphicsDevice, Content, 25f, Color.Black);
            Cube.World = Matrix.CreateTranslation(new Vector3(200, 175, -475));
            Track.Add(Cube);


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

            Bola.Draw(Camera.View, Camera.Projection);

            

            foreach (GeometricPrimitive primitive in Track) {
                primitive.Draw(Camera.View, Camera.Projection);
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