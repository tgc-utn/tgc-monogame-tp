using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;

namespace TGC.MonoGame.TP.Content.Models
{
    class RacingCar : BaseCar
	{
        public RacingCar(ContentManager content)
        {
            Model = content.Load<Model>(ContentFolder3D + "racingcar/racingcar");
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