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

        public Vector3 Position { get; set; }

        public List<BoundingBox> BoundingBox = new List<BoundingBox>();

        public Matrix EnemyWorld { get; set; }

        public GameModel EnemyModel { get; set; }

        public Effect EnemyEffect { get; set; }

        public Texture EnemyTexture { get; set; }
        public BodyHandle EnemyHandle { get; private set; }
        private float time { get; set; }

        public Vector3 PosDirection { get; private set; }

        public bool Activated;

        public List<Matrix> EnemyListWorld = new List<Matrix>();

        private int ArenaWidth = 200;
        private int ArenaHeight = 200;

        private Random _random = new Random();

        public Enemy(Box enemyBox , NumericVector3 initialPos, Simulation simulation )
        {
            Position  = initialPos;

            PosDirection = initialPos;

            EnemyHandle = simulation.Bodies.Add(
                BodyDescription.CreateConvexDynamic(
           new NumericVector3(0, 0, 0),
           new BodyVelocity(new NumericVector3(0, 0, 0)),
           1,
           simulation.Shapes,
           enemyBox
            ));
        }

        public void LoadContent(ContentManager Content , Simulation simulation) {

            EnemyEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            //EnemyModel = new GameModel(Content.Load<Model>(ContentFolder3D + "weapons/Vehicle"), EnemyEffect, 0.05f, Position, simulation, new Box(7.5f, 5f, 7.5f));

            //EnemyListWorld = EnemyModel.World;

            //EnemyListWorld.ForEach(World => BoundingBox.Add(BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(2.5f, 2.5f, 2.5f) * World)));
        
        }

        public void Update(CarConvexHull MainCar, GameTime gameTime , Simulation simulation) {

            // Calcula la dirección hacia la que debe moverse el auto perseguidor para alcanzar al objetivo
            Vector3 direction = Vector3.Normalize(MainCar.Position - EnemyModel.Position);
            
            // Actualiza la posición del auto perseguidor usando el vector de dirección rotado
            PosDirection += direction * 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            EnemyModel.Position = PosDirection;

            var bodyReference = simulation.Bodies.GetBodyReference(EnemyHandle);
            bodyReference.Awake = true;
            bodyReference.Pose.Position = new NumericVector3(PosDirection.X, PosDirection.Y, PosDirection.Z);
            //bodyReference.Pose = new NumericVector3(PosDirection.X, PosDirection.Y, PosDirection.Z);

            //EnemyModel.World[0] =  Matrix.CreateTranslation(PosDirection);

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

        public void Draw(FollowCamera Camera , GameTime gameTime) {

            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            EnemyEffect.Parameters["World"].SetValue(EnemyListWorld.First());
            EnemyEffect.Parameters["View"].SetValue(Camera.View);
            EnemyEffect.Parameters["Projection"].SetValue(Camera.Projection);
            EnemyEffect.Parameters["ModelTexture"].SetValue(EnemyTexture);
            EnemyEffect.Parameters["Time"]?.SetValue(Convert.ToSingle(time));

            //EnemyModel.Draw(EnemyListWorld , Camera);
        }

        public void Destroy() { }   

    }
}
