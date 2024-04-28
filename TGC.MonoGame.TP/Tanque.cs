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
            //Model = Content.Load<Model>("Models/Panzer/Panzer");

        }

        public void Update(GamePadState gamepadState, KeyboardState keyboardState)
        {
            Vector3 futurePosition = Position;
            float turnAmount = 0;

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
        }

        private bool ValidateMovement(Vector3 futurePosition)
        {
            //Do not allow off-terrain driving
            if ((Math.Abs(futurePosition.X) > MaxRange) || (Math.Abs(futurePosition.Z) > MaxRange))
                return false;

            return true;
        }


        public void Draw(Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix rotationYMatrix = Matrix.CreateRotationY(ForwardDirection);
            Matrix translateMatrix = Matrix.CreateTranslation(Position);

            worldMatrix = rotationYMatrix * translateMatrix;

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
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