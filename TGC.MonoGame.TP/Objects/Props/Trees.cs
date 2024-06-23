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
    public class Trees : GameProp
    {
        public List<ModelMesh> TreesModels {  get; set; }
        private ModelMesh modelMesh { get; set; }
        
        public void LoadList(ContentManager content, Effect effect)
        {
            TreesModels = new List<ModelMesh>();
            Model = content.Load<Model>(ContentFolder3D + "64Trees/firs_1");
            Texture = content.Load<Texture2D>(ContentFolder3D + "64Trees/nature_bark_fir_01_l_0001_b");
            Effect = effect;

            MaxBox = new Vector3(Position.X + 100, Position.Y + 500, Position.Z + 100);
            MinBox = new Vector3(Position.X - 100, 0, Position.Z - 100);

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

        public override void Draw(Matrix view, Matrix projection)
        {
            foreach (Effect effect in modelMesh.Effects)
            {
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
            }

            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["World"].SetValue(modelMesh.ParentBone.Transform * Matrix.CreateScale(4) * WorldMatrix);

            modelMesh.Draw();
        }

    }
}
