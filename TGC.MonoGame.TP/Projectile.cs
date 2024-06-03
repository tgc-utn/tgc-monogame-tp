using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThunderingTanks.Objects;
using static System.Formats.Asn1.AsnWriter;
namespace ThunderingTanks
{
    public class Projectile : GameObject
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";


        public Vector3 PositionVector = new(0, 0, 0);
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        public float GunRotation { get; set; }
        public Matrix PositionMatrix { get; set; }
        public Effect Effect { get; set; }
        private Model projectile { get; set; }
        private Texture2D TexturaProjectile { get; set; }
        public float Scale { get; set; } // Nueva propiedad


        public Projectile(Matrix matrix, float rotation, float speed, float scale)
        {
            this.PositionMatrix = matrix;
            this.PositionVector = PositionMatrix.Translation;

            this.Direction = matrix.Backward;
            this.Speed = speed;

            this.GunRotation = rotation;

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

        }

        public void Draw(Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(180)) * PositionMatrix;

            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            //Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());

            //Effect.Parameters["ModelTexture"].SetValue(false);

            foreach (var mesh in projectile.Meshes)
            {

                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrix);
                mesh.Draw();

            }
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PositionVector += Direction * Speed * time;
            this.PositionMatrix = Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(PositionVector);
        }

        private BoundingBox CalculateBoundingBox()
        {
            // Aqui debes calcular la BoundingBox inicial del modelo del proyectil
            // Por simplicidad, asumimos un tama�o peque�o centrado en (0,0,0)
            return new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f));
        }

    }


}
