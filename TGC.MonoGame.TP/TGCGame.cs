using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }
    
        // Graphics
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        
        // Skybox
        private SkyBox SkyBox { get; set; }
        
        // Camera
        private Camera Camera { get; set; }
        private TargetCamera TargetCamera { get; set; }
        
        // Scene
        private Matrix SphereWorld { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        
        // Geometries
        private SpherePrimitive Sphere { get; set; }
        private QuadPrimitive Quad { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        
        // Sphere position & rotation
        private Vector3 SpherePosition { get; set; }
        private Matrix SphereScale { get; set; }
        
        // World matrices
        private List<Matrix> _platformMatrices;
        
        // Effects
        private Effect Effect { get; set; }
        private Effect TextureEffect { get; set; }
        
        // private Effect SkyboxEffect { get; set; }
        
        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D MarbleTexture { get; set; }
        private Texture2D RubberTexture { get; set; }
        private Texture2D MetalTexture { get; set; }
        
        // private Texture2D Sk

        // Models
        private Model StarModel { get; set; }
        private Model SphereModel { get; set; }
        private Matrix StarWorld { get; set; }

        private Player _player;
        
        
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            
            // Configuro las dimensiones de la pantalla.
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();
            
            // Camera
            var size = GraphicsDevice.Viewport.Bounds.Size;
            size.X /= 2;
            size.Y /= 2;
            //Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 40, 200), size);
            TargetCamera = new TargetCamera(GraphicsDevice.Viewport.AspectRatio, Vector3.One * 100f, Vector3.Zero);
            
            // Configuramos nuestras matrices de la escena.
            SphereWorld = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            
            // Sphere
            SpherePosition = new Vector3(0f, 10f, 0f);
            SphereScale = Matrix.CreateScale(5f);
            
            // Player
            _player = new Player(SphereScale, SpherePosition);
            
            // Star
            StarWorld = Matrix.Identity;
            
            // Box/platforms
            _platformMatrices = new List<Matrix>();
            
            Prefab.CreateSquareCircuit(Vector3.Zero);
            Prefab.CreateSquareCircuit(new Vector3(-600, 0f, 0f));
            _platformMatrices = Prefab.PlatformMatrices;
            
            /*
             ===================================================================================================
             Bridge between Circuit 1 and Circuit 2
             ===================================================================================================
            */
            
            // Platform
            CreatePlatform(new Vector3(90f, 6f, 30f), new Vector3(-50f, 0f, 0f));
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-120f, 0f, 0f));
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-160f, 0f, 0f));
            
            // Ramp
            CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-190f, 5f, 0f), Matrix.CreateRotationZ(-0.3f));

            base.Initialize();
        }

        /// <summary>
        ///     Creates a platform with the specified scale, position and rotation.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        /// <param name="rotation">The rotation of the platform</param>
        private void CreatePlatform(Vector3 scale, Vector3 position, Matrix rotation)
        {
            var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
            _platformMatrices.Add(platformWorld);
        }
        
        /// <summary>
        ///     Creates a platform with the specified scale and position.
        /// </summary>
        /// <param name="scale">The scale of the platform</param>
        /// <param name="position">The position of the platform</param>
        private void CreatePlatform(Vector3 scale, Vector3 position)
        {
            var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            _platformMatrices.Add(platformWorld);
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
            
            StonesTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            MarbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble_black_01_c");
            RubberTexture = Content.Load<Texture2D>(ContentFolderTextures + "goma_diffuse");
            MetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal_diffuse");
            
            // Create our Quad (to draw the Floor)
            Quad = new QuadPrimitive(GraphicsDevice);
            
            // Create our box
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, StonesTexture);

            // Cargo el modelo del logo.
            //Model = Content.Load<Model>(ContentFolder3D + "tgc-logo/tgc-logo");
            StarModel = Content.Load<Model>(ContentFolder3D + "star/Gold_Star");

            SphereModel = Content.Load<Model>(ContentFolder3D + "geometries/sphere");

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            loadEffectOnMesh(StarModel, Effect);

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "BasicTextureShader");
            loadEffectOnMesh(SphereModel, TextureEffect);

            SphereWorld = SphereScale * Matrix.CreateTranslation(SpherePosition);

            // SkyboxEffect = Content.Load<Effect>()
            
            var skyBox = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 1000f);

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            /*foreach (var mesh in Model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }*/

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
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            SphereWorld = _player.Update(time, keyboardState);
            
            // Capturar Input teclado
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            UpdateCamera(_player.SpherePosition, _player.Yaw);

            base.Update(gameTime);
        }
        
        private void UpdateCamera(Vector3 position, float yaw)
        {
            // Create a position that orbits the Robot by its direction (Rotation)

            // Create a normalized vector that points to the back of the Robot
            var sphereBackDirection = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(yaw));
            // Then scale the vector by a radius, to set an horizontal distance between the Camera and the Robot
            var orbitalPosition = sphereBackDirection * 60f;

            // We will move the Camera in the Y axis by a given distance, relative to the Robot
            var upDistance = Vector3.Up * 15f;

            // Calculate the new Camera Position by using the Robot Position, then adding the vector orbitalPosition that sends 
            // the camera further in the back of the Robot, and then we move it up by a given distance
            TargetCamera.Position = position + orbitalPosition + upDistance;

            // Set the Target as the Robot, the Camera needs to be always pointing to it
            TargetCamera.TargetPosition = position;

            // Build the View matrix from the Position and TargetPosition
            TargetCamera.BuildView();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            foreach (var platformWorld in _platformMatrices)
            {
                // Configura la matriz de mundo del efecto con la matriz del Floor actual
                Effect.Parameters["World"].SetValue(platformWorld);
                Effect.Parameters["View"].SetValue(TargetCamera.View);
                Effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                Effect.Parameters["DiffuseColor"].SetValue(Color.ForestGreen.ToVector3());
                
                BoxPrimitive.Draw(Effect);
            }  
            
            //Sphere.Draw(World, TargetCamera.View, TargetCamera.Projection); // TODO: no usar

            DrawTexturedModel(SphereWorld, SphereModel, TextureEffect, RubberTexture);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-450f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(150f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Graphics.GraphicsDevice.RasterizerState = rasterizerState;


            
            SkyBox.Draw(TargetCamera.View, TargetCamera.Projection, new Vector3(0f,0f,0f));
            GraphicsDevice.RasterizerState = originalRasterizerState;
        }

        private void DrawModel(Matrix world, Model model, Effect effect){
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());

            foreach (var mesh in model.Meshes)
            {
                Matrix meshMatrix = mesh.ParentBone.Transform;
                effect.Parameters["World"].SetValue(meshMatrix * world);
                mesh.Draw();
            }
        }
        
        private void DrawTexturedModel(Matrix worldMatrix, Model model, Effect effect, Texture2D texture){
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"]?.SetValue(Color.IndianRed.ToVector3());
            effect.Parameters["Texture"]?.SetValue(texture);

            chequearPropiedadesTextura(texture);

            foreach (var mesh in model.Meshes)
            {   
                mesh.Draw();
            }
        }
        
        private void DrawGeometry(GeometricPrimitive geometry, Matrix worldMatrix, Effect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["View"].SetValue(TargetCamera.View);
            effect.Parameters["Projection"].SetValue(TargetCamera.Projection);
            effect.Parameters["DiffuseColor"]?.SetValue(Color.IndianRed.ToVector3());
            effect.Parameters["Texture"]?.SetValue(StonesTexture);
            geometry.Draw(effect);
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

        public static void loadEffectOnMesh(Model modelo,Effect efecto)
        {
            foreach (var mesh in modelo.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = efecto;
                }
            }
        }

        public void chequearPropiedadesTextura(Texture2D texture){
            //La bola de marmol acelera mas lento
            //La bola de goma salta mas alto
            //La bola de metal acelera mas rápido
            if(texture == MarbleTexture){
                _player.Acceleration = 30f;
            }else if(texture == RubberTexture){
                _player.MaxJumpHeight = 70f;
            }else if(texture == MetalTexture){
                _player.Acceleration = 100f;
                _player.MaxSpeed = 230f;
            }
        }
    }
}