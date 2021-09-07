﻿using System;
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
        private Model island { get; set; }
        
        private Model Rock { get; set; }
        private Model Barco { get; set; }
        private Model Barco2 { get; set; }
        private Model Barco3 { get; set; }
        private float position { get; set; }
        private Model Projektil { get; set; }
        
        private Model Projektil2 { get; set; }
        private Model Terreno2 { get; set; }
        private Effect Effect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private Camera Camera { get; set; }

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
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-350, 50, 400), screenSize);

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
            Model = Content.Load<Model>(ContentFolder3D + "WarVessel/1124");
            island = Content.Load<Model>(ContentFolder3D + "Isla_V2");
            Barco = Content.Load<Model>(ContentFolder3D + "Barco");
            Barco2 = Content.Load<Model>(ContentFolder3D + "Barco2");
            Barco3 = Content.Load<Model>(ContentFolder3D + "Barco3");
            Projektil = Content.Load<Model>(ContentFolder3D + "projektil FBX");
            Terreno2 = Content.Load<Model>(ContentFolder3D + "FBX");
            Projektil2 = Content.Load<Model>(ContentFolder3D + "9x18 pm");
            Rock = Content.Load<Model>(ContentFolder3D + "RockSet06-A");
            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            var modelEffect = (BasicEffect)Model.Meshes[0].Effects[0];

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            //foreach (var mesh in Model.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            //foreach (var meshPart in mesh.MeshParts)
                //meshPart.Effect = Effect;

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
            island.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(300, 0, 0), Camera.View, Camera.Projection);
            Barco2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateTranslation(-10f, 0, 0), Camera.View, Camera.Projection);
            Barco3.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-100f, 0, 0), Camera.View, Camera.Projection);
            Barco.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(-200f, 0, 0), Camera.View, Camera.Projection);
            Projektil.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(5f) * Matrix.CreateTranslation(-400f, 100, 0), Camera.View, Camera.Projection);
            Terreno2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(-550, 50, 0), Camera.View, Camera.Projection);
            Projektil2.Draw( Matrix.CreateRotationY(MathHelper.Pi/2) * Matrix.CreateTranslation(-650, 100, 0), Camera.View, Camera.Projection);
            Rock.Draw( Matrix.CreateRotationY(MathHelper.Pi/2)*Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-800, 20, 0), Camera.View, Camera.Projection);
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