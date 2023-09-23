using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGamers.Camera;
using MonoGamers.PowerUps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericVector3 = System.Numerics.Vector3;

namespace MonoGamers.Geometries
{
    internal class MonoSphere
    {

        //Tipo de esfera
        enum SphereType
        {
            Common,
            Stone,
            Metal,
            Gum
        }

        private const float standardSideSpeed = 2000f;
        private const float standardJumpSpeed = 100000f;

        public float SphereSideSpeed;
        public float SphereJumpSpeed;

        public SpherePrimitive spherePrimitive { get; set; }

        // Sphere internal matrices and vectors
        public Matrix SphereRotation { get; set; }
        public Vector3 SpherePosition { get; set; }
        public Vector3 SphereVelocity { get; set; }
        public Vector3 SphereAcceleration { get; set; }
        public Vector3 SphereFrontDirection { get; set; }
        public Vector3 SphereLateralDirection { get; set; }
        public Matrix SphereWorld { get; set; }


        // A boolean indicating if the Sphere is on the ground
        public bool OnGround { get; set; }

        // Textures
        public Texture2D SphereCommonTexture { get; set; }
        public Texture2D SphereStoneTexture { get; set; }
        public Texture2D SphereMetalTexture { get; set; }
        public Texture2D SphereGumTexture { get; set; }

        //handler
        public BodyHandle SphereHandle { get; set; }

        public Sphere SphereShape { get; set; }

        public float velocidadAngularYAnt;
        public float velocidadLinearYAnt;

        public MonoSphere(Vector3 InitialPosition, float Gravity, Simulation Simulation)
        {
            OnGround = true;

            SphereSideSpeed = standardSideSpeed;
            SphereJumpSpeed = standardJumpSpeed;

            SpherePosition = InitialPosition;
            SphereRotation = Matrix.Identity;
            SphereFrontDirection = Vector3.Backward;
            SphereLateralDirection = Vector3.Right;
            // Set the Acceleration (which in this case won't change) to the Gravity pointing down
            SphereAcceleration = Vector3.Down * Gravity;

            SphereShape = new Sphere(10f);
            var position = new NumericVector3(100f, 20f, 150f);
            var initialVelocity = new BodyVelocity(new NumericVector3((float)0f, 0f, 0f));
            var mass = SphereShape.Radius * SphereShape.Radius * SphereShape.Radius;
            var bodyDescription = BodyDescription.CreateConvexDynamic(position, initialVelocity, mass, Simulation.Shapes, SphereShape);
            SphereHandle = Simulation.Bodies.Add(bodyDescription);

            // Initialize the Velocity as zero
            SphereVelocity = Vector3.Zero;
        }

        public void Update(Simulation Simulation, TargetCamera Camera, KeyboardState KeyboardState)
        {
            var SphereBody = Simulation.Bodies.GetBodyReference(SphereHandle);
            SphereBody.Awake = true;
            SphereRotation = Camera.CameraRotation;
            SphereFrontDirection = Vector3.Transform(Vector3.Backward, SphereRotation);
            SphereLateralDirection = Vector3.Transform(Vector3.Right, SphereRotation);

            // Check for key presses and add a velocity in the Sphere's Front Direction

            if (KeyboardState.IsKeyDown(Keys.D))
            {
                MoveRight();
                SphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X, SphereVelocity.Y, SphereVelocity.Z));
            }
            else if (KeyboardState.IsKeyDown(Keys.A))
            {
                MoveLeft();
                SphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X, SphereVelocity.Y, SphereVelocity.Z));
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                MoveFront();
                SphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X, SphereVelocity.Y, SphereVelocity.Z));
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                MoveBack();
                SphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X, SphereVelocity.Y, SphereVelocity.Z));
            }
            if (MathHelper.Distance(SphereBody.Velocity.Linear.Y, velocidadLinearYAnt) < 0.1
                    && MathHelper.Distance(SphereBody.Velocity.Angular.Y, velocidadAngularYAnt) < 0.1)
                        OnGround = true; // Se revisa que la velocidad lineal como la angular de la esfera en Y, su distancia se menor a 0,1 con respecto a la velocidad anterior

            if (KeyboardState.IsKeyDown(Keys.Space) && OnGround)
            {
                Jump();
                SphereBody.ApplyLinearImpulse(new NumericVector3(SphereVelocity.X, SphereVelocity.Y, SphereVelocity.Z));
            }

            velocidadAngularYAnt = SphereBody.Velocity.Angular.Y;
            velocidadLinearYAnt = SphereBody.Velocity.Linear.Y;

            var pose = Simulation.Bodies.GetBodyReference(SphereHandle).Pose;
            SpherePosition = pose.Position;
            SphereWorld = Matrix.CreateScale(SphereShape.Radius*2) *
                Matrix.CreateFromQuaternion(pose.Orientation) * Matrix.CreateTranslation(SpherePosition);
        }

        public void MoveLeft()
        {
            LateralMove(1f);
        }

        public void MoveRight()
        {
            LateralMove(-1f);
        }

        public void MoveFront()
        {
            FrontalMove(1f);
        }

        public void MoveBack()
        {
            FrontalMove(-1f);
        }

        private void LateralMove(float Direction)
        {
            SphereVelocity = Direction * SphereLateralDirection * SphereSideSpeed;
        }

        private void FrontalMove(float Direction)
        {
            SphereVelocity = Direction * SphereFrontDirection * SphereSideSpeed;
        }

        public void Jump()
        {
            SphereVelocity = Vector3.Up * SphereJumpSpeed;
            OnGround = false;
        }

        public bool SphereFalling(float LimitY)
        {
            return SpherePosition.Y < LimitY;
        }
    }
}
