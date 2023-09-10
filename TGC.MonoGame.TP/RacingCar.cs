using System;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Content.Models
{
    class RacingCar
    {
        public const string ContentFolder3D = "Models/racingcar/";
        public const string ContentFolderEffects = "Effects/";

        #region Properties
        public float CurrentWheelRotation = 0f;
        public float CurrentSteeringWheelRotation = 0f;
        private float CurrentSpeed = 0f;

        // todo: constructor
        private const float DefaultSteeringSpeed = 0.03f;
        private const float DefaultSteeringRotation = 25f;
        private const float DefaultAcceleration = 10f;
        private const float DefaultBrakingForce = 30f;
        private const float DefaultMaxSpeed = 3500f;
        private const float DefaultMaxReverseSpeed = -800f;
        private const float DefaultJumpSpeed = 1000f;
        private const float DefaultBoostSpeed = 3f;

        // todo: global
        private const float Gravity = 50f;
        #endregion Properties

        public RacingCar(ContentManager content)
        {
            Model = content.Load<Model>(ContentFolder3D + "RacingCar");
            Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

            FrontRightWheelBone = Model.Bones["WheelA"];
            FrontLeftWheelBone = Model.Bones["WheelB"];
            BackLeftWheelBone = Model.Bones["WheelC"];
            BackRightWheelBone = Model.Bones["WheelD"];
            CarBone = Model.Bones["Car"];
            CarTransform = CarBone.Transform;
            FrontLeftWheelTransform = FrontLeftWheelBone.Transform;
            FrontRightWheelTransform = FrontRightWheelBone.Transform;
            BackLeftWheelTransform = BackLeftWheelBone.Transform;
            BackRightWheelTransform = BackRightWheelBone.Transform;
            BoneTransforms = new Matrix[Model.Bones.Count];

            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
            }
        }

		public void Draw(GameTime gameTime, Matrix view, Matrix projection)
		{
			// Set the world matrix as the root transform of the model.
			Model.Root.Transform = World;

			// Calculate matrices based on the current animation position.
			var wheelRotationX = Matrix.CreateRotationX(CurrentWheelRotation);
			var steeringRotationY = Matrix.CreateRotationY(CurrentSteeringWheelRotation);

			// Apply matrices to the relevant bones.
			FrontLeftWheelBone.Transform = wheelRotationX * steeringRotationY * FrontLeftWheelTransform;
			FrontRightWheelBone.Transform = wheelRotationX * steeringRotationY * FrontRightWheelTransform;
			BackLeftWheelBone.Transform = wheelRotationX * BackLeftWheelTransform;
			BackRightWheelBone.Transform = wheelRotationX * BackRightWheelTransform;
			CarBone.Transform = CarTransform;

			// Look up combined bone matrices for the entire model.
			Model.CopyAbsoluteBoneTransformsTo(BoneTransforms);

			// For each mesh in the model,
			foreach (var mesh in Model.Meshes)
			{
				// Obtain the world matrix for that mesh (relative to the parent)
				var meshWorld = BoneTransforms[mesh.ParentBone.Index];
				Effect.Parameters["World"].SetValue(meshWorld);
				Effect.Parameters["View"].SetValue(view);
				Effect.Parameters["Projection"].SetValue(projection);
				mesh.Draw();
			}
		}

		public void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if(Position.Y == 0f) 
            {
                // para tener control sobre el auto hice que deba estar sobre el suelo, ninguna razon en particular, me gusto asi
                Turn(keyboardState.IsKeyDown(Keys.A), keyboardState.IsKeyDown(Keys.D));
                if(CurrentSpeed >= 0f)
                    Drive(keyboardState.IsKeyDown(Keys.W), keyboardState.IsKeyDown(Keys.S), keyboardState.IsKeyDown(Keys.LeftShift), elapsedTime);
				else
					Reverse(keyboardState.IsKeyDown(Keys.S), keyboardState.IsKeyDown(Keys.W), elapsedTime);
				Jump(keyboardState.IsKeyDown(Keys.Space));
            }
            else 
            {
                CurrentSpeed /= 1 + elapsedTime; // para que vaya desacelerando gradualemente
                DirectionSpeed -= Vector3.Up * Gravity;
            }

            // combino las velocidades horizontal y vertical
            DirectionSpeed = Direction * CurrentSpeed + Vector3.Up * DirectionSpeed.Y;
            Position += DirectionSpeed * elapsedTime;
            
            if (Position.Y < 0f) 
            {
                // si quedara por debajo del suelo lo seteo en 0
                Position.Y = 0f;
                DirectionSpeed.Y = 0f;
            }

            var sasssa = new Vector2(DirectionSpeed.X,DirectionSpeed.Z).LengthSquared();

            World = Rotation * Matrix.CreateTranslation(Position);
        }

		#region Movement
		private void Drive(bool isAccelerating, bool isBraking, bool isUsingBoost, float elapsedTime)
		{
            if (isAccelerating && isUsingBoost)
            {
                CurrentSpeed += DefaultAcceleration * DefaultBoostSpeed;
				CurrentSpeed = CurrentSpeed > DefaultMaxSpeed * DefaultBoostSpeed ? DefaultMaxSpeed * DefaultBoostSpeed : CurrentSpeed;
            }
            else if (isAccelerating)
			{
                CurrentSpeed += DefaultAcceleration;
				CurrentSpeed = CurrentSpeed > DefaultMaxSpeed ? DefaultMaxSpeed : CurrentSpeed;
			}

			if (isBraking)
				CurrentSpeed -= DefaultBrakingForce;

			if (!isAccelerating && !isBraking)
				CurrentSpeed /= 1 + elapsedTime; // para que vaya desacelerando gradualemente

			CurrentWheelRotation += ToRadians(CurrentSpeed / 10f);
		}

		private void Reverse(bool isAccelerating, bool isBraking, float elapsedTime)
		{
			if (isAccelerating)
			{
				CurrentSpeed -= DefaultAcceleration;
				CurrentSpeed = CurrentSpeed < DefaultMaxReverseSpeed ? DefaultMaxReverseSpeed : CurrentSpeed;
			}

			if (isBraking)
				CurrentSpeed += DefaultBrakingForce;

			if (!isAccelerating && !isBraking)
				CurrentSpeed /= 1 + elapsedTime; // para que vaya desacelerando gradualemente

			CurrentWheelRotation += ToRadians(CurrentSpeed / 10f);
		}

        private void Turn(bool isTurningLeft, bool isTurningRight) 
        {
            if (isTurningLeft && !isTurningRight) {
                Rotation *= Matrix.CreateRotationY(DefaultSteeringSpeed * CurrentSpeed / DefaultMaxSpeed);
                Direction = Vector3.Transform(Vector3.Backward, Rotation);
                CurrentSteeringWheelRotation = ToRadians(DefaultSteeringRotation);
            }
            else if (isTurningRight && !isTurningLeft)
            {
                Rotation *= Matrix.CreateRotationY(-DefaultSteeringSpeed * CurrentSpeed / DefaultMaxSpeed);
                Direction = Vector3.Transform(Vector3.Backward, Rotation);
                CurrentSteeringWheelRotation = ToRadians(-DefaultSteeringRotation);
            }
            else CurrentSteeringWheelRotation = 0f;
        }

        private void Jump(bool isJumping) {
            if (isJumping)
                DirectionSpeed += Vector3.Up * DefaultJumpSpeed;
        }
        #endregion

        #region utils
        private float ToRadians(float angle) {
            return angle * (MathF.PI / 180f);
        }
        #endregion

        #region Fields
        private Effect Effect;
        private Model Model;

        private ModelBone FrontRightWheelBone;
        private ModelBone FrontLeftWheelBone;
        private ModelBone BackLeftWheelBone;
        private ModelBone BackRightWheelBone;
        private ModelBone CarBone;
        private Matrix FrontRightWheelTransform;
        private Matrix FrontLeftWheelTransform;
        private Matrix BackLeftWheelTransform;
        private Matrix BackRightWheelTransform;
        private Matrix CarTransform;
        private Matrix[] BoneTransforms;

        public Matrix World = Matrix.Identity;
        private Matrix Rotation = Matrix.Identity;
        private Vector3 Position = Vector3.Zero;
        private Vector3 Direction = Vector3.Backward;
        private Vector3 DirectionSpeed = Vector3.Backward;
        #endregion Fields
    }
}