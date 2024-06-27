using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderingTanks.Objects.Props
{
    public class GameProp
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        public Model Model { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix WorldMatrix { get; set; }
        public Effect Effect { get; set; }
        public Vector3 Position { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public Vector3 MaxBox;
        public Vector3 MinBox;

        public GameProp()
        {
            WorldMatrix = Matrix.Identity;
        }

        public void Load(Model model, Texture2D texture, Effect effect)
        {
            Model = model;
            Texture = texture;
            Effect = effect;
        }

        public virtual void Draw(Matrix view, Matrix projection)
        {
            foreach (var mesh in Model.Meshes)
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
                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * WorldMatrix);

                mesh.Draw();

            }
        }

        public void SpawnPosition(Vector3 position)
        {
            WorldMatrix = Matrix.CreateTranslation(position);
            Position = position;
        }
        public BoundingBox CreateBoundingBox(Model model, Matrix escala, Vector3 position)
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
