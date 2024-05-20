using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
    public class Roca
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model RocaModel { get; set; }

        private Texture2D TexturaRoca { get; set; }
        public Matrix[] RocaWorlds { get; set; }
        public Effect Effect { get; set; }

        public List<BoundingBox> BoundingBoxes { get; private set; }


        public Roca()
        {
            RocaWorlds = new Matrix[] { };
            BoundingBoxes = new List<BoundingBox>();

        }

        public void AgregarRoca(Vector3 Position)
        {
            Matrix escala = Matrix.CreateScale(2.5f);
            var nuevaRoca = new Matrix[]{
                escala * Matrix.CreateTranslation(Position),
            };
            RocaWorlds = RocaWorlds.Concat(nuevaRoca).ToArray();

            // Crear BoundingBox para la nueva roca
            BoundingBox box = CreateBoundingBox(RocaModel, escala, Position);
            BoundingBoxes.Add(box);
        }

        public void LoadContent(ContentManager Content)
        {
            RocaModel = Content.Load<Model>(ContentFolder3D + "nature/rock/Rock_1");

            TexturaRoca = Content.Load<Texture2D>(ContentFolder3D + "nature/rock/Yeni klasör/Rock_1_Base_Color");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in RocaModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
            foreach (var mesh in RocaModel.Meshes)
            {

                for (int i = 0; i < RocaWorlds.Length; i++)
                {
                    Matrix _cartelWorld = RocaWorlds[i];
                    Effect.Parameters["ModelTexture"].SetValue(TexturaRoca);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _cartelWorld);
                    mesh.Draw();
                }

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