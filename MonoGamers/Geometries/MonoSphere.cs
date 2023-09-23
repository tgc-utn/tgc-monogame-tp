using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MonoSphere(Vector3 InitialPosition, float Gravity)
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

            // Initialize the Velocity as zero
            SphereVelocity = Vector3.Zero;
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
