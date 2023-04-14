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

        // Necesarias para el juego
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Effect Effect { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private FollowCamera Camera { get; set; }


        // Modelos incluidos al juego
        private Car AutoPrincipal;

        private IACar AutoSecundario;

        private List<IElemento> Elementos;

        // Esto hay que sacarlo y tratarlo como un elemento más del mapa
        // (Elemento World : Todo lo que se tenga que cargar y dibujar en el mapa)
        // private World elemento { get; set; }; 
        
        private float Rotation { get; set; }
        private float Rotation2 { get; set; }
        private Matrix World { get; set; }
        private QuadPrimitive Floor { get; set; }
        private QuadPrimitive Floor2 { get; set; }

        protected override void Initialize()
        {
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Necesarias del juego
            Camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            //Acá inicializamos los objetos que queremos cargar... Después carga la hace automáticamente
            AutoPrincipal = new Car("RacingCarA/RacingCar", new Vector3(-1000f,0,1000f));
            AutoSecundario = new IACar("", new Vector3(-800f,0,-800f));

            Elementos = new List<IElemento>();
            Elementos.Add(AutoPrincipal);
            Elementos.Add(AutoSecundario);
            

            // Esto no sirve acá, tendría que irse a la clase del mapa
            World = Matrix.Identity;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ////    Deberíamos poder cargar los modelos sistemáticamente.
            //      Para eso todos los elementos que vayamos a cargar tienen que implementar el método .Load(ContentManager)
            
            foreach (var elemento in Elementos){
                    elemento.Load(Content);
            }
            
            Floor = new QuadPrimitive(GraphicsDevice);
            Floor2 = new QuadPrimitive(GraphicsDevice);

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            ////    Deberíamos poder hacer update de los elementos NO ESTÁTICOS sistemáticamente.
            //      Para eso todos los elementos que vayamos a cargar tienen que implementar el método 
            //      de dos maneras : .Update() a los que se mueven automáticamente y .Update( g : Gametime, k : KeyboardState) 
            //      a los controlables
            // Controlables
            AutoPrincipal.Update(gameTime, Keyboard.GetState());
            AutoSecundario.Update(gameTime, Keyboard.GetState());
            Camera.Update(gameTime, AutoPrincipal.getWorld());


            // Esto tiene que volar a la clase del mapa
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Rotation2 -= 0.5f*Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            World = Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(0f, 50f, 0f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ////    Deberíamos poder dibujar los modelos sistemáticamente.
            //      Para eso todos los elementos que vayamos a cargar tienen que implementar el método .Draw(ContentManager)
            
            foreach (var elemento in Elementos){
                    elemento.Draw(Camera.View, Camera.Projection);
            }
           
            
            
            //AutoPrincipal.Draw(Camera.View, Camera.Projection);
            
            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkBlue.ToVector3());
            Effect.Parameters["World"].SetValue(World*Matrix.CreateScale(2500f, 0f, 2500f));

            Floor.Draw(Effect);

            Effect.Parameters["View"].SetValue(Camera.View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.Crimson.ToVector3());
            Effect.Parameters["World"].SetValue(World*Matrix.CreateScale(1000f, 0f, 1000f)*Matrix.CreateRotationY(Rotation2)*Matrix.CreateTranslation(10f, 50f, 10f) );

            Floor2.Draw(Effect);
        }

        protected override void UnloadContent()
        {
            Content.Unload();

            base.UnloadContent();
        }
    }
}