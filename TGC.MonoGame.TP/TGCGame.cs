using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.MainCharacter;
using TGC.MonoGame.TP.Stages;

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

        // Camera to draw the scene
        private FollowCamera FollowCamera { get; set; }

        // BOLITA
        private Character MainCharacter;
        private Stage Stage;
        protected List<Entity> Entities;
        

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {

            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;

            //var cameraPosition = new Vector3(25f, 100f, -1100f);
            //Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, cameraPosition, size);
            // Creo una camaar para seguir a nuestro auto.
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);


            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            base.Initialize();
        }
        private SpriteFont SpriteFont{get;set;}
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        public Effect BallEffect;
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Entities = new List<Entity>();

            Stage = new Stage_01(GraphicsDevice, Content);

            MainCharacter = new Character(Content, Stage, Entities);

            BallEffect = Content.Load<Effect>(ContentFolderEffects + "PBR");

            MergeEntities(Stage.Track, Stage.Obstacles, Stage.Signs, Stage.Pickups);

            base.LoadContent();
        }

        private void MergeEntities(List<GeometricPrimitive> Track, List<GeometricPrimitive> Obstacles, List<GeometricPrimitive> Signs, List<GeometricPrimitive> Pickups)
        {
            foreach(GeometricPrimitive myTrack in Track)
            {
                Entities.Add(myTrack);
            }
            
            foreach(GeometricPrimitive myObstacle in Obstacles)
            {
                Entities.Add(myObstacle);
            }

            foreach(GeometricPrimitive mySign in Signs)
            {
                Entities.Add(mySign);
            }

            foreach(GeometricPrimitive myPickup in Pickups)
            {
                Entities.Add(myPickup);
            }
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }


            MainCharacter.Update(gameTime);

            


            FollowCamera.Update(gameTime, MainCharacter.World);
            BallEffect.Parameters["eyePosition"].SetValue(FollowCamera.CamPosition);

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


            MainCharacter.Draw(FollowCamera.View, FollowCamera.Projection);

            Stage.Draw(FollowCamera.View, FollowCamera.Projection);

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