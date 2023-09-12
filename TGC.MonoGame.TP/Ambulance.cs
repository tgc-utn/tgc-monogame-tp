using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Content.Models
{
	class Ambulance : BaseCar
	{
		public Ambulance(ContentManager content)
		{
			Model = content.Load<Model>(ContentFolder3D + "ambulance");
			Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

			FrontRightWheelBone = Model.Bones["wheel_frontLeft"];
			FrontLeftWheelBone = Model.Bones["wheel_frontRight"];
			BackLeftWheelBone = Model.Bones["wheel_backRight"];
			BackRightWheelBone = Model.Bones["wheel_backLeft"];
			CarBone = Model.Bones["body"];
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

			DefaultSteeringSpeed = 0.03f;
			DefaultSteeringRotation = 25f;
			DefaultBrakingForce = 30f;
			DefaultJumpSpeed = 1000f;
			DefaultBoostSpeed = 3f;
			MaxSpeed = new float[6] { 800f, 0f, 900f, 1500f, 2000f, 3500f }; // R-N-1-2-3-4
			Acceleration = new float[6] { 15f, -3f, 20f, 15f, 7.5f, 2f }; // R-N-1-2-3-4
		}
	}
}
