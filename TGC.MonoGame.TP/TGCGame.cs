using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using System.Collections.Generic;
using TGC.MonoGame.TP.Quads;
using TGC.MonoGame.TP.SkyBoxs;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.MonedasItem;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.CollisionRuleManagement;

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
        private Monedas monedas { get; set; }

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
        public Texture2D OrangeLiquid { get; set; }
        public Texture2D VolcanicStone { get; set; }
        public TextureCube SkyboxTexture { get; set; }
        private float Rotation { get; set; }
        public bool OnGround { get; private set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private Camera Camera { get; set; }
        public Quad quad { get; set; }
        private SkyBox skybox { get; set; }
        private Vector3 PelotaChica1Posicion { get; set; }
        private Matrix PelotChica1World { get; set; }
        private Vector3 PelotaChica2Posicion { get; set; }
        private Matrix PelotChica2World { get; set; }
        private Vector3 PelotaLavaPosicion { get; set; }
        private Matrix PelotLavaWorld { get; set; }
        private OrientedBoundingBox AlasBox { get; set; }
        private Vector3 AlasPosicion { get; set; }
        private Matrix AlasWorld { get; set; }
        private OrientedBoundingBox lava1Box { get; set; }
        private Vector3 lava1Posicion { get; set; }
        private Matrix lava1World { get; set; }
        private OrientedBoundingBox lava2Box { get; set; }
        private Vector3 lava2Posicion { get; set; }
        private Matrix lava2World { get; set; }
        private OrientedBoundingBox lava3Box { get; set; }
        private Vector3 lava3Posicion { get; set; }
        private Matrix lava3World { get; set; }
        private OrientedBoundingBox lava4Box { get; set; }
        private Vector3 lava4Posicion { get; set; }
        private Matrix lava4World { get; set; }
        private OrientedBoundingBox lava5Box { get; set; }
        private Vector3 lava5Posicion { get; set; }
        private Matrix lava5World { get; set; }
        private OrientedBoundingBox lava6Box { get; set; }
        private Vector3 lava6Posicion { get; set; }
        private Matrix lava6World { get; set; }

        private Box[] platformColliders;
        private Sphere MarbleSphere;

        private OrientedBoundingBox lava7Box { get; set; }
        private Vector3 lava7Posicion { get; set; }
        private Matrix lava7World { get; set; }

        private OrientedBoundingBox lava8Box { get; set; }
        private Vector3 lava8Posicion { get; set; }
        private Matrix lava8World { get; set; }


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
        private bool death { get; set; }
        private bool TocandoPoderPelotaChica { get; set; }
        private bool TocandoPoderPelotaLava { get; set; }
        public Sphere LavaPowerupCollider { get; private set; }
        private bool TocandoLava { get; set; }
        private bool TocandoAlas { get; set; }

        public VertexDeclaration vertexDeclaration { get; set; }
        public Matrix MarbleScale { get; private set; }

        private float JumpSpeed = 10f;
        public float DefaultSpeed = 30f;
        public float PelotaRapida = 5f;
        public float LinearSpeed = 3f;
        public float PelotaLenta = 2f;


        private float SkyBoxSize = 400f;
        private const float EPSILON = 0.00001f;
        private float Gravity = -10f;
        private Matrix marbleCopy;
        private float rotacionAngular;

        private Space space;
        private Sphere AluminioPowerupCollider;
        public Sphere AluminioPowerupCollider2 { get; private set; }

        public CollisionGroupPair MarblePowerUpGroupPair { get; private set; }
        public CollisionGroupPair MarbleCheckpointGroupPair { get; private set; }
        public CollisionGroupPair PlatformCheckpointGroupPair { get; private set; }
        public CollisionGroupPair PlatformPowerUpGroupPair { get; private set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.
            // Seria hasta aca.
            OnGround = false;
            death = false;
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

            MarblePosition = new Vector3(-10f, -10f, 0f); //<- Original
            MarblePosition = new Vector3(3f, 28f, 3f); //<- Para Probar
            RespawnPosition = MarblePosition;
            
            MarbleVelocity = Vector3.Zero;
            MarbleScale = Matrix.CreateScale(0.02f);
            MarbleRotation = Matrix.Identity;
            MarbleFrontDirection = Vector3.Backward;
            MarbleWorld = Matrix.Identity;
            marbleCopy = MarbleWorld;
            mouseRotationBuffer.X = -90;
            rotacionAngular = 0;

            space = new Space();

            //Set up two stacks which go through each other
            var MarbleGroup = new CollisionGroup();
            var PowerUpGroup = new CollisionGroup();
            var CheckpointGroup = new CollisionGroup();
            var PlatformGroup = new CollisionGroup();

            //Adding this rule to the space's collision group rules will prevent entities belong to these two groups from generating collision pairs with each other.
            MarblePowerUpGroupPair = new CollisionGroupPair(MarbleGroup, PowerUpGroup);
            MarbleCheckpointGroupPair = new CollisionGroupPair(MarbleGroup, CheckpointGroup);
            PlatformCheckpointGroupPair = new CollisionGroupPair(CheckpointGroup, PlatformGroup);
            PlatformPowerUpGroupPair = new CollisionGroupPair(PlatformGroup, PowerUpGroup);
            CollisionRules.CollisionGroupRules.Add(MarblePowerUpGroupPair, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(MarbleCheckpointGroupPair, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(PlatformCheckpointGroupPair, CollisionRule.NoBroadPhase);
            CollisionRules.CollisionGroupRules.Add(PlatformPowerUpGroupPair, CollisionRule.NoBroadPhase);

            CreatePlatformsBoxes(PlatformGroup);
            CreateCheckpoints(CheckpointGroup);

            MarbleSphere = new Sphere(new BEPUutilities.Vector3(MarblePosition.X, MarblePosition.Y, MarblePosition.Z), 2f, 1f);
            MarbleSphere.Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
            MarbleSphere.CollisionInformation.CollisionRules.Group = MarbleGroup;

            //powerups bounding boxes
            //PelotaChica1Posicion = new Vector3(95f, -10f, 13f);
            //PelotChica1World = Matrix.CreateTranslation(PelotaChica1Posicion);
            TocandoPoderPelotaChica = false;
            AluminioPowerupCollider = new Sphere(new BEPUutilities.Vector3(95f, -10f, 13f), 1f);
            AluminioPowerupCollider.CollisionInformation.CollisionRules.Group = PowerUpGroup;
            AluminioPowerupCollider.CollisionInformation.Events.DetectingInitialCollision += HandleAluminioPowerUpCollision;
            
            // powerup pelota chica 2 
            //PelotaChica2Posicion = new Vector3(2.5f, -7.5f, 105f);
            //PelotChica2World = Matrix.CreateTranslation(PelotaChica2Posicion);
            AluminioPowerupCollider2 = new Sphere(new BEPUutilities.Vector3(2.5f, -7.5f, 105f), 1f);
            AluminioPowerupCollider2.CollisionInformation.CollisionRules.Group = PowerUpGroup;
            AluminioPowerupCollider2.CollisionInformation.Events.DetectingInitialCollision += HandleAluminioPowerUpCollision;

            //powerups pelota lava
            TocandoPoderPelotaLava = false;
            //PelotaLavaPosicion = new Vector3(65f, -13f, 112f);
            //PelotLavaWorld = Matrix.CreateTranslation(PelotaLavaPosicion);
            LavaPowerupCollider = new Sphere(new BEPUutilities.Vector3(65f, -13f, 112f), 1f);
            LavaPowerupCollider.CollisionInformation.CollisionRules.Group = PowerUpGroup;
            LavaPowerupCollider.CollisionInformation.Events.DetectingInitialCollision += HandleLavaPowerUpCollision;

            // lava
            TocandoLava = false;
            lava1Posicion = new Vector3(40f, -20f, 110f);
            lava1World = Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(lava1Posicion);
            lava2Posicion = new Vector3(22f, -18f, 110f);
            lava2World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava2Posicion);
            lava3Posicion = new Vector3(22f, -18f, 110f);
            lava3World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava3Posicion);
            lava4Posicion = new Vector3(22f, -18f, 110f);
            lava4World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava4Posicion);
            lava5Posicion = new Vector3(22f, -18f, 110f);
            lava5World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava5Posicion); 
            lava6Posicion = new Vector3(22f, -18f, 110f);
            lava6World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava6Posicion);
            lava7Posicion = new Vector3(22f, -18f, 110f);
            lava7World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava7Posicion);
            lava8Posicion = new Vector3(22f, -18f, 110f);
            lava8World = Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(lava8Posicion);
            //Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 17f))

            //powerup alas
            TocandoAlas = false;
            AlasPosicion = new Vector3(86f, -16f, 45f);
            AlasWorld = Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(AlasPosicion);
            //( Matrix.CreateScale(0.007f) * Matrix.CreateRotationX(-0.785398f) * Matrix.CreateTranslation(new Vector3(86f, -16f, 45f)) ), Color.BlueViolet, Wings);

            space.Add(MarbleSphere);
            space.Add(AluminioPowerupCollider);
            space.Add(AluminioPowerupCollider2);
            space.Add(LavaPowerupCollider);
            space.ForceUpdater.Gravity = new BEPUutilities.Vector3(0f, Gravity, 0f);

            base.Initialize();
        }

        private void CreateCheckpoints(CollisionGroup checkpointGroup)
        {
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(82f, -10f, 0f), 20f, 20f, 0.5f); //84f, -11f, -4f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(17f, -10f, 110f), 20f, 20f, 0.5f); //16f, -11f, 114f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(-87.5f, 12f, 65f), 20f, 20f, 0.5f); //-87.5f, 12f, 65f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(3f, 28f, 3f), 0.5f, 20f, 20f); //0f, 28f, 3f
        }

        private void CreateCheckpoint(CollisionGroup checkpointGroup, BEPUutilities.Vector3 pos, float width, float height, float length)
        {
            var checkpoint = new Box(pos, width, height, length);

            checkpoint.CollisionInformation.CollisionRules.Group = checkpointGroup;
            checkpoint.CollisionInformation.Events.DetectingInitialCollision += HandleCheckpointCollision;

            space.Add(checkpoint);
        }

        private void CreatePlatformsBoxes(CollisionGroup platformGroup)
        {
            platformColliders = new Box[43];

            CreatePlatformBox(0, platformGroup, new BEPUutilities.Vector3(-10f, -17f, 0f), 30f, 2f, 30f);
            CreatePlatformBox(1, platformGroup, new BEPUutilities.Vector3(19f, -17f, 0f), 10f, 2f, 10f);
            CreatePlatformBox(2, platformGroup, new BEPUutilities.Vector3(28f, -15f, 0f), 14.9f, 2f, 10f); //Rampa
            platformColliders[2].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, 0f, BEPUutilities.MathHelper.ToRadians(45f));
            CreatePlatformBox(3, platformGroup, new BEPUutilities.Vector3(38f, -9.2f, 0f), 9f, 2f, 10f);
            CreatePlatformBox(4, platformGroup, new BEPUutilities.Vector3(70f, -17f, 0f), 30f, 2f, 10f); //70f, -18f, 0f
            CreatePlatformBox(5, platformGroup, new BEPUutilities.Vector3(70f, -13f, 0f), 8f, 2f, 10f);
            CreatePlatformBox(6, platformGroup, new BEPUutilities.Vector3(70f, -10f, 0f), 2f, 8f, 2f); //Plataforma que sube y baja
            CreatePlatformBox(7, platformGroup, new BEPUutilities.Vector3(84f, -11f, -4f), 0.5f, 8f, 0.5f); //Mastil del Checkpoint 1
            CreatePlatformBox(8, platformGroup, new BEPUutilities.Vector3(84f, -17f, 30f), 40f, 2f, 5f); //rotation: Y -8f
            platformColliders[8].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(8f, 0f, 0f);
            CreatePlatformBox(9, platformGroup, new BEPUutilities.Vector3(84f, -8f, 30f), 10f, 10f, 10f); //84f, -10f, 30f
            CreatePlatformBox(10, platformGroup, new BEPUutilities.Vector3(75f, -17f, 85f), 60f, 2f, 5f);  //75f, -18f, 85f
            platformColliders[10].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(7.5f, 0f, 0f);
            CreatePlatformBox(11, platformGroup, new BEPUutilities.Vector3(75f, -8f, 80f), 40f, 10f, 15f);  //75f, -8f, 80f
            platformColliders[11].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(7.5f, 0f, 0f);
            CreatePlatformBox(12, platformGroup, new BEPUutilities.Vector3(52f, -17f, 110f), 10f, 2f, 10f); //52f, -18f, 110f
            CreatePlatformBox(13, platformGroup, new BEPUutilities.Vector3(35f, -19f, 110f), 40f, 2f, 8f);  //35f, -20f, 110f
            CreatePlatformBox(14, platformGroup, new BEPUutilities.Vector3(23f, -17f, 110f), 16f, 2f, 10f);  //23f, -18f, 110f
            CreatePlatformBox(15, platformGroup, new BEPUutilities.Vector3(16f, -11f, 114f), 0.5f, 8f, 0.5f); //16f, -11f, 119f //Segundo Checkpoint
            CreatePlatformBox(16, platformGroup, new BEPUutilities.Vector3(-3f, -17f, 100f), 30f, 2f, 6f);  //-3f, -18f, 100f
            platformColliders[16].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(17, platformGroup, new BEPUutilities.Vector3(4f, -12f, 115f), 5f, 2f, 5f);  //4f, -12f, 115f //Plataforma que sube y baja
            platformColliders[17].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(18, platformGroup, new BEPUutilities.Vector3(-3f, -10f, 100), 20f, 5f, 6f);  //-3f, -12f, 100
            platformColliders[18].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(19, platformGroup, new BEPUutilities.Vector3(-5.5f, -4.5f, 98.9f), 14f, 5f, 6f);  //-5.5f, -6.5f, 98.9f
            platformColliders[19].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(20, platformGroup, new BEPUutilities.Vector3(-3f, 1f, 100f), 20f, 5f, 6f);  //-3f, -1f, 100f
            platformColliders[20].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(21, platformGroup, new BEPUutilities.Vector3(-36f, -17f, 83f), 36f, 2f, 6f); //-36f, -18f, 83f
            platformColliders[21].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(22, platformGroup, new BEPUutilities.Vector3(-27f, -17f, 84f), 4f, 9f, 6f); //-27f, -18f, 84f
            platformColliders[22].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(23, platformGroup, new BEPUutilities.Vector3(-37f, -17f, 81f), 4f, 18f, 6f); //-37f, -18f, 81f
            platformColliders[23].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(24, platformGroup, new BEPUutilities.Vector3(-52f, -17f, 76f), 4f, 18f, 6f); //-52f, -18f, 76f
            platformColliders[24].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(25, platformGroup, new BEPUutilities.Vector3(-58f, -9f, 72f), 10f, 2f, 6f); //-59f, -9.2f, 72f
            platformColliders[25].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(26, platformGroup, new BEPUutilities.Vector3(-70f, -6f, 67.5f), 10f, 2f, 10f); //-70f, -7f, 67.5f Matrix.CreateRotationY(MathHelper.ToRadians(15f) //Plataforma que Gira en eje Z
            platformColliders[26].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(15f), 0f, 0f);
            CreatePlatformBox(27, platformGroup, new BEPUutilities.Vector3(-80f, -4f, 67.5f), 10f, 2f, 5f); //-80f, -4f, 67.5f
            platformColliders[27].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(28, platformGroup, new BEPUutilities.Vector3(-87.5f, -2f, 65f), 10f, 20f, 6f); //-87.5f, -2f, 65f
            platformColliders[28].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f);
            CreatePlatformBox(29, platformGroup, new BEPUutilities.Vector3(-92.5f, 12f, 65f), 0.5f, 10f, 0.5f);  //-92.5f, 12f, 65f //Tercer Checkpoint
            CreatePlatformBox(30, platformGroup, new BEPUutilities.Vector3(-87.5f, 10f, 42f), 6f, 2f, 36f); //-87.5f, 10f, 42f
            CreatePlatformBox(31, platformGroup, new BEPUutilities.Vector3(-87.5f, 0f, 25f), 4f, 1f, 4f); //-87.5f, 0f, 25f
            CreatePlatformBox(32, platformGroup, new BEPUutilities.Vector3(-87.5f, 8f, 20f), 4f, 2f, 4f); //-87.5f, 8f + (4 * MathF.Cos(totalGameTime)), 20f
            CreatePlatformBox(33, platformGroup, new BEPUutilities.Vector3(-80f, 8f, 17f), 4f, 2f, 4f); //-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f
            CreatePlatformBox(34, platformGroup, new BEPUutilities.Vector3(-72.5f, 8f, 17f), 4f, 2f, 4f); //-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f
            CreatePlatformBox(35, platformGroup, new BEPUutilities.Vector3(-62.5f, 8f, 17f), 4f, 2f, 4f); //-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f
            CreatePlatformBox(36, platformGroup, new BEPUutilities.Vector3(-51f, 8f, 17f), 4f, 2f, 4f); //-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f
            CreatePlatformBox(37, platformGroup, new BEPUutilities.Vector3(-43f, 8f, 17f), 4f, 2f, 4f); //-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f
            CreatePlatformBox(38, platformGroup, new BEPUutilities.Vector3(-25f, 20f, 17f), 30f, 2f, 6f); //-25f, 20f, 17f
            CreatePlatformBox(39, platformGroup, new BEPUutilities.Vector3(-6.5f, 18f, 24f), 1f, 24f, 2f);//-6.5f, 18f, 24f Molino 1
            CreatePlatformBox(40, platformGroup, new BEPUutilities.Vector3(-6.5f, 18f, 24f), 1f, 24f, 2f);//-6.5f, 18f, 24f Molino 2 Roll.MathHelper.ToRadians(90f)
            platformColliders[40].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, MathHelper.ToRadians(90f), 0f);
            CreatePlatformBox(41, platformGroup, new BEPUutilities.Vector3(2f, 22f, 10f), 6f, 2f, 30f); //2f, 22f, 10f
            CreatePlatformBox(42, platformGroup, new BEPUutilities.Vector3(0f, 28f, 3f), 0.5f, 10f, 0.5f); //0f, 28f, 3f //Checkered Flag
        }

        private void CreatePlatformBox(int index, CollisionGroup platformGroup, BEPUutilities.Vector3 pos, float width, float height, float length)
        {
            platformColliders[index] = new Box(pos, width, height, length);
            platformColliders[index].CollisionInformation.CollisionRules.Group = platformGroup;
            platformColliders[index].CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
            space.Add(platformColliders[index]);
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
            OrangeLiquid = Content.Load<Texture2D>(ContentFolderTextures + "Orange_Liquid");
            VolcanicStone = Content.Load<Texture2D>(ContentFolderTextures + "volcanic_stone");

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

            MarbleWorld = MarbleScale * MarbleRotation;

            skybox = new SkyBox(Skybox, SkyboxTexture, SkyboxEffect, SkyBoxSize);

            /*
            //Hace que se pegue a la pelota Chica
            //pelota chica 2
            PelotaChica1Box = Tp.Collisions.BoundingVolumesExtensions.CreateSphereFrom(Esfera);
            PelotaChica1Box.Center = PelotaChica1Posicion;
            PelotaChica1Box.Radius = 1f;
            //PelotaChica1Box = Tp.Collisions.BoundingVolumesExtensions.Scale(PelotaChica1Box, 0.01f);
            //pelota chica 1
            PelotaChica2Box = Tp.Collisions.BoundingVolumesExtensions.CreateSphereFrom(Esfera);
            PelotaChica2Box.Center = PelotaChica2Posicion;
            PelotaChica2Box.Radius = 1f;
            //PowerUp Pelota de lava
            PelotaLavaBox = Tp.Collisions.BoundingVolumesExtensions.CreateSphereFrom(Esfera);
            PelotaLavaBox.Center = PelotaLavaPosicion;
            PelotaLavaBox.Radius = 1f;
            */

            //Power up alas
            var tempCube = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Wings);
            tempCube = Tp.Collisions.BoundingVolumesExtensions.Scale(tempCube, 0.007f);

            //power up collision box con fallas, despues lo arreglo

            AlasBox = OrientedBoundingBox.FromAABB(tempCube);
            AlasBox.Center = AlasPosicion;
            AlasBox.Orientation = Matrix.CreateRotationX(-0.785398f);

            var templava1 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava1 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava1, 0.007f);

            lava1Box = OrientedBoundingBox.FromAABB(templava1);
            lava1Box.Center = lava1Posicion;

            var templava2 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava2 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava2, 0.007f);

            lava2Box = OrientedBoundingBox.FromAABB(templava2);
            lava2Box.Center = lava2Posicion;

            var templava3 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava3 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava3, 0.007f);

            lava3Box = OrientedBoundingBox.FromAABB(templava3);
            lava3Box.Center = lava3Posicion;

            var templava4 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava4 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava4, 0.007f);

            lava4Box = OrientedBoundingBox.FromAABB(templava4);
            lava4Box.Center = lava4Posicion;
            var templava5 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava5 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava5, 0.007f);

            lava5Box = OrientedBoundingBox.FromAABB(templava5);
            lava5Box.Center = lava1Posicion;

            var templava6 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava6 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava6, 0.007f);

            lava6Box = OrientedBoundingBox.FromAABB(templava6);
            lava6Box.Center = lava6Posicion;

            var templava7 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava7 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava7, 0.007f);

            lava7Box = OrientedBoundingBox.FromAABB(templava3);
            lava7Box.Center = lava7Posicion;

            var templava8 = Tp.Collisions.BoundingVolumesExtensions.CreateAABBFrom(Cubo);
            templava8 = Tp.Collisions.BoundingVolumesExtensions.Scale(templava8, 0.007f);

            lava8Box = OrientedBoundingBox.FromAABB(templava8);
            lava8Box.Center = lava8Posicion;

            //monedas cargadas
            monedas = new Monedas(Content);

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>

        protected override void Update(GameTime gameTime)
        {
            float totalGameTime = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);

            UpdatePlatformsColliders(totalGameTime);

            space.Update();

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

            // Save previous mouse state
            previousMouseState = currentMouseState;

            // Aca deberiamos poner toda la logica de actualizacion del juego.
            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Check for the Jump key press, and add velocity in Y only if the marble is on the ground
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && OnGround)
            {
                MarbleSphere.LinearVelocity += BEPUutilities.Vector3.Up * JumpSpeed;
                OnGround = false;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                MarbleSphere.AngularMomentum += new BEPUutilities.Vector3(marbleCopy.Forward.Z, marbleCopy.Forward.Y, -marbleCopy.Forward.X) * currentMarbleVelocity;
                MarbleSphere.LinearVelocity += new BEPUutilities.Vector3(marbleCopy.Forward.X, marbleCopy.Forward.Y, marbleCopy.Forward.Z) * LinearSpeed;
            } 
           
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                MarbleSphere.AngularMomentum += new BEPUutilities.Vector3(marbleCopy.Backward.Z, marbleCopy.Backward.Y, -marbleCopy.Backward.X) * currentMarbleVelocity;
                MarbleSphere.LinearVelocity += new BEPUutilities.Vector3(marbleCopy.Backward.X, marbleCopy.Backward.Y, marbleCopy.Backward.Z) * LinearSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                MarbleSphere.AngularMomentum += new BEPUutilities.Vector3(marbleCopy.Left.Z, marbleCopy.Left.Y, -marbleCopy.Left.X) * currentMarbleVelocity;
                MarbleSphere.LinearVelocity += new BEPUutilities.Vector3(marbleCopy.Left.X, marbleCopy.Left.Y, marbleCopy.Left.Z) * LinearSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MarbleSphere.AngularMomentum += new BEPUutilities.Vector3(marbleCopy.Right.Z, marbleCopy.Right.Y, -marbleCopy.Right.X) * currentMarbleVelocity;
                MarbleSphere.LinearVelocity += new BEPUutilities.Vector3(marbleCopy.Right.X, marbleCopy.Right.Y, marbleCopy.Right.Z) * LinearSpeed;
            }

            /*
            TocandoPoderPelotaLava = PelotaLavaBox.Intersects(MarbleSphere);
            if (TocandoPoderPelotaLava)
            {
                TocandoPoderPelotaLava = true;
                currentMarbleVelocity = PelotaLenta;
                MarbleScale = Matrix.CreateScale(0.03f);
                MarbleTexture = StoneTexture;
            }
            TocandoAlas = AlasBox.Intersects(MarbleSphere);
            if (TocandoAlas)
            {
                TocandoPoderPelotaChica = true;
                MarbleTexture = OrangeLiquid;
                currentMarbleVelocity *= 2f;
            }

            TocandoLava = lava1Box.Intersects(MarbleSphere) || lava2Box.Intersects(MarbleSphere)|| lava3Box.Intersects(MarbleSphere) || lava4Box.Intersects(MarbleSphere)
                || lava5Box.Intersects(MarbleSphere) || lava6Box.Intersects(MarbleSphere) || lava7Box.Intersects(MarbleSphere) || lava8Box.Intersects(MarbleSphere); 
            if (TocandoLava)
            {
                DateTime start = DateTime.Now;
                if(DateTime.Now  > start.AddSeconds(30) && MarbleTexture == StoneTexture)
                {
                    death = true;
                }
                else {
                    death = true;
                }
               
            }
            */

            float moduloVelocidad = MathF.Sqrt(MathF.Pow(MarbleSphere.AngularVelocity.X, 2) + MathF.Pow(MarbleSphere.AngularVelocity.Z, 2));

            Vector3 normalNormalizado;

            //if (MarbleVelocity.Z == 0 && MarbleVelocity.X == 0)
            if (MarbleSphere.AngularVelocity.Z == 0 && MarbleSphere.AngularVelocity.X == 0)
                normalNormalizado = new Vector3(0, 0, 0);
            else
                normalNormalizado = Vector3.Normalize(new Vector3(MarbleSphere.AngularVelocity.X, 0, MarbleSphere.AngularVelocity.Z));
            rotacionAngular += (moduloVelocidad / 0.8f) * deltaTime;
            Matrix rotateArround = Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(normalNormalizado, rotacionAngular));

            SolveCheckpoint();

            MarblePosition = new Vector3(MarbleSphere.Position.X, MarbleSphere.Position.Y, MarbleSphere.Position.Z);

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
            //Principio /

            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 15f) * Matrix.CreateTranslation(new Vector3(-10f, -18f, 0f)) ), BluePlatformTexture, Platform);

            //Plataforma con rampa
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateRotationZ(MathHelper.ToRadians(45f)) * Matrix.CreateTranslation(new Vector3(30f, -14f, 0f)) ), BluePlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(37.1f, -11.1f, 0f)) ), BluePlatformBasicTexture, Platform);


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
            Vector3 PosicionPelotaChica1 = TocandoPoderPelotaChica ? new Vector3(0, -20, 0): new Vector3(95f, -10f + MathF.Cos(totalGameTime * 2), 13f)  ;
            DrawMeshes( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionPelotaChica1), Aluminio, Esfera);

            // plataforma para power up 1
            DrawMeshes((Matrix.CreateScale(2f, 0.2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(88f, -11.7f, 13f))), GreenPlatformBasicTexture, Platform);
            // plataforma para power up 2
            DrawMeshes((Matrix.CreateScale(2f, 0.2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(95f, -12.3f, 13f))), GreenPlatformBasicTexture, Platform);
            //cubo que necesita pelota chica del nivel 3
            DrawMeshes( ( Matrix.CreateScale(5f, 5f, 5f) * Matrix.CreateTranslation(new Vector3(84f, -8f, 30f)) ), GreenPlatformBasicTexture, Platform);

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
            DrawMeshes( ( Matrix.CreateScale(20f, 5f, 8f) * Matrix.CreateRotationY(7.5f) * Matrix.CreateTranslation(new Vector3(75f, -8f, 80f)) ), GreenPlatformBasicTexture, Platform);


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
            DrawMeshes( ( Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(new Vector3(40f, -20f, 110f)) ), MagmaTexture, Cubo);

            //plataforma 2 
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(23f, -18f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"2
            DrawMeshes( (Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 110f)) ), MagmaTexture, Cubo);

            //fuente de lava
            DrawMeshes( ( Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(22f, 0f, 110f)) ), VolcanicStone, Cubo);

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
            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -10f, 100f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-5.5f, -4.5f, 98.9f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, 1f, 100f)) ), RedPlatformBasicTexture, Platform);

            //pelota para ser chica 2
            Vector3 PosicionPelotaChica2 = TocandoPoderPelotaChica ? new Vector3(0, -20, 0) : new Vector3(2.5f, -7.5f + MathF.Cos(totalGameTime * 2), 105f);
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionPelotaChica2) ), Aluminio, Esfera);

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
            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-58f, -9f, 72f)) ), RedPlatformBasicTexture, Platform);

            //plataforma rotando
            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-78f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(5f, 1f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-80f, -4f, 67.5f)) ), RedPlatformBasicTexture, Platform);

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
            DrawMeshes((Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(-57.5f, 40f, 17f))), VolcanicStone, Cubo);

            //"lava"3
            DrawMeshes( ( Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 17f)) ), MagmaTexture, Cubo);

            //asensor 5
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 6
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f)) ), RedPlatformTexture, Platform);


            //Parte 4.3
            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(-25f, 20f, 17f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota normal
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-43.5f, 15f + MathF.Cos(totalGameTime * 2), 17f)) ), BluePlaceholderTexture, Esfera);

            //"lava"4
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-37f, 16f + (3f * MathF.Cos((totalGameTime * 2f) + 4)), 17f)) ), MagmaTexture, Cubo);

            //"lava"5
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-32f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 3)), 17f)) ), MagmaTexture, Cubo);

            //"lava"6
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-27f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 2)), 17f)) ), MagmaTexture, Cubo);

            //"lava"7
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-22f, 16f + (4f * MathF.Cos((totalGameTime * 2f) + 1)), 17f)) ), MagmaTexture, Cubo);

            //"lava"8
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-17f, 16f + (4f * MathF.Cos(totalGameTime * 2f)), 17f)) ), MagmaTexture, Cubo);

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

        //TODO: que no pueda saltar si choca con una pared
        private void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            OnGround = true;
        }

        private void HandleAluminioPowerUpCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            space.Remove(sender.Entity);
            LinearSpeed = PelotaRapida;
            MarbleScale = Matrix.CreateScale(0.01f);
            MarbleSphere.Radius = 1f;
            MarbleTexture = Aluminio;
            TocandoPoderPelotaChica = true;
        }
        private void HandleLavaPowerUpCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            space.Remove(sender.Entity);
            LinearSpeed = PelotaRapida;
            MarbleScale = Matrix.CreateScale(0.03f);
            MarbleSphere.Radius = 3f;
            MarbleTexture = StoneTexture;
            TocandoPoderPelotaLava = true;
        }

        //respawn logic, te devuelve al ultimo check point y te frena la velocidad a 0.
        private void SolveCheckpoint()
        {
            if(MarbleSphere.Position.Y < -20f)
            {
                MarbleSphere.Position = new BEPUutilities.Vector3(RespawnPosition.X, RespawnPosition.Y, RespawnPosition.Z);
                MarbleSphere.AngularVelocity = BEPUutilities.Vector3.Zero;
                MarbleSphere.LinearVelocity = BEPUutilities.Vector3.Zero;
            }
        }

        private void HandleCheckpointCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            BEPUutilities.Vector3 pos = sender.Entity.Position;

            RespawnPosition = new Vector3(pos.X, pos.Y, pos.Z);
            
            space.Remove(sender.Entity);
            return;
        }

        private void UpdatePlatformsColliders(float TotalTime)
        {
            platformColliders[6].Position = new BEPUutilities.Vector3(platformColliders[6].Position.X, (-4f * MathF.Cos(TotalTime)) - 12f, platformColliders[6].Position.Z);

            platformColliders[17].Position = new BEPUutilities.Vector3(platformColliders[17].Position.X, 8f + (8 * MathF.Cos(TotalTime)), platformColliders[17].Position.Z);

            /*
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
            */

            platformColliders[39].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, Rotation, 0f);

            platformColliders[40].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, Rotation + MathHelper.ToRadians(90f), 0f);
        }
    }
}

// idea obstaculo: El cartel puede ser un obstaculo que si lo hacemos rotar el jugador tendria que evitarlo