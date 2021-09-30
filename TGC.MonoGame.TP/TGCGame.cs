using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using System.Collections.Generic;
using TGC.MonoGame.TP.Quads;
using TGC.MonoGame.TP.SkyBoxs;
using TGC.MonoGame.TP.Collisions;



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
        //importa monedas del archivo monedas
        private Tp.Monedas monedas { get; set; }

        //Modelos
        private Model Cartel { get; set; }
        private Model Esfera { get; set; }
        private Model TunnelChico { get; set; }
        private Model Cubo { get; set; }
        private Model Platform { get; set; }
        private Model Flag { get; set; }
        private Model Pinches { get; set; }
        private Model Wings { get; set; }
        private Model Skybox { get; set; }
        private Effect Effect { get; set; }
        public Effect TextureEffect { get; set; }
        public Effect LavaEffect { get; set; }
        public Effect SkyboxEffect { get; set; }
        private Texture2D MarbleTexture { get; set; }
        private Texture2D SpikesTexture { get; set; }
        private Texture2D LavaTexture { get; set; }
        private Texture2D MagmaTexture { get; set; }
        private Texture2D CartelTexture { get; set; }
        private Texture2D WoodTexture { get; set; }
        private Texture2D StoneTexture { get; set; }
        private Texture2D MetalTexture { get; set; }
        public Texture2D FlagCheckeredTexture { get; set; }
        public Texture2D FlagCheckpointTexture { get; set; }
        public Texture2D BluePlatformTexture { get; set; }
        public Texture2D BluePlatformBasicTexture { get; set; }
        public Texture2D RedPlatformTexture { get; set; }
        public Texture2D RedPlatformBasicTexture { get; set; }
        public Texture2D GreenPlatformTexture { get; set; }
        public Texture2D GreenPlatformBasicTexture { get; set; }
        public Texture2D BluePlaceholderTexture { get; set; }
        public Texture2D Aluminio { get; set; }
        public Texture2D Empty { get; set; }
        public Texture2D WhitePlaceholderTexture { get; set; }
        public TextureCube SkyboxTexture { get; set; }
        private float Rotation { get; set; }
        public bool OnGround { get; private set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private Camera Camera { get; set; }
        public Quad quad { get; set; }
        private SkyBox skybox { get; set; }

        private BoundingBox[] platformColliders;
        private OrientedBoundingBox[] rotatedPlatformsColliders;
        private List<BoundingBox> checkpoints;
        private BoundingSphere PelotaChica1Box; 
        private Vector3 PelotaChica1Posicion { get; set; }
        private Matrix PelotChica1World { get; set; }
        private BoundingBox AlasBox { get; set; }
        private Vector3 AlasPosicion { get; set; }
        private Matrix AlasWorld { get; set; }

        public Vector3 MarblePosition { get; private set; }
        public Vector3 RespawnPosition { get; set; }
        public Matrix MarbleWorld { get; set; }
        public Vector3 MarbleVelocity { get; set; }
        public Vector3 MarbleAcceleration { get; set; }
        private Vector3 MarbleFrontDirection { get; set; }
        public Matrix MarbleRotation { get; set; }
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private float mouseSensitivity;
        private Vector3 mouseRotationBuffer;
        private bool TocandoPoderPelotaChica { get; set; }
        private bool TocandoAlas { get; set; }


        private BoundingSphere MarbleSphere;

        public VertexDeclaration vertexDeclaration { get; set; }
        public Matrix MarbleScale { get; private set; }
        //public Matrix MarbleRotation { get; private set; }
        public Matrix ChairOBBWorld { get; private set; }

        private float Gravity = 100f;
        private float JumpSpeed = 50f;
        public float DefaultSpeed = 15f;
        public float PelotaRapida = 45f;
        public float PelotaLenta = 5f;
        

        private float SkyBoxSize = 400f;
        private const float EPSILON = 0.00001f;
        private Matrix marbleCopy;
        private float rotacionAngular;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            // Seria hasta aca.
            OnGround = false;
            mouseSensitivity = 0.1f;
            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            //configuro pantalla
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 0, 0), screenSize);

            float yPositionFloor = -20f;
            float xScaleFloor = 400f;
            float zScaleFloor = 400f;

            quad = new Quad(new Vector3(0f, yPositionFloor, 0f), Vector3.Up, Vector3.Forward, xScaleFloor, zScaleFloor);

            platformColliders = new BoundingBox[24];

            //Plataformas que no estan rotadas
            platformColliders[0] = new BoundingBox(new Vector3(-25f, -21f, -15f), new Vector3(5f, -16f, 15f));
            platformColliders[1] = new BoundingBox(new Vector3(15f, -21f, -5f), new Vector3(30f, -16f, 5f)); //22f, -18f, 0f
            //platformColliders[2] = new BoundingBox(new Vector3(26f, -16f, -5f), new Vector3(34f, -9f, 5f)); //Rampa ( NO USAR: la rampa no se puede hacer con un AABB )
            platformColliders[2] = new BoundingBox(new Vector3(32f, -14f, -5f), new Vector3(42f, -9f, 5f)); //37f, -11f, 0f
            platformColliders[3] = new BoundingBox(new Vector3(55f, -21f, -5f), new Vector3(85f, -16f, 5f)); //70f, -18f, 0f
            platformColliders[4] = new BoundingBox(new Vector3(66f, -14f, -5f), new Vector3(74f, -12f, 5f));
            platformColliders[5] = new BoundingBox(new Vector3(68f, -15f, -5f), new Vector3(72f, -12f, 5f)); //Plataforma que sube y baja
            platformColliders[6] = new BoundingBox(new Vector3(84f, -15f, -4.5f), new Vector3(86f, -8f, -3.5f)); //84f, -11f, -4f //Primer Checkpoint
            platformColliders[7] = new BoundingBox(new Vector3(79f, -10f, 25f), new Vector3(89f, -5f, 35f)); //84f, -10f, 30f
            platformColliders[8] = new BoundingBox(new Vector3(47f, -21f, 105f), new Vector3(57f, -16f, 115f)); //52f, -18f, 110f
            platformColliders[9] = new BoundingBox(new Vector3(30f, -21f, 105f), new Vector3(47f, -18f, 115f)); //35f, -20f, 110f
            platformColliders[10] = new BoundingBox(new Vector3(15f, -21f, 105f), new Vector3(31f, -16f, 115f)); //23f, -18f, 110f
            platformColliders[11] = new BoundingBox(new Vector3(15.75f, -21f, 113.75f), new Vector3(16.25f, -8f, 114.25f)); //16f, -11f, 119f //Segundo Checkpoint
            platformColliders[12] = new BoundingBox(new Vector3(-90, 8f, 24f), new Vector3(-85f, 11f, 60f)); //-87.5f, 10f, 42f
            platformColliders[13] = new BoundingBox(new Vector3(-90, -1f, 24f), new Vector3(-85f, 0.25f, 30f)); //-87.5f, 0f, 25f
            platformColliders[14] = new BoundingBox(new Vector3(-89, 8f, 18f), new Vector3(-86f, 10f, 22f)); //Plataformas que suben y bajan
            platformColliders[15] = new BoundingBox(new Vector3(-82, 8f, 15f), new Vector3(-78f, 10f, 19f)); //-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f
            platformColliders[16] = new BoundingBox(new Vector3(-74f, 8f, 15f), new Vector3(-71f, 10f, 19f)); //-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f
            platformColliders[17] = new BoundingBox(new Vector3(-64f, 8f, 15f), new Vector3(-60f, 10f, 19f)); //-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f
            platformColliders[18] = new BoundingBox(new Vector3(-53f, 8f, 15f), new Vector3(-49f, 10f, 19f)); //-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f
            platformColliders[19] = new BoundingBox(new Vector3(-45f, 8f, 15f), new Vector3(-41f, 10f, 19f)); //-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f
            platformColliders[20] = new BoundingBox(new Vector3(-40f, 20f, 15f), new Vector3(-10f, 21f, 19f)); //-25f, 20f, 17f
            platformColliders[21] = new BoundingBox(new Vector3(-1f, 21f, -5f), new Vector3(5f, 23f, 25f)); //2f, 22f, 10f
            platformColliders[22] = new BoundingBox(new Vector3(-92.75f, 8f, 65.75f), new Vector3(-92.25f, 24f, 66.25f)); //-87.5f, 12f, 65f //Tercer Checkpoint
            platformColliders[23] = new BoundingBox(new Vector3(-0.25f, 20f, 2.75f), new Vector3(0.25f, 30f, 3.25f)); //0f, 28f, 3f //Checkered Flag

            //Checkpoints
            checkpoints = new List<BoundingBox>();
            checkpoints.Add(new BoundingBox(new Vector3(82f, -16f, -5f), new Vector3(83f, -5f, 5f))); //84f, -11f, -4f
            checkpoints.Add(new BoundingBox(new Vector3(16f, -16f, 105f), new Vector3(17f, -5f, 115f))); //16f, -11f, 114f
            checkpoints.Add(new BoundingBox(new Vector3(-86f, 10f, 55f), new Vector3(-87f, 15f, 75f))); //-87.5f, 12f, 65f
            checkpoints.Add(new BoundingBox(new Vector3(-1f, 23f, -5f), new Vector3(5f, 35f, 0f))); //0f, 28f, 3f

            MarblePosition = new Vector3(-10f, -10f, 0f); //<- Original
            RespawnPosition = MarblePosition;
            MarblePosition = new Vector3(82f, -12f, 0f); //<- Para Probar
            MarbleVelocity = Vector3.Zero;
            MarbleSphere = new BoundingSphere(MarblePosition, 2f);
            MarbleScale = Matrix.CreateScale(0.02f);
            //MarbleRotation = Matrix.CreateRotationY(-MathHelper.ToRadians(90));
            MarbleRotation = Matrix.Identity;
            MarbleFrontDirection = Vector3.Backward;
            MarbleWorld = Matrix.Identity;
            marbleCopy = MarbleWorld;
            mouseRotationBuffer.X = -90;
            rotacionAngular = 0;
            //powerups  pelota chica bounding boxes
            TocandoPoderPelotaChica = false;
            PelotaChica1Posicion = new Vector3(95f, -10f, 13f);
            PelotChica1World = Matrix.CreateTranslation(PelotaChica1Posicion);
            //powerup alas
            TocandoAlas = false;
            AlasPosicion = new Vector3(86f, -16f, 45f);
            AlasWorld = Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(AlasPosicion);
            //( Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(new Vector3(86f, -16f, 45f)) ), Color.BlueViolet, Wings);

            

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

            // Cargo el Cartel
            Cartel = Content.Load<Model>(ContentFolder3D + "Marbel/Sign/StreetSign");
            //Cargo la esfera
            Esfera = Content.Load<Model>(ContentFolder3D + "Marbel/Pelota/pelota");
            //cargo tunel
            TunnelChico = Content.Load<Model>(ContentFolder3D + "Marbel/TunelChico/TunnelChico");
            //cargo Cubo
            Cubo = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/cubo");
            //cargo Bandera
            Flag = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/flag");
            //cargo Plataforma
            Platform = Content.Load<Model>(ContentFolder3D + "Marbel/Cubo/platform");
            //cargo pinches
            Pinches = Content.Load<Model>(ContentFolder3D + "Marbel/Pinches/Pinches");
            //cargo wings
            Wings = Content.Load<Model>(ContentFolder3D + "Marbel/Wings/Wings");
            //cargo Skybox
            Skybox = Content.Load<Model>(ContentFolder3D + "Marbel/Skybox/cube");

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            TextureEffect = Content.Load<Effect>(ContentFolderEffects + "TextureShader");

            LavaEffect = Content.Load<Effect>(ContentFolderEffects + "LavaShader");

            SkyboxEffect = Content.Load<Effect>(ContentFolderEffects + "SkyBox");

            MarbleTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble");
            SpikesTexture = Content.Load<Texture2D>(ContentFolderTextures + "Spikes");
            LavaTexture = Content.Load<Texture2D>(ContentFolderTextures + "Lava");
            MagmaTexture = Content.Load<Texture2D>(ContentFolderTextures + "Rock");
            CartelTexture = Content.Load<Texture2D>(ContentFolderTextures + "Sign");
            WoodTexture = Content.Load<Texture2D>(ContentFolderTextures + "caja-madera-2");
            StoneTexture = Content.Load<Texture2D>(ContentFolderTextures + "stones");
            MetalTexture = Content.Load<Texture2D>(ContentFolderTextures + "metal");
            FlagCheckeredTexture = Content.Load<Texture2D>(ContentFolderTextures + "CheckeredFlag");
            FlagCheckpointTexture = Content.Load<Texture2D>(ContentFolderTextures + "CheckpointFlag");
            BluePlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformBlue");
            BluePlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformBlueNoStar");
            RedPlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformRed");
            RedPlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformRedNoStar");
            GreenPlatformTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformGreen");
            GreenPlatformBasicTexture = Content.Load<Texture2D>(ContentFolderTextures + "platformGreenNoStar");
            SkyboxTexture = Content.Load<TextureCube>(ContentFolderTextures + "hot_skybox");
            BluePlaceholderTexture = Content.Load<Texture2D>(ContentFolderTextures + "Blue");
            Aluminio = Content.Load<Texture2D>(ContentFolderTextures + "aluminio");
            Empty = Content.Load<Texture2D>(ContentFolderTextures + "Empty");
            WhitePlaceholderTexture = Content.Load<Texture2D>(ContentFolderTextures + "White");

            LavaEffect.Parameters["Texture"].SetValue(LavaTexture);
            LavaEffect.Parameters["tiling"].SetValue(new Vector2(4f, 4f));

            TextureEffect.Parameters["Texture"].SetValue(BluePlaceholderTexture);

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            });

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            //mesh Cartel
            foreach (var mesh in Cartel.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh Cubo
            foreach (var mesh in Cubo.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh bandera
            foreach (var mesh in Flag.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh platform
            foreach (var mesh in Platform.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh esfera
            foreach (var mesh in Esfera.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh tunel
            foreach (var mesh in TunnelChico.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            //mesh pinches
            foreach (var mesh in Pinches.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = TextureEffect;
            //mesh wings
            foreach (var mesh in Wings.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;

            //Plataformas Rotadas
            rotatedPlatformsColliders = new OrientedBoundingBox[20];

            CreateOrientedBoundingBox(0, new Vector3(20f, 2f, 2f), new Vector3(84f, -18f, 30f), Matrix.CreateRotationY(-8f)); //<- Por alguna razon la rotacion tiene que ser el inverso del mesh
            CreateOrientedBoundingBox(1, new Vector3(30f, 2f, 2f), new Vector3(75f, -18f, 85f), Matrix.CreateRotationY(-7.5f));
            CreateOrientedBoundingBox(2, new Vector3(20f, 5f, 8f), new Vector3(75f, -9f, 80f), Matrix.CreateRotationY(-7.5f));
            CreateOrientedBoundingBox(3, new Vector3(15f, 2f, 3f), new Vector3(-3f, -18f, 100f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(4, new Vector3(2f, 1f, 2f), new Vector3(4f, -12f, 115f), Matrix.CreateRotationY(0.436332f)); //Plataforma que sube y baja
            CreateOrientedBoundingBox(5, new Vector3(10f, 2.5f, 3f), new Vector3(-3f, -12f, 100f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(6, new Vector3(7f, 3f, 3f), new Vector3(-5.5f, -6.5f, 98.9f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(7, new Vector3(10f, 2.5f, 3f), new Vector3(-3f, -1f, 100f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(8, new Vector3(18f, 2f, 3f), new Vector3(-36f, -18f, 83f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(9, new Vector3(2f, 5f, 3f), new Vector3(-27f, -18f, 84f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(10, new Vector3(2f, 10f, 3f), new Vector3(-37f, -18f, 81f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(11, new Vector3(2f, 10f, 3f), new Vector3(-52f, -18f, 76f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(12, new Vector3(8f, 1f, 3f), new Vector3(-59f, -9.2f, 72f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(13, new Vector3(5f, 1f, 5f), new Vector3(-70f, -7f, 67.5f), Matrix.CreateRotationY(MathHelper.ToRadians(15f))); //Plataforma que Gira en eje Z
            CreateOrientedBoundingBox(14, new Vector3(5f, 1f, 5f), new Vector3(-78f, -4f, 67.5f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(15, new Vector3(8f, 1f, 3f), new Vector3(-80f, -4f, 67.5f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(16, new Vector3(5f, 10f, 3f), new Vector3(-87.5f, -2f, 65f), Matrix.CreateRotationY(0.436332f));
            CreateOrientedBoundingBox(17, new Vector3(0.5f, 12f, 1f), new Vector3(-6.5f, 18f, 24f), Matrix.CreateRotationX(0f)); //Molino
            CreateOrientedBoundingBox(18, new Vector3(0.5f, 12f, 1f), new Vector3(-6.5f, 18f, 24f), Matrix.CreateRotationX(MathHelper.ToRadians(90f))); //Molino
            CreateOrientedBoundingBox(19, new Vector3(5f, 2f, 5f), new Vector3(30f, -14f, 0f), Matrix.CreateRotationZ(MathHelper.ToRadians(45f))); //Rampa <-- NO FUNCIONA?

            MarbleWorld = MarbleScale * MarbleRotation;

            skybox = new SkyBox(Skybox, SkyboxTexture, SkyboxEffect, SkyBoxSize);

            //Hace que se pegue a la pelota Chica
            PelotaChica1Box = Tp.Collisions.BoundingVolumesExtensions.CreateSphereFrom(Esfera);
            PelotaChica1Box.Center = PelotaChica1Posicion;
            PelotaChica1Box.Radius = 1f;
            //PelotaChica1Box = Tp.Collisions.BoundingVolumesExtensions.Scale(PelotaChica1Box, 0.01f);

            //Power up alas
            var tempCube = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Wings);
            tempCube = Tp.Collisions.BoundingVolumesExtensions.Scale(tempCube, 0.007f);
            
            //power up collision box con fallas, despues lo arreglo
            
            /*AlasBox = Tp.Collisions.OrientedBoundingBox.FromAABB(tempCube);
            AlasBox.Center = AlasPosicion;
            AlasBox.Orientation = Matrix.CreateRotationX(-0.785398f);*/

            //monedas cargadas

            monedas = new Tp.Monedas(Content);



            base.LoadContent();
        }

        private void CreateOrientedBoundingBox(int index, Vector3 scale, Vector3 newCenter, Matrix rotation)
        {
            // Create an OBB for a model
            // First, get an AABB from the model
            var temporaryCubeAABB = CreateAABBFrom(Cubo);
            // Scale it to match the model's transform
            Vector3 center = (temporaryCubeAABB.Max + temporaryCubeAABB.Min) * 0.5f;
            Vector3 extents = (temporaryCubeAABB.Max - temporaryCubeAABB.Min) * 0.5f;
            Vector3 scaledExtents = extents * scale;

            temporaryCubeAABB = new BoundingBox(center - scaledExtents, center + scaledExtents);

            rotatedPlatformsColliders[index] = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            rotatedPlatformsColliders[index].Center = newCenter;
            rotatedPlatformsColliders[index].Orientation = rotation;
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>

        protected override void Update(GameTime gameTime)
        {
            float currentMarbleVelocity = DefaultSpeed;
            //float maxVelocity = currentTypeMarbleVelocity * 2f;
            float deltaX;

            // Mouse State & Keyboard State
            currentMouseState = Mouse.GetState();

            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (currentMouseState != previousMouseState)
            {
                // Cache mouse location
                deltaX = currentMouseState.X - (GraphicsDevice.Viewport.Width / 2);

                // Create the rotation
                mouseRotationBuffer.X -= 0.01f * deltaX * mouseSensitivity;

            }

            // Center the mouse
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            MarbleAcceleration = Vector3.Down * Gravity;

            // Save previous mouse state
            previousMouseState = currentMouseState;
   
            float totalGameTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Aca deberiamos poner toda la logica de actualizacion del juego.
            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Check for the Jump key press, and add velocity in Y only if the marble is on the ground
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && OnGround)
                MarbleVelocity += Vector3.Up * JumpSpeed;
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //MarbleVelocity += Math.min(marbleCopy.Forward * currentTypeMarbleVelocity, marbleCopy.Forward * maxVelocity); //Cambiar Vector3 por CAMARA
                MarbleVelocity += (marbleCopy.Forward * currentMarbleVelocity);
            } 
           
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                MarbleVelocity += marbleCopy.Backward * currentMarbleVelocity; //Cambiar Vector3 por CAMARA
            }
           
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                MarbleVelocity += marbleCopy.Left * currentMarbleVelocity; //Cambiar Vector3 por CAMARA
            }
           
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MarbleVelocity += marbleCopy.Right * currentMarbleVelocity; //Cambiar Vector3 por CAMARA
            }

            TocandoPoderPelotaChica = PelotaChica1Box.Intersects(MarbleSphere);
            if (TocandoPoderPelotaChica)
            {
                TocandoPoderPelotaChica = true;
                currentMarbleVelocity = PelotaRapida;
                //PelotaChica1Posicion = new Vector3 (0, -20, 0);
                MarbleScale = Matrix.CreateScale(0.01f);
                MarbleTexture = Aluminio;
            }
            TocandoAlas = AlasBox.Intersects(MarbleSphere);
            if (TocandoAlas)
            {
                TocandoPoderPelotaChica = true;
                currentMarbleVelocity *= 2f;
            }
            
            MarbleVelocity += MarbleAcceleration * deltaTime;

            float moduloVelocidad = MathF.Sqrt(MathF.Pow(MarbleVelocity.X, 2) + MathF.Pow(MarbleVelocity.Z, 2));

            Vector3 normalNormalizado;

            if (MarbleVelocity.Z == 0 && MarbleVelocity.X == 0)
                normalNormalizado = new Vector3(0, 0, 0);
            else
                normalNormalizado = Vector3.Normalize(new Vector3(MarbleVelocity.Z, 0, -MarbleVelocity.X));
            rotacionAngular += (moduloVelocidad / 0.8f) * deltaTime;
            Matrix rotateArround = Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(normalNormalizado, rotacionAngular));
            

            // Scale the velocity by deltaTime
            var scaledVelocity = MarbleVelocity * deltaTime;

            UpdatePlatformsColliders(totalGameTime);

            // Solve the Vertical Movement first (could be done in other order)
            SolveVerticalMovement(scaledVelocity);

            // Take only the horizontal components of the velocity
            scaledVelocity = new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            SolveHorizontalMovementSliding(scaledVelocity);

            SolveCheckpoint();

            // Update the Position based on the updated Cylinder center
            // Update the Robot World Matrix
            MarblePosition = MarbleSphere.Center;

            marbleCopy = MarbleScale * Matrix.CreateRotationY(mouseRotationBuffer.X) * Matrix.CreateTranslation(MarblePosition);

            Vector3 cameraPosition = marbleCopy.Translation + (marbleCopy.Backward *700 ) + marbleCopy.Up *400;
            MarbleWorld = MarbleScale * rotateArround * Matrix.CreateTranslation(MarblePosition);

            Vector3 cameraLookAt = MarbleWorld.Translation;
            View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);

            Camera.Update(gameTime);

            previousMouseState = currentMouseState;
            base.Update(gameTime);

        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        /// 

        public void DrawMeshes(Matrix matrizDelModelo, Color color, Model modelo)
        {
            foreach (var mesh in modelo.Meshes)
            {
                World = mesh.ParentBone.Transform * matrizDelModelo;
                Effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
        }

        public void DrawMeshes(Matrix matrizDelModelo, Texture2D texture, Model modelo)
        {
            foreach (var mesh in modelo.Meshes)
            {
                World = mesh.ParentBone.Transform * matrizDelModelo;
                TextureEffect.Parameters["Texture"].SetValue(texture);
                TextureEffect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            float totalGameTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(View);///
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            TextureEffect.Parameters["View"].SetValue(View);///
            TextureEffect.Parameters["Projection"].SetValue(Camera.Projection);
            
            // Para el piso
            LavaEffect.Parameters["World"].SetValue(Matrix.Identity);
            LavaEffect.Parameters["View"].SetValue(View);///
            LavaEffect.Parameters["Projection"].SetValue(Camera.Projection);
            LavaEffect.Parameters["Time"].SetValue(totalGameTime);

            foreach (var pass in LavaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);
            }

            //Para el Skybox
            var originalRasterizerState = GraphicsDevice.RasterizerState;
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(Camera.View, Camera.Projection, Camera.Position);

            GraphicsDevice.RasterizerState = originalRasterizerState;

            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            //Jugador
            DrawMeshes(MarbleWorld, MarbleTexture, Esfera);

            ////Se agregan la esferas
            DrawMeshes( ( Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(Rotation * 0.2f) * Matrix.CreateTranslation(new Vector3(-50f, -10f, 0f)) ), MagmaTexture, Esfera);

            DrawMeshes( ( Matrix.CreateScale(0.04f) * Matrix.CreateRotationX(Rotation * 1.5f) * Matrix.CreateTranslation(new Vector3(100f, -0f, -100f) ) * Matrix.CreateRotationZ(Rotation * 0.1f)), LavaTexture, Esfera);

            //Se agrega los tuneles


            //Pista de Obstaculos
            //Nivel 1
            //Principio

            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 15f) * Matrix.CreateTranslation(new Vector3(-10f, -18f, 0f)) ), BluePlatformTexture, Platform);

            //Plataforma con rampa
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -14f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(37f, -11f, 0f)) ), BluePlatformBasicTexture, Platform);


            //Plataforma con Obstaculo
            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -18f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(4f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(70f, -14f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(2f, 4f, 4.9f) * Matrix.CreateTranslation(new Vector3(70f, (-4f * MathF.Cos(totalGameTime)) - 12f, 0f)) ), BluePlatformBasicTexture, Platform);

            //tunel
            DrawMeshes( ( Matrix.CreateScale(0.008f) * Matrix.CreateRotationY(7.9f) * Matrix.CreateTranslation(new Vector3(70f, -12f, 0f)) ), Color.Salmon, TunnelChico);



            //Primer punto de control (bandera)
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(84f, -11f, -4f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(85.8f, -7.5f, -4f)) ), FlagCheckpointTexture, Flag);

            //primera plataforma del nivel 2
            //parte 2.1
            DrawMeshes( ( Matrix.CreateScale(20f, 2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(84f, -18f, 30f)) ), GreenPlatformBasicTexture, Platform); //Este no deberia tener color

            //Transformador a pelota chica, pasa por agujeros chicos
            Vector3 PosicionPelotaChica = TocandoPoderPelotaChica ? new Vector3(0, -20, 0): new Vector3(95f, -10f + MathF.Cos(totalGameTime * 2), 13f)  ;
            DrawMeshes( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionPelotaChica), Aluminio, Esfera);

            // plataforma para power up 1
            DrawMeshes((Matrix.CreateScale(2f, 0.2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(88f, -11.7f, 13f))), GreenPlatformBasicTexture, Platform);
            // plataforma para power up 2
            DrawMeshes((Matrix.CreateScale(2f, 0.2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(95f, -12.3f, 13f))), GreenPlatformBasicTexture, Platform);
            //cubo que necesita pelota chica del nivel 3
            DrawMeshes( ( Matrix.CreateScale(5f, 5f, 5f) * Matrix.CreateTranslation(new Vector3(84f, -7f, 30f)) ), GreenPlatformBasicTexture, Platform);

            //pinches que suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateTranslation(new Vector3(86f, -9f - (-8f * MathF.Cos(totalGameTime)), 40f)) ), SpikesTexture, Pinches);

            //alas de velocidad
            //Vector3 PosicionAlas = TocandoAlas ? new Vector3(0, -20, 0) : new Vector3(86f, -16f, 45f);
            var colorWings = TocandoAlas ? Color.BlueViolet : Color.Transparent;
            DrawMeshes( Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(86f, -16f, 45), colorWings, Wings);

            //parte 2.2
            //Plataforma
            DrawMeshes( ( Matrix.CreateScale(30f, 2f, 2f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -18f, 85f)) ), GreenPlatformBasicTexture, Platform);

            //cubo que necesita pelota chica del nivel 3.1
            DrawMeshes( ( Matrix.CreateScale(20f, 5f, 8f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -9f, 80f)) ), GreenPlatformBasicTexture, Platform);


            //pinches que suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(83f, -7f - (-7f * MathF.Cos(totalGameTime * 2)), 60f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(80f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 1)), 70f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(77f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 2)), 80f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(74f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 3)), 90f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.0008f) * Matrix.CreateRotationZ(3.14159f) * Matrix.CreateRotationY(0.5f) * Matrix.CreateTranslation(new Vector3(71f, -7f - (-7f * MathF.Cos((totalGameTime * 2) - 4)), 100f)) ), SpikesTexture, Pinches);


            //Transformador a pelota de roca, resistente a la lava
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(65f, -13f + MathF.Cos(totalGameTime * 2), 112f)) ), BluePlaceholderTexture, Esfera);


            //parte 2.3
            //plataforma 1 
            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(52f, -18f, 110f)) ), GreenPlatformTexture, Platform);

            //base
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 4f) * Matrix.CreateTranslation(new Vector3(35f, -20f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"1
            DrawMeshes( ( Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(new Vector3(40f, -20f, 110f)) ), BluePlaceholderTexture, Cubo);

            //plataforma 2 
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(23f, -18f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"2
            DrawMeshes( (Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 110f)) ), BluePlaceholderTexture, Cubo);

            //fuente de lava
            DrawMeshes( ( Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(22f, 0f, 110f)) ), MagmaTexture, Cubo);

            //Segundo CheckPoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(16f, -11f, 114f)) ), BluePlaceholderTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(14.2f, -7.5f, 114f)) ), FlagCheckpointTexture, Flag);




            //Nivel 3
            //part 3.1
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -18f, 100f)) ), RedPlatformBasicTexture, Platform);

            //asensor para subir a parte de arriba
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(4f, -12f + (4 * MathF.Cos(totalGameTime * 2)), 115f)) ), RedPlatformTexture, Platform);

            //parte de arriba
            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -12f, 100f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-5.5f, -6.5f, 98.9f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -1f, 100f)) ), RedPlatformBasicTexture, Platform);

            //pelota para ser chica
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(2.5f, -7.5f + MathF.Cos(totalGameTime * 2), 105f)) ), Aluminio, Esfera);

            //parte 3.2
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-36f, -18f, 83f)) ), RedPlatformBasicTexture, Platform);

            //bloque salto 1
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-27f, -18f, 84f)) ), RedPlatformBasicTexture, Platform);

            //pelota para saltar doble
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-32f, -13f + MathF.Cos(totalGameTime * 2), 82f)) ), BluePlaceholderTexture, Esfera);

            //bloque salto 2
            DrawMeshes( ( Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-37f, -18f, 81f)) ), RedPlatformBasicTexture, Platform);

            //bloque salto 3
            DrawMeshes( ( Matrix.CreateScale(2f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-52f, -18f, 76f)) ), RedPlatformBasicTexture, Platform);

            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-59f, -9.2f, 72f)) ), RedPlatformBasicTexture, Platform);

            //plataforma rotando
            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-78f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(8f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-80f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //pinches suben y baja
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(new Vector3(-80f, -9f + (-6f * MathF.Cos(totalGameTime)), 67.5f)) ), SpikesTexture, Pinches);

            //bloque 4
            DrawMeshes( ( Matrix.CreateScale(5f, 10f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-87.5f, -2f, 65f)) ), RedPlatformTexture, Platform);

            //Tercer CheckPoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(-92.5f, 12f, 65f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(-94.2f, 15.5f, 65f)) ), FlagCheckpointTexture, Flag);

            //parte 4
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(3f, 1f, 18f) * Matrix.CreateTranslation(new Vector3(-87.5f, 10f, 42f)) ), RedPlatformBasicTexture, Platform);

            //pinches
            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 49f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 43f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(-90f)) * Matrix.CreateTranslation(new Vector3(-87.5f + (MathF.Cos(totalGameTime) * 8), 13f, 37f)) ), SpikesTexture, Pinches);

            DrawMeshes( ( Matrix.CreateScale(0.001f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-87.5f - (MathF.Cos(totalGameTime) * 8), 13f, 31f)) ), SpikesTexture, Pinches);




            //Parte 4.2
            //plataforma fija
            DrawMeshes( ( Matrix.CreateScale(2f, 0.3f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 0f, 25f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota de roca, resistente a la lava
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-87.5f, 3f + MathF.Cos(totalGameTime * 2), 25f)) ), BluePlaceholderTexture, Esfera);

            //asensor 1
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 8f + (4 * MathF.Cos(totalGameTime)), 20f)) ), RedPlatformTexture, Platform);

            //asensor 2
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 3
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 4
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f)) ), RedPlatformTexture, Platform);

            //fuente de lava
            DrawMeshes((Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(-57.5f, 40f, 17f))), MagmaTexture, Cubo);

            //"lava"2
            DrawMeshes( ( Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 17f)) ), BluePlaceholderTexture, Cubo);

            //asensor 5
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 6
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f)) ), RedPlatformTexture, Platform);


            //Parte 4.3
            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(-25f, 20f, 17f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota normal
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + MathF.Cos(totalGameTime * 2), 17f)) ), BluePlaceholderTexture, Esfera);

            //"lava"1
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-37f, 16f + (3f * MathF.Cos((totalGameTime * 2f) + 4)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"2
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-32f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 3)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"3
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-27f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 2)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"4
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-22f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 1)), 17f)) ), BluePlaceholderTexture, Cubo);

            //"lava"5
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-17f, 16f + (4f * MathF.Cos(totalGameTime * 2f)), 17f)) ), BluePlaceholderTexture, Cubo);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(3f, 1f, 15f) * Matrix.CreateTranslation(new Vector3(2f, 22f, 10f)) ), RedPlatformBasicTexture, Platform);
            
            //Ultimo Checkpoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(0f, 28f, 3f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(1.8f, 31.5f, 3f)) ), FlagCheckeredTexture, Flag);


            //Background
            //Se agregan cubos

            //Molino
            DrawMeshes( ( Matrix.CreateScale(2f, 20f, 2f) * Matrix.CreateTranslation(new Vector3(-4f, 0f, 24f)) ), StoneTexture, Cubo);

            DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);

            DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(20f, 0.5f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(-40f, -18f, 0f)) ), Color.Crimson, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(3f, 3f, 1f) * Matrix.CreateRotationY(MathHelper.ToRadians(75f)) * Matrix.CreateTranslation(new Vector3(-30f, -18f, 10f)) ), Color.Pink, Cubo);

            DrawMeshes( ( Matrix.CreateScale(6f, 6f, 6f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(120f, -12f, 0f)) ), MagmaTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(3f, 3f, 3f) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(-60f, 0f, 100f)) ), MagmaTexture, Cubo);

            //Nubes
            DrawMeshes( ( Matrix.CreateScale(10f, 2f, 10f) * Matrix.CreateTranslation(new Vector3(-30f, 30f, -50f)) ), WhitePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(10f, 2f, 10f) * Matrix.CreateTranslation(new Vector3(30f, 30f, 50f)) ), WhitePlaceholderTexture, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(2f, 2f, 10f) * Matrix.CreateRotationX(MathHelper.ToRadians(45f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -10f, 20f)) ), Color.WhiteSmoke, Cubo);

            //DrawMeshes( ( Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateTranslation(new Vector3(30f, -15f, 20f)) ), Color.RoyalBlue, Cubo);

            DrawMeshes( ( Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(15f, 10f, 85f)) ), MagmaTexture, Cubo);

            //Helicoptero
            DrawMeshes( ( Matrix.CreateScale(1f, 0.1f, 11f) * Matrix.CreateRotationY(Rotation * 10) * Matrix.CreateTranslation(new Vector3(10f, 30f, -20f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 0.1f, 11f) * Matrix.CreateRotationY(Rotation * 10 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(10f, 30f, -20f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(0.5f, 2.7f, 0.5f) * Matrix.CreateTranslation(new Vector3(10f, 28f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateTranslation(new Vector3(8f, 22f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 1f) * Matrix.CreateTranslation(new Vector3(-4f, 24f, -20f)) ), BluePlaceholderTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 5f, 0.1f) * Matrix.CreateRotationZ(Rotation * 5) * Matrix.CreateTranslation(new Vector3(-7f, 24f, -21f)) ), MetalTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(1f, 5f, 0.1f) * Matrix.CreateRotationZ(Rotation * 5 + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-7f, 24f, -21f)) ), MetalTexture, Cubo);
            //se dibuja las monedas
            monedas.Draw(gameTime,View, Projection, totalGameTime, World);
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

        private void SolveVerticalMovement(Vector3 scaledVelocity)
        {
            // If the Robot has vertical velocity
            if (scaledVelocity.Y == 0f)
                return;

            // Start by moving the Cylinder
            MarbleSphere.Center += Vector3.Up * scaledVelocity.Y;
            // Set the OnGround flag on false, update it later if we find a collision
            OnGround = false;

            var collided = false;
            var foundIndex = -1;
            
            for (var index = 0; index < platformColliders.Length; index++)
            {
                if (MarbleSphere.Intersects(platformColliders[index]))
                {
                    // If we collided with something, set our velocity in Y to zero to reset acceleration
                    MarbleVelocity = new Vector3(MarbleVelocity.X, 0f, MarbleVelocity.Z);

                    // Set our index and collision flag to true
                    // The index is to tell which collider the Robot intersects with
                    collided = true;
                    foundIndex = index;
                }
            }

            // We correct based on differences in Y until we don't collide anymore
            // Not usual to iterate here more than once, but could happen
            while (collided)
            {
                var collider = platformColliders[foundIndex];
                var max = collider.Max;
                var min = collider.Min;
                Vector3 center = (max + min) * 0.5f;
                var colliderY = center.Y;
                var marbleY = MarbleSphere.Center.Y;
                Vector3 extents = (max - min) * 0.5f;

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (marbleY > colliderY)
                {
                    penetration = colliderY + extents.Y - marbleY + MarbleSphere.Radius;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -marbleY - MarbleSphere.Radius + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                MarbleSphere.Center += Vector3.Up * penetration;
                collided = false;

                // Check for collisions again
                for (var index = 0; index < platformColliders.Length; index++)
                {
                    if (!MarbleSphere.Intersects(platformColliders[index]))
                        continue;
                    if (IsFloor(platformColliders[index]))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    collided = true;
                    foundIndex = index;
                    break;
                }
            }

            for (var index = 0; index <rotatedPlatformsColliders.Length; index++)
            {
                if (rotatedPlatformsColliders[index].Intersects(MarbleSphere))
                {
                    // If we collided with something, set our velocity in Y to zero to reset acceleration
                    MarbleVelocity = new Vector3(MarbleVelocity.X, 0f, MarbleVelocity.Z);

                    // Set our index and collision flag to true
                    // The index is to tell which collider the Robot intersects with
                    collided = true;
                    foundIndex = index;
                }
            }

            while (collided)
            {
                var collider = rotatedPlatformsColliders[foundIndex];
                Vector3 center = collider.Center;
                var colliderY = center.Y;
                var marbleY = MarbleSphere.Center.Y;
                Vector3 extents = collider.Extents;

                float penetration;
                // If we are on top of the collider, push up
                // Also, set the OnGround flag to true
                if (marbleY > colliderY)
                {
                    penetration = colliderY + extents.Y - marbleY + MarbleSphere.Radius;
                    OnGround = true;
                }

                // If we are on bottom of the collider, push down
                else
                    penetration = -marbleY - MarbleSphere.Radius + colliderY - extents.Y;

                // Move our Cylinder so we are not colliding anymore
                MarbleSphere.Center += Vector3.Up * penetration;
                collided = false;

                // Check for collisions again
                for (var index = 0; index < rotatedPlatformsColliders.Length; index++)
                {
                    if (!rotatedPlatformsColliders[index].Intersects(MarbleSphere))
                        continue;
                    if (IsFloor(rotatedPlatformsColliders[index]))
                        continue;

                    // Iterate until we don't collide with anything anymore
                    collided = true;
                    foundIndex = index;
                    break;
                }
            }
        }

        private void SolveHorizontalMovementSliding(Vector3 scaledVelocity)
        {
            // Has horizontal movement?
            if (Vector3.Dot(scaledVelocity, new Vector3(1f, 0f, 1f)) == 0f)
                return;

            // Start by moving the Cylinder horizontally
            MarbleSphere.Center += new Vector3(scaledVelocity.X, 0f, scaledVelocity.Z);

            // Check intersection for every collider
            for (var index = 0; index < platformColliders.Length; index++)
            {
                if (!MarbleSphere.Intersects(platformColliders[index]))
                    continue;
                if (IsFloor(platformColliders[index]))
                    continue;

                // Get the intersected collider and its center
                var collider = platformColliders[index];
                var max = collider.Max;
                var min = collider.Min;
                Vector3 center = (max + min) * 0.5f;

                // The Robot collided with this thing
                // Is it a step? Can the Robot climb it?
                //bool stepClimbed = SolveStepCollision(collider, index);

                // If the Robot collided with a step and climbed it, stop here
                // Else go on
                //if (stepClimbed)
                  //  return;

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = MarbleSphere.Center;
                sameLevelCenter.Y = center.Y;

                // Find the closest horizontal point from the box
                Vector3 point = sameLevelCenter;
                point.X = MathHelper.Clamp(point.X, min.X, max.X);
                point.Y = MathHelper.Clamp(point.Y, min.Y, max.Y);
                point.Z = MathHelper.Clamp(point.Z, min.Z, max.Z);
                var closestPoint = point;

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                // Our penetration is the difference between the radius of the Cylinder and the Normal Vector
                // For precission problems, we push the cylinder with a small increment to prevent re-colliding into the geometry
                var penetration = MarbleSphere.Radius - normalVector.Length() + EPSILON;

                // Push the center out of the box
                // Normalize our Normal Vector using its length first
                MarbleSphere.Center += (normalVector / normalVectorLength * penetration);
            }

        }

        //respawn logic, te devuelve al ultimo check point y te frena la velocidad a 0.
        private void SolveCheckpoint()
        {
            if(MarbleSphere.Center.Y < -20f)
            {
                MarbleSphere.Center = RespawnPosition;
                MarbleVelocity = Vector3.Zero; 
            }
            
            foreach(BoundingBox checkpoint in checkpoints)
            {
                if(MarbleSphere.Intersects(checkpoint))
                {
                    RespawnPosition = (checkpoint.Max + checkpoint.Min) * 0.5f;
                    checkpoints.Remove(checkpoint);
                    return;
                }
            }
        }
        
        /// <summary>
        ///     Retorna True si el bounding box tiene como penetracion cero, False si tiene distinto de cero
        /// </summary>
        private bool IsFloor(BoundingBox collider)
        {
            var max = collider.Max;
            var min = collider.Min;
            Vector3 center = (max + min) * 0.5f;
            var colliderY = center.Y;
            var marbleY = MarbleSphere.Center.Y;
            Vector3 extents = (max - min) * 0.5f;

            float penetration = 0;

            if (marbleY > colliderY)
            {
                penetration = colliderY + extents.Y - marbleY + MarbleSphere.Radius;
            }
            else
                penetration = -marbleY - MarbleSphere.Radius + colliderY - extents.Y;

            return (Math.Abs(penetration) < EPSILON);
        }

        private bool IsFloor(OrientedBoundingBox collider)
        {
            Vector3 center = collider.Center;
            var colliderY = center.Y;
            var marbleY = MarbleSphere.Center.Y;
            Vector3 extents = collider.Extents;

            float penetration = 0;

            if (marbleY > colliderY)
            {
                penetration = colliderY + extents.Y - marbleY + MarbleSphere.Radius;
            }
            else
                penetration = -marbleY - MarbleSphere.Radius + colliderY - extents.Y;

            return (Math.Abs(penetration) < EPSILON);
        }

        public static BoundingBox CreateAABBFrom(Model model)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            var meshes = model.Meshes;
            for (int index = 0; index < meshes.Count; index++)
            {
                var meshParts = meshes[index].MeshParts;
                for (int subIndex = 0; subIndex < meshParts.Count; subIndex++)
                {
                    var vertexBuffer = meshParts[subIndex].VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[meshes[index].ParentBone.Index];
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }
            return new BoundingBox(minPoint, maxPoint);
        }

        private void UpdatePlatformsColliders(float TotalTime)
        {
            //DrawMeshes( ( Matrix.CreateScale(2f, 4f, 4.9f) * Matrix.CreateTranslation(new Vector3(70f, (-4f * MathF.Cos(totalGameTime)) - 12f, 0f)) ), BluePlatformBasicTexture, Platform);
            MovePlatformBoundingBox(5, TotalTime, (-4f * MathF.Cos(TotalTime)), -12f, 4f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 8f + (8 * MathF.Cos(totalGameTime)), 20f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(14, TotalTime, (4 * MathF.Cos(TotalTime)), 8f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(15, TotalTime, 8 * MathF.Cos((TotalTime * 2) + 2), 8f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(16, TotalTime, 8 * MathF.Cos((TotalTime * 1.5f) + 4), 8f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(17, TotalTime, 8 * MathF.Cos((TotalTime * 3f) + 6), 8f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(18, TotalTime, 8 * MathF.Cos((TotalTime * 2.5f) + 8), 8f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f)) ), RedPlatformTexture, Platform);
            MovePlatformBoundingBox(19, TotalTime, 9 * MathF.Cos((TotalTime * 4f) + 10), 15f, 1f);

            //DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(4f, -12f + (4 * MathF.Cos(totalGameTime * 2)), 115f)) ), RedPlatformTexture, Platform);
            MoveOrientedBoundingBox(4, Matrix.CreateRotationY(0.436332f), new Vector3(4f, -12f + (4 * MathF.Cos(TotalTime * 2)), 115f));
            
            //DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 67.5f)) ), RedPlatformBasicTexture, Platform);
            MoveOrientedBoundingBox(13, Matrix.CreateRotationY(MathHelper.ToRadians(15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * TotalTime)), new Vector3(-70f, -7f, 67.5f));

            //DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);
            MoveOrientedBoundingBox(17, Matrix.CreateRotationX(Rotation), new Vector3(-6.5f, 18f, 24f));

            //DrawMeshes((Matrix.CreateScale(0.5f, 12f, 1f) * Matrix.CreateRotationX(Rotation + MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(new Vector3(-6.5f, 18f, 24f))), WoodTexture, Cubo);
            MoveOrientedBoundingBox(18, Matrix.CreateRotationX(Rotation + MathHelper.ToRadians(90f)), new Vector3(-6.5f, 18f, 24f));
        }

        private void MovePlatformBoundingBox(int index, float TotalTime, float newPosition, float meshHeight, float boxHeight)
        {
            Vector3 yPosMin = new Vector3(platformColliders[index].Min.X, 0f, platformColliders[index].Min.Z);
            Vector3 yPosMax = new Vector3(platformColliders[index].Max.X, boxHeight, platformColliders[index].Max.Z);
            yPosMin.Y = newPosition;
            yPosMax.Y = yPosMin.Y + yPosMax.Y;
            platformColliders[index] = new BoundingBox(new Vector3(0, meshHeight, 0) + yPosMin, new Vector3(0, meshHeight, 0) + yPosMax);
        }

        private void MoveOrientedBoundingBox(int index, Matrix rotation, Vector3 newCenter)
        {
            rotatedPlatformsColliders[index].Center = newCenter;
            rotatedPlatformsColliders[index].Orientation = rotation;
        }
    }
}

// idea obstaculo: El cartel puede ser un obstaculo que si lo hacemos rotar el jugador tendria que evitarlo