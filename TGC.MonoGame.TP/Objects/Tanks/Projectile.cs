using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Tanks
{
    public class Projectile : GameObject
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        #region Projectile
        private Model projectile { get; set; }
        public Effect Effect { get; set; }
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

        public void LoadContent(ContentManager Content)
        {
            projectile = Content.Load<Model>(ContentFolder3D + "TankBullets/Aps-c");

            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in projectile.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }

            ProjectileBox = CollisionsClass.CreateBoundingBox(projectile, Matrix.CreateScale(0.5f), PositionVector);

        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            LastPosition = PositionVector;
            PositionVector += Direction * Speed * time;

            PositionMatrix = Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(PositionVector);

            MovementVector = PositionVector - LastPosition;
            ProjectileBox = new BoundingBox(ProjectileBox.Min + MovementVector, ProjectileBox.Max + MovementVector);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(180)) * PositionMatrix;

            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in projectile.Meshes)
            {
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrix);
                mesh.Draw();
            }
        }
    }
}




