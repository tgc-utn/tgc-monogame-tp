using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects.Props
{
    public class Molino
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";

        public Model MolinoModel { get; set; }
        public Texture2D MolinoTexture { get; set; }
        public Matrix MolinoMatrix { get; set; }
        public Effect MolinoEffect { get; set; }
        public Vector3 Position;
        public BoundingBox MolinoBox { get; set; }

        public Molino(Matrix molinoMatrix)
        {
            MolinoMatrix = molinoMatrix;
        }

        public void LoadContent(ContentManager Content)
        {
            MolinoModel = Content.Load<Model>(ContentFolder3D + "ModelosVarios/MolinoProp/MolinoProp");
            MolinoTexture = Content.Load<Texture2D>(ContentFolder3D + "ModelosVarios/MolinoProp/T_Windpump_D");
            MolinoEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            foreach (var mesh in MolinoModel.Meshes)
            {
                if (mesh.Name == "SM_WindPump")
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = MolinoEffect;
                    }
                }
            }
            Position = Matrix.Invert(MolinoMatrix).Translation;
            MolinoBox = CreateBoundingBox(MolinoModel, Matrix.CreateScale(2.5f), Position);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            MolinoEffect.Parameters["View"].SetValue(view);
            MolinoEffect.Parameters["Projection"].SetValue(projection);
            MolinoEffect.Parameters["ModelTexture"].SetValue(MolinoTexture);

            Matrix _molinoMatrix = MolinoMatrix * Matrix.CreateTranslation(100, 0, 555);

            foreach (var mesh in MolinoModel.Meshes)
            {
                if (mesh.Name == "SM_WindPump")
                {
                    MolinoEffect.Parameters["ModelTexture"].SetValue(MolinoTexture);
                    MolinoEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _molinoMatrix);

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
