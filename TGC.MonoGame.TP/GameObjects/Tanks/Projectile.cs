using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Tanks
{
    public class Projectile : GameObject
    {

        #region Projectile
        private Model projectile { get; set; }
        public BoundingBox ProjectileBox { get; set; }
        public Matrix PositionMatrix { get; set; }
        private Vector3 LastPosition { get; set; }
        private Vector3 MovementVector { get; set; }

        public Vector3 PositionVector = new(0, 0, 0);
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        public float Scale { get; set; }
        public float GunRotation { get; set; }
        public bool WasShot { get; private set; }
        #endregion

        public Projectile(Matrix matrix, float rotation, float speed, float scale)
        {
            PositionMatrix = matrix;
            PositionVector = PositionMatrix.Translation;

            Direction = matrix.Backward;
            Speed = speed;

            GunRotation = rotation;

            Scale = scale;

        }

        public override void LoadContent(ContentManager Content, Effect effect)
        {
            projectile = Content.Load<Model>(ContentFolder3D + "TankBullets/Apcr");

            Texture = Content.Load<Texture2D>(ContentFolder3D + "TankBullets/ApcrTexture");

            Effect = effect;

            ProjectileBox = CollisionsClass.CreateBoundingBox(projectile, Matrix.CreateScale(0.5f), PositionVector);

        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            LastPosition = PositionVector;
            PositionVector += Direction * Speed * time;

            PositionMatrix = Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(PositionVector);

            MovementVector = PositionVector - LastPosition;
            ProjectileBox = new BoundingBox(ProjectileBox.Min + MovementVector, ProjectileBox.Max + MovementVector);
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(180)) * PositionMatrix;

            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in projectile.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                }

                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

                Effect.Parameters["ModelTexture"].SetValue(Texture);
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrix);

                mesh.Draw();
            }
        }
    }
}




