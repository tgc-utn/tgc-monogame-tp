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

        //Signs

        private Model SignModel { get; set; }
        private Matrix[] Signs { get; set; }

        //Sonic

        private Model SonicModel { get; set; }  
        private Matrix Sonic { get; set; }

        // GraphicsDevice
        private GraphicsDevice GraphicsDevice { get; set; }
        
        // Simulation           
        private Simulation Simulation { get; set; }

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
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x, y + 30f, z + 210f),
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x, y + 30f, z + 720f),
                basicRush * Matrix.CreateTranslation(x + 1200f, y + 130f, z + 1350f),
                basicRush * Matrix.CreateRotationY(-1.5707f) * Matrix.CreateTranslation(x + 1500f, y + 150f, z + 2000f)
            };


            //lista de plataformas
            Platforms = new Matrix[]
            {
                Matrix.CreateScale(300f, 5f, 500f) * Matrix.CreateTranslation(x, y, z),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 600f),
                Matrix.CreateScale(300f, 5f, 300f) * Matrix.CreateTranslation(x, y, z + 1100f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x, y, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 100f, y + 45f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 200f, y + 90f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 300f, y + 135f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 400f, y + 170f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 550f, y + 170f, z + 1350f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 850f, y + 170f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1200f, y + 100f, z + 1350f),
                Matrix.CreateScale(100f, 5f, 300f) * Matrix.CreateTranslation(x + 1450f, y + 100f, z + 1350f),
                Matrix.CreateScale(500f, 5f, 500f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 1800f),
                Matrix.CreateScale(500f, 5f, 100f) * Matrix.CreateTranslation(x + 1500f, y + 120f, z + 2150f),
                Matrix.CreateScale(100f, 5f, 100f) * Matrix.CreateTranslation(x + 1900f, y + 140f, z + 2150f),

            };
            
            for (int index = 0; index < Platforms.Length; index++)
            {
                var matrix = Platforms[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                    Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));

            }

            //Cajas movibles
            MovingBoxes = new Matrix[]
            {
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x + 150f, y + 50f, z + 830f),
               Matrix.CreateScale(100f, 100f, 20f) * Matrix.CreateTranslation(x - 150f, y + 50f, z + 870f)
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
            

            //Signs 

            Signs = new Matrix[]
            {
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 45f, y - 130f, z+210f),
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateTranslation(x + 1080f, y + 45f, z+1380f),
                Matrix.CreateScale(25f, 25f, 25f) * Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 1430, y - 10f, z+2010f)
            };

            for (int index = 0; index < Signs.Length; index++)
            {
                var matrix = Signs[index];
                matrix.Decompose(out scale, out rot, out translation);
                Simulation.Statics.Add(new StaticDescription(Utils.ToNumericVector3(translation),
                    Simulation.Shapes.Add( new Box(scale.X,scale.Y, scale.Z))));

            }
            //Sonic
            Sonic = Matrix.CreateScale(2f, 2f, 2f) *
                Matrix.CreateRotationY((float)(Math.PI/2)) *
                Matrix.CreateTranslation(x + 20f, y, z+100f);
        }

        private void LoadContent(ContentManager Content)
        {
            // Cargar Texturas
            Texture2D CobbleTexture = Content.Load<Texture2D>(
                ConfigurationManager.AppSettings["ContentFolderTextures"] + "floor/adoquin");

            //Carga modelo Rush
            RushModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3DPowerUps"] + "arrowpush/tinker") ;
            foreach (var mesh in RushModel.Meshes)
               ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

            //Carga modelo sign
            SignModel = Content.Load<Model>("Models/signs/warningSign/untitled");
            foreach (var mesh in SignModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

            //Carga modelo Sonic
            SonicModel = Content.Load<Model>(
                ConfigurationManager.AppSettings["ContentFolder3D"] + "/sonic/source/sonic");
            foreach (var mesh in SonicModel.Meshes)
                ((BasicEffect)mesh.Effects.FirstOrDefault())?.EnableDefaultLighting();

            // Cargar Primitiva de caja con textura
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One, CobbleTexture);
        }

        public void Update()
        {
            // AnnoyingMovingWalls
            for (int index = 0; index < MovingBoxesBodyHandle.Length; index++)
            {
                var MovingBoxesBodyRef = Simulation.Bodies.GetBodyReference(MovingBoxesBodyHandle[index]);
                if (index%2 == 0){
                    if (MovingBoxesBodyRef.Pose.Position.X >= InitialLeftXPositionMovingBoxes)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 70f ));
                    if (MovingBoxesBodyRef.Pose.Position.X <= InitialLeftXPositionMovingBoxes - LengthDistanceDifference)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 70f ));
                }
                else
                {
                    if (MovingBoxesBodyRef.Pose.Position.X <= InitialRigthXPositionMovingBoxes)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Right * 70f ));
                    if (MovingBoxesBodyRef.Pose.Position.X >= InitialRigthXPositionMovingBoxes + LengthDistanceDifference)
                        MovingBoxesBodyRef.Velocity = new BodyVelocity(Utils.ToNumericVector3(Vector3.Left * 70f ));
                }

            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Array.ForEach(Platforms, Platform => BoxPrimitive.Draw(Platform, view, projection));
            Array.ForEach(RushPowerups, PowerUp => RushModel.Draw(PowerUp, view, projection));
            Array.ForEach(Signs, Sign => SignModel.Draw(Sign, view, projection));
            
            // Draw AnnoyingMovingWallsWorld
            for (int index = 0; index < MovingBoxes.Length; index++)
            {
                var poseMw = Simulation.Bodies.GetBodyReference(MovingBoxesBodyHandle[index]).Pose;
                MovingBoxes[index] = Matrix.CreateScale(MovingBoxesScale) * Matrix.CreateTranslation(poseMw.Position.X, poseMw.Position.Y, poseMw.Position.Z);
                BoxPrimitive.Draw(MovingBoxes[index], view, projection);

            }

            
            SonicModel.Draw(Sonic, view, projection);
        }

    }
}
