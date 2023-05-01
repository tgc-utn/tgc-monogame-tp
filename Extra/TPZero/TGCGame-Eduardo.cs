using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Content.Models;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Clase principal del juego.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        // Propiedades de movimiento del auto 
        private const float velocidadAngular = 1.5f;
        private const float velocidadMovimiento = 5.5f;
        private const float fuerzaSalto = 1000f;
        private const float fuerzaCaida = 30f;
        private const float rozamiento = 0.99f;
        private const float correctorSalto = 0.04f;

        private GraphicsDeviceManager Graphics { get; }
        private CityScene City { get; set; }
        private Model CarModel { get; set; }
        private Matrix CarWorld { get; set; }
        private FollowCamera FollowCamera { get; set; }
        private float Velocidad { get; set; }
        private float Direccion { get; set; }
        private float Altura { get; set; }
        private Matrix Rotacion { get; set; }
        private Matrix Traslacion { get; set; }
        private Boolean puedeSaltar { get; set; }


        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Se encarga de la configuracion y administracion del Graphics Device.
            Graphics = new GraphicsDeviceManager(this);

            // Carpeta donde estan los recursos que vamos a usar.
            Content.RootDirectory = "Content";

            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        /// <summary>
        ///     Llamada una vez en la inicializacion de la aplicacion.
        ///     Escribir aca todo el codigo de inicializacion: Todo lo que debe estar precalculado para la aplicacion.
        /// </summary>
        protected override void Initialize()
        {
            // Enciendo Back-Face culling.
            // Configuro Blend State a Opaco.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;

            // Configuro las dimensiones de la pantalla.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            // Creo una camara para seguir a nuestro auto.
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            // Configuro la matriz de mundo del auto.
            CarWorld = Matrix.Identity;
            Rotacion = Matrix.Identity;
            Traslacion = Matrix.Identity;

            // El auto inicia pudiendo saltar
            puedeSaltar = true;

            base.Initialize();
        }

        /// <summary>
        ///     Llamada una sola vez durante la inicializacion de la aplicacion, luego de Initialize, y una vez que fue configurado GraphicsDevice.
        ///     Debe ser usada para cargar los recursos y otros elementos del contenido.
        /// </summary>
        protected override void LoadContent()
        {
            // Creo la escena de la ciudad.
            City = new CityScene(Content);

            // La carga de contenido debe ser realizada aca.

            CarModel = Content.Load<Model>(ContentFolder3D + "scene/car");

            base.LoadContent();
        }

        /// <summary>
        ///     Es llamada N veces por segundo. Generalmente 60 veces pero puede ser configurado.
        ///     La logica general debe ser escrita aca, junto al procesamiento de mouse/teclas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Caputo el estado del teclado.

            var DeltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Obtengo el valor para limitar las rotaciones a baja velocidad y las direcciones de rotacion correctas
            float  velocidadDeRotacion = (float)Math.Atan(Velocidad) / MathHelper.PiOver2;

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // Salgo del juego.
                Exit();
            }

            if (keyboardState.IsKeyDown(Keys.A)){
                
                // Giro a la izquierda
                Direccion += velocidadAngular * DeltaTime * velocidadDeRotacion;
            }

            if (keyboardState.IsKeyDown(Keys.D)){

                // Giro a la derecha
                Direccion -= velocidadAngular * DeltaTime * velocidadDeRotacion;
            }

            if (keyboardState.IsKeyDown(Keys.W)){

                // Acelerador
                Velocidad += velocidadMovimiento * DeltaTime;
            }

            if (keyboardState.IsKeyDown(Keys.S)){

                // Freno y reversa
                Velocidad -= velocidadMovimiento * DeltaTime;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && puedeSaltar){

                // Salto y bloqueo del salto en el aire
                Altura += fuerzaSalto * DeltaTime;
                puedeSaltar = false;
            }

            // Gestiona la caida y vuelve a habilitar el salto al volver al suelo
            if ( !puedeSaltar ){
                Altura -= fuerzaCaida * DeltaTime;
                if(Altura <= -fuerzaSalto * 1.005f * DeltaTime){
                    Altura = 0f;
                    puedeSaltar = true;
                }
            }

            Velocidad *= rozamiento; // Limita la velocidad maxima y frena 
            
            // Matriz de inclinacion del salto
            var saltoRotacion = Matrix.CreateRotationZ( velocidadDeRotacion * CarWorld.Forward.X * Altura * correctorSalto )* 
                                Matrix.CreateRotationX( -velocidadDeRotacion * CarWorld.Forward.Z * Altura * correctorSalto );

            // Matriz de rotacion
            Rotacion = Matrix.CreateRotationY(Direccion) * saltoRotacion;

            // Genera la direccion de traslacion
            var vectorSalto = new Vector3(0f, Altura, 0f);
            var vectorTraslacion = CarWorld.Forward * Velocidad + vectorSalto;

            // Matriz de Traslacion
            Traslacion *= Matrix.CreateTranslation(vectorTraslacion);

            // Fija el auto al suelo mientras no esta saltando
            if(puedeSaltar){
                Traslacion = Matrix.CreateTranslation(Traslacion.Translation.X, 0f, Traslacion.Translation.Z);
            }

            // Actualizacion de la matriz de mundo del auto
            CarWorld = Rotacion * Traslacion;

            // Actualizo la camara, enviandole la matriz de mundo del auto.
            FollowCamera.Update(gameTime, CarWorld);

            base.Update(gameTime);
        }


        /// <summary>
        ///     Llamada para cada frame.
        ///     La logica de dibujo debe ir aca.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Limpio la pantalla.
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Dibujo la ciudad.
            City.Draw(gameTime, FollowCamera.View, FollowCamera.Projection);

            // El dibujo del auto debe ir aca.

            CarModel.Draw(CarWorld, FollowCamera.View, FollowCamera.Projection);

            base.Draw(gameTime);
        }

        /// <summary>
        ///     Libero los recursos cargados.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos cargados desde Content Manager.
            Content.Unload();

            base.UnloadContent();
        }
    }
}