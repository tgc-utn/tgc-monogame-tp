using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace ThunderingTanks
{
    public class Projectile : GameObject
    {
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        public Vector3 PositionVector = new(0,0,0);
        public new Matrix Position { get; set; }


        public Projectile(Matrix matrix, float speed)
        {
            this.Position = matrix;
            PositionVector = Position.Translation;

            this.Direction = matrix.Backward;

            this.Speed = speed;
        }

        public void Draw(Model projectileModel, Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Position;
            // Dibujar el proyectil en su posición actual
            foreach (ModelMesh mesh in projectileModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PositionVector += Direction * Speed * time;
            this.Position = Matrix.CreateTranslation(PositionVector);
        }

    }


}
