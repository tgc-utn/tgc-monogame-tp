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
using ThunderingTanks.Collisions;
//using BepuUtilities;

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

        public float Scale { get; set; }

        public BoundingBox ProjectileBox { get; set; }

        public bool WasShot { get; private set; }

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
            ProjectileBox = CreateBoundingBox(projectile, Matrix.CreateScale(0.5f), PositionVector);
        }

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PositionVector += Direction * Speed * time;

            PositionMatrix = Matrix.CreateRotationY(GunRotation) * Matrix.CreateTranslation(PositionVector);

            ProjectileBox = CreateBoundingBox(projectile, Matrix.CreateScale(0.5f), PositionVector);
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
 
        private BoundingBox CreateBoundingBox(Model model, Matrix escala, Vector3 position)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in model.Meshes)
            {
                var meshParts = mesh.MeshParts;
                foreach (var meshPart in meshParts)
                {
                    var vertexBuffer = meshPart.VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[mesh.ParentBone.Index] * escala;
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }

            return new BoundingBox(minPoint + position, maxPoint + position);
        }
    }
}




