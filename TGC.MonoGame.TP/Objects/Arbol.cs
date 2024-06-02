using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects
{
    public class Arbol
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model ArbolModel { get; set; }

        private Texture2D TexturaArbol { get; set; }
        public Matrix[] ArbolWorlds { get; set; }
        public Matrix ArbolWorld { get; set; }
        public BoundingBox ArbolBox { get; set; }
        public Vector3 Position { get; set; }
        public Effect Effect { get; set; }

        public Arbol()
        {
            ArbolWorlds = new Matrix[] { };
            ArbolWorld = Matrix.Identity;
        }
        public void LoadContent(ContentManager Content)
        {
            ArbolModel = Content.Load<Model>(ContentFolder3D + "nature/tree/Southern Magnolia-CORONA");

            TexturaArbol = Content.Load<Texture2D>(ContentFolder3D + "nature/tree/MagnoliaBark");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in ArbolModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            ArbolWorld = Matrix.CreateScale(1f) * Matrix.CreateTranslation(Position);
            ArbolBox = CreateBoundingBox(ArbolModel, Matrix.CreateScale(1f), Position);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"]?.SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(Color.Brown.ToVector3());
            foreach (var mesh in ArbolModel.Meshes)
            {
                Matrix _arbolWorld = ArbolWorld;
                Effect.Parameters["ModelTexture"]?.SetValue(TexturaArbol);

                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _arbolWorld);
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