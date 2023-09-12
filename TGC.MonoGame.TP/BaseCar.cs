using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP.Content.Models
{
	class BaseCar
	{
		public const string ContentFolder3D = "Models/";
		public const string ContentFolderEffects = "Effects/";

		#region Properties
		//current state
		public int CurrentGear = 1;
		public float CurrentSpeed = 0f;
		public float CurrentWheelRotation = 0f;
		public float CurrentSteeringWheelRotation = 0f;
		public bool IsAccelerating = false;
		public bool IsBraking = false;
		public bool IsUsingBoost = false;
		public bool IsTurningLeft = false;
		public bool IsTurningRight = false;
		public bool IsJumping = false;

		//car specs
		public float DefaultSteeringSpeed;
		public float DefaultSteeringRotation;
		public float DefaultBrakingForce;
		public float DefaultJumpSpeed;
		public float DefaultBoostSpeed;
		public float[] MaxSpeed;
		public float[] Acceleration;

		// todo: global
		public const float Gravity = 50f;

		public Effect Effect;
		public Model Model;
		public Matrix World = Matrix.Identity;
		public Matrix Rotation = Matrix.Identity;
		public Vector3 Position = Vector3.Zero;
		public Vector3 Direction = Vector3.Backward;
		public Vector3 DirectionSpeed = Vector3.Backward;

		public ModelBone FrontRightWheelBone;
		public ModelBone FrontLeftWheelBone;
		public ModelBone BackLeftWheelBone;
		public ModelBone BackRightWheelBone;
		public ModelBone CarBone;
		public Matrix FrontRightWheelTransform;
		public Matrix FrontLeftWheelTransform;
		public Matrix BackLeftWheelTransform;
		public Matrix BackRightWheelTransform;
		public Matrix CarTransform;
		public Matrix[] BoneTransforms;
		#endregion Properties

		public BaseCar() { }

		public void Update(GameTime gameTime)
		{
			var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
			if (Position.Y == 0f)
			{
				// para tener control sobre el auto hice que deba estar sobre el suelo, ninguna razon en particular, me gusto asi
				Turn();
				Drive(elapsedTime);
				if (IsJumping) Jump();
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

			World = Rotation * Matrix.CreateTranslation(Position);
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

		#region Movement
		public void Drive(float elapsedTime)
		{
			if (CurrentSpeed < 0f)
			{
				Reverse(elapsedTime);
				return;
			}

			if (IsAccelerating)
			{
				float boost = IsUsingBoost ? DefaultBoostSpeed : 1f;
				CurrentSpeed += Acceleration[CurrentGear] * boost;
				if (CurrentSpeed > MaxSpeed[CurrentGear]) CurrentGear++;
				CurrentSpeed = CurrentSpeed > MaxSpeed[CurrentGear] * boost ? MaxSpeed[CurrentGear] * boost : CurrentSpeed;
			}

			if (IsBraking)
			{
				CurrentSpeed -= DefaultBrakingForce;
				if (CurrentGear > 1 && CurrentSpeed < MaxSpeed[CurrentGear - 1]) CurrentGear--;
			}

			if (!IsAccelerating && !IsBraking)
			{
				CurrentSpeed /= 1 + elapsedTime; // para que vaya desacelerando gradualemente
				if (CurrentGear > 1 && CurrentSpeed < MaxSpeed[CurrentGear - 1]) CurrentGear--;
			}

			CurrentWheelRotation += ToRadians(CurrentSpeed / 10f);
		}

		public void Reverse(float elapsedTime)
		{
			if (IsAccelerating)
			{
				CurrentGear = 0;
				CurrentSpeed -= Acceleration[CurrentGear];
				CurrentSpeed = CurrentSpeed < -MaxSpeed[CurrentGear] ? -MaxSpeed[CurrentGear] : CurrentSpeed;
			}

			if (IsBraking)
			{
				CurrentSpeed += DefaultBrakingForce;
				if (CurrentSpeed > MaxSpeed[1]) CurrentGear++;
			}

			if (!IsAccelerating && !IsBraking)
			{
				CurrentSpeed /= 1 + elapsedTime; // para que vaya desacelerando gradualemente
				if (CurrentSpeed > MaxSpeed[1]) CurrentGear++;
			}

			CurrentWheelRotation += ToRadians(CurrentSpeed / 10f);
		}

		public void Turn()
		{
			if (IsTurningLeft && !IsTurningRight)
			{
				if (CurrentGear != 1)
				{
					Rotation *= Matrix.CreateRotationY(DefaultSteeringSpeed * (CurrentSpeed / MaxSpeed[CurrentGear]));
					Direction = Vector3.Transform(Vector3.Backward, Rotation);
				}
				CurrentSteeringWheelRotation = ToRadians(DefaultSteeringRotation);
			}
			else if (IsTurningRight && !IsTurningLeft)
			{
				if (CurrentGear != 1)
				{
					Rotation *= Matrix.CreateRotationY(-DefaultSteeringSpeed * (CurrentSpeed / MaxSpeed[CurrentGear]));
					Direction = Vector3.Transform(Vector3.Backward, Rotation);
				}
				CurrentSteeringWheelRotation = ToRadians(-DefaultSteeringRotation);
			}
			else CurrentSteeringWheelRotation = 0f;
		}

		public void Jump()
		{
			DirectionSpeed += Vector3.Up * DefaultJumpSpeed;
		}
		#endregion

		#region utils
		public void SetKeyboardState()
		{
			KeyboardState keyboardState = Keyboard.GetState();
			bool goingForward = CurrentSpeed >= 0;
			IsAccelerating = goingForward ? keyboardState.IsKeyDown(Keys.W) : keyboardState.IsKeyDown(Keys.S);
			IsBraking = goingForward ? keyboardState.IsKeyDown(Keys.S) : keyboardState.IsKeyDown(Keys.W);
			IsTurningLeft = keyboardState.IsKeyDown(Keys.A);
			IsTurningRight = keyboardState.IsKeyDown(Keys.D);
			IsUsingBoost = keyboardState.IsKeyDown(Keys.Space);
			IsJumping = keyboardState.IsKeyDown(Keys.Space);
		}

		public float ToRadians(float angle)
		{
			return angle * (MathF.PI / 180f);
		}
		#endregion
	}
}
