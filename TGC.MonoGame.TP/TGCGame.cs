using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Model T90 { get; set; }
        private Model Panzer{ get; set; }
        private Effect Effect { get; set; }
        private float Rotation { get; set; }

        private List<Object> objetos3D { get; set; }  

        private Object Prueba { get; set; }
        private Texture2D Textura { get; set; }

        private FollowCamera FollowCamera { get; set; }

        private Suelo Suelo {get; set;}
        private Model roca {get; set;}
        private Object Roca {get;set;}
        private Effect EffectRoca {get;set;}

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

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            objetos3D = new List<Object>();

            // Configuramos nuestras matrices de la escena, en este caso se realiza en el objeto FollowCamara
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);


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

            // Cargo el modelo del tanque.
            T90 = Content.Load<Model>(ContentFolder3D + "T90");

            // Cargo el efecto basico propio declarado en el Content pipeline.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            // Cargo la textura correspondiente                        
            Textura = Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA");

            //Prueba = new Object(Vector3.Left * 10, T90, Effect, Textura);
            
            InitializeTanks();
            objetos3D.ForEach(o => o.LoadContent());
            //Prueba.LoadContent();

            Suelo = new Suelo(GraphicsDevice);
            Suelo.Effect = Effect;

            roca = Content.Load<Model>(ContentFolder3D + "sphere");
            EffectRoca = Content.Load<Effect>(ContentFolderEffects + "BasicShaderRock");
            Roca = new Object(new Vector3(0f,0f,0f), roca, EffectRoca,null);
            Roca.LoadContent();

            base.LoadContent();
        }

        private void InitializeTanks()
        {
            objetos3D.Add(new Object(new Vector3(1000f, 0, 0), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullA")));
            objetos3D.Add(new Object(new Vector3(-1000f, 0, 0), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullB")));
            objetos3D.Add(new Object(new Vector3(1000f, 0, 1000f), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullC")));
            objetos3D.Add(new Object(new Vector3(-1000f, 0, 1000f), T90, Content.Load<Effect>(ContentFolderEffects + "BasicShader"), Content.Load<Texture2D>(ContentFolder3D + "textures_mod/hullC")));
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            //objetos3D[1].Position += Vector3.Backward * 5;
            //objetos3D[1].World = Matrix.CreateTranslation(objetos3D[1].Position);

            // Lógica del juego acá (por ahora solo renderiza un mundo)
            

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.BlueViolet);

            //Prueba.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
            objetos3D.ForEach(a => a.Draw(gameTime, FollowCamera.View, FollowCamera.Projection));
            FollowCamera.Update(gameTime, objetos3D[0].World);

            Suelo.Draw(gameTime,GraphicsDevice, FollowCamera.View, FollowCamera.Projection);

            Roca.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);
            
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