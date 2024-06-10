using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects.Props
{
    public class Roca
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model RocaModel { get; set; }

        private Texture2D TexturaRoca { get; set; }
        public Matrix[] RocaWorlds { get; set; }
        public Matrix RocaWorld { get; set; }
        public Effect Effect { get; set; }

        public BoundingBox RocaBox { get; set; }
        public Vector3 Position { get; set; }

        public List<BoundingBox> BoundingBoxes { get; private set; }

        public bool IsDestroyed { get; private set; } // Indica si la roca ha sido destruida


        public Roca()
        {
            RocaWorld = Matrix.Identity;
        }
        public void LoadContent(ContentManager content)
        {
            RocaModel = content.Load<Model>(ContentFolder3D + "nature/rock/Rock_1");
            TexturaRoca = content.Load<Texture2D>(ContentFolder3D + "nature/rock/Yeni klasör/Rock_1_Base_Color");
            Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in RocaModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            RocaWorld = Matrix.CreateScale(2.5f) * Matrix.CreateTranslation(Position);
            RocaBox = CreateBoundingBox(RocaModel, Matrix.CreateScale(2.5f), Position);
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection)

        {
            if (!IsDestroyed)
            {
                Effect.Parameters["Projection"]?.SetValue(projection);
                Effect.Parameters["View"]?.SetValue(view);

                foreach (var mesh in RocaModel.Meshes)
                {
                    Matrix _cartelWorld = RocaWorld;
                    Effect.Parameters["ModelTexture"]?.SetValue(TexturaRoca);
                    Effect.Parameters["World"]?.SetValue(mesh.ParentBone.Transform * _cartelWorld);
                    mesh.Draw();
                }
            }

        }

        public void Destroy()
        {
            IsDestroyed = true;
            DestruirRecursos();
            DestruirBoundingBox();
        }

        public void DestruirRecursos()
        {
            //RocaModel?.Dispose();
            //TexturaRoca?.Dispose();
            //Effect?.Dispose();

            RocaModel = null;
            TexturaRoca = null;
            Effect = null;
            RocaBox = new BoundingBox(Vector3.Zero, Vector3.Zero);
            //BoundingBoxes = null;
        }

        private void DestruirBoundingBox()
        {
            if (BoundingBoxes != null)
            {
                BoundingBoxes.Remove(RocaBox);
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