using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ThunderingTanks.Objects.Props
{
    public class AntiTanque
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public Model AntitanqueModel { get; set; }

        private Texture2D TexturaAntitanque { get; set; }
        public Matrix[] AntitanqueWorlds { get; set; }
        public Effect Effect { get; set; }
        public BoundingBox AntiTanqueBox { get; set; }

        private Vector3 originalPosition;

        public AntiTanque()
        {
            AntitanqueWorlds = new Matrix[] { };
        }

        public void AgregarAntitanque(Vector3 Position)
        {
            Matrix posicionAntitanque = Matrix.CreateTranslation(Position) * Matrix.Identity;
            var nuevoAntitanque = new Matrix[]{
                posicionAntitanque * Matrix.CreateScale(500),
            };
            AntitanqueWorlds = AntitanqueWorlds.Concat(nuevoAntitanque).ToArray();
            originalPosition = Position;
        }

        public void LoadContent(ContentManager Content)
        {
            AntitanqueModel = Content.Load<Model>(ContentFolder3D + "assets militares/rsg_military_antitank_hedgehog_01");

            TexturaAntitanque = Content.Load<Texture2D>(ContentFolder3D + "assets militares/Textures/UE/T_rsg_military_sandbox_01_BC");
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in AntitanqueModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
            AntiTanqueBox = CreateBoundingBox(AntitanqueModel, Matrix.CreateScale(2.5f), originalPosition);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, SimpleTerrain terrain)
        {
            // Update the tree's position based on the terrain height
            float terrainHeight = terrain.Height(originalPosition.X, originalPosition.Z);
            Vector3 adjustedPosition = new Vector3(originalPosition.X, terrainHeight - 1100, originalPosition.Z);

            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["ModelTexture"].SetValue(TexturaAntitanque);

            //Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
            foreach (var mesh in AntitanqueModel.Meshes)
            {

                for (int i = 0; i < AntitanqueWorlds.Length; i++)
                {
                    Matrix _antitanqueWorld = AntitanqueWorlds[i] * Matrix.CreateTranslation(adjustedPosition);
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _antitanqueWorld);
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