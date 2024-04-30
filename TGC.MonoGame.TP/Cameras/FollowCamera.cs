/*using System;
using Microsoft.Xna.Framework;
using ThunderingPersonal;

namespace ThunderingPersonal
{
    public class FollowCamera
    {
        public Vector3 AvatarHeadOffset { get; set; }
        public Vector3 TargetOffset { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public FollowCamera()
        {
            AvatarHeadOffset = new Vector3(0, 7, -100);
            TargetOffset = new Vector3(0, 5, 0);
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
        }

        public void Update(float avatarYaw, Vector3 position, float aspectRatio)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);

            Vector3 transformedheadOffset = Vector3.Transform(AvatarHeadOffset, rotationMatrix);
            Vector3 transformedReference = Vector3.Transform(TargetOffset, rotationMatrix);

            Vector3 cameraPosition = position + transformedheadOffset;
            Vector3 cameraTarget = position + transformedReference;

            //Calculate the camera's view and projection matrices based on current values.
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(GameConstants.ViewAngle),
                aspectRatio, GameConstants.NearClip, GameConstants.FarClip);
        }
    }
}
*/