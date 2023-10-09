using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Collisions;
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
        private QuadPrimitive Quad { get; set; }
        private BoxPrimitive BoxPrimitive { get; set; }
        
        // Sphere position & rotation
        private Vector3 InitialSpherePosition { get; set; }
        private Matrix SphereScale { get; set; }

        // Effects
        // Effect for the Platforms
        private Effect PlatformEffect { get; set; }

        // Effect for the ball
        private Effect Effect { get; set; }
        private Effect TextureEffect { get; set; }
        
        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D MarbleTexture { get; set; }
        private Texture2D RubberTexture { get; set; }
        private Texture2D MetalTexture { get; set; }

        // Models
        private Model StarModel { get; set; }
        private Model SphereModel { get; set; }
        private Matrix StarWorld { get; set; }
        private Player _player;
        
        // Colliders
        private Gizmos.Gizmos Gizmos { get; set; }


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
            InitialSpherePosition = new Vector3(0f, 10f, 0f);
            SphereScale = Matrix.CreateScale(5f);
            
            // Player
            _player = new Player(SphereScale, InitialSpherePosition, new BoundingSphere(InitialSpherePosition, 5f));
            
            // Gizmos
            Gizmos = new Gizmos.Gizmos
            {
                Enabled = true
            };

            // Star
            StarWorld = Matrix.Identity;
            
            Prefab.CreateSquareCircuit(Vector3.Zero);
            Prefab.CreateSquareCircuit(new Vector3(-600, 0f, 0f));
            Prefab.CreateBridge();
            Prefab.CreateSwitchbackRamp();
            
            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            StonesTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            MarbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble_black_01_c");
            RubberTexture = Content.Load<Texture2D>(ContentFolderTextures + "goma_diffuse");
            MetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal_diffuse");
            
            Quad = new QuadPrimitive(GraphicsDevice);
            
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, StonesTexture);
            
            StarModel = Content.Load<Model>(ContentFolder3D + "star/Gold_Star");

            SphereModel = Content.Load<Model>(ContentFolder3D + "geometries/sphere");
            
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            PlatformEffect = Content.Load<Effect>(ContentFolderEffects + "PlatformShader");
            loadEffectOnMesh(StarModel, Effect);

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "BasicTextureShader");
            loadEffectOnMesh(SphereModel, TextureEffect);

            SphereWorld = SphereScale * Matrix.CreateTranslation(InitialSpherePosition);
            
            var skyBox = Content.Load<Model>(ContentFolder3D + "skybox/cube");
            var skyBoxTexture = Content.Load<TextureCube>(ContentFolderTextures + "/skyboxes/skybox");
            var skyBoxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");
            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect, 1000f);

            Gizmos.LoadContent(GraphicsDevice, Content);

            base.LoadContent();
        }
        
        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            SphereWorld = _player.Update(time, keyboardState);
            
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            
            if (_player.SpherePosition.Y <= -150f || keyboardState.IsKeyDown(Keys.R))
            {
                _player.SpherePosition = InitialSpherePosition;
            }

            UpdateCamera(_player.SpherePosition, _player.Yaw);

            Prefab.UpdateMovingPlatforms();
            
            Gizmos.UpdateViewProjection(TargetCamera.View, TargetCamera.Projection);

            base.Update(gameTime);
        }
        
        private void UpdateCamera(Vector3 position, float yaw)
        {
            var sphereBackDirection = Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(yaw));
            
            var orbitalPosition = sphereBackDirection * 60f;
            
            var upDistance = Vector3.Up * 15f;
            
            TargetCamera.Position = position + orbitalPosition + upDistance;

            TargetCamera.TargetPosition = position;
            
            TargetCamera.BuildView();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            foreach (var platformWorld in Prefab.PlatformMatrices)
            {
                PlatformEffect.Parameters["World"].SetValue(platformWorld);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                BoxPrimitive.Draw(PlatformEffect);
            }
            
            foreach (var rampWorld in Prefab.RampMatrices)
            {
                PlatformEffect.Parameters["World"].SetValue(rampWorld);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                
                BoxPrimitive.Draw(PlatformEffect);
            } 

            foreach (var boundingBox in Prefab.PlatformAbb)
            {
                var center = BoundingVolumesExtensions.GetCenter(boundingBox);
                var extents = BoundingVolumesExtensions.GetExtents(boundingBox);
                Gizmos.DrawCube(center, extents * 2f, Color.Red);
            }

            foreach (var movingPlatform in Prefab.MovingPlatforms)
            {
                PlatformEffect.Parameters["World"].SetValue(movingPlatform.World);
                PlatformEffect.Parameters["View"].SetValue(TargetCamera.View);
                PlatformEffect.Parameters["Projection"].SetValue(TargetCamera.Projection);
                PlatformEffect.Parameters["Textura_Plataformas"].SetValue(StonesTexture);
                
                BoxPrimitive.Draw(PlatformEffect);
                
                var movingBoundingBox = movingPlatform.MovingBoundingBox;
                var center = BoundingVolumesExtensions.GetCenter(movingBoundingBox);
                var extents = BoundingVolumesExtensions.GetExtents(movingBoundingBox);
                Gizmos.DrawCube(center, extents * 2f, Color.GreenYellow);
            }

            foreach (var orientedBoundingBox in Prefab.RampObb)
            {
                var orientedBoundingBoxWorld = Matrix.CreateScale(orientedBoundingBox.Extents * 2f) 
                                               * orientedBoundingBox.Orientation * Matrix.CreateTranslation(orientedBoundingBox.Center);
                Gizmos.DrawCube(orientedBoundingBoxWorld, Color.Red);
            }

            DrawTexturedModel(SphereWorld, SphereModel, TextureEffect, RubberTexture);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-450f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            StarWorld = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(150f, 5f, 0f);
            DrawModel(StarWorld, StarModel, Effect);
            
            Gizmos.DrawSphere(_player.BoundingSphere.Center, _player.BoundingSphere.Radius * Vector3.One, Color.Yellow);
            Gizmos.Draw();
            
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