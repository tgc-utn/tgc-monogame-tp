using System;
using System.Collections.Generic;
using System.Linq;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThunderingTanks.Collisions;

namespace ThunderingTanks.Objects
{
    public class CasaAbandonada
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderTextures = "Textures/";

        public Model CasaModel { get; set; }

        public Vector3 Position { get; set; }

        private Texture2D TexturaCasa { get; set; }
        public Matrix[] CasaWorlds { get; set; }
        public Effect Effect { get; set; }

        public BoundingBox CasaBox { get; set; }

        public Matrix CasaWorld { get; set; }

        public CasaAbandonada()
        {
            CasaWorlds = new Matrix[] { };

        }
        /*
                public void AgregarCasa(Vector3 Position)
                {
                    Matrix escala = Matrix.CreateScale(500f);
                    var nuevaCasa = new Matrix[]{
                        escala * Matrix.CreateTranslation(Position),
                    };
                    CasaWorlds = CasaWorlds.Concat(nuevaCasa).ToArray();
                }
        */
        public void LoadContent(ContentManager Content)
        {
            CasaModel = Content.Load<Model>(ContentFolder3D + "casa/house");

            TexturaCasa = Content.Load<Texture2D>(ContentFolderTextures + "casaAbandonada/Medieval_Brick_Texture_by_goodtextures");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in CasaModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            CasaWorld = Matrix.CreateScale(500f) * Matrix.CreateTranslation(Position);
            CasaBox = CreateBoundingBox(CasaModel, Matrix.CreateScale(500f), Position);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            //Effect.Parameters["DiffuseColor"].SetValue(Color.Azure.ToVector3());
            foreach (var mesh in CasaModel.Meshes)
            {
                Matrix _casaWorld = CasaWorld;
                Effect.Parameters["ModelTexture"].SetValue(TexturaCasa);

                Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _casaWorld);
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