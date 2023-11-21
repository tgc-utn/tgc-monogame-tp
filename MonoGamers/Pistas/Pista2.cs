using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using MonoGamers.Geometries.Textures;
using BepuPhysics;
using NumericVector3 = System.Numerics.Vector3;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using MonoGamers.Utilities;
using MonoGamers.PowerUps;

namespace MonoGamers.Pistas
{
    public class Pista2
    {
        // _____ Geometries _______
        private BoxPrimitive BoxPrimitive { get; set; }

        // ____ World matrices ____
        // Plataformas principales
        private Matrix[] Platforms { get; set; }

        //Obstaculos movibles
        private Matrix[] MovingBoxes { get; set; }
        private BodyHandle[] MovingBoxesBodyHandle { get; set; }
        private Vector3 MovingBoxesScale { get; set; }
        private float InitialLeftXPositionMovingBoxes;
        private float InitialRigthXPositionMovingBoxes;
        private float LengthDistanceDifference = 150f;

        //Rush powerup
        private Model RushModel { get; set; }
        private Matrix[] RushPowerups { get; set; }

        private Texture2D RushTexture { get; set; }

        // GraphicsDevice
        private GraphicsDevice GraphicsDevice { get; set; }

        // Simulation           
        private Simulation Simulation { get; set; }

        //efects

        public Effect Effect { get; set; }
        public Effect EffectB { get; set; }

        private Texture2D CobbleTexture { get; set; }
        private Texture2D FloorTexture { get; set; }
        private Texture2D FloorNormalTexture { get; set; }


        public Pista2(ContentManager Content, GraphicsDevice graphicsDevice, float x, float y, float z, Simulation simulation)
        {
            GraphicsDevice = graphicsDevice;
            Simulation = simulation;
            Initialize(x, y, z);

            LoadContent(Content);

        }

        private void Initialize(float x, float y, float z)
        {
            Vector3 scale;
            Quaternion rot;
            Vector3 translation;
            //powerUps

            Matrix basicRush = Matrix.CreateScale(0.2f, 0.2f, 0.2f) * Matrix.CreateRotationX(1.5707f);
            RushPowerups = new Matrix[]
            {
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x, y + 30f, z + 210f)
            };


            //lista de plataformas
            Platforms = new Matrix[]
            {
                Matrix.CreateScale(300f, 5f, 500f) * Matrix.CreateTranslation(x, y, z),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 1000f),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 1500f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x, y, z + 1750f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 100f, y + 45f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 200f, y + 90f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 300f, y + 135f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 400f, y + 170f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 550f, y + 170f, z + 1750f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 850f, y + 170f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1200f, y + 100f, z + 1750f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 1450f, y + 100f, z + 1750f),
                Matrix.CreateScale(500f, 5f, 500f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 2200f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 2550f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1900f, y + 140f, z + 2550f),

            };

            for (int index = 0; index < Platforms.Length; index++)
            {
                var matrix = Platforms[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                    Simulation.Shapes.Add(new Box(scale.X, scale.Y, scale.Z))));

            }

            //Cajas movibles
            MovingBoxes = new Matrix[]
            {
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x + 150f, y + 50f, z + 1230f),
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x - 150f, y + 50f, z + 1270f)
            };

            MovingBoxesBodyHandle = new BodyHandle[MovingBoxes.Length];
            MovingBoxesScale = new Vector3(100f, 100f, 20f);
            InitialLeftXPositionMovingBoxes = x + 150f;
            InitialRigthXPositionMovingBoxes = x - 150f;



            for (int index = 0; index < MovingBoxes.Length; index++)
            {
                var matrix = MovingBoxes[index];

                matrix.Decompose(out scale, out rot, out translation);

                System.Numerics.Quaternion rotationMB = new System.Numerics.Quaternion(rot.X, rot.Y, rot.Z, rot.W);

                var initialPosMw = new RigidPose(Utils.ToNumericVector3(translation), rotationMB);

                var annoyingMovingWallsShape = new Box(MovingBoxesScale.X, MovingBoxesScale.Y, MovingBoxesScale.Z);
                MovingBoxesBodyHandle[index] = (
                    Simulation.Bodies.Add(BodyDescription.CreateKinematic(initialPosMw,
                        new CollidableDescription(Simulation.Shapes.Add(annoyingMovingWallsShape), 0.1f,
                            ContinuousDetection.Passive),
                        new BodyActivityDescription(-0.1f))));

            }
        }

        private void LoadContent(ContentManager Content)
        {
            // Cargar Texturas
            CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");

            FloorTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/grass");
            FloorNormalTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/grass-normal");

             EffectB = Content.Load<Effect>(
                 ConfigurationManager.AppSettings["ContentFolderEffects"] + "BasicShader");

            RushTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "plastic/color");

            //Carga modelo Rush
            RushModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "arrowpush/tinker");

            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
        }

        public void Update()
        {

            // AnnoyingMovingWalls
            for (int index = 0; index < MovingBoxesBodyHandle.Length; index++)
            {
                var MovingBoxesBodyRef = Simulation.Bodies.GetBodyReference(MovingBoxesBodyHandle[index]);
                if (index % 2 == 0)
                {
                    if (MovingBoxesBodyRef.Pose.Position.X >= InitialLeftXPositionMovingBoxes)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 70f));
                    if (MovingBoxesBodyRef.Pose.Position.X <= InitialLeftXPositionMovingBoxes - LengthDistanceDifference)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 70f));
                }
                else
                {
                    if (MovingBoxesBodyRef.Pose.Position.X <= InitialRigthXPositionMovingBoxes)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 70f));
                    if (MovingBoxesBodyRef.Pose.Position.X >= InitialRigthXPositionMovingBoxes + LengthDistanceDifference)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 70f));
                }

            }
        }

        public void Draw(Camera.Camera camera)
        {


            var viewProjection = camera.View * camera.Projection;

            Effect.Parameters["eyePosition"].SetValue(camera.Position);
            Effect.Parameters["Tiling"].SetValue(new Vector2(1f, 1f));
            Effect.Parameters["ModelTexture"].SetValue(FloorTexture);
            Effect.Parameters["NormalTexture"]?.SetValue(FloorNormalTexture);


            Effect.Parameters["KAmbient"]?.SetValue(0.5f);
            Effect.Parameters["KDiffuse"].SetValue(0.3f);
            Effect.Parameters["shininess"].SetValue(16.0f);
            Effect.Parameters["KSpecular"].SetValue(0.2f);

            // Draw Platforms
            for (int index = 0; index < Platforms.Length; index++)
            {
                var matrix = Platforms[index];
                Effect.Parameters["World"].SetValue(matrix);
                Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(matrix)));
                Effect.Parameters["WorldViewProjection"].SetValue(matrix * viewProjection);
                BoxPrimitive.Draw(Effect);

            }
            EffectB.Parameters["View"].SetValue(camera.View);
            EffectB.Parameters["Projection"].SetValue(camera.Projection);
            Array.ForEach(RushPowerups, PowerUp =>
            {
                EffectB.Parameters["World"].SetValue(PowerUp);
                EffectB.Parameters["ModelTexture"].SetValue(RushTexture);
                var meshes = RushModel.Meshes;
                foreach (var mesh in meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = EffectB;
                    }

                    mesh.Draw();
                }

            });

            // Draw MovingBoxes
            for (int index = 0; index < MovingBoxes.Length; index++)
            {
                var poseMw = Simulation.Bodies.GetBodyReference(MovingBoxesBodyHandle[index]).Pose;
                MovingBoxes[index] = Matrix.CreateScale(MovingBoxesScale) * Matrix.CreateTranslation(poseMw.Position.X, poseMw.Position.Y, poseMw.Position.Z);
                Effect.Parameters["World"].SetValue(MovingBoxes[index]);
                Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(MovingBoxes[index])));
                Effect.Parameters["WorldViewProjection"].SetValue(MovingBoxes[index] * viewProjection);
                BoxPrimitive.Draw(Effect);

            }
        }

    }
}
