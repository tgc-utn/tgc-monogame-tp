using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Maps;
using TGC.MonoGame.TP.Props.PropType.StaticProps;
using TGC.MonoGame.TP.References;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Effect Effect { get; set; }
        private Map Map { get; set; }
        private Camera Camera { get; set; }
        public Gizmos.Gizmos Gizmos { get; }
        
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
            
            Gizmos = new Gizmos.Gizmos();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            
            var player = new Tank(References.Models.Tank.KF51, new Vector3(400f, 0, 0));
            PropReference propReference = new PropReference(References.Models.Props.Rock_3, new Vector3(300f, 0f, 0f));
            Map = new Desert(0, Models.Tank.KF51, Models.Tank.T90, player, new SmallStaticProp(propReference));
            // Configuramos nuestras matrices de la escena.
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

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(Effects.BasicShader.Path);
            
            Map.Load(Content, Effect);
            
            Gizmos.LoadContent(GraphicsDevice, new ContentManager(Content.ServiceProvider, "Content"));

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
            var keyboardState = Keyboard.GetState();
            // Capturar Input teclado
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }
            Camera.Update(gameTime, Map.Player.World);
            Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);
            Map.Update(gameTime, keyboardState);
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.White);
            Map.Draw(Camera.View, Camera.Projection);
            Gizmos.DrawCube((Map.Player.Box.Max + Map.Player.Box.Min) / 2f, Map.Player.Box.Max - Map.Player.Box.Min, Color.Aqua);
            // var box = BoundingVolumesExtension.CreateAABBFrom(Map.Prop.);
            // box = new BoundingBox(Box.Min + Reference.Position, Box.Max + Reference.Position);
            Gizmos.DrawCube((Map.Prop.Box.Max + Map.Prop.Box.Min) / 2f, Map.Prop.Box.Max - Map.Prop.Box.Min, Color.Red);
            // foreach (var prop in Map.Props)
            //     Gizmos.DrawCube((prop.Box.Max + prop.Box.Min) / 2f, prop.Box.Max - prop.Box.Min, Color.Aqua);
            Gizmos.Draw();
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            Gizmos.Dispose();
            base.UnloadContent();
        }
    }
}