/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP
{
    public class Tanque : GameObject
    {
        public float ForwardDirection { get; set; }
        public int MaxRange { get; set; }
        private Vector3 startPosition = new Vector3(0, GameConstants.HeightOffset, 0);

        float TorretTurn = 0f;
        private ModelBone TurretBone { get; set; }
        private ModelBone CannonBone { get; set; }
        private Matrix TurretMatrix {  get; set; }
        private Matrix CannonMatrix { get; set; }

        private Matrix[] BoneTransform;

        public Tanque()
            : base()
        {
            ForwardDirection = 0.0f;
            Position = startPosition;
            MaxRange = GameConstants.MaxRange;
        }

        public void LoadContent(ContentManager content, string modelName)
        {
            Model = content.Load<Model>(modelName);

            TurretBone = Model.Bones["Turret"];
            CannonBone = Model.Bones["Cannon"];

            TurretMatrix = TurretBone.Transform;
            CannonMatrix = CannonBone.Transform;

            BoneTransform = new Matrix[Model.Bones.Count];

        }

        public void Update(GamePadState gamepadState, KeyboardState keyboardState)
        {
            Vector3 futurePosition = Position;
            float turnAmount = 0;

            if (keyboardState.IsKeyDown(Keys.H))
            {
                TorretTurn += 1f;
            }

            if (keyboardState.IsKeyDown(Keys.J))
            {
                TorretTurn += -1f;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                turnAmount = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                turnAmount = -1;
            }
            else if (gamepadState.ThumbSticks.Left.X != 0)
            {
                turnAmount = -gamepadState.ThumbSticks.Left.X;
            }

            ForwardDirection += turnAmount * GameConstants.TurnSpeed;

            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 movement = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                movement.Z = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                movement.Z = -1;
            }
            else if (gamepadState.ThumbSticks.Left.Y != 0)
            {
                movement.Z = gamepadState.ThumbSticks.Left.Y;
            }

            Vector3 speed = Vector3.Transform(movement, orientationMatrix);
            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            if (ValidateMovement(futurePosition))
            {
                Position = futurePosition;
            }

            TurretMatrix = Matrix.CreateRotationY(TorretTurn * GameConstants.TurnSpeed);

        }

        private bool ValidateMovement(Vector3 futurePosition)
        {
            //Do not allow off-terrain driving
            //if ((Math.Abs(futurePosition.X) > MaxRange) || (Math.Abs(futurePosition.Z) > MaxRange))
              //  return false;

            return true;
        }


        public void Draw(Matrix view, Matrix projection)
        {
            Matrix worldMatrix;
            Matrix rotationYMatrix = Matrix.CreateRotationY(ForwardDirection);
            Matrix translateMatrix = Matrix.CreateTranslation(Position);

            worldMatrix = rotationYMatrix * translateMatrix;

            TurretBone.Transform = TurretMatrix * worldMatrix;
            BoneTransform[24] = TurretBone.Transform;
            BoneTransform[25] = TurretBone.Transform;
            

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    if (BoneTransform[mesh.ParentBone.Index] != TurretMatrix * worldMatrix)
                        effect.World = worldMatrix;
                    else effect.World = BoneTransform[mesh.ParentBone.Index];

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }
        }
    }
}
*/