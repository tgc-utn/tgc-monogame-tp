using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGamers.SkyBoxes
{
    public class SkyBox
    {
        public SkyBox(Model model, TextureCube texture, Effect effect) : this(model, texture, effect, 2500f)
        {
        }
            

        public SkyBox(Model model, TextureCube texture, Effect effect, float scale)
        {
            Model = model;
            Texture = texture;
            Effect = effect;
            Scale = scale;
        }

        private float Scale { get; set; }
        private Model Model { get; set; }
        private TextureCube Texture { get; set; }
        private Effect Effect { get; set; }

        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var mesh in Model.Meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = Effect;
                        part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(Scale) * Matrix.CreateTranslation(cameraPosition));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(Texture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    }

                    mesh.Draw();
                }
            }
        }
    }
}