using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThunderingTanks.Objects.Props
{
    public class Trees : GameObject
    {
        public List<ModelMesh> TreesModels {  get; set; }
        private ModelMesh modelMesh { get; set; }

        private Vector3 originalPosition;


        public void LoadList(ContentManager content, Effect effect, SimpleTerrain terrain)
        {
            TreesModels = new List<ModelMesh>();
            Model = content.Load<Model>(ContentFolder3D + "64Trees/firs_1");
            Texture = content.Load<Texture2D>(ContentFolder3D + "64Trees/nature_bark_fir_01_l_0001_b");
            Effect = effect;

            float terrainHeight = terrain.Height(Position.X, Position.Z);
            Vector3 adjustedPosition = new Vector3(Position.X, terrainHeight - 400, Position.Z);

            MaxBox = new Vector3(adjustedPosition.X + 100, adjustedPosition.Y + 500, adjustedPosition.Z + 100);
            MinBox = new Vector3(adjustedPosition.X - 100, adjustedPosition.Y, adjustedPosition.Z - 100);

            BoundingBox = new BoundingBox(MinBox, MaxBox);      

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach(ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }

                TreesModels.Add(mesh);
            }

            Random random = new Random();
            modelMesh = TreesModels[random.Next(TreesModels.Count())];


        }

        public void Draw(Matrix view, Matrix projection, GraphicsDevice graphicsDevice, SimpleTerrain terrain)
        {

            var originalRasterizerState = graphicsDevice.RasterizerState;

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;

            graphicsDevice.RasterizerState = rasterizerState;

            originalPosition = Position; 

            float terrainHeight = terrain.Height(originalPosition.X, originalPosition.Z);
            Vector3 adjustedPosition = new Vector3(originalPosition.X, terrainHeight-420, originalPosition.Z);
            WorldMatrix = Matrix.CreateTranslation(adjustedPosition);

            foreach (Effect effect in modelMesh.Effects)
            {
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
            }

            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["World"].SetValue(modelMesh.ParentBone.Transform * Matrix.CreateScale(4) * WorldMatrix);

            modelMesh.Draw();

            graphicsDevice.RasterizerState = originalRasterizerState;   

        }

    }
}
