using System;
using System.Collections.Generic;
using System.Linq;
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

        public BoundingBox CasaBox { get; set;}

        public Matrix CasaWorld {get; set;}

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
            CasaBox = BoundingVolumesExtensions.FromMatrix(CasaWorld);
            Console.WriteLine($"Casa creada: Min={CasaBox.Min}, Max={CasaBox.Max}");
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
        public BoundingBox CreateAABBFrom(Model model)
        {
            var minPoint = Vector3.One * float.MaxValue;
            var maxPoint = Vector3.One * float.MinValue;

            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            var meshes = model.Meshes;
            for (int index = 0; index < meshes.Count; index++)
            {
                var meshParts = meshes[index].MeshParts;
                for (int subIndex = 0; subIndex < meshParts.Count; subIndex++)
                {
                    var vertexBuffer = meshParts[subIndex].VertexBuffer;
                    var declaration = vertexBuffer.VertexDeclaration;
                    var vertexSize = declaration.VertexStride / sizeof(float);

                    var rawVertexBuffer = new float[vertexBuffer.VertexCount * vertexSize];
                    vertexBuffer.GetData(rawVertexBuffer);

                    for (var vertexIndex = 0; vertexIndex < rawVertexBuffer.Length; vertexIndex += vertexSize)
                    {
                        var transform = transforms[meshes[index].ParentBone.Index];
                        var vertex = new Vector3(rawVertexBuffer[vertexIndex], rawVertexBuffer[vertexIndex + 1], rawVertexBuffer[vertexIndex + 2]);
                        vertex = Vector3.Transform(vertex, transform);
                        minPoint = Vector3.Min(minPoint, vertex);
                        maxPoint = Vector3.Max(maxPoint, vertex);
                    }
                }
            }
            return new BoundingBox(minPoint, maxPoint);
        }
    }
}
