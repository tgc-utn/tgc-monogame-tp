using BepuPhysics.Collidables;
using BepuPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
using Microsoft.Xna.Framework;
using NumericVector3 = System.Numerics.Vector3;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using TGC.MonoGame.TP.Camaras;
using static System.Formats.Asn1.AsnWriter;
using System.Transactions;

namespace TGC.MonoGame.TP.Entities
{
    public class Enemy
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        public const string ContentFolderSoundEffects = "SoundEffects/";

        public OrientedBoundingBox EnemyOBB { get; set; }

        public Box EnemyBox { get; set; }

        public Matrix EnemyWorld { get; set; }

        public Model EnemyModel { get; set; }

        public Effect EnemyEffect { get; set; }

        public Texture EnemyTexture { get; set; }

        public BodyHandle EnemyHandle { get; private set; }

        private float time { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 PosDirection { get; private set; }
        public Matrix EnemyOBBPosition { get; private set; }
        public Matrix EnemyOBBWorld { get; private set; }

        public bool Activated;

        private int ArenaWidth = 200;
        private int ArenaHeight = 200;

        private Random _random = new Random();

        public Enemy(Vector3 initialPos)
        {
            Position = initialPos;
        }

        public void LoadContent(ContentManager Content, Simulation simulation)
        {

            EnemyEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            EnemyModel = Content.Load<Model>(ContentFolder3D + "weapons/Vehicle");

            EnemyTexture = ((BasicEffect)EnemyModel.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;

            var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(EnemyModel);
            // Scale it to match the model's transform
            temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 0.001f);
            // Create an Oriented Bounding Box from the AABB
            EnemyOBB = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            // Move the center
            EnemyOBB.Center = Position;

            EnemyBox = new Box(EnemyOBB.Extents.X, EnemyOBB.Extents.Y, EnemyOBB.Extents.Z);

            EnemyHandle = simulation.Bodies.Add(
               BodyDescription.CreateConvexDynamic(
                  new NumericVector3(Position.X, Position.Y, Position.Z),
                  new BodyVelocity(new NumericVector3(0, 0, 0)),
                  10,
                  simulation.Shapes,
                  EnemyBox
                ));

        }

        public void Update(CarConvexHull MainCar, GameTime gameTime, Simulation simulation)
        {
            Vector3 scale;
            Microsoft.Xna.Framework.Quaternion rot;
            Vector3 translation;

            var bodyReference = simulation.Bodies.GetBodyReference(EnemyHandle);
            bodyReference.Awake = true;
            bodyReference.Pose.Position = new NumericVector3(Position.X, Position.Y, Position.Z);

            // Calcula la dirección hacia la que debe moverse el auto perseguidor para alcanzar al objetivo
            Vector3 direction = Vector3.Normalize(MainCar.Position - Position);

            EnemyWorld =  Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(Position);
           
            // Actualiza la posición del auto perseguidor usando el vector de dirección rotado
            PosDirection = direction * 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position  += PosDirection;

            EnemyWorld.Decompose(out scale, out rot, out translation);

            EnemyOBB.Orientation = Matrix.CreateFromQuaternion(rot);

            EnemyOBBPosition = Matrix.CreateTranslation(translation);

            EnemyOBB.Center = translation;

            EnemyOBBWorld = Matrix.CreateScale(EnemyOBB.Extents) *
                            EnemyOBB.Orientation *
                            EnemyOBBPosition;

        }

        public List<Vector3> GenerateRandomPositions(int count, float y)
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(-ArenaWidth, ArenaWidth);
                int z = _random.Next(-ArenaHeight, ArenaHeight);
                positions.Add(new Vector3(x, y, z));
            }

            return positions;
        }

        public List<Vector3> GenerateRandomPositions(int count)
        {
            var positions = new List<Vector3>();

            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(-ArenaWidth, ArenaWidth);
                int z = _random.Next(-ArenaHeight, ArenaHeight);
                positions.Add(new Vector3(x, 0, z));
            }

            return positions;
        }

        public void Draw(FollowCamera Camera, GameTime gameTime)
        {

            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            EnemyEffect.Parameters["World"].SetValue(EnemyWorld);
            EnemyEffect.Parameters["View"].SetValue(Camera.View);
            EnemyEffect.Parameters["Projection"].SetValue(Camera.Projection);
            EnemyEffect.Parameters["ModelTexture"].SetValue(EnemyTexture);
            EnemyEffect.Parameters["Time"]?.SetValue(Convert.ToSingle(time));

            var mesh = EnemyModel.Meshes.FirstOrDefault();
            if (mesh != null)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = EnemyEffect;
                }

                mesh.Draw();
            }
        }

        public void Destroy() { }

    }
}
