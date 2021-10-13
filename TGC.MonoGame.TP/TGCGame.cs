using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.Samples.Cameras;
using System.Collections.Generic;
using TGC.MonoGame.TP.Quads;
using TGC.MonoGame.TP.SkyBoxs;
using TGC.MonoGame.TP.MonedasItem;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Entities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.CollisionRuleManagement;
using Microsoft.Xna.Framework.Audio;

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
        public Texture2D NormalTexture { get; private set; }
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
        public Song BGM { get; private set; }
        public SoundEffect JumpSFX { get; private set; }
        public TextureCube SkyboxTexture { get; set; }
        private float Rotation { get; set; }
        public bool OnGround { get; private set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private Camera Camera { get; set; }
        public Quad quad { get; set; }
        private SkyBox skybox { get; set; }

        private Box[] DynamicPlatformColliders;
        private Box[] DynamicLavaColliders;
        private Sphere MarbleSphere;

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
        private float RespawnTimer { get; set; }
        private bool death { get; set; }
        public Sphere LavaPowerupCollider { get; private set; }
        private bool TocandoLava { get; set; }

        public VertexDeclaration vertexDeclaration { get; set; }
        public Matrix MarbleScale { get; private set; }

        private float JumpSpeed = 10f;
        public float DefaultSpeed = 30f;
        public float PelotaRapida = 5f;
        public float PelotaNormal = 3f;
        public float PelotaLenta = 2f;

        public float LinearSpeed = 3f;

        private float SkyBoxSize = 400f;
        private const float EPSILON = 0.00001f;
        private float Gravity = -10f;
        private Matrix marbleCopy;
        private float rotacionAngular;

        private Space space;

        public CollisionGroup MarbleGroup { get; private set; }
        public Sphere AluminioPowerupCollider2 { get; private set; }

        public CollisionGroupPair MarblePowerUpGroupPair { get; private set; }
        public CollisionGroupPair MarbleCheckpointGroupPair { get; private set; }
        public CollisionGroupPair MarbleSpikesGroupPair { get; private set; }
        public CollisionGroupPair MarblePlatformGroupPair { get; private set; }
        public CollisionGroupPair LavaMarbleGroupPair { get; private set; }
        public Box[] SpikesColliders { get; private set; }

        public struct PowerUp
        {
            public Entity Collider;
            public bool Obtained;
        }

        public List<PowerUp> powerUps;

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

            SoundEffect.MasterVolume = 0.4f; //<-- Debe ser Configurable
            MediaPlayer.Volume = 0.3f; //<-- Debe ser Configurable

            MarblePosition = new Vector3(-10f, -10f, 0f); //<- Original
            //MarblePosition = new Vector3(86, -5, 40); //<- Para Probar
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

            MarbleGroup = new CollisionGroup();
            var PowerUpGroup = new CollisionGroup();
            var CheckpointGroup = new CollisionGroup();
            var PlatformGroup = new CollisionGroup();
            var LavaGroup = new CollisionGroup();
            var SpikesGroup = new CollisionGroup();

            MarblePowerUpGroupPair = new CollisionGroupPair(MarbleGroup, PowerUpGroup);
            MarbleCheckpointGroupPair = new CollisionGroupPair(MarbleGroup, CheckpointGroup);
            MarbleSpikesGroupPair = new CollisionGroupPair(MarbleGroup, SpikesGroup);
            MarblePlatformGroupPair = new CollisionGroupPair(MarbleGroup, PlatformGroup);
            LavaMarbleGroupPair = new CollisionGroupPair(LavaGroup, MarbleGroup);

            //Se agregan reglas de colision:
            //- NoSolver: No hay colision, pero si interseccion
            //- NoBroadPhase: No hay colision ni interseccion
            CollisionRules.CollisionGroupRules.Add(MarblePowerUpGroupPair, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(MarbleCheckpointGroupPair, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(MarbleSpikesGroupPair, CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(MarblePlatformGroupPair, CollisionRule.Normal);
            CollisionRules.CollisionGroupRules.Add(LavaMarbleGroupPair, CollisionRule.NoSolver);

            CollisionRules.DefaultCollisionRule = CollisionRule.NoBroadPhase;

            CreatePlatformsBoxes(PlatformGroup);
            CreateCheckpoints(CheckpointGroup);
            CreateLavas(LavaGroup);
            CreatePowerUps(PowerUpGroup);
            CreateSpikes(SpikesGroup);

            MarbleSphere = new Sphere(new BEPUutilities.Vector3(MarblePosition.X, MarblePosition.Y, MarblePosition.Z), 2f, 1f);
            MarbleSphere.Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
            MarbleSphere.CollisionInformation.CollisionRules.Group = MarbleGroup;

            space.Add(MarbleSphere);
            space.ForceUpdater.Gravity = new BEPUutilities.Vector3(0f, Gravity, 0f);

            base.Initialize();
        }

        private void CreateSpikes(CollisionGroup spikesGroup)
        {
            SpikesColliders = new Box[11];

            SpikesColliders[0] = CreateSpike(new BEPUutilities.Vector3(86, -9, 40), 2, 2, 8, spikesGroup);
            SpikesColliders[1] = CreateSpike(new BEPUutilities.Vector3(83, -7, 60), 2, 2, 8, spikesGroup);
            SpikesColliders[2] = CreateSpike(new BEPUutilities.Vector3(80, -7, 70), 2, 2, 8, spikesGroup);
            SpikesColliders[3] = CreateSpike(new BEPUutilities.Vector3(77, -7, 80), 2, 2, 8, spikesGroup);
            SpikesColliders[4] = CreateSpike(new BEPUutilities.Vector3(74, -7, 90), 2, 2, 8, spikesGroup);
            SpikesColliders[5] = CreateSpike(new BEPUutilities.Vector3(71, -7, 100), 2, 2, 8, spikesGroup);
            SpikesColliders[6] = CreateSpike(new BEPUutilities.Vector3(-80, -9, 67.5f), 2, 2, 8, spikesGroup);
            SpikesColliders[7] = CreateSpike(new BEPUutilities.Vector3(-87.5f, 13, 49), 8, 2, 2, spikesGroup);
            SpikesColliders[8] = CreateSpike(new BEPUutilities.Vector3(-87.5f, 13, 43), 8, 2, 2, spikesGroup);
            SpikesColliders[9] = CreateSpike(new BEPUutilities.Vector3(-87.5f, 13, 37), 8, 2, 2, spikesGroup);
            SpikesColliders[10] = CreateSpike(new BEPUutilities.Vector3(-87.5f, 13, 31), 8, 2, 2, spikesGroup);
        }

        private Box CreateSpike(BEPUutilities.Vector3 pos, float width, float height, float length, CollisionGroup group)
        {
            Box Spike = new Box(pos, width, height, length);
            Spike.CollisionInformation.CollisionRules.Group = group;
            Spike.CollisionInformation.Events.DetectingInitialCollision += HandleSpikeContactCollision;
            space.Add(Spike);

            return Spike;
        }

        private void CreatePowerUps(CollisionGroup powerUpGroup)
        {
            powerUps = new List<PowerUp>();

            PowerUp newPowerUp;

            //Aluminio -0
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(95f, -10f, 13f), 1f);
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleAluminioPowerUpCollision;
            powerUps.Add(newPowerUp);

            //Alas -1
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(86f, -16f, 45f), 0.5f, 1f, 0.5f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.785398f, 0f, 0f));
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleWingsPowerUp;
            powerUps.Add(newPowerUp);

            //Piedra -2
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(65f, -13f, 112f), 1f);
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleLavaPowerUpCollision;
            powerUps.Add(newPowerUp);

            //Aluminio2 -3
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(2.5f, -7.5f, 105f), 1f);
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleAluminioPowerUpCollision;
            powerUps.Add(newPowerUp);

            //Piedra2 -4
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(-87.5f, 3f, 25f), 1f);
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleLavaPowerUpCollision;
            powerUps.Add(newPowerUp);

            //Normal -5
            newPowerUp = CreatePowerUp(powerUpGroup, new BEPUutilities.Vector3(-43.5f, 15f, 17f), 1f);
            newPowerUp.Collider.CollisionInformation.Events.DetectingInitialCollision += HandleNormalPowerUpCollision;
            powerUps.Add(newPowerUp);
        }

        private PowerUp CreatePowerUp(CollisionGroup group, BEPUutilities.Vector3 pos, float width, float height, float length, BEPUutilities.Quaternion rotation)
        {
            PowerUp newPowerUp = new PowerUp();

            Box powerUp = new Box(pos, width, height, length);
            powerUp.Orientation = rotation;
            powerUp.CollisionInformation.CollisionRules.Group = group;

            newPowerUp.Collider = powerUp;
            newPowerUp.Obtained = false;

            space.Add(powerUp);

            return newPowerUp;
        }

        private PowerUp CreatePowerUp(CollisionGroup group, BEPUutilities.Vector3 pos, float radius)
        {
            PowerUp newPowerUp = new PowerUp();

            Sphere powerUp = new Sphere(pos, radius);
            powerUp.CollisionInformation.CollisionRules.Group = group;

            newPowerUp.Collider = powerUp;
            newPowerUp.Obtained = false;

            space.Add(powerUp);

            return newPowerUp;
        }

        private void CreateLavas(CollisionGroup lavaGroup)
        {
            RespawnTimer = 99f;
            TocandoLava = false;
            DynamicLavaColliders = new Box[5];

            CreateLava(lavaGroup, new BEPUutilities.Vector3(39f, -18f, 110f), 14f, 2f, 8f);
            CreateLava(lavaGroup, new BEPUutilities.Vector3(22f, -18f, 110f), 6f, 20f, 8f);
            CreateLava(lavaGroup, new BEPUutilities.Vector3(-57.5f, 0f, 17f), 6f, 40f, 8f);
            DynamicLavaColliders[0] = CreateLava(lavaGroup, new BEPUutilities.Vector3(-37f, 18f, 17f), 4f, 10f, 2f);
            DynamicLavaColliders[1] = CreateLava(lavaGroup, new BEPUutilities.Vector3(-32f, 15f, 17f), 4f, 10f, 2f);
            DynamicLavaColliders[2] = CreateLava(lavaGroup, new BEPUutilities.Vector3(-27f, 12f, 17f), 4f, 10f, 2f);
            DynamicLavaColliders[3] = CreateLava(lavaGroup, new BEPUutilities.Vector3(-22f, 12.5f, 17f), 4f, 10f, 2f);
            DynamicLavaColliders[4] = CreateLava(lavaGroup, new BEPUutilities.Vector3(-17f, 16f, 17f), 4f, 10f, 2f);
        }

        private Box CreateLava(CollisionGroup lavaGroup, BEPUutilities.Vector3 pos, float width, float height, float length)
        {
            Box Lava = new Box(pos, width, height, length);
            Lava.CollisionInformation.CollisionRules.Group = lavaGroup;
            Lava.CollisionInformation.Events.DetectingInitialCollision += HandleLavaContactCollision;
            Lava.CollisionInformation.Events.CollisionEnding += HandleLavaExitCollision;
            space.Add(Lava);

            return Lava;
        }

        private void CreateCheckpoints(CollisionGroup checkpointGroup)
        {
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(82f, -10f, 0f), 0.5f, 20f, 20f); //84f, -11f, -4f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(16f, -10f, 110f), 0.5f, 20f, 20f); //16f, -11f, 114f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(-87.5f, 12f, 65f), 0.5f, 20f, 10f); //-87.5f, 12f, 65f
            CreateCheckpoint(checkpointGroup, new BEPUutilities.Vector3(3f, 28f, 3f), 20f, 20f, 0.5f); //0f, 28f, 3f
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
            DynamicPlatformColliders = new Box[45];

            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-10f, -17f, 0f), 30f, 2f, 30f);
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(19f, -17f, 0f), 10f, 2f, 10f);
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(30f, -12.5f, 0f), 8f, 2f, 10f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, 0f, BEPUutilities.MathHelper.ToRadians(45f))); //Rampa 30f, -14f, 0f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(37.1f, -10.1f, 0f), 10f, 2f, 10f); //37.1f, -11.1f, 0f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(70f, -17f, 0f), 30f, 2f, 10f); //70f, -18f, 0f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(70f, -13f, 0f), 8f, 2f, 10f);
            DynamicPlatformColliders[0] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(70f, -12f, 0f), 4f, 8f, 10f); //Plataforma que sube y baja
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(84f, -11f, -4f), 0.5f, 8f, 0.5f); //Mastil del Checkpoint 1
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(84f, -17f, 30f), 40f, 2f, 5f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(8f, 0f, 0f)); //rotation: Y -8f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(84f, -8f, 30f), 10f, 10f, 10f); //84f, -10f, 30f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(75f, -17f, 85f), 60f, 2f, 5f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(7.5f, 0f, 0f));  //75f, -18f, 85f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(75f, -8f, 80f), 40f, 10f, 15f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(7.5f, 0f, 0f));  //75f, -8f, 80f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(64f, -14f, 114.5f), 6f, 2f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-20), MathHelper.ToRadians(90), 0f));
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(52f, -17f, 110f), 10f, 2f, 10f); //52f, -18f, 110f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(35f, -19f, 110f), 40f, 2f, 9f);  //35f, -20f, 110f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(23f, -17f, 110f), 16f, 2f, 10f);  //23f, -18f, 110f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(16f, -11f, 114f), 0.5f, 8f, 0.5f); //16f, -11f, 119f //Segundo Checkpoint
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-3f, -17f, 100f), 30f, 2f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f));  //-3f, -18f, 100f
            DynamicPlatformColliders[1] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(4f, -12f, 115f), 5f, 2f, 5f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f));  //4f, -12f, 115f //Plataforma que sube y baja
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-3f, -10f, 100), 20f, 5f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f));  //-3f, -12f, 100
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-5.5f, -4.5f, 98.9f), 14f, 5f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f));  //-5.5f, -6.5f, 98.9f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-3f, 1f, 100f), 20f, 5f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f));  //-3f, -1f, 100f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-36f, -17f, 83f), 36f, 2f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-36f, -18f, 83f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-27f, -17f, 84f), 4f, 9f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-27f, -18f, 84f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-37f, -17f, 81f), 4f, 18f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-37f, -18f, 81f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-52f, -17f, 76f), 4f, 18f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-52f, -18f, 76f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-58f, -9f, 72f), 10f, 2f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-59f, -9.2f, 72f
            DynamicPlatformColliders[2] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-70f, -6f, 67.5f), 10f, 2f, 10f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(15f), 0f, 0f)); //-70f, -7f, 67.5f Matrix.CreateRotationY(MathHelper.ToRadians(15f) //Plataforma que Gira en eje Z
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-80f, -4f, 67.5f), 10f, 2f, 5f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-80f, -4f, 67.5f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-87.5f, -2f, 65f), 10f, 20f, 6f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(-0.436332f, 0f, 0f)); //-87.5f, -2f, 65f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-92.5f, 12f, 65f), 0.5f, 10f, 0.5f);  //-92.5f, 12f, 65f //Tercer Checkpoint
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-87.5f, 10f, 42f), 6f, 2f, 36f); //-87.5f, 10f, 42f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-87.5f, 0f, 25f), 4f, 1f, 4f); //-87.5f, 0f, 25f
            DynamicPlatformColliders[3] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-87.5f, 8f, 20f), 4f, 2f, 4f); //-87.5f, 8f + (4 * MathF.Cos(totalGameTime)), 20f
            DynamicPlatformColliders[4] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-80f, 8f, 17f), 4f, 2f, 4f); //-80f, 8f + (8 * MathF.Cos((totalGameTime * 2) + 2)), 17f
            DynamicPlatformColliders[5] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-72.5f, 8f, 17f), 4f, 2f, 4f); //-72.5f, 8f + (8 * MathF.Cos((totalGameTime * 1.5f) + 4)), 17f
            DynamicPlatformColliders[6] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-62.5f, 8f, 17f), 4f, 2f, 4f); //-62.5f, 8f + (8 * MathF.Cos((totalGameTime * 3f) + 6)), 17f
            DynamicPlatformColliders[7] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-51f, 8f, 17f), 4f, 2f, 4f); //-51f, 8f + (8 * MathF.Cos((totalGameTime * 2.5f) + 8)), 17f
            DynamicPlatformColliders[8] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-43f, 15f, 17f), 4f, 2f, 4f); //-43f, 15f + (9 * MathF.Cos((totalGameTime * 4f) + 10)), 17f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-25f, 20f, 17f), 30f, 2f, 6f); //-25f, 20f, 17f
            DynamicPlatformColliders[9] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-6.5f, 18f, 24f), 1f, 24f, 2f);//-6.5f, 18f, 24f Molino 1
            DynamicPlatformColliders[10] = CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(-6.5f, 18f, 24f), 1f, 24f, 2f, BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, MathHelper.ToRadians(90f), 0f));//-6.5f, 18f, 24f Molino 2 Roll.MathHelper.ToRadians(90f)
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(2f, 22f, 10f), 6f, 2f, 30f); //2f, 22f, 10f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(0f, 28f, 3f), 0.5f, 10f, 0.5f); //0f, 28f, 3f //Checkered Flag
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(88f, -12.2f, 13f), 4f, 1f, 4f); //88f, -11.7f, 13f
            CreatePlatformBox(platformGroup, new BEPUutilities.Vector3(95f, -12.8f, 13f), 4f, 1f, 4f); //95f, -12.3f, 13f
        }

        private Box CreatePlatformBox(CollisionGroup platformGroup, BEPUutilities.Vector3 pos, float width, float height, float length, BEPUutilities.Quaternion orientation)
        {
            Box platformCollider = new Box(pos, width, height, length);
            platformCollider.Orientation = orientation;
            platformCollider.CollisionInformation.CollisionRules.Group = platformGroup;
            platformCollider.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
            platformCollider.CollisionInformation.Events.CollisionEnded += HandleCollisionExit;
            space.Add(platformCollider);

            return platformCollider;
        }

        private Box CreatePlatformBox(CollisionGroup platformGroup, BEPUutilities.Vector3 pos, float width, float height, float length)
        {
            Box platformCollider = new Box(pos, width, height, length);
            platformCollider.CollisionInformation.CollisionRules.Group = platformGroup;
            platformCollider.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
            space.Add(platformCollider);

            return platformCollider;
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

            NormalTexture = Content.Load<Texture2D>(ContentFolderTextures + "marble");
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

            BGM = Content.Load<Song>(ContentFolderMusic + "SM64BowserRoad");
            MediaPlayer.Play(BGM);

            JumpSFX = Content.Load<SoundEffect>(ContentFolderSounds + "MarbleJump");

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

            MarbleTexture = NormalTexture;
            MarbleWorld = MarbleScale * MarbleRotation;

            skybox = new SkyBox(Skybox, SkyboxTexture, SkyboxEffect, SkyBoxSize);

            //monedas cargadas
            monedas = new Monedas(Content, space, MarbleGroup);

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
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            UpdatePlatformsColliders(totalGameTime);

            space.Update();

            float currentMarbleVelocity = DefaultSpeed;
            //float maxVelocity = currentTypeMarbleVelocity * 2f;
            float deltaX;

            // Mouse State & Keyboard State
            currentMouseState = Mouse.GetState();
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
            USAR FRUsTUM CULLING
            */

            if (TocandoLava)
            {
                RespawnTimer -= deltaTime;
                if(RespawnTimer < 0f)
                    death = true;
            }

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
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Camera.Projection);
            TextureEffect.Parameters["View"].SetValue(View);
            TextureEffect.Parameters["Projection"].SetValue(Camera.Projection);
            
            // Para el piso
            LavaEffect.Parameters["World"].SetValue(Matrix.Identity);
            LavaEffect.Parameters["View"].SetValue(View);
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

            DrawMeshes( ( Matrix.CreateScale(2f, 4f, 4.9f) * Matrix.CreateTranslation(new Vector3(70f, (-4f * MathF.Cos(totalGameTime + MathHelper.PiOver2)) - 12f, 0f)) ), BluePlatformBasicTexture, Platform);

            //tunel
            DrawMeshes( ( Matrix.CreateScale(0.008f) * Matrix.CreateRotationY(7.9f) * Matrix.CreateTranslation(new Vector3(70f, -12f, 0f)) ), Color.Salmon, TunnelChico);



            //Primer punto de control (bandera)
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(84f, -11f, -4f)) ), WoodTexture, Cubo);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(85.8f, -7.5f, -4f)) ), FlagCheckpointTexture, Flag);

            //primera plataforma del nivel 2
            //parte 2.1
            DrawMeshes( ( Matrix.CreateScale(20f, 2f, 2f) * Matrix.CreateRotationY(8f) * Matrix.CreateTranslation(new Vector3(84f, -18f, 30f)) ), GreenPlatformBasicTexture, Platform); //Este no deberia tener color

            //Transformador a pelota chica, pasa por agujeros chicos
            Vector3 PosicionPelotaChica1 = powerUps[0].Obtained ? new Vector3(0, -20, 0): new Vector3(95f, -10f + MathF.Cos(totalGameTime * 2), 13f)  ;
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
            var colorWings = powerUps[1].Obtained ? Color.Transparent : Color.BlueViolet;
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
            Vector3 PosicionGrande1 = powerUps[2].Obtained ? new Vector3(0, -20, 0) : new Vector3(65f, -13f + MathF.Cos(totalGameTime * 2), 112f);
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionGrande1) ), StoneTexture, Esfera);

            DrawMeshes((Matrix.CreateScale(3f, 1f, 3f) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(-20), MathHelper.ToRadians(90), 0f) * Matrix.CreateTranslation(new Vector3(64f, -14f, 114.5f))), GreenPlatformTexture, Platform);

            //parte 2.3
            //plataforma 1 
            DrawMeshes( ( Matrix.CreateScale(5f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(52f, -18f, 110f)) ), GreenPlatformTexture, Platform);

            //base
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 4.5f) * Matrix.CreateTranslation(new Vector3(35f, -20f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"1
            DrawMeshes( ( Matrix.CreateScale(10f, 3f, 4f) * Matrix.CreateTranslation(new Vector3(40f, -20f, 110f)) ), MagmaTexture, Cubo);

            //plataforma 2 
            DrawMeshes( ( Matrix.CreateScale(8f, 2f, 5f) * Matrix.CreateTranslation(new Vector3(23f, -18f, 110f)) ), GreenPlatformBasicTexture, Platform);

            //"lava"2
            DrawMeshes( (Matrix.CreateScale(3f, 20f, 4f) * Matrix.CreateTranslation(new Vector3(22f, -18f, 110f)) ), MagmaTexture, Cubo);

            //fuente de lava
            DrawMeshes( ( Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(22f, 0f, 110f)) ), VolcanicStone, Cubo);

            //Segundo CheckPoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(16f, -11f, 114f)) ), WoodTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(4f, 3f, 0.2f) * Matrix.CreateTranslation(new Vector3(14.2f, -7.5f, 114f)) ), FlagCheckpointTexture, Flag);




            //Nivel 3
            //part 3.1
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(15f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -18f, 100f)) ), RedPlatformBasicTexture, Platform);

            //asensor para subir a parte de arriba
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(4f, -12f + (4 * MathF.Cos((totalGameTime * 2) + MathHelper.PiOver2)), 115f)) ), RedPlatformTexture, Platform);

            //parte de arriba
            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, -10f, 100f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(7f, 3f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-5.5f, -4.5f, 98.9f)) ), RedPlatformBasicTexture, Platform);

            DrawMeshes( ( Matrix.CreateScale(10f, 2.5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-3f, 1f, 100f)) ), RedPlatformBasicTexture, Platform);

            //pelota para ser chica 2
            Vector3 PosicionPelotaChica2 = powerUps[3].Obtained ? new Vector3(0, -20, 0) : new Vector3(2.5f, -7.5f + MathF.Cos(totalGameTime * 2), 105f);
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionPelotaChica2) ), Aluminio, Esfera);

            //parte 3.2
            //plataforma 1
            DrawMeshes( ( Matrix.CreateScale(18f, 2f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-36f, -18f, 83f)) ), RedPlatformBasicTexture, Platform);

            //bloque salto 1
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 3f) * Matrix.CreateRotationY(-0.436332f) * Matrix.CreateTranslation(new Vector3(-27f, -18f, 84f)) ), RedPlatformBasicTexture, Platform);

            //pelota para saltar doble
            //DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(-32f, -13f + MathF.Cos(totalGameTime * 2), 82f)) ), BluePlaceholderTexture, Esfera);

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
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(-92.5f, 12f, 65f)) ), WoodTexture, Cubo);

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
            Vector3 PosicionGrande2 = powerUps[4].Obtained ? new Vector3(0, -20, 0) : new Vector3(-87.5f, 3f + MathF.Cos(totalGameTime * 2), 25f);
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionGrande2) ), StoneTexture, Esfera);

            //asensor 1
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-87.5f, 8f + (4 * MathF.Cos(totalGameTime + MathHelper.PiOver2)), 20f)) ), RedPlatformTexture, Platform);

            //asensor 2
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-80f, 8f + (8 * MathF.Cos(totalGameTime * 2 + MathHelper.PiOver2)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 3
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-72.5f, 8f + (8 * MathF.Cos(totalGameTime * 1.5f + MathHelper.PiOver2)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 4
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-62.5f, 8f + (8 * MathF.Cos(totalGameTime * 3f + MathHelper.PiOver2)), 17f)) ), RedPlatformTexture, Platform);

            //fuente de lava
            DrawMeshes((Matrix.CreateScale(5f, 3f, 5f) * Matrix.CreateTranslation(new Vector3(-57.5f, 40f, 17f))), VolcanicStone, Cubo);

            //"lava"3
            DrawMeshes( ( Matrix.CreateScale(3f, 40f, 4f) * Matrix.CreateTranslation(new Vector3(-57.5f, 0f, 17f)) ), MagmaTexture, Cubo);

            //asensor 5
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-51f, 8f + (8 * MathF.Cos(totalGameTime * 2.5f + MathHelper.PiOver2)), 17f)) ), RedPlatformTexture, Platform);

            //asensor 6
            DrawMeshes( ( Matrix.CreateScale(2f, 1f, 2f) * Matrix.CreateTranslation(new Vector3(-43f, 15f + (9 * MathF.Cos(totalGameTime * 4f + MathHelper.PiOver2)), 17f)) ), RedPlatformTexture, Platform);


            //Parte 4.3
            //plataforma 2
            DrawMeshes( ( Matrix.CreateScale(15f, 1f, 3f) * Matrix.CreateTranslation(new Vector3(-25f, 20f, 17f)) ), RedPlatformBasicTexture, Platform);

            //Transformador a pelota normal
            Vector3 PosicionNormal1 = powerUps[5].Obtained ? new Vector3(0, -20, 0) : new Vector3(-43.5f, 15f + MathF.Cos(totalGameTime * 2), 17f);
            DrawMeshes( ( Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(PosicionNormal1) ), NormalTexture, Esfera);

            //"lava"4
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-37f, 16f + (4f * MathF.Cos((totalGameTime * 2f + MathHelper.PiOver2) + 4)), 17f)) ), MagmaTexture, Cubo);

            //"lava"5
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-32f, 16f + (4f * MathF.Cos((totalGameTime * 2f + MathHelper.PiOver2) + 3)), 17f)) ), MagmaTexture, Cubo);

            //"lava"6
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-27f, 16f + (4f * MathF.Cos((totalGameTime * 2f + MathHelper.PiOver2) + 2)), 17f)) ), MagmaTexture, Cubo);

            //"lava"7
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-22f, 16f + (4f * MathF.Cos((totalGameTime * 2f + MathHelper.PiOver2) + 1)), 17f)) ), MagmaTexture, Cubo);

            //"lava"8
            DrawMeshes( ( Matrix.CreateScale(2f, 5f, 1f) * Matrix.CreateTranslation(new Vector3(-17f, 16f + (4f * MathF.Cos(totalGameTime * 2f + MathHelper.PiOver2)), 17f)) ), MagmaTexture, Cubo);

            //plataforma 3
            DrawMeshes( ( Matrix.CreateScale(3f, 1f, 15f) * Matrix.CreateTranslation(new Vector3(2f, 22f, 10f)) ), RedPlatformBasicTexture, Platform);
            
            //Ultimo Checkpoint
            DrawMeshes( ( Matrix.CreateScale(0.2f, 5f, 0.2f) * Matrix.CreateTranslation(new Vector3(0f, 28f, 3f)) ), WoodTexture, Cubo);

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
            
            //se dibujan las monedas
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

        private void HandleCollisionExit(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            OnGround = false;
        }

        private void HandleAluminioPowerUpCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            UpdatePowerUpStatus(sender.Entity);

            space.Remove(sender.Entity);
            LinearSpeed = PelotaRapida;
            MarbleScale = Matrix.CreateScale(0.01f);
            MarbleSphere.Radius = 1f;
            MarbleTexture = Aluminio;
        }

        private void HandleLavaPowerUpCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            UpdatePowerUpStatus(sender.Entity);

            space.Remove(sender.Entity);
            LinearSpeed = PelotaLenta;
            MarbleScale = Matrix.CreateScale(0.03f);
            MarbleSphere.Radius = 3f;
            MarbleTexture = StoneTexture;
        }
        private void HandleWingsPowerUp(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            UpdatePowerUpStatus(sender.Entity);

            LinearSpeed = PelotaLenta * 3;
        }

        private void HandleNormalPowerUpCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            UpdatePowerUpStatus(sender.Entity);

            space.Remove(sender.Entity);
            LinearSpeed = PelotaNormal;
            MarbleScale = Matrix.CreateScale(0.02f);
            MarbleSphere.Radius = 2f;
            MarbleTexture = NormalTexture;
        }

        private void HandleLavaContactCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            TocandoLava = true;
            if (MarbleTexture == StoneTexture)
                RespawnTimer = 30f;
            else
                RespawnTimer = 0f;
        }

        private void HandleLavaExitCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            TocandoLava = false;
        }

        private void HandleSpikeContactCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            death = true;
        }

        private void UpdatePowerUpStatus(Entity entity)
        {
            int index = powerUps.FindIndex(x => entity.Equals(x.Collider));
            PowerUp powerUp = powerUps[index];
            powerUp.Obtained = true;
            powerUps.RemoveAt(index);
            powerUps.Insert(index, powerUp);
        }

        //respawn logic, te devuelve al ultimo check point y te frena la velocidad a 0.
        private void SolveCheckpoint()
        {
            if(MarbleSphere.Position.Y < -20f || death)
            {
                MarbleSphere.Position = new BEPUutilities.Vector3(RespawnPosition.X, RespawnPosition.Y, RespawnPosition.Z);
                MarbleSphere.AngularVelocity = BEPUutilities.Vector3.Zero;
                MarbleSphere.LinearVelocity = BEPUutilities.Vector3.Zero;
                death = false;
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
            //No se puede actualizar posicion
            //platformColliders[6].Position = new BEPUutilities.Vector3(platformColliders[6].Position.X, (-4f * MathF.Cos(TotalTime)) - 12f, platformColliders[6].Position.Z);
            DynamicPlatformColliders[0].LinearVelocity = new BEPUutilities.Vector3(0, 4f * MathF.Cos(TotalTime), 0);

            //-12f + (4 * MathF.Cos(totalGameTime * 2))
            DynamicPlatformColliders[1].LinearVelocity = new BEPUutilities.Vector3(0, -8f * MathF.Cos(TotalTime * 2f), 0);

            //DrawMeshes( ( Matrix.CreateScale(5f, 1f, 5f) * Matrix.CreateRotationY(MathHelper.ToRadians(-15f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-25f * totalGameTime)) * Matrix.CreateTranslation(new Vector3(-70f, -7f, 67.5f)) ), RedPlatformBasicTexture, Platform);
            DynamicPlatformColliders[2].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-15f), MathHelper.ToRadians(-25f * TotalTime), 0f); //<- ARREGLAR

            //8f + (4 * MathF.Cos(totalGameTime + MathHelper.PiOver2))
            DynamicPlatformColliders[3].LinearVelocity = new BEPUutilities.Vector3(0, -4f * MathF.Cos(TotalTime), 0);

            //8 * MathF.Cos(TotalTime * 2 + MathHelper.PiOver2);
            DynamicPlatformColliders[4].LinearVelocity = new BEPUutilities.Vector3(0, -16f * MathF.Cos(TotalTime * 2), 0);

            //8f + (8 * MathF.Cos(totalGameTime * 1.5f + MathHelper.PiOver2))
            DynamicPlatformColliders[5].LinearVelocity = new BEPUutilities.Vector3(0, -12f * MathF.Cos(TotalTime * 1.5f), 0);

            //8f + (8 * MathF.Cos(totalGameTime * 3f + MathHelper.PiOver2))
            DynamicPlatformColliders[6].LinearVelocity = new BEPUutilities.Vector3(0, -24f * MathF.Cos(TotalTime * 3f), 0);

            //8f + (8 * MathF.Cos(totalGameTime * 2.5f + MathHelper.PiOver2))
            DynamicPlatformColliders[7].LinearVelocity = new BEPUutilities.Vector3(0, -20f * MathF.Cos(TotalTime * 2.5f), 0);

            //15f + (9 * MathF.Cos(totalGameTime * 4f + MathHelper.PiOver2))
            DynamicPlatformColliders[8].LinearVelocity = new BEPUutilities.Vector3(0, -36f * MathF.Cos(TotalTime * 4f), 0);

            DynamicPlatformColliders[9].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, Rotation, 0f);

            DynamicPlatformColliders[10].Orientation = BEPUutilities.Quaternion.CreateFromYawPitchRoll(0f, Rotation + MathHelper.ToRadians(90f), 0f);

            //Lava
            DynamicLavaColliders[0].LinearVelocity = new BEPUutilities.Vector3(0, -8 * MathF.Cos((TotalTime * 2f) + 4), 0);

            DynamicLavaColliders[1].LinearVelocity = new BEPUutilities.Vector3(0, -8 * MathF.Cos((TotalTime * 2f) + 3), 0);

            DynamicLavaColliders[2].LinearVelocity = new BEPUutilities.Vector3(0, -8 * MathF.Cos((TotalTime * 2f) + 2), 0);

            DynamicLavaColliders[3].LinearVelocity = new BEPUutilities.Vector3(0, -8 * MathF.Cos((TotalTime * 2f) + 1), 0);

            DynamicLavaColliders[4].LinearVelocity = new BEPUutilities.Vector3(0, -8 * MathF.Cos((TotalTime * 2f)), 0);

            //Spikes
            SpikesColliders[0].Position = new BEPUutilities.Vector3(SpikesColliders[0].Position.X, -9f - (-8f * MathF.Cos(TotalTime)), SpikesColliders[0].Position.Z);

            SpikesColliders[1].Position = new BEPUutilities.Vector3(SpikesColliders[1].Position.X, -7f - (-7f * MathF.Cos(TotalTime * 2)), SpikesColliders[1].Position.Z);

            SpikesColliders[2].Position = new BEPUutilities.Vector3(SpikesColliders[2].Position.X, -7f - (-7f * MathF.Cos(TotalTime * 2) - 1), SpikesColliders[2].Position.Z);

            SpikesColliders[3].Position = new BEPUutilities.Vector3(SpikesColliders[3].Position.X, -7f - (-7f * MathF.Cos(TotalTime * 2) - 2), SpikesColliders[3].Position.Z);

            SpikesColliders[4].Position = new BEPUutilities.Vector3(SpikesColliders[4].Position.X, -7f - (-7f * MathF.Cos(TotalTime * 2) - 3), SpikesColliders[4].Position.Z);

            SpikesColliders[5].Position = new BEPUutilities.Vector3(SpikesColliders[5].Position.X, -7f - (-7f * MathF.Cos(TotalTime * 2) - 4), SpikesColliders[5].Position.Z);

            SpikesColliders[6].Position = new BEPUutilities.Vector3(SpikesColliders[6].Position.X, -9f + (-6f * MathF.Cos(TotalTime)), SpikesColliders[6].Position.Z);

            SpikesColliders[7].Position = new BEPUutilities.Vector3(-87.5f + (MathF.Cos(TotalTime) * 8), SpikesColliders[7].Position.Y, SpikesColliders[7].Position.Z);

            SpikesColliders[8].Position = new BEPUutilities.Vector3(-87.5f - (MathF.Cos(TotalTime) * 8), SpikesColliders[8].Position.Y, SpikesColliders[8].Position.Z);

            SpikesColliders[9].Position = new BEPUutilities.Vector3(-87.5f + (MathF.Cos(TotalTime) * 8), SpikesColliders[9].Position.Y, SpikesColliders[9].Position.Z);

            SpikesColliders[10].Position = new BEPUutilities.Vector3(-87.5f - (MathF.Cos(TotalTime) * 8), SpikesColliders[10].Position.Y, SpikesColliders[10].Position.Z);
        }
    }
}

// idea obstaculo: El cartel puede ser un obstaculo que si lo hacemos rotar el jugador tendria que evitarlo